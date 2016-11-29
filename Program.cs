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
                    () => 
                        DbAccess.AddAnnotation(
                            new Repository.Annotation(DateTime.UtcNow, "Test", "Test", string.Empty)
                            )
                ); 

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
