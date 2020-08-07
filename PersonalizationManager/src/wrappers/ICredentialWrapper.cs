using CredentialManagement;
using System.Security;

namespace GDSoft.Utilities.PersonalizationManager
{
    public interface ICredentialWrapper
    {
        bool Save();

        bool Load();

        string Username { get; set; }

        SecureString SecurePassword { get; set; }

        string Password { get; set; }

        CredentialType Type { get; set; }

        string Target { get; set; }

        object Object { get; }
    }
}