using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Runtime.Caching.Redis;
using Abp.Runtime.Caching.Redis.SettingManagement;

namespace Abp.Zero.Redis.PerRequestRedisCache
{
    [DependsOn(typeof(AbpAspNetCorePerRequestRedisCacheModule))]
    public class AbpSettingManagerWithPerRequestRedisCacheModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Settings.Providers.Add<AbpZeroCoreTestsSettingProvider>();
            
            Configuration.Caching.UseRedis();
            Configuration.UsePerRequestRedisCacheForSettingManager();
        }
        
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AbpSettingManagerWithPerRequestRedisCacheModule).GetAssembly());
        }
    }
}