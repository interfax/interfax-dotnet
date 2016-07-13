namespace Interfax.ClientLib.Entities
{
    /// <summary>
    /// Represents detailed error reporting block
    /// </summary>
    public class ErrorBlock
    {
        /// <summary>
        /// Code as returned from service
        /// </summary>
        public int Code;

        /// <summary>
        /// The base error message
        /// </summary>
        public string Message;

        /// <summary>
        /// Detailed information about the error
        /// </summary>
        public string MoreInfo;
    }
}
