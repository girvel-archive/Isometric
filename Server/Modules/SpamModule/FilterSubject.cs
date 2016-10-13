using System;
using System.Net.Sockets;

namespace Isometric.Server.Modules.SpamModule
{
    public class FilterSubject
    {
        // TODO filter spam by ip

        #region Data singleton

        [Obsolete("using backing field")]
        private static SpamFilterData _data;

        #pragma warning disable 0618

        public static SpamFilterData Data
        {
            get { return _data ?? (_data = new SpamFilterData()); }

            set
            {
                #if DEBUG
                if (_data != null)
                {
                    throw new ArgumentException("Data is already set");
                }
                #endif

                _data = value;
            }
        }

        #pragma warning restore 0618

        #endregion



        public Account Account { get; }

        public double Reputation { get; set; }



        /// <summary>
        /// Analyzes result of last command execution and changes account reputation
        /// </summary>
        public bool Add(CommandResult result, Socket socket)
        {
            var step = 0f;
            switch (result)
            {
                case CommandResult.Successful:
                    step = Data.ReputationStepSuccessful;
                    break;

                case CommandResult.Unsuccessful:
                    step = Data.ReputationStepUnsuccessful;
                    break;

                case CommandResult.Spam:
                    step = Data.ReputationStepSpam;
                    break;

                default:
                    throw new NotImplementedException();
            }

            Reputation = Math.Min(Data.ReputationMaximal, Reputation + step);

            if (Reputation + step <= Data.ReputationLevelBan)
            {
                Account.Ban(Data.DefaultBanTime);
                return true;
            }

            return false;
        }
    }
}

