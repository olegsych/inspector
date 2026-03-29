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

# Repository layout

```
Inspector.slnx              # Solution file (3 projects)
src/Inspector.csproj         # Main library (netstandard2.0;netstandard2.1)
tst/Tests.csproj             # Unit tests - xUnit v3 (net8.0;net9.0;net10.0;net472 on Windows)
examples/Examples.csproj     # Example tests (not run in CI, has pre-existing failures)
Directory.Build.props        # Symlink → modules/csharp.common/ (shared build properties)
Directory.Build.targets      # Symlink → modules/csharp.common/ (shared build targets)
Directory.Packages.props     # Central package version management
.editorconfig                # Symlink → modules/csharp.common/ (C# coding style rules)
StrongName.snk               # Symlink → modules/csharp.common/ (assembly signing key)
version.json                 # Nerdbank.GitVersioning config (version 0.3.x)
modules/csharp.common/       # Git submodule with shared build/style config
.github/workflows/build.yml  # CI workflow (build → test → pack on Windows+Ubuntu)
.github/instructions/        # Symlink → modules/csharp.common/ (Copilot instructions)
```

**Key source directories:**
- `src/` — Public API: `ObjectExtensions.cs`, `TypeExtensions.cs`, `Type.cs`, and member types (`Field.cs`, `Property.cs`, `Method.cs`, `Event.cs`, `Constructor.cs`) plus generic variants. `src/Implementation/` has internal helpers (filters, factories, selectors).
- `tst/` — One test file per source file. `tst/Implementation/` mirrors `src/Implementation/`.

# Build
Use [Visual Studio](https://visualstudio.microsoft.com/downloads) or command line. Always run commands from the repository root.
```PowerShell
dotnet build .\Inspector.slnx
```

The `examples/` project may emit pre-existing nullable reference warnings during local builds; these are not CI-gated. In Release configuration, `TreatWarningsAsErrors` is enabled.

# Test
Use Visual Studio or command line. Always use `--no-build` after building to avoid redundant compilation.
```PowerShell
dotnet test --no-build .\tst\Tests.csproj
```

To run tests for a specific framework:
```PowerShell
dotnet test --no-build .\tst\Tests.csproj --framework net9.0
```

Do not run `dotnet test` against the full solution or `examples/Examples.csproj` — examples have pre-existing test failures unrelated to the library and are not tested in CI.

# Pack
```PowerShell
dotnet pack .\src\Inspector.csproj
```

Creates NuGet packages in `out/packages/`.

# Pull requests
Pull requests are automatically validated by the [build](https://github.com/olegsych/inspector/actions/workflows/build.yml)
workflow. NuGet and symbol packages are uploaded to build artifacts.

# Continuous integration
The [build](https://github.com/olegsych/inspector/actions/workflows/build.yml) is automatically triggered for the `master`
branch. NuGet and symbol packages are published to [public package feed](https://www.nuget.org/packages/inspector).

The CI build runs on both `windows-latest` and `ubuntu-latest`:

1. `dotnet build` — builds entire solution
2. `dotnet test --no-build tst/Tests.csproj --logger trx` — runs unit tests only
3. `dotnet pack src/Inspector.csproj` — creates NuGet packages

The CI checks out with `submodules: recursive` and `fetch-depth: 0` (full history required for Nerdbank.GitVersioning).

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

# Examples

The `examples/` project demonstrates API usage. When adding new APIs:

- Add examples for **all new overloads**, including type-based and position-based combinations.
- Design example test types with **multiple parameters of the same type** when demonstrating type+position overloads, so the examples show the real disambiguation benefit.
- New examples for newly-implemented APIs must pass. Do not add examples for APIs that are not yet implemented.

# Important notes

- The `out/`, `bin/`, and `obj/` directories are in `.gitignore` — never commit build artifacts.
- Central package management is used (`Directory.Packages.props`) — specify package versions there, not in individual `.csproj` files.
- Assemblies are strong-named via `StrongName.snk`.
- The `examples/` project demonstrates API usage but has known test failures — do not attempt to fix them.
