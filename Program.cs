using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace appinsightsloggerpatterntest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Console with App Settings Using LoggerMessage Pattern");

            Startup.Initialise();

        }
    }

}
