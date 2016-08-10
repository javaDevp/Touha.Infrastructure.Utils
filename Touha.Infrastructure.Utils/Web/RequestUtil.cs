using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Touha.Infrastructure.Utils.Web
{
    public class RequestUtil
    {
        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetClientIP()
        {
            if (HttpContext.Current.Request.ServerVariables.Get("HTTP_X_FORWARDED_FOR") != null)
                return HttpContext.Current.Request.ServerVariables.Get("HTTP_X_FORWARDED_FOR").ToString().Trim();
            else
                return HttpContext.Current.Request.ServerVariables.Get("Remote_Addr").ToString().Trim();
        }

        
    }
}
