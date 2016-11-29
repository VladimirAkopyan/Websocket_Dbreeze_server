using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.Security.Cryptography.X509Certificates;

namespace EdisonBrick
{
    public class Program
    {
        public static void Main(string[] args)
        {
            X509Certificate2 cert = new X509Certificate2("YourFileName.pfx",
    "YOURPWD");
            var host = new WebHostBuilder()
                .UseKestrel(cfg => cfg.UseHttps('www_quickbird_uk.crt',))
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
