using System;

namespace ServiceCenterProvider
{
    class Output
    {
        public static void RedLine(string Message)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(Message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void GreenLine(string Message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(Message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void BlueLine(string Message)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(Message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void DarkLine(string Message)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(Message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
