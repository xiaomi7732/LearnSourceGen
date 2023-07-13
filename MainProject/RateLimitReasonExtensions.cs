namespace LearnSourceGen;

public static partial class RateLimitReasonExtensions
{
    private static readonly IDictionary<RateLimitReason, string> _descriptionMap = new Dictionary<RateLimitReason, string>();
    static RateLimitReasonExtensions()
    {
        InitializeDescriptionDictionary();
    }

    static partial void InitializeDescriptionDictionary();

    public static string GetDescription(this RateLimitReason key, string? fallback = null)
    {
        if (_descriptionMap.TryGetValue(key, out string? description))
        {
            return description;
        }
        return fallback ?? key.ToString();
    }
}