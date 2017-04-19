using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Touha.Infrastructure.Utils.System
{
    public class SystemHelper
    {
        private static OperatingSystem _os = Environment.OSVersion;

        public static string GetServicePack()
        {
            return _os.ServicePack;
        }

        public static string GetVersionString()
        {
            return _os.VersionString;
        }

        public static string GetPaltform()
        {
            return _os.Platform.ToString();
        }
    }
}
