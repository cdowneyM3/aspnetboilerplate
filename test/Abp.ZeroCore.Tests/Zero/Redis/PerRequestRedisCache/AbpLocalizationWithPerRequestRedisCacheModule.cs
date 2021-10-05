using Abp.AutoMapper;
using Abp.Modules;
using Abp.Notifications;
using Abp.Reflection.Extensions;
using Abp.Runtime.Caching.Redis;
using Abp.Runtime.Caching.Redis.Localization;
using Abp.TestBase;
using Abp.Zero.Configuration;
using Abp.Zero.Notifications;
using Abp.ZeroCore.SampleApp;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers;
using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries.Json;
using Abp.Localization.Sources;
using Abp.Zero.Localization;

namespace Abp.Zero.Redis.PerRequestRedisCache
{
    [DependsOn(typeof(AbpZeroCommonModule))]
    [DependsOn(typeof(AbpTestBaseModule))]
    [DependsOn(typeof(AbpZeroCoreSampleAppModule))]
    [DependsOn(typeof(AbpAspNetCorePerRequestRedisCacheModule))]
    public class AbpLocalizationWithPerRequestRedisCacheModule : AbpModule
    {
        public AbpLocalizationWithPerRequestRedisCacheModule(AbpZeroCoreSampleAppModule sampleAppModule)
        {
            sampleAppModule.SkipDbContextRegistration = true;
        }

        public override void PreInitialize()
        {
#pragma warning disable CS0618 // Type or member is obsolete, this line will be removed once the UseStaticMapper is removed
            Configuration.Modules.AbpAutoMapper().UseStaticMapper = false;
#pragma warning restore CS0618 // Type or member is obsolete, this line will be removed once the UseStaticMapper is removed
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();
            Configuration.UnitOfWork.IsTransactional = false;

            Configuration.ReplaceService<INotificationDistributer, FakeNotificationDistributer>();
           
            Configuration.Settings.Providers.Add<AbpZeroCoreTestsSettingProvider>();
            
            Configuration.Localization.Sources.Extensions.Add(
                new LocalizationSourceExtensionInfo(
                    AbpConsts.LocalizationSourceName,
                    new JsonEmbeddedFileLocalizationDictionaryProvider(
                        typeof(MyCustomJsonLangModule).GetAssembly(), "Abp.Zero.Localization.Sources.Extensions.Json.Abp"
                    )
                )
            );

            Configuration.Caching.UseRedis();
            Configuration.UsePerRequestRedisCacheForLocalization();
        }

        public override void Initialize()
        {           
            TestServiceCollectionRegistrar.Register(IocManager);
            IocManager.RegisterAssemblyByConvention(typeof(AbpLocalizationWithPerRequestRedisCacheModule).GetAssembly());
            IocManager.IocContainer.Register(
                Component.For<ILazyComponentLoader>().ImplementedBy<LazyOfTComponentLoader>()
            );
        }
    }
}