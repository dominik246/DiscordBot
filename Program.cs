using System;

namespace DiscordBot
{
    public class Program
    {
        public static void Main(string[] args)
            => new ProgramAsync().MainAsync().GetAwaiter().GetResult();
    }
}
