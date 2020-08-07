namespace GDSoft.Utilities.PersonalizationManager
{
    /// <summary>
    /// Factory interface
    /// </summary>
    public interface IPersonalizationManagerUtilityFactory
    {
        IConsoleWrapper CreateConsole { get; }

        IWindowsIdentityWrapper CreateIdentity { get; }

        ICredentialWrapper CreateCredential();
    }
}