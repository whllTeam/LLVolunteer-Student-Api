using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolManager.Common.Entities
{
    /// <summary>
    /// 登陆 表单model
    /// </summary>
    public class LoginPostModel
    {
        /// <summary>
        /// ViewState   必填
        /// </summary>
        public string __VIEWSTATE { get; set; }
        /// <summary>
        /// 学号   必填
        /// </summary>
        public string txtUserName { get; set; }
        public string Textbox1 { get; set; }
        /// <summary>
        /// 密码    必填
        /// </summary>
        public string TextBox2 { get; set; }
        /// <summary>
        /// 验证码   必填
        /// </summary>
        public string txtSecretCode { get; set; }
        /// <summary>
        /// 身份 (学生)   必填(已有默认值)
        /// </summary>
        public string RadioButtonList1 { get; set; } = "%D1%A7%C9%FA";

        public string Button1 { get; set; }
        public string lbLanguage { get; set; }
        public string hidPdrs { get; set; }
        public string hidsc { get; set; }
    }
}
