using System;
using System.Security.Cryptography;
using System.Text;
namespace Extensions.Convert.ConvertHelper
{
    public static class StringHelper
    {
        public static bool ToBool(this string obj)
        {
            bool result = false;
            bool.TryParse(obj.ToString(),out result);
            return result;
        }
        public static int ToInt(this string obj)
        {
            int result = -1;
            int.TryParse(obj.ToString(), out result);
            return result;
        }
    }
}
