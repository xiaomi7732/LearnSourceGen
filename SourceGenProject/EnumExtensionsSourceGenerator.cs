using System.ComponentModel;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;

namespace LearnSourceGen;

public abstract class EnumExtensionsSourceGenerator<T> : IIncrementalGenerator
    where T : struct, IConvertible
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var compilationProvider = context.CompilationProvider;

        context.RegisterSourceOutput(
            compilationProvider,
            (context, compilation) =>
        {
            string typeName = ExtensionClassName;

            StringBuilder dictionaryFiller = new();
            foreach (FieldInfo fieldInfo in typeof(T).GetFields())
            {
                DescriptionAttribute? description = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
                if (description != null)
                {
                    dictionaryFiller.AppendLine($"""{MappingDictionaryFieldName}[({typeof(T).Name}){fieldInfo.GetRawConstantValue()!}] = "{description.Description}";""");
                }
            }

            string source = $$"""
                // Auto-generated code
                namespace {{ExtensionNamespace}};
                
                public static partial class {{typeName}}
                {
                    static partial void {{InitializationMethodName}}()
                    {
                        {{dictionaryFiller.ToString()}}
                    }
                }                
                """;

            // Add the source code to the compilation
            context.AddSource($"{typeName}.g.cs", source);
        });
    }

    protected abstract string ExtensionClassName { get; }

    protected abstract string ExtensionNamespace { get; }

    protected virtual string InitializationMethodName { get; } = "InitializeDescriptionDictionary";

    protected virtual string MappingDictionaryFieldName { get; } = "_descriptionMap";
}