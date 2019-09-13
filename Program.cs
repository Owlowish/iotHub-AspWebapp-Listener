using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace IotSensorWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
         var config = new ConfigurationBuilder()
            .AddCommandLine(args)
            .Build();

        var host = new WebHostBuilder()
            .UseConfiguration(config)
            .UseSetting(WebHostDefaults.PreventHostingStartupKey, "true")
            .ConfigureLogging(factory =>
            {
                factory.AddConsole();
            })
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseEnvironment("Development")
            .UseStartup<Startup>()
            .Build();

         host.Run();
        }

    
    }
}
