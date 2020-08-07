using CredentialManagement;
using System;
using System.Security;

namespace GDSoft.Utilities.PersonalizationManager
{
    public class PersonalizationManager
    {
        IPersonalizationManagerUtilityFactory _factory;
        private IConsoleWrapper _console;
        private IWindowsIdentityWrapper _identity;

        public PersonalizationManager(IPersonalizationManagerUtilityFactory factory)
        {
            _factory = factory;
            _console = factory.CreateConsole;
            _identity = factory.CreateIdentity;
        }

        /// <summary>
        /// Set the credentials for the target in the windows credential manager
        /// </summary>
        /// <param name="target">target of the credentials</param>
        /// <returns>true - credentials set. false - failure to set credentials </returns>
        public bool SetCredentials(string target)
        {
            ICredentialWrapper cred = _factory.CreateCredential();
            cred.Target = target;
            cred.Username = _identity.Name;
            cred.SecurePassword = GetSecurePasswordFromConsole();
            cred.Type = CredentialType.Generic;

            return cred.Save();
        }

        /// <summary>
        /// Retrieve the credentials for the target from the windows credential manager
        /// </summary>
        /// <param name="target">target of the credentials</param>
        /// <returns>user name and password</returns>
        public (string user, SecureString password) GetCredentials(string target)
        {
            ICredentialWrapper cred = _factory.CreateCredential();
            cred.Target = target;

            if (!cred.Load())
            {
                return (user:null, password:null);
            }

            return (user: cred.Username, password: cred.SecurePassword);
        }

        /// <summary>
        /// Console input for the the user password
        /// </summary>
        /// <returns>SecureString containing the user password</returns>
        internal SecureString GetSecurePasswordFromConsole()
        {
            SecureString securePass = new SecureString();
            _console.Write("Password: ");
            do
            {
                ConsoleKeyInfo key = _console.ReadKey(true);
                // Backspace Should Not Work
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    securePass.AppendChar(key.KeyChar);
                    _console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && securePass.Length > 0)
                    {
                        securePass.RemoveAt(securePass.Length - 1);
                        _console.Write("\b \b");
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                }
            } while (true);

            return securePass;
        }
    }
}
