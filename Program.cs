using System;

/*******************************************************************************
** This sample utilises the LoggerMessage pattern for high performance logging
** See: https://docs.microsoft.com/en-us/dotnet/core/extensions/high-performance-logging
**
*******************************************************************************/
namespace appinhsightsplay
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Console with App Settings Using LoggerMessage Pattern");

            Startup.Initialise();
        }
    }

}
