using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Test.Configurations
{
    public class ConfigLoader
    {
        public static string assemblyPath = Assembly.LoadFrom("EBird.Api.dll").Location;
        public static IConfiguration LoadConfig()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(assemblyPath))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            return builder.Build();
        }
    }
}
