using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SchoolManager.Common
{
    public class ModelHelper
    {
        public static Dictionary<string, string> ObjectToMap<T>(T obj, bool isIgnoreNull = false)
        {
            Dictionary<string, string> map = new Dictionary<string, string>();

            Type t = obj.GetType(); // 获取对象对应的类， 对应的类型

            PropertyInfo[] pi = t.GetProperties();

            foreach (PropertyInfo p in pi)
            {
                var info = t.GetProperty(p.Name);
                var value = info?.GetValue(obj, null);
                if (value != null)
                {
                    map.Add(p.Name, value.ToString()); // 向字典添加元素
                }
                else
                {
                    map.Add(p.Name, "");
                }
            }
            return map;
        }
    }
}
