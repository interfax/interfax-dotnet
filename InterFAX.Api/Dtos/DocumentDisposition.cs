namespace InterFAX.Api.Dtos
{
    /// <summary>
    /// The retention policy of an uploaded document.
    /// </summary>
    public enum DocumentDisposition
    {
        /// <summary>
        /// The document can only be used once.
        /// </summary>
        SingleUse,

        /// <summary>
        /// The document is deleted 60 minutes after the last usage.
        /// </summary>
        MultiUse,

        /// <summary>
        /// The document remains available until removed.
        /// </summary>
        Permanent
    }
}