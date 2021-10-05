using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Localization;
using Abp.Localization.Dictionaries;
using Abp.Runtime.Session;

namespace Abp.Runtime.Caching.Redis.Localization
{
    public class PerRequestRedisCachedMultiTenantLocalizationDictionary : MultiTenantLocalizationDictionary
    {
        public PerRequestRedisCachedMultiTenantLocalizationDictionary(
            string sourceName,
            ILocalizationDictionary internalDictionary,
            IRepository<ApplicationLanguageText, long> customLocalizationRepository,
            IAbpPerRequestRedisCacheManager perRequestRedisCacheManager,
            IAbpSession session,
            IUnitOfWorkManager unitOfWorkManager)
            : base(sourceName, internalDictionary, customLocalizationRepository, perRequestRedisCacheManager, session,
                unitOfWorkManager)
        {
        }
    }
}