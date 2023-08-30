using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace msp_actions.Action_Config
{
    public class Action_Config
    {
        public Action_Config(IHostingEnvironment h) {
            environment = h;
        }
        public IHostingEnvironment environment;
    }

    public static class ConfigManager
    {
        public static IConfiguration AppSetting { get; }

        static ConfigManager()
        {
#if DEBUG
            LOGGING.Logging.Log("CHECKING VARIABLES", "DEBUGGER", null, "Debug build");
            AppSetting = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.development.json")
                .Build();
#else
                    LOGGING.Logging.Log("CHECKING VARIABLES","DEBUGGER",null, "Production build");
                    AppSetting = new ConfigurationBuilder()
                        .SetBasePath(Environment.CurrentDirectory)
                        .AddJsonFile("appsettings.json")
                        .Build();
#endif

        }
    }
}
