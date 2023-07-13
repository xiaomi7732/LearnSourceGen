using Microsoft.CodeAnalysis;

namespace LearnSourceGen;

[Generator]
public class RateLimitReasonSourceGenerator : EnumExtensionsSourceGenerator<RateLimitReason>
{
    protected override string ExtensionClassName => "RateLimitReasonExtensions";

    protected override string ExtensionNamespace => "LearnSourceGen";
}