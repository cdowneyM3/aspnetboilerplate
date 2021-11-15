Commands to configure nuget

	dotnet nuget list source
	dotnet nuget remove source Nexus
	dotnet nuget add source "https://pkgs.dev.azure.com/ccok/Nexus/_packaging/Nexus/nuget/v3/index.json" --name Nexus --username "<UserUserName>" --password "<YourPAT>"

	replace <UserUserName> with your user name
	replace <YourPAT> with your personal access tokens
	
	Learn about personal access tokens
		https://docs.microsoft.com/en-us/azure/devops/organizations/accounts/use-personal-access-tokens-to-authenticate?view=azure-devops&tabs=preview-page


Before Publishing
	Update the version number of the project under
		Project Properties -> Package -> Package Version


Execute these commands from the root folder of the Abp.RedisCache.Hybrid project.
	dotnet pack Abp.RedisCache.Hybrid.csproj --output nupkgs
	dotnet nuget push --source "Nexus" --api-key na nupkgs\Abp.RedisCache.Hybrid.5.3.1.nupkg



Other Redis Tools
	https://joeferner.github.io/redis-commander/