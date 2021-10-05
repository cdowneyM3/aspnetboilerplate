using System.Collections.Generic;
using Abp.Authorization;
using Abp.Configuration;

namespace Abp.Zero
{
    public class AbpZeroCoreTestsSettingProvider : SettingProvider
    {
        public const string TestSetting1 = "AbpWebCommonTestModule.Test.Setting1";
        public const string TestSetting2 = "AbpWebCommonTestModule.Test.Setting2";

        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
            {
                new SettingDefinition(
                    TestSetting1,
                    "TestValue1",
                    scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User,
                    clientVisibilityProvider: new VisibleSettingClientVisibilityProvider()),

                new SettingDefinition(
                    TestSetting2,
                    "TestValue2",
                    scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User,
                    clientVisibilityProvider: new RequiresPermissionSettingClientVisibilityProvider(
                        new SimplePermissionDependency("Permission1")))
            };
        }
    }
}