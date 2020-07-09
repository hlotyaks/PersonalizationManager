using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Utilities.PersonalizationManager;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            IPersonalizationManagerUtilityFactory factory = new PersonalizationManagerUtilityFactory();
            PersonalizationManager pmanager = new PersonalizationManager(factory);

            bool status = pmanager.SetCredentials("ConsoleApp");

            (string user, SecureString password) = pmanager.GetCredentials("ConsoleApp");

            var targetBstr = Marshal.SecureStringToBSTR(password);
            var targetString = Marshal.PtrToStringBSTR(targetBstr);
        }
    }
}
