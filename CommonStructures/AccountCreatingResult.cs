namespace CommonStructures
{
    public enum AccountCreatingResult
    {
        WrongLogin, // FIXME Unity regex checking before sending login
        ExistingLogin,
        ExistingEmail,
        Successful,
    }
}

