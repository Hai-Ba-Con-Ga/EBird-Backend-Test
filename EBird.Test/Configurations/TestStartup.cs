using EBird.Api.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Test.Configurations
{
    public static class TestStartup
    {
        public static ServiceProvider CreateServices()
        {

            var builder = new ConfigurationBuilder()
             .SetBasePath(AppContext.BaseDirectory) // set the base path to the application directory
             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
             .AddJsonFile("", optional: true, reloadOnChange: true);
            var services = new ServiceCollection();

            // Add the services you want to use in your tests
            services.AddAppServices();
            

            return services.BuildServiceProvider();
        }
    }
}
