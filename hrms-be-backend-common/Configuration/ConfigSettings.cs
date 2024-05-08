using hrms_be_backend_common.Models;
using Microsoft.Extensions.Configuration;

//using Microsoft.Extensions.Configuration;
//using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace GTB.Common
{
    public static class ConfigSettings
    {

        private static IConfigurationRoot _ConfigRoot;

        private static string ApplicationExeDirectory() => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        private static IConfigurationRoot GetAppSettings()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder().SetBasePath(ApplicationExeDirectory()).AddJsonFile("appsettings.json");
            if (_ConfigRoot == null)
                _ConfigRoot = configurationBuilder.Build();
            return _ConfigRoot;
        }




        // public static string BasisConnString => GTBEncryptLib.DecryptText(ConfigSettings.GetAppSettings()["ConnectionString:BasisConnString"]);
        //public static ConnectionStrings ConnectionStrings => GetAppSettings().GetSection("ConnectionStrings").Get<ConnectionStrings>();
        public static LeaveRequestConfig leaveRequestConfig => GetAppSettings().GetSection("LeaveRequest").Get<LeaveRequestConfig>();
    }
}