using System;
using System.Runtime.Serialization;

namespace GameBasics.PlayerClasses {
    /// <summary>
    /// Progress tree element
    /// </summary>
    [Serializable]
    public abstract class ProgressElement : IRefreshable
    { // TODO not abstract progress element
        /// <summary>
        /// When progress element is ready, but someone sets <c>Researching</c> true
        /// </summary>
        [Serializable]
        public class ProgressPointsMaxException : Exception
        {
            public ProgressPointsMaxException() {}

            public ProgressPointsMaxException(string message) : base(message) {}

            public ProgressPointsMaxException(string message, Exception inner)
                : base(message, inner) {}

            protected ProgressPointsMaxException(SerializationInfo info, StreamingContext context)
                : base(info, context) {}
        }




        /// <summary>
        /// Unical permanent name of progress element
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// How many progress points does it need to be ready
        /// </summary>
        /// <value>The progress points max.</value>
        public abstract int ProgressPointsMax { get; }

        /// <summary>
        /// Progress element's owner
        /// </summary>
        public Player Owner { get; }

        /// <summary>
        /// Is this progress element a target of current research
        /// </summary>
        public bool Researching { 
            get {
                return _researching;
            }
            set {
                if (value && Ready)
                    throw new ProgressPointsMaxException();
                _researching = value;
            }
        }
        private bool _researching;

        /// <summary>
        /// Is it ready
        /// </summary>
        public bool Ready { 
            get { return ProgressPointsNow >= ProgressPointsMax; }
            set { 
                if (value)
                    _progressPointsNow = ProgressPointsMax;
                else
                    _progressPointsNow = _oldProgressPointsNow;
            }
        }

        /// <summary>
        /// How many progress points does it have now
        /// </summary>
        public int ProgressPointsNow { 
            get { return _progressPointsNow; } 
            set {
                _progressPointsNow = value;
                _oldProgressPointsNow = value;
            }
        }

        private int _progressPointsNow;
        private int _oldProgressPointsNow;




        public ProgressElement(Player owner, int progressPointsNow = 0)
        {
            Owner = owner;
            ProgressPointsNow = progressPointsNow;
        }




        /// <summary>
        /// Happens every <c>RefreshHelper.RefreshPeriodDays</c> days;
        /// adds ProgressElement bonuses.
        /// </summary>
        /// <remarks>
        /// ProgressPoints increase happens in ProgressGraph.
        /// </remarks>
        public abstract void Refresh(); 
    }
}

