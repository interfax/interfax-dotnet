namespace Interfax.ClientLib.Enums
{
    /// <summary>
    /// Defines groups of statuses
    /// </summary>
    public enum StatusFamily
    {
        /// <summary>
        /// any status
        /// </summary>
        All,

        /// <summary>
        /// A specific status
        /// </summary>
        Specific,

        /// <summary>
        /// Completed faxes, whether successful or failed
        /// </summary>
        Completed,

        /// <summary>
        /// successfully-completed faxes
        /// </summary>
        Success,

        /// <summary>
        /// failed faxes
        /// </summary>
        Failed,

        /// <summary>
        /// faxes in process (not completed)
        /// </summary>
        Inprocess
    }
}
