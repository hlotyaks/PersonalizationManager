using System;

namespace Utilities.PersonalizationManager
{
    public interface IConsoleWrapper
    {
        ConsoleKeyInfo ReadKey(bool intercept);

        void Write(string value);

    }
}