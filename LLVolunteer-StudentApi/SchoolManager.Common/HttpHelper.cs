using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManager.Common
{
    public static class HttpHelper
    {
        /// <summary>
        /// 默认编码 gb2312
        /// </summary>
        public static Encoding DefaultEncoding = Encoding.GetEncoding("gb2312");
        public static string UserAgent { get; set; } = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/73.0.3683.103 Safari/537.36";
        /// <summary>
        /// Post 提交数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <param name="userAgent"></param>
        /// <returns></returns>
        public static async Task<HttpWebResponse> CreatePostHttpResponse(string url, IDictionary<string, string> parameters)
        {
            var request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Referer = request.RequestUri.ToString();
            request.Headers.Add("Origin", "http://jw1.hustwenhua.net");
            request.UserAgent = UserAgent;
            request.Accept =
                @"text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3
            ";
            //发送POST数据  
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                        i++;
                    }
                }
                byte[] data = DefaultEncoding.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.GetCookies(request.RequestUri);
            return await request.GetResponseAsync() as HttpWebResponse;
        }

        public static async Task<HttpWebResponse> CreateHttpResponse(string url, string referer=null)
        {
            var request = WebRequest.Create(url) as HttpWebRequest;
            request.UserAgent = UserAgent;
            if (!string.IsNullOrEmpty(referer))
            {
                request.Referer = referer;
            }
            return await request.GetResponseAsync() as HttpWebResponse;
        }
        /// <summary>
        /// 获取返回数据的字符串形式
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static async Task<string> GetResponseString(this HttpWebResponse response)
        {
            using (Stream s = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(s, DefaultEncoding);
                return await reader.ReadToEndAsync();
            }
        }
    }
}
