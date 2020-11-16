# clone
This repository contains submodules and symlinks.
```PowerShell
git clone --recurse-submodules -c core.symlinks=true https://github.com/olegsych/inspector.git
```

# build
Use [Visual Studio](https://visualstudio.microsoft.com/downloads) or command line.
```PowerShell
dotnet build .\Inspector.sln
```

# test
Use Visual Studio or command line.
```PowerShell
dotnet test .\tst\Tests.csproj
```

# continuous integration and release
[AppVeyor](https://ci.appveyor.com/project/olegsych/inspector) builds are automatically triggered for the master branch.
Nuget and symbol packages are automatically published to [nuget.org](https://www.nuget.org/packages/inspector).
Build settings are defined in [appveyor.yml](.\appveyor.yml).

# pull requests
Pull requests are automatically validated by [AppVeyor](https://ci.appveyor.com/project/olegsych/inspector).
Note to self: All changes must be merged into master through pull requests to avoid unnecessary churn of NuGet package versions.
