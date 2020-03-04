using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolManager.Core.Entities.UserInfoManager
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfoDTO: BaseEntity
    {
        /// <summary>
        /// 学号
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealUserName { get; set; }
        /// <summary>
        /// 原账户名称
        /// </summary>
        public string LoginUserName { get; set; }
        /// <summary>
        /// 年级名称
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// 专业信息
        /// </summary>
        public string MajorName { get; set; }
        /// <summary>
        /// 学院信息
        /// </summary>
        public string College { get; set; }
        /// <summary>
        /// 是否同步
        /// </summary>
        public bool IsSynchronization { get; set; }
    }
}
