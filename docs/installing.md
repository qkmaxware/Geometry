# Installation
Qkmaxware.Geometry is distributed as a Nuget package via [Github's Package Repository](https://github.com/qkmaxware/Geometry/packages).

## Github Access Token
Github requires that all users will need to authenticate with Github in order to download packages. At the time of writing, the process for creating such a token located on [docs.github.com](https://docs.github.com/en/github/authenticating-to-github/creating-a-personal-access-token). When creating this token make sure that it permissions to `read:packages` enabled which is required to download github packages.

## Adding Package 
1. Create an empty project or use an existing .Net project

```
dotnet new console
```

2. Create a new file at the root of the project named `nuget.config` and paste the following xml into it. Replace `%USERNAME%` with your Github username and `%ACCESS_TOKEN%` with the access token that you created previously.

```
<?xml version="1.0" encoding="utf-8"?>
<configuration>
    <packageSources>
        <add key="qkmaxware" value="https://nuget.pkg.github.com/qkmaxware/index.json" />
    </packageSources>
    <packageSourceCredentials>
        <qkmaxware>
            <add key="Username" value="%USERNAME%" />
            <add key="ClearTextPassword" value="%ACCESS_TOKEN%" />
        </qkmaxware>
    </packageSourceCredentials>
</configuration>
```

3. Add the `Qkmaxware.Geometry` package, make sure to specify Github as the package source

```
dotnet add package Qkmaxware.Geometry --source "https://nuget.pkg.github.com/qkmaxware/index.json"
```
