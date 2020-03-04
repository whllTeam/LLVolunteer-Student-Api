using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using SchoolManager.Core.Entities.UserInfoManager;

namespace SchoolManager.Infrastructure.Database
{
    public class SchoolManagerContext : DbContext
    {
        public SchoolManagerContext(DbContextOptions options) : base(options)
        {
        }

        public SchoolManagerContext()
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserInfoDTO>().ToTable("UserInfo");
        }
        /// <summary>
        ///  学生信息
        /// </summary>
        public DbSet<UserInfoDTO> UserInfoDtos { get; set; }
    }
}
