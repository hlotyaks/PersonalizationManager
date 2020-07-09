using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.PersonalizationManager
{
    /// <summary>
    /// Wrapper for windowsidentity class.  Useful for unit testing
    /// </summary>
    internal class WindowsIdentityWrapper : IWindowsIdentityWrapper
    {
        System.Security.Principal.WindowsIdentity _identity;

        public WindowsIdentityWrapper()
        {
            _identity = System.Security.Principal.WindowsIdentity.GetCurrent();
        }

        public string Name => _identity.Name;
    }
}
