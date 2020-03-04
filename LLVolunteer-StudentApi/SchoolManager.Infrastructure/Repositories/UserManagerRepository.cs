using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchoolManager.Core.Entities.Enum;
using SchoolManager.Core.Entities.ResponseModel.Base;
using SchoolManager.Core.Entities.UserInfoManager;
using SchoolManager.Core.Entities.ViewModel;
using SchoolManager.Core.Interfaces;
using SchoolManager.Infrastructure.Database;

namespace SchoolManager.Infrastructure.Repositories
{
    public class UserManagerRepository : IUserManagerRepository
    {
        public SchoolManagerContext Context { get; }

        public UserManagerRepository(
            SchoolManagerContext context
        )
        {
            Context = context;
        }
        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddUserInfo(UserInfoDTO model)
        {
            Context.UserInfoDtos.Add(model);
            return await Context.SaveChangesAsync() > 0;
        }
        /// <summary>
        /// 查找用户信息
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<UserInfoDTO> FindUserInfo(string uid, string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException($"{nameof(userName)}不能为空");
            }
            uid = uid.Trim();
            userName = userName.Trim();
            var result = await Context.UserInfoDtos.FirstOrDefaultAsync(t => t.UserId == uid && t.LoginUserName == userName);
            return result;
        }
    }
}
