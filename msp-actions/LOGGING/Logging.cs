using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace msp_actions.LOGGING
{
    public class Logging
    {
        public static void Log(object? actionYouAreDoing, object? ForWhatOBJ, object? idEmailOrUsersTag, string? Result)
        {
            Console.WriteLine(
                _break +
                $"{_ToString(actionYouAreDoing).ToUpper() ?? "NULL"} {_ToString(ForWhatOBJ).ToUpper() ?? "NULL"} as '{_ToString(idEmailOrUsersTag).ToUpper() ?? "NULL"}' is '{Result?.ToUpper() ?? "NULL"}' at {DateTime.UtcNow.AddHours(-4)}" +
                _break
            );
        }
        public static string _ToString(Object? o)
        {
            if (o != null)
            {
                return JsonConvert.SerializeObject(o);
            }
            else
            {
                return "NULL OBJ";
            }
        }

        public static string _break = "\n\t\t====================================\n";
    }
}
