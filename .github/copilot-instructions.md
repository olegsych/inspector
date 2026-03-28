# Copilot Instructions for Inspector

## Overview

Inspector is a .NET Reflection API library for white-box unit testing. It provides strongly-typed extension methods on `Object` and `Type` to access non-public members (constructors, fields, properties, methods, events) without breaking encapsulation. The main library targets `netstandard2.0` and `netstandard2.1`.

## Repository Layout

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
.github/instructions/        # Symlink → modules/csharp.common/ (doc instructions)
```

**Key source directories:**
- `src/` — Public API: `ObjectExtensions.cs`, `TypeExtensions.cs`, `Type.cs`, and member types (`Field.cs`, `Property.cs`, `Method.cs`, `Event.cs`, `Constructor.cs`) plus generic variants. `src/Implementation/` has internal helpers (filters, factories, selectors).
- `tst/` — One test file per source file. `tst/Implementation/` mirrors `src/Implementation/`.

## Prerequisites

**Always initialize submodules first.** The build depends on symlinked files from the `modules/csharp.common` submodule. Without it, `Directory.Build.props`, `Directory.Build.targets`, `.editorconfig`, and `StrongName.snk` are broken symlinks and the build will fail.

```shell
git submodule update --init --recursive
```

## Build, Test, and Pack

Always run commands from the repository root.

### Build (required first step)

```shell
dotnet build
```

Builds all 3 projects. Takes ~18 seconds. Expect 0 errors and ~46 nullable reference warnings in examples (pre-existing, safe to ignore). In Release configuration, `TreatWarningsAsErrors` is enabled.

### Test

```shell
dotnet test --no-build tst/Tests.csproj
```

Runs 385 tests. Always use `--no-build` after building to avoid redundant compilation. All tests should pass on all target frameworks (net8.0, net9.0, net10.0). To run tests for a specific framework:

```shell
dotnet test --no-build tst/Tests.csproj --framework net9.0
```

**Do not run** `dotnet test` against the full solution or `examples/Examples.csproj` — examples have pre-existing test failures unrelated to the library and are not tested in CI.

### Pack

```shell
dotnet pack src/Inspector.csproj
```

Creates NuGet packages in `out/packages/`.

## CI Workflow (.github/workflows/build.yml)

The CI build runs on every PR to `master` on both `windows-latest` and `ubuntu-latest`:

1. `dotnet build` — builds entire solution
2. `dotnet test --no-build tst/Tests.csproj --logger trx` — runs unit tests only
3. `dotnet pack src/Inspector.csproj` — creates NuGet packages

The CI checks out with `submodules: recursive` and `fetch-depth: 0` (full history required for Nerdbank.GitVersioning).

## Coding Conventions

- **C# style** is enforced by `.editorconfig` with many rules set to `error` severity. Key rules:
  - K&R brace style (new line before open brace only for types/namespaces)
  - No `this.` qualifier, no redundant `private` modifier, no braces for single-line blocks
  - Use `var` only when type is apparent; use explicit types elsewhere
  - Expression-bodied members when on single line
  - No space after `if`/`for`/`while` keywords before parenthesis
- **Nullable reference types** are enabled (`<Nullable>enable</Nullable>`)
- **C# language version**: `latest`
- **XML documentation**: Follow conventions in `.github/instructions/docs.instructions.md`
- **Test framework**: xUnit v3 with NSubstitute for mocking
- **Test naming**: Test classes mirror source classes with `Test` suffix; nested classes group related tests
- **Versioning**: Nerdbank.GitVersioning — do not manually edit assembly versions

## Important Notes

- The `out/`, `bin/`, and `obj/` directories are in `.gitignore` — never commit build artifacts.
- Central package management is used (`Directory.Packages.props`) — specify package versions there, not in individual `.csproj` files.
- Assemblies are strong-named via `StrongName.snk`.
- The `examples/` project demonstrates API usage but has known test failures — do not attempt to fix them.

Trust these instructions. Only search the codebase if information here is incomplete or found to be incorrect.
