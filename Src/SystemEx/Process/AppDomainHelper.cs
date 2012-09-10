using System;

namespace Amido.SystemEx.Process
{
    public static class AppDomainHelper
    {
        public static System.AppDomain CreateAppDomain(string name)
        {
            AppDomainSetup domainSetup = new AppDomainSetup()
            {
                ApplicationBase = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                ConfigurationFile = System.AppDomain.CurrentDomain.SetupInformation.ConfigurationFile,
                ApplicationName = System.AppDomain.CurrentDomain.SetupInformation.ApplicationName,
                LoaderOptimization = LoaderOptimization.MultiDomain,
                PrivateBinPath = System.AppDomain.CurrentDomain.SetupInformation.PrivateBinPath,
                PrivateBinPathProbe = System.AppDomain.CurrentDomain.SetupInformation.PrivateBinPathProbe,
                DynamicBase = System.AppDomain.CurrentDomain.SetupInformation.DynamicBase,
            };

            var appDomain = System.AppDomain.CreateDomain(name, null, domainSetup);
            return appDomain;
        }

    }
}
