using System.Globalization;
using Abp.Localization;
using Abp.Localization.Dictionaries;
using Abp.Runtime.Caching.Redis.Localization;
using NSubstitute;
using Shouldly;
using StackExchange.Redis;
using Xunit;

namespace Abp.Zero.Redis.PerRequestRedisCache
{
    public class AbpLocalizationWithPerRequestRedisCache_Tests : PerRequestRedisCacheTestsBase<AbpLocalizationWithPerRequestRedisCacheModule>
    {
        private readonly IMultiTenantLocalizationDictionary _multiTenantLocalizationDictionary;
        private readonly IApplicationLanguageManager _applicationLanguageManager;

        public AbpLocalizationWithPerRequestRedisCache_Tests()
        {
            _applicationLanguageManager = LocalIocManager.Resolve<IApplicationLanguageManager>();
            
            var internalDic = Substitute.For<ILocalizationDictionary>();
            internalDic.CultureInfo.Returns(new CultureInfo("en"));
            _multiTenantLocalizationDictionary = LocalIocManager.Resolve<IMultiTenantLocalizationDictionary>(new
            {
                sourceName = AbpConsts.LocalizationSourceName,
                internalDictionary = internalDic
            });
        }

        [Fact]
        public void Should_Set_Per_Request_Redis_Cached_Items()
        {
            var isMultiTenantLocalizationDictionaryReplaced =
                _multiTenantLocalizationDictionary is PerRequestRedisCachedMultiTenantLocalizationDictionary;
            isMultiTenantLocalizationDictionaryReplaced.ShouldBeTrue();

            var isApplicationLanguageManagerReplaced =
                _applicationLanguageManager is PerRequestRedisCachedApplicationLanguageManager;
            isApplicationLanguageManagerReplaced.ShouldBeTrue();
        }

        [Fact]
        public void Should_Multi_Tenant_Localization_Dictionary_Request_Once_For_Same_Context()
        {
            RedisDatabase.ClearReceivedCalls();
            
            ChangeHttpContext();
            
            var allStrings = _multiTenantLocalizationDictionary.GetAllStrings(null);
            var allStrings2 = _multiTenantLocalizationDictionary.GetAllStrings(null);

            RedisDatabase.Received(1).StringGet(Arg.Any<RedisKey>());
        }

        [Fact]
        public void Should_Multi_Tenant_Localization_Dictionary_Request_Again_If_Context_Changed()
        {
            RedisDatabase.ClearReceivedCalls();
            
            ChangeHttpContext();
            var allStrings = _multiTenantLocalizationDictionary.GetAllStrings(null);

            ChangeHttpContext();
            var allStrings2 = _multiTenantLocalizationDictionary.GetAllStrings(null);

            RedisDatabase.Received(2).StringGet(Arg.Any<RedisKey>());
        }

        [Fact]
        public void Should_Multi_Tenant_Localization_Dictionary_Not_Request_For_Same_Context()
        {
            var context1 = GetNewContextSubstitute();
            var context2 = GetNewContextSubstitute();

            RedisDatabase.ClearReceivedCalls();
            
            CurrentHttpContext = context1;
            var allStrings = _multiTenantLocalizationDictionary.GetAllStrings(null); //should request db

            CurrentHttpContext = context2;
            var allStrings2 = _multiTenantLocalizationDictionary.GetAllStrings(null); //should request db

            CurrentHttpContext = context1;
            var allStrings3 = _multiTenantLocalizationDictionary.GetAllStrings(null); //should not request db

            CurrentHttpContext = context2;
            var allStrings4 = _multiTenantLocalizationDictionary.GetAllStrings(null); //should not request db

            RedisDatabase.Received(2).StringGet(Arg.Any<RedisKey>());
        }

        [Fact]
        public void Should_Application_Language_Manager_Request_Once_For_Same_Context()
        {
            RedisDatabase.ClearReceivedCalls();
            ChangeHttpContext();

            var languages = _applicationLanguageManager.GetLanguages(null);
            var languages2 = _applicationLanguageManager.GetLanguages(null);

            RedisDatabase.Received(1).StringGet(Arg.Any<RedisKey>());
        }

        [Fact]
        public void Should_Application_Language_Manager_Request_Again_If_Context_Changed()
        {
            RedisDatabase.ClearReceivedCalls();
            ChangeHttpContext();
            var languages = _applicationLanguageManager.GetLanguages(null);

            ChangeHttpContext();
            var languages2 = _applicationLanguageManager.GetLanguages(null);

            RedisDatabase.Received(2).StringGet(Arg.Any<RedisKey>());
        }
        
        [Fact]
        public void Should_Application_Language_Manager_Not_Request_For_Same_Context()
        {
            var context1 = GetNewContextSubstitute();
            var context2 = GetNewContextSubstitute();
            
            RedisDatabase.ClearReceivedCalls();
            
            CurrentHttpContext = context1;
            var languages = _applicationLanguageManager.GetLanguages(null); //should request db

            CurrentHttpContext = context2;
            var languages2 = _applicationLanguageManager.GetLanguages(null); //should request db

            CurrentHttpContext = context1;
            var languages3 = _applicationLanguageManager.GetLanguages(null); //should not request db

            CurrentHttpContext = context2;
            var languages4 = _applicationLanguageManager.GetLanguages(null); //should not request db

            RedisDatabase.Received(2).StringGet(Arg.Any<RedisKey>());
        }
    }
}