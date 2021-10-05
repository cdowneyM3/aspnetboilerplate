using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Dependency;

namespace Abp.Runtime.Caching.Redis.SettingManagement
{
    public static class SettingsConfigurationExtensions
    {
        public static void UsePerRequestRedisCacheForSettingManager(this IAbpStartupConfiguration startupConfiguration)
        {
            startupConfiguration.ReplaceService<ISettingManager, PerRequestRedisCachedSettingManager>(DependencyLifeStyle.Singleton);
        }
    }
}