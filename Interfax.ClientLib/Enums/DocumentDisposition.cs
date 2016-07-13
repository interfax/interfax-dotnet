namespace Interfax.ClientLib.Enums
{
    /// <summary>
    /// Upload documents disposition
    /// </summary>
    public enum DocumentDisposition
    {
        /// <summary>
        /// can be used once
        /// </summary>
        SingleUse,

        /// <summary>
        /// deleted 60 minutes after the last usage 
        /// </summary>
        MultiUse,

        /// <summary>
        /// remains available until specifically removed by user
        /// </summary>
        Permanent
    }
}
