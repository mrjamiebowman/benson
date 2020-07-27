using System;
using System.Collections.Generic;
using System.Text;
using BensonCLI.Enums;

namespace BensonCLI.Helpers
{
    public static class ConsoleOutput
    {
        public static void WriteLine(string message, ConsoleStatusType status = ConsoleStatusType.Normal)
        {
            if (status != ConsoleStatusType.Normal)
            {
                if (status == ConsoleStatusType.Positive)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("[+]");
                }
                else if (status == ConsoleStatusType.Warning)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("[?]");
                }
                else if (status == ConsoleStatusType.Danger)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("[-]");
                }
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($" {message}");
        }
    }
}
