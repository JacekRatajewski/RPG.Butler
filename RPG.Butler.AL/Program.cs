using Microsoft.Extensions.DependencyInjection;
using RPG.Butler.AL.Clients;
using System;
using System.Threading.Tasks;

namespace RPG.Butler.AL
{
    class Program
    {
        public static ButlerClient _butler;
        public static async Task Main()
        {
            try
            {
                Console.WriteLine("Initialization of Butler in progress...");
                await Init();
                Console.WriteLine("Initialization of Butler done.");
                await Repeat(Console.ReadLine());
            }
            catch (Exception ex)
            {
                await _butler.CloseAsync();
                Console.WriteLine(ex.GetBaseException().Message);
                Environment.Exit(-1);
            }
        }

        private static async Task Repeat(string input)
        {
            if (input.ToLower().Equals("exit"))
            {
                await _butler.CloseAsync();
            }
            else
            {
                Console.WriteLine("What ?");
                await Repeat(Console.ReadLine());
                Environment.Exit(-1);
            }
        }

        private static async Task Init()
        {
            var startup = new Startup();
            using (startup.Services as IDisposable)
            {
                await startup.RegisterCommands();
                _butler = startup.Services.GetService<ButlerClient>();
                await _butler.RunAsync();
            }
        }
    }
}
