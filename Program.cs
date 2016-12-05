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
            Task task = Task.Run(
                    () => DbAccess.AnnotationsList); //Purposeless access, just to start the database on a different thread
            
            var pfxFile = Path.Combine("./1.devices.quickbird.uk.pfx");
            X509Certificate2 certificate = new X509Certificate2(pfxFile, "donkeyballs");

            var host = new WebHostBuilder()
                .UseKestrel(options => 
                { 
                    options.UseHttps(certificate); 
                }) 
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseUrls("https://*:433","http://*:80") //Can set it to localhost for internal use only
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
