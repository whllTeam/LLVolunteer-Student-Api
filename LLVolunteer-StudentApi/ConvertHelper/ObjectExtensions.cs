using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ConvertHelper
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// data yyyy-MM-dd hh:mm:ss
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataToStr(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd hh:mm:ss");
        }
        /// <summary>
        /// 通过序列化与反序列化 clone obj
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T ObjectClone<T>(this object obj)
        {
            var str = JsonConvert.SerializeObject(obj);
            return (T) JsonConvert.DeserializeObject(str);
        }
    }
}
