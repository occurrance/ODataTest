using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace OdataTest
{
    /// <summary>
    /// Startup Class for OdataTest
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main Entrypoint for OdataTest
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {

            var i = -1;
            var host = new WebHostBuilder()            
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .ConfigureLogging((hostingContext, logging) =>
                {
                    // Requires `using Microsoft.Extensions.Logging;`
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();

                    logging.AddFilter("Microsoft", LogLevel.Trace);
                    //logging.AddFilter("System", LogLevel.Debug);
                    //logging.ClearProviders();
                    //logging.AddConsole();
                    //logging.AddEventSourceLogger();
                })
                .Build();

            host.Run();
        }
    }
}
