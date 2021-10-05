using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Localization;

namespace Abp.Runtime.Caching.Redis.Localization
{
    public static class MultiTenantLocalizationDictionaryExtensions
    {
        public static void UsePerRequestRedisCacheForLocalization(this IAbpStartupConfiguration startupConfiguration)
        {
            startupConfiguration.ReplaceService<IMultiTenantLocalizationDictionary, PerRequestRedisCachedMultiTenantLocalizationDictionary>(DependencyLifeStyle.Transient);
            startupConfiguration.ReplaceService<IApplicationLanguageManager, PerRequestRedisCachedApplicationLanguageManager>(DependencyLifeStyle.Singleton);
        }
    }
}