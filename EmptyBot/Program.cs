﻿// Generated with Bot Builder V4 SDK Template for Visual Studio EmptyBot v4.22.0

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace EmptyBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
