using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SchoolManager.Core.Entities.ResponseModel.Base;
using SchoolManager.Core.Entities.UserInfoManager;
using SchoolManager.Core.Entities.ViewModel;

namespace SchoolManager.Core.Interfaces
{
    public interface ISchoolInfoService
    {
        /// <summary>
        /// 获取 验证码
        /// </summary>
        /// <returns></returns>
        Task<ResponseMessage<ValidateCodeModel>> GetValidateImage();
        /// <summary>
        /// 验证码转 base64
        /// </summary>
        /// <returns></returns>
        Task<ResponseMessage<ValidateCodeModel>> GetValidateImageBase64();
        /// <summary>
        /// 根据  学号、密码  获取 用户 教务网 消息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseMessage<UserInfoDTO>>StudentLogin(UserLoginRequest model);
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="uid">学号</param>
        /// <param name="userName">登录名</param>
        /// <returns></returns>
        Task<ResponseMessage<UserInfoDTO>> GetLocalUserInfo(string uid, string userName);
    }
}
