using System;
using System.IO;

namespace Interfax.ClientLib.Entities
{
    /// <summary>
    /// Represents a single document to be faxes
    /// </summary>
    public abstract class OutboundDocument
    {
    }

    /// <summary>
    /// Represents an already-uploaded document to be faxed
    /// </summary>
    public class LinkedOutboundDocument : OutboundDocument
    {
        /// <summary>
        /// The uri of the uploaded document as returned by calling Upload() method
        /// </summary>
        public Uri UploadedDocument; 
    }

    /// <summary>
    /// Represents a single document with inline content to be faxes
    /// </summary>
    public class InlineOutboundDocument : OutboundDocument
    {

        private string _fileType;
        private string _charSet;
        private byte[] _data;

        /// <summary>
        /// Constructor: Initialize with a file
        /// </summary>
        /// <param name="path">The fully qualified path to the file containing the document to be faxed</param>
        /// <param name="charSet">In case of a text (e.g Html), this should be specified</param>
        public InlineOutboundDocument(string path, string charSet = null)
            : this(File.ReadAllBytes(path), Path.GetExtension(path).TrimStart('.'), charSet)
        { }

        /// <summary>
        /// Constructor: Initialize with a Stream
        /// </summary>
        /// <param name="dataStream">The IO stream containing the document to be faxed</param>
        /// <param name="fileType">The type of the document to be faxed (e.g 'pdf')</param>
        /// <param name="closeStream">Optional (default=true), tells the method whether to close the stream after using its data</param>
        /// <param name="charSet">In case of a text (e.g Html), this should be specified</param>
        public InlineOutboundDocument(Stream dataStream, string fileType, bool closeStream = false, string charSet = null) : 
            this(StreamToByteArray(dataStream,closeStream), fileType, charSet)
        { }

        private static byte[] StreamToByteArray(Stream dataStream, bool closeStream = false)
        {
            var data = new byte[dataStream.Length - dataStream.Position];
            dataStream.Read(data, 0, data.Length);
            if (closeStream)
                dataStream.Close();
            return data;
        }


        /// <summary>
        /// Constructor: Initialize with a byte array
        /// </summary>
        /// <param name="data">The byte-array containing the document to be faxed</param>
        /// <param name="fileType">The type of the document to be faxed (e.g 'pdf')</param>
        /// <param name="charSet">In case of a text (e.g Html), this should be specified</param>
        public InlineOutboundDocument(byte[] data, string fileType, string charSet = null)
        {
            _data = data;
            _fileType = fileType;
            _charSet = charSet;
        }

        /// <summary>
        /// The raw data 
        /// </summary>
        public byte[] Data { get { return _data; } }


        /// <summary>
        /// The type of the document to be faxed.
        /// The File type must be in the list of supported file types as specifies in https://www.interfax.net/en/help/supported_file_types.
        /// </summary>
        public string FileType { get { return _fileType; } }

        /// <summary>
        /// The character set encoding (applies in case of textual content)
        /// </summary>
        public string CharSet { get { return _charSet; } }
    }
}
