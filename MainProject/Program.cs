namespace LearnSourceGen;
partial class Program
{
    static void Main(string[] args)
    {
        foreach (RateLimitReason reason in Enum.GetValues<RateLimitReason>())
        {
            Console.WriteLine("{0} - {1}", reason, reason.GetDescription());
        }
    }
}