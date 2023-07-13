namespace LearnSourceGen;
partial class Program
{
    private static readonly IDictionary<RateLimitReason, string> _reasonDescriptions = new Dictionary<RateLimitReason, string>();
    static void Main(string[] args)
    {
        InitializeRateLimitReasons();

        foreach (RateLimitReason reason in Enum.GetValues<RateLimitReason>())
        {
            Console.WriteLine("{0} - {1}", reason, _reasonDescriptions[reason]);
        }
    }

    static partial void InitializeRateLimitReasons();
    // static void InitializeRateLimitReasons()
    // {
    //     foreach (FieldInfo fieldInfo in typeof(RateLimitReason).GetFields())
    //     {
    //         DescriptionAttribute? description = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
    //         if (description != null)
    //         {
    //             _reasonDescriptions[(RateLimitReason)fieldInfo.GetRawConstantValue()!] = description.Description;
    //         }
    //     }
    // }
}