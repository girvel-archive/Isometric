namespace Isometric.CommonStructures
{
    public enum AccountCreatingResult
    {
        WrongLogin, // TODO F Unity regex checking before sending login
        ExistingLogin,
        ExistingEmail,
        Successful,
    }
}

