# Learn how to use Source Generator

This is an example to use source generator to provide descriptions for enums at the build time. Compare to reflection, the main benefits is that this saves start up time.

## Source Generator Basics

Source generator lives in its own project and runs in a specific phase in compilation. Refer to [Source Generators
](https://learn.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/source-generators-overview) for more details.

Here's a basic usage:

1. Create a generator project, for example: [LearnSourceGen.Generator.csproj](./SourceGenProject/LearnSourceGen.Generator.csproj). A reference to this package is needed to access the interfaces:

    ```xml
    <ItemGroup>
        <PackageReference Include="Microsoft.CodeAnalysis.CSharp.Workspaces" Version="4.0.1" PrivateAssets="all" />
    </ItemGroup>
    ```

1. Create a class to implement the interface of `IIncrementalGenerator`. For an example, refer to [SourceGenProject/EnumExtensionsSourceGenerator.cs](./SourceGenProject/EnumExtensionsSourceGenerator.cs).

1. Add a `[GeneratorAttribute]` to the class, or in our case, the inherit class. For example, [SourceGenProject/RateLimitReasonSourceGenerator.cs](./SourceGenProject/RateLimitReasonSourceGenerator.cs).

1. Note, to avoid circular dependency, the generator project can't dependent on the target project. For an easy access the property info, `RateLimitReason` is added as a linked file in the generator, see [SourceGenProject/LearnSourceGen.Generator.csproj](./SourceGenProject/LearnSourceGen.Generator.csproj) for details.

1. Add reference from the consumer project (the one that the source file is generated into) to the generator project, WITH some special attributes. For example:

    ```xml
    <ItemGroup>
        <ProjectReference Include="..\SourceGenProject\LearnSourceGen.Generator.csproj" OutputItemType="Analyzer" ReferenceOutputAssembly="false" />
    </ItemGroup>
    ```
    1. The `OutputItemType` should be `Analyzer`.
    2. The `ReferenceOutputAssembly` should be `false`.

    See [MainProject/LearnSourceGen.csproj](./MainProject/LearnSourceGen.csproj) for an example.

    3. Optionally, keep the generated class on disk for debugging:

        ```xml
        <EmitCompilerGeneratedFiles Condition=" '$(Configuration)' == 'Debug' ">true</EmitCompilerGeneratedFiles>
        ```
    
        Find the output in obj folder like `obj/Debug/net7.0/generated/LearnSourceGen.Generator/LearnSourceGen.RateLimitReasonSourceGenerator/RateLimitReasonExtensions.g.cs`

## Caveats

Generated file will not reflect the source immediately after the first build. To clear the cache, run this before build:

```shell
dotnet build-server shutdown
```
