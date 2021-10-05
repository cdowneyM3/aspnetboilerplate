using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Domain.Uow;
using Abp.MultiTenancy;

namespace Abp.Runtime.Caching.Redis.SettingManagement
{
    public class PerRequestRedisCachedSettingManager : SettingManager
    {
        public PerRequestRedisCachedSettingManager(
            ISettingDefinitionManager settingDefinitionManager,
            IAbpPerRequestRedisCacheManager perRequestRedisCacheManager,
            IMultiTenancyConfig multiTenancyConfig,
            ITenantStore tenantStore,
            ISettingEncryptionService settingEncryptionService,
            IUnitOfWorkManager unitOfWorkManager)
            : base(settingDefinitionManager, perRequestRedisCacheManager, multiTenancyConfig, tenantStore,
                settingEncryptionService, unitOfWorkManager)
        {
        }
    }
}