using System;

namespace VisualServer.Modules.SpamModule
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

