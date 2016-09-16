namespace InterFAX.Api.Dtos
{
    /// <summary>
    /// Document upload status
    /// </summary>
    public enum DocumentStatus
    {
        /// <summary>
        /// Upload session was started, but no data has been uploaded
        /// </summary>
        Created,

        /// <summary>
        /// Upload session was started, and some (not complete) data has been uploaded
        /// </summary>
        PartiallyUploaded,

        /// <summary>
        /// Data upload is in progress
        /// </summary>
        Uploading,

        /// <summary>
        /// Document is ready to be used
        /// </summary>
        Ready,

        /// <summary>
        /// Document is being deleted
        /// </summary>
        Deleting
    }
}