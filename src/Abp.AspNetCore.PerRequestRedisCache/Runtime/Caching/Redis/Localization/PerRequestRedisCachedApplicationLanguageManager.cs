using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Localization;

namespace Abp.Runtime.Caching.Redis.Localization
{
    public class PerRequestRedisCachedApplicationLanguageManager : ApplicationLanguageManager
    {
        public PerRequestRedisCachedApplicationLanguageManager(
            IRepository<ApplicationLanguage> languageRepository,
            IAbpPerRequestRedisCacheManager perRequestRedisCacheManager,
            IUnitOfWorkManager unitOfWorkManager,
            ISettingManager settingManager)
            : base(languageRepository, perRequestRedisCacheManager, unitOfWorkManager, settingManager)
        {
        }
    }
}