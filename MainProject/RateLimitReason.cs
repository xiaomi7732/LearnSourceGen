using System.ComponentModel;

namespace LearnSourceGen;

public enum RateLimitReason
{
    [Description("No throttling by the rate limit.")]
    NoThrottling,
    [Description("Reached the quota for anonymous artifacts.")]
    AnonymousQuota,
}