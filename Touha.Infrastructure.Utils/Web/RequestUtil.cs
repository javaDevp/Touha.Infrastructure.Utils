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

        /// <summary>
        /// 获取客户端IP地址（IPV4）
        /// </summary>
        /// <returns></returns>
        public static string GetClientIPv4()
        {
            string ipv4 = string.Empty;

            foreach (IPAddress ip in Dns.GetHostAddresses(GetClientIP()))
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    ipv4 = ip.ToString();
                    break;
                }
            }

            if (ipv4 != string.Empty)
            {
                return ipv4;
            }

            // 原作使用 Dns.GetHostName 方法取回的是 Server 端資訊，非 Client 端。
            // 改寫為利用 Dns.GetHostEntry 方法，由獲取的 IPv6 位址反查 DNS 紀錄，
            // 再逐一判斷何者屬 IPv4 協定，即可轉為 IPv4 位址。
            foreach (IPAddress ip in Dns.GetHostEntry(GetClientIP()).AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    ipv4 = ip.ToString();
                    break;
                }
            }

            return ipv4;
        }
    }
}
