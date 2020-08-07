namespace GDSoft.Utilities.PersonalizationManager
{
    /// <summary>
    /// Factory class for creating wrappers needed by personalization manager
    /// </summary>
    public class PersonalizationManagerUtilityFactory : IPersonalizationManagerUtilityFactory
    {
        public IConsoleWrapper CreateConsole => new ConsoleWrapper() as IConsoleWrapper;

        public IWindowsIdentityWrapper CreateIdentity => new WindowsIdentityWrapper();

        public ICredentialWrapper CreateCredential() => new CredentialWrapper();

    }
}
