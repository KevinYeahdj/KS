using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Common
{
    public static class ConvertHelper
    {
        public static Dictionary<string, string> DicConvert(Dictionary<string, string> dicOri)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var item in dicOri)
            {
                dic.Add(item.Value, item.Key);
            }
            return dic;
        }
    }
}
