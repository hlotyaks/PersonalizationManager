using System;

namespace GDSoft.Utilities.PersonalizationManager
{
    /// <summary>
    /// Wrapper for console.  Useful for unit testing
    /// </summary>
    internal class ConsoleWrapper : IConsoleWrapper
    {
        public ConsoleKeyInfo ReadKey(bool intercept)
        {
            return Console.ReadKey(true);
        }

        public void Write(string value)
        {
            Console.Write(value);
        }
    }
}
