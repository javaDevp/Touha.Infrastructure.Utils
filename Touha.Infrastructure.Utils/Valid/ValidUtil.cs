using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Touha.Infrastructure.Utils.Valid
{
    public class ValidUtil
    {
        //整数正则
        private static readonly Regex RegNumber = new Regex("^[0-9]+$");

        //小数正则
        private static readonly Regex RegDecimal = new Regex("^[0-9]+[.]?[0-9]+$");

        //中文正则（根据unicode编码)
        private static readonly Regex RegCHZN = new Regex("[\u4e00-\u9fa5]");

        //邮件正则
        private static readonly Regex RegEmail = new Regex(@"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$");

        #region 数值验证
        /// <summary>
        /// 是否数字字符串
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns>返回值</returns>
        public static bool IsNumber(string inputData)
        {
            Match m = RegNumber.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 是否是浮点数
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns>返回值</returns>
        public static bool IsDecimal(string inputData)
        {
            Match m = RegDecimal.Match(inputData);
            return m.Success;
        }
        #endregion 

        #region 中文验证
        /// <summary>
        /// 检测是否有中文字符
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns>返回值</returns>
        public static bool IsHasCHZN(string inputData)
        {
            Match m = RegCHZN.Match(inputData);
            return m.Success;
        }
        #endregion

        /// <summary>
        /// 是否是邮件地址
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns>返回值</returns>
        public static bool IsEmail(string inputData)
        {
            Match m = RegEmail.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 邮编有效性
        /// </summary>
        /// <param name="zip">输入字符串</param>
        /// <returns>返回值</returns>
        public static bool IsValidZip(string zip)
        {
            var rx = new Regex(@"^\d{6}$", RegexOptions.None);
            Match m = rx.Match(zip);
            return m.Success;
        }

        /// <summary>
        /// 固定电话有效性
        /// </summary>
        /// <param name="phone">输入字符串</param>
        /// <returns>返回值</returns>
        public static bool IsValidPhone(string phone)
        {
            var rx = new Regex(@"^(\(\d{3,4}\)|\d{3,4}-)?\d{7,8}$", RegexOptions.None);
            Match m = rx.Match(phone);
            return m.Success;
        }

        /// <summary>
        /// 手机有效性
        /// </summary>
        /// <param name="mobile">输入字符串</param>
        /// <returns>返回值</returns>
        public static bool IsValidMobile(string mobile)
        {
            var rx = new Regex(@"^(13|15)\d{9}$", RegexOptions.None);
            Match m = rx.Match(mobile);
            return m.Success;
        }

        /// <summary>
        /// 电话有效性（固话和手机 ）
        /// </summary>
        /// <param name="number">输入字符串</param>
        /// <returns>返回值</returns>
        public static bool IsValidPhoneAndMobile(string number)
        {
            var rx = new Regex(@"^(\(\d{3,4}\)|\d{3,4}-)?\d{7,8}$|^(13|15)\d{9}$", RegexOptions.None);
            Match m = rx.Match(number);
            return m.Success;
        }

        /// <summary>
        /// Url有效性
        /// </summary>
        /// <param name="url">输入字符串</param>
        /// <returns>返回值</returns>
        public static bool IsValidURL(string url)
        {
            return Regex.IsMatch(url,
                                 @"^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&%\$#\=~])*[^\.\,\)\(\s]$");
        }

        /// <summary>
        /// IP有效性
        /// </summary>
        /// <param name="ip">输入字符串</param>
        /// <returns>返回值</returns>
        public static bool IsValidIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        /// <summary>
        /// domain 有效性
        /// </summary>
        /// <param name="host">域名</param>
        /// <returns>返回值</returns>
        public static bool IsValidDomain(string host)
        {
            var r = new Regex(@"^\d+$");
            if (host.IndexOf(".") == -1)
            {
                return false;
            }
            return !r.IsMatch(host.Replace(".", string.Empty));
        }

        /// <summary>
        /// 判断是否为base64字符串
        /// </summary>
        /// <param name="str">输入字符串</param>
        /// <returns>返回值</returns>
        public static bool IsBase64String(string str)
        {
            return Regex.IsMatch(str, @"[A-Za-z0-9\+\/\=]");
        }

        /// <summary>
        /// 验证字符串是否是GUID
        /// </summary>
        /// <param name="guid">字符串</param>
        /// <returns>返回值</returns>
        public static bool IsGuid(string guid)
        {
            if (string.IsNullOrEmpty(guid))
                return false;

            return Regex.IsMatch(guid, "[A-F0-9]{8}(-[A-F0-9]{4}){3}-[A-F0-9]{12}|[A-F0-9]{32}", RegexOptions.IgnoreCase);
        }
    }
}
