using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
namespace SchoolManager.Core.Entities.ViewModel
{
    /// <summary>
    /// 用户登陆教务网Model
    /// </summary>
    public class UserLoginRequest
    {
        /// <summary>
        /// 学号
        /// </summary>
        [Required]
        public string UserId { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        [Required]
        public string ValidateCode { get; set; }
        /// <summary>
        /// 验证码所对应的PreUrl
        /// </summary>
        [Required]
        public string PreUrl { get; set; }
        /// <summary>
        /// 登陆Url
        /// </summary>
        public string LoginUrl { get; set; }
        /// <summary>
        /// 登陆页面 ViewState
        /// </summary>
        [Required]
        public string LoginViewState { get; set; }
        /// <summary>
        /// 登陆用户名
        /// </summary>
        [Required]
        public string LoginUserName { get; set; }
    }
}
