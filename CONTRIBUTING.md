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

# pull requests
Pull requests are automatically validated by the [build](https://github.com/olegsych/inspector/actions/workflows/build.yml)
workflow. NuGet and symbol packages are uploaded to build artifacts.

# continuous integration
The [build](https://github.com/olegsych/inspector/actions/workflows/build.yml) is automatically triggered for the `master`
branch. NuGet and symbol packages are published to [public package feed](https://www.nuget.org/packages/inspector) 
