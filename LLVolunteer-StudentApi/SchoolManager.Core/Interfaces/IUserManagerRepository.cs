using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using SchoolManager.Core.Entities.ResponseModel.Base;
using SchoolManager.Core.Entities.UserInfoManager;
using SchoolManager.Core.Entities.ViewModel;

namespace SchoolManager.Core.Interfaces
{
    public interface IUserManagerRepository
    {
        /// <summary>
        /// 添加 用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> AddUserInfo(UserInfoDTO model);
        /// <summary>
        /// 通过学号  查找  用户信息
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<UserInfoDTO> FindUserInfo(string uid, string userName);
    }
}
