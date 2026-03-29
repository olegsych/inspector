# Clone
This repository contains submodules and symlinks.
```PowerShell
git clone --recurse-submodules -c core.symlinks=true https://github.com/olegsych/inspector.git
```

If you already cloned without `--recurse-submodules`, initialize them manually:
```PowerShell
git submodule update --init --recursive
```

The build depends on symlinked files from the `modules/csharp.common` submodule. Without it, `Directory.Build.props`, `Directory.Build.targets`, `.editorconfig`, and `StrongName.snk` are broken symlinks and the build will fail.

# Build
```PowerShell
dotnet build .\Inspector.slnx
```

The `examples/` project may emit pre-existing nullable reference warnings during local builds; these are not CI-gated. In Release configuration, `TreatWarningsAsErrors` is enabled.

# Test
Run tests on all frameworks to verify your work.
```PowerShell
dotnet test --no-build .\tst\Tests.csproj
```

Run tests for a specific framework to speed up iteration during development.
```PowerShell
dotnet test --no-build .\tst\Tests.csproj --framework net9.0
```

`examples/Examples.csproj` has pre-existing test failures unrelated to the library and are not tested in CI.

# Pack
```PowerShell
dotnet pack .\src\Inspector.csproj
```

Creates NuGet packages in the `out/packages/` directory.

# Pull requests
Pull requests are automatically validated by the [build](https://github.com/olegsych/inspector/actions/workflows/build.yml) workflow. NuGet and symbol packages are uploaded to build artifacts.

# Continuous integration
The [build](https://github.com/olegsych/inspector/actions/workflows/build.yml) is automatically triggered for the `master` branch. NuGet and symbol packages are published to [public package feed](https://www.nuget.org/packages/inspector).

Assemblies are strong-named via `StrongName.snk`.

# Coding conventions

- **C# style** is enforced by `.editorconfig` with many rules set to `error` severity. Key rules:
  - K&R brace style (new line before open brace only for types/namespaces)
  - No `this.` qualifier, no redundant `private` modifier, no braces for single-line blocks
  - Use `var` only when type is apparent; use explicit types elsewhere
  - Expression-bodied members when on single line
  - No space after `if`/`for`/`while` keywords before parentheses
- **Nullable reference types** are enabled (`<Nullable>enable</Nullable>`)
- **C# language version**: `latest`
- **XML documentation**: Follow conventions in `.github/instructions/docs.instructions.md`
- **Test framework**: xUnit v3 with NSubstitute for mocking
- **Test naming**: Test classes mirror source classes with `Test` suffix; nested classes group related tests
- **Versioning**: Nerdbank.GitVersioning — do not manually edit assembly versions
