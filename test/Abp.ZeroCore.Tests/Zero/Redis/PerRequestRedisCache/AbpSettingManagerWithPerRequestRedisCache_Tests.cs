using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Runtime.Caching.Redis.SettingManagement;
using NSubstitute;
using Shouldly;
using StackExchange.Redis;
using Xunit;

namespace Abp.Zero.Redis.PerRequestRedisCache
{
    public class AbpSettingManagerWithPerRequestRedisCache_Tests : PerRequestRedisCacheTestsBase<AbpSettingManagerWithPerRequestRedisCacheModule>
    {
        private readonly ISettingManager _settingManager;
        private readonly ISettingsConfiguration _settingConfiguration;

        public AbpSettingManagerWithPerRequestRedisCache_Tests()
        {
            _settingConfiguration = LocalIocManager.Resolve<ISettingsConfiguration>();
            _settingManager = LocalIocManager.Resolve<ISettingManager>();
        }

        [Fact]
        public void Should_Set_Per_Request_Redis_Cache_Manager_To_Configuration()
        {
            var isSettingManagerReplaced = _settingManager is PerRequestRedisCachedSettingManager;
            isSettingManagerReplaced.ShouldBeTrue();
        }

        [Fact]
        public void Should_Request_Once_For_Same_Context()
        {
            ChangeHttpContext();

            var setting = _settingManager.GetSettingValue(AbpZeroCoreTestsSettingProvider.TestSetting1);
            var setting2 = _settingManager.GetSettingValue(AbpZeroCoreTestsSettingProvider.TestSetting1);

            RedisDatabase.Received(1).StringGet(Arg.Any<RedisKey>());
        }

        [Fact]
        public void Should_Request_Again_If_Context_Change()
        {
            ChangeHttpContext();
            var setting = _settingManager.GetSettingValue(AbpZeroCoreTestsSettingProvider.TestSetting1);

            ChangeHttpContext();
            var setting2 = _settingManager.GetSettingValue(AbpZeroCoreTestsSettingProvider.TestSetting1);

            RedisDatabase.Received(2).StringGet(Arg.Any<RedisKey>());
        }

        [Fact]
        public void Should_Not_Request_For_Same_Context()
        {
            var context1 = GetNewContextSubstitute();
            var context2 = GetNewContextSubstitute();
            
            CurrentHttpContext = context1;
            var setting = _settingManager.GetSettingValue(AbpZeroCoreTestsSettingProvider.TestSetting1);//should request db

            CurrentHttpContext = context2;
            var setting2 = _settingManager.GetSettingValue(AbpZeroCoreTestsSettingProvider.TestSetting1);//should request db
            
            CurrentHttpContext = context1;
            var setting3 = _settingManager.GetSettingValue(AbpZeroCoreTestsSettingProvider.TestSetting1);//should not request db

            CurrentHttpContext = context2;
            var setting4 = _settingManager.GetSettingValue(AbpZeroCoreTestsSettingProvider.TestSetting1);//should not request db

            RedisDatabase.Received(2).StringGet(Arg.Any<RedisKey>());
        }
    }
}