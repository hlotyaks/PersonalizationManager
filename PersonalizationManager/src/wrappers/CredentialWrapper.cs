using CredentialManagement;
using System;
using System.Collections.Generic;
using System.Security;


namespace GDSoft.Utilities.PersonalizationManager
{
    /// <summary>
    /// Wrapper for credential. Useful for unit testing
    /// </summary>
    internal class CredentialWrapper : ICredentialWrapper
    {
        Credential _cred;

        public CredentialWrapper()
        {
            _cred = new Credential();
        }

        public bool Save() => _cred.Save();

        public bool Load() => _cred.Load();

        public string Username { get => _cred.Username; set => _cred.Username = value; }

        public SecureString SecurePassword { get => _cred.SecurePassword; set => _cred.SecurePassword = value; }

        public string Password { get => _cred.Password; set => _cred.Password = value; }

        public CredentialType Type { get => _cred.Type; set => _cred.Type = value; }

        public string Target { get => _cred.Target; set => _cred.Target = value; }

        public object Object => _cred as object;

    }
}
