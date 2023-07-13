using System.ComponentModel;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;

namespace LearnSourceGen;

[Generator]
public sealed class ProgramSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var compilationProvider = context.CompilationProvider;

        context.RegisterSourceOutput(
            compilationProvider,
            static (context, compilation) =>
        {
            // Get the entry point method
            var mainMethod = compilation.GetEntryPoint(context.CancellationToken);
            var typeName = mainMethod!.ContainingType.Name;

            StringBuilder dictionaryFiller = new();
            foreach (FieldInfo fieldInfo in typeof(RateLimitReason).GetFields())
            {
                DescriptionAttribute? description = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
                if (description != null)
                {
                    dictionaryFiller.AppendLine($@"_reasonDescriptions[(RateLimitReason){fieldInfo.GetRawConstantValue()!}] = ""{description.Description}"";");
                }
            }

            string source = $$"""
                // Auto-generated code
                namespace {{mainMethod.ContainingNamespace.ToDisplayString()}};
                
                public static partial class {{typeName}}
                {
                    static partial void InitializeRateLimitReasons()
                    {
                        {{dictionaryFiller.ToString()}}
                    }
                }                
                """;

            // Add the source code to the compilation
            context.AddSource($"{typeName}.g.cs", source);
        });
    }
}