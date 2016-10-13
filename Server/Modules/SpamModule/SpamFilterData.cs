using System;
using System.Collections.Generic;
using System.Net;

namespace Isometric.Server.Modules.SpamModule
{
    public class SpamFilterData
    {
        internal SpamFilterData() {}



        public List<IPAddress> BannedIPs { get; } = new List<IPAddress>();

        public float 
            ReputationStepSuccessful = 0.1f,
            ReputationStepUnsuccessful = -0.2f,
            ReputationStepSpam = -0.5f,

            ReputationLevelBan = -5f,
            ReputationMaximal = 3f;

        public TimeSpan 
            DefaultBanTime = new TimeSpan(1, 0, 0);
    }
}

