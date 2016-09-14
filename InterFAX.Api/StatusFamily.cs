namespace InterFAX.Api
{
    /// <summary>
    /// Defines groups of statuses
    /// </summary>
    public enum StatusFamily
    {
        /// <summary>
        /// Any status code.
        /// </summary>
        All,

        /// <summary>
        /// Completed faxes, whether successful or failed.
        /// </summary>
        Completed,

        /// <summary>
        /// Successfully completed faxes.
        /// </summary>
        Success,

        /// <summary>
        /// Failed faxes.
        /// </summary>
        Failed,

        /// <summary>
        /// Faxes in process (not completed).
        /// </summary>
        Inprocess
    }
}