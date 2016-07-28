namespace GameBasics
{
    /// <summary>
    /// Inheritor changes something every <c>RefreshHelper.RefreshPeriodDays</c> days
    /// </summary>
    public interface IRefreshable
    {
        /// <summary>
        /// Happens every <c>RefreshHelper.RefreshPeriodDays</c> days
        /// </summary>
        void Refresh();
    }
}

