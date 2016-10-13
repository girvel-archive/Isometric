namespace Isometric.Server.Modules.SpamModule
{
    public enum FilterResult
    {
        Normal,
        SpamError,
        SpamSessionEnd,
        SpamBan,
        SpamPermanentBan,
    }
}

