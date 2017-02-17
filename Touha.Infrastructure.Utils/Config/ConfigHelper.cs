using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Touha.Infrastructure.Utils.Config
{
    /// <summary>
    /// 配置文件辅助类
    /// </summary>
    public class ConfigHelper
    {
        #region Fields
        private static volatile Configuration _config;
        private static volatile ConfigHelper _configHelper;
        private static readonly object _lockObj = new object();
        #endregion

        #region Constructors
        private ConfigHelper()
        {

        }

        /// <summary>
        /// 单例方法
        /// </summary>
        /// <param name="configFileType"></param>
        /// <returns></returns>
        public static ConfigHelper GetInstance(ConfigFileType configFileType)
        {
            if (_configHelper == null)
            {
                lock (_lockObj)
                {
                    if (_configHelper == null)
                    {
                        _configHelper = new ConfigHelper();
                        switch (configFileType)
                        {
                            case ConfigFileType.AppConfig:
                                _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                                break;
                            case ConfigFileType.WebConfig:
                                _config = WebConfigurationManager.OpenWebConfiguration("~");
                                break;
                        }
                    }
                }
            }
            return _configHelper;
        }
        #endregion

        #region Methods
        /// <summary>
        /// 添加AppSettings配置节
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddAppSetting(string key, string value)
        {
            if (_config.AppSettings.Settings[key] == null)
            {
                //配置节点不存在，添加
                _config.AppSettings.Settings.Add(key, value);
            }
            else if (_config.AppSettings.Settings[key].Value != value)
            {
                //配置节点存在，值变化，更新值
                _config.AppSettings.Settings[key].Value = value;
            }
            _config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(_config.AppSettings.SectionInformation.Name);

        }

        /// <summary>
        /// 获取APPSetting节点的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetAppSetting(string key)
        {
            return _config.AppSettings.Settings[key].Value;
        }

        /// <summary>
        /// 获取指定name的连接字符串
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetConnectionString(string name)
        {
            return _config.ConnectionStrings.ConnectionStrings[name].ConnectionString;
        }
        #endregion
    }

    /// <summary>
    /// 配置文件类型
    /// </summary>
    public enum ConfigFileType
    {
        AppConfig,
        WebConfig
    }
}
