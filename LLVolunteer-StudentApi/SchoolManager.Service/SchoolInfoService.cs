using System;
using System.Drawing;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using SchoolInfoManager.MQ.Publish;
using SchoolManager.Common;
using SchoolManager.Common.Entities;
using SchoolManager.Core.Entities.ResponseModel.Base;
using SchoolManager.Core.Entities.UserInfoManager;
using SchoolManager.Core.Entities.ViewModel;
using SchoolManager.Core.Interfaces;

namespace SchoolManager.Service
{
    public class SchoolInfoService : ISchoolInfoService
    {
        public ILogger Logger { get; }
        public IUserManagerRepository UserManagerRepository { get; }
        public SchoolUserPublish SchoolUser { get; set; }
        public SchoolInfoService(
            ILogger<SchoolInfoService> logger,
            IUserManagerRepository userManagerRepository,
            SchoolUserPublish schoolUser
            )
        {
            Logger = logger;
            UserManagerRepository = userManagerRepository;
            SchoolUser = schoolUser;
        }

        #region 本不应该  出现在这个地方

        public string BaseUrl = "http://jw1.hustwenhua.net";
        public string LoginViewState = string.Empty;
        public string LoginUrl = string.Empty;
        public string UserName = string.Empty;

        #endregion

        /// <summary>
        /// 教务网登陆、获取 用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseMessage<UserInfoDTO>> StudentLogin(UserLoginRequest model)
        {

            var resMessage = new ResponseMessage<UserInfoDTO>();
            try
            {
                var userInfo = await GetNatUserInfo(model);
                resMessage.Ok(userInfo);
            }
            catch (Exception e)
            {
                resMessage.Error(e);
            }

            return resMessage;
        }
        /// <summary>
        /// 获取本地  用户信息
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<ResponseMessage<UserInfoDTO>> GetLocalUserInfo(string uid, string userName)
        {
            var resMessage = new ResponseMessage<UserInfoDTO>();
            try
            {
                var model = await UserManagerRepository.FindUserInfo(uid, userName);
                resMessage.Ok(model);
            }
            catch (Exception e)
            {
                resMessage.Error(e);
            }

            return resMessage;
        }
        /// <summary>
        /// 通过教务网  获取  用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task<UserInfoDTO> GetNatUserInfo(UserLoginRequest model)
        {
            var userInfo = new UserInfoDTO();
            var postModel = new LoginPostModel()
            {
                __VIEWSTATE = System.Web.HttpUtility.UrlEncode(model.LoginViewState, HttpHelper.DefaultEncoding),
                txtUserName = model.UserId,
                TextBox2 = System.Web.HttpUtility.UrlEncode(model.Password, HttpHelper.DefaultEncoding),
                txtSecretCode = model.ValidateCode
            };
            var dic = ModelHelper.ObjectToMap<LoginPostModel>(postModel);
            try
            {
                var response = await HttpHelper.CreatePostHttpResponse(model.LoginUrl, dic);
                if (!response.ResponseUri.ToString().Contains("xs_main"))
                {
                    throw new ArgumentNullException("用户名或密码有误");
                }

                var str = await response.GetResponseString();
                var doc = new HtmlDocument();
                doc.LoadHtml(str);
                string name = doc.GetElementbyId("xhxm").InnerText.Replace("同学", "");
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentNullException("服务异常，稍后在尝试");
                }

                UserName = name;
                // 课表url
                string kbUrl =
                    $"{model.PreUrl}/xskbcx.aspx?xh={model.UserId}&xm={System.Web.HttpUtility.UrlEncode(UserName, HttpHelper.DefaultEncoding)}&gnmkdm=N121602";
                var kbResponse =
                    await HttpHelper.CreateHttpResponse(kbUrl, $"{model.PreUrl}/xs_main.aspx?xh={model.UserId}");
                var kbStr = await kbResponse.GetResponseString();
                doc.LoadHtml(kbStr);
                string college = doc.GetElementbyId("Label7").InnerText;
                string majorName = doc.GetElementbyId("Label8").InnerText;
                string className = doc.GetElementbyId("Label9").InnerText;

                userInfo.College = college.Substring(college.LastIndexOf('：') + 1);
                userInfo.MajorName = majorName.Substring(majorName.LastIndexOf('：') + 1);
                userInfo.ClassName = className.Substring(className.LastIndexOf('：') + 1);
                userInfo.RealUserName = UserName;
                userInfo.UserId = model.UserId;
                userInfo.LoginUserName = model.LoginUserName;
                try
                {
                    // MQ  同步到志愿管理服务 中
                    SchoolUser.PushMessage("school", new
                    {
                        UserName = userInfo.LoginUserName,
                        SchoolUserName = userInfo.RealUserName,
                        UserId = userInfo.UserId
                    });
                    userInfo.IsSynchronization = true;
                    Logger.LogInformation("MQ发送消息成功");
                }
                catch (Exception e)
                {
                    userInfo.IsSynchronization = false;
                    Logger.LogError(e, "MQ 发送消息失败");
                }
                var addResult = await UserManagerRepository.AddUserInfo(userInfo);
                if (addResult == false)
                {
                    throw new ArgumentNullException("同步本地数据失败");
                }
                return userInfo;
            }
            catch (Exception e)
            {
                if (e is NetworkInformationException)
                {
                    throw new ArgumentNullException("服务器忙，稍后再试");
                }
                else
                {
                    throw e;
                }
            }
        }
        /// <summary>
        /// 获取 验证码信息
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseMessage<ValidateCodeModel>> GetValidateImage()
        {
            var resMessage = new ResponseMessage<ValidateCodeModel>();
            try
            {
                var imageCoder = await GetValidate();
                resMessage.Ok(imageCoder);
            }
            catch (Exception e)
            {
                resMessage.Error(e);
            }
            return resMessage;
        }
        public async Task<ResponseMessage<ValidateCodeModel>> GetValidateImageBase64()
        {
            var resMessage = new ResponseMessage<ValidateCodeModel>();
            try
            {
                var imageCoder = await GetValidateBase64();
                resMessage.Ok(imageCoder);
            }
            catch (Exception e)
            {
                resMessage.Error(e);
            }
            return resMessage;
        }
        /// <summary>
        /// 获取验证码 stream 形式
        /// </summary>
        /// <returns></returns>
        private async Task<ValidateCodeModel> GetValidate()
        {
            try
            {
                LoginViewState = await GetTrueUrl();
                string preUrl = LoginUrl.Remove(LoginUrl.LastIndexOf('/'));
                var response = await HttpHelper.CreateHttpResponse($"{preUrl}/CheckCode.aspx", LoginUrl);
                var model = new ValidateCodeModel()
                {
                    PreUrl = preUrl,
                    LoginUrl = LoginUrl,
                    LoginViewState = LoginViewState
                };
                using (var stream = response.GetResponseStream())
                {
                    Byte[] buffer = new Byte[response.ContentLength];
                    int offset = 0, actuallyRead = 0;
                    do
                    {
                        actuallyRead = stream.Read(buffer, offset, buffer.Length - offset);
                        offset += actuallyRead;
                    }
                    while (actuallyRead > 0);
                    model.ImageStream = new MemoryStream(buffer);
                }
                response.Close();

                return model;
            }
            catch (Exception e)
            {
                if (e is NetworkInformationException)
                {
                    throw new ArgumentNullException("服务器忙，稍后再试");
                }
                else
                {
                    throw new ArgumentNullException("服务内部错误");
                }
            }
        }
        /// <summary>
        /// 获取验证码 base64
        /// </summary>
        /// <returns></returns>
        private async Task<ValidateCodeModel> GetValidateBase64()
        {
            try
            {
                LoginViewState = await GetTrueUrl();
                string preUrl = LoginUrl.Remove(LoginUrl.LastIndexOf('/'));
                var response = await HttpHelper.CreateHttpResponse($"{preUrl}/CheckCode.aspx", LoginUrl);
                var model = new ValidateCodeModel()
                {
                    PreUrl = preUrl,
                    LoginUrl = LoginUrl,
                    LoginViewState = LoginViewState
                };
                MemoryStream ms = null;
                using (var stream = response.GetResponseStream())
                {
                    Byte[] buffer = new Byte[response.ContentLength];
                    int offset = 0, actuallyRead = 0;
                    do
                    {
                        actuallyRead = stream.Read(buffer, offset, buffer.Length - offset);
                        offset += actuallyRead;
                    }
                    while (actuallyRead > 0);
                    ms = new MemoryStream(buffer);
                }
                response.Close();
                Image image = Image.FromStream(ms);
                Bitmap bitmap = new Bitmap(image);
                ms = new MemoryStream();
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                model.Arr = arr;
                model.ImageStream = ms;
                model.ImageBase64 = Convert.ToBase64String(arr);
                return model;
            }
            catch (Exception e)
            {
                if (e is NetworkInformationException)
                {
                    throw new ArgumentNullException("服务器忙，稍后再试");
                }
                else
                {
                    throw new ArgumentNullException("服务内部错误");
                }
            }
        }
        /// <summary>
        /// 获取  教务网登陆url
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetTrueUrl()
        {
            if (string.IsNullOrEmpty(LoginViewState))
            {
                var response = await HttpHelper.CreateHttpResponse(BaseUrl);
                string responseContent = (await HttpHelper.GetResponseString(response)).Trim('\r').Trim('\n');
                Regex regex = new Regex("((.*)VIEWSTATE\" value=\")(.*)(\" />)");
                string viewStateLogin = regex.Match(responseContent).Groups[3].Value;
                LoginUrl = response.ResponseUri.ToString().TrimEnd('/');
                return viewStateLogin;
            }
            else
            {
                return LoginViewState;
            }
        }
    }
}
