using System;
using System.Collections.Generic;
using System.Text;
using ConvertHelper;

namespace SchoolManager.Core.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public bool IsDel { get; set; } = false;
        public string CreateTime { get; set; } = DateTime.Now.DataToStr();
    }
}
