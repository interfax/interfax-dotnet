namespace InterFAX.Api.Dtos
{
    /// <summary>
    /// An error response returned by some of the REST API methods.
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Code as returned from service.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// The base error message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Detailed information about the error.
        /// </summary>
        public string MoreInfo { get; set; }
        
        public override string ToString()
        {
            return $"Code : [{Code}], Message : [{Message}], MoreInfo : [{MoreInfo}]";
        }
    }
}