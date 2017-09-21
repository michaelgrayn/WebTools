// WebApplication.Program.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace WebApplication
{
    using System.IO;
    using Microsoft.AspNetCore.Hosting;

    public static class Program
    {
        public static void Main()
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();

            host.Run();
        }
    }
}
