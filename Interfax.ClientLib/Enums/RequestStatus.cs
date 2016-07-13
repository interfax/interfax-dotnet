namespace Interfax.ClientLib.Enums
{
    /// <summary>
    /// Status for requests
    /// </summary>
    public enum RequestStatus
    {
        /// <summary>
        /// Success
        /// </summary>
        OK,

        /// <summary>
        /// Resource created
        /// </summary>
        Created,

        /// <summary>
        /// Request accepted, but resource is not completed
        /// </summary>
        Accepted,

        /// <summary>
        /// Resource not found
        /// </summary>
        NotFound,

        /// <summary>
        /// System error
        /// </summary>
        SystemError,

        /// <summary>
        /// Request time out
        /// </summary>
        TimedOut,

        /// <summary>
        /// Authentication error
        /// </summary>
        AuthenticationError,

        /// <summary>
        /// Request was over limit(s)
        /// </summary>
        OverLimits,

        /// <summary>
        /// The request has some bad input.
        /// </summary>
        BadParameters
    }
}
