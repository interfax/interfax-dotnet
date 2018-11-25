using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace InterFAX.Api
{
    public interface IFaxDocument
    {
        HttpContent ToHttpContent();
    }

    /// <summary>
    /// Represents a fax document referenced by a URI.
    /// </summary>
    public class UriDocument : IFaxDocument
    {
        public Uri FileUri { get; private set; }

        internal UriDocument(Uri fileUri)
        {
            if (fileUri == null)
                throw new ArgumentException("uri");

            FileUri = fileUri;
        }

        public HttpContent ToHttpContent()
        {
            var content = new ByteArrayContent(new byte[0]);
            content.Headers.ContentLocation = FileUri;
            content.Headers.ContentLength = 0;
            return content;
        }
    }

    /// <summary>
    /// Represents a fax document of a loaded byte array document
    /// </summary>
    public class FileDocumentArray : IFaxDocument
    {
        public byte[] File { get; private set; }
        public string MediaType { get; private set; }

        internal FileDocumentArray(byte[] file, string mediaType)
        {
            File = file;
            MediaType = mediaType;
        }

        public HttpContent ToHttpContent()
        {
            var content = new ByteArrayContent(File);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaType);
            return content;
        }
    }

    /// <summary>
    /// Represents a fax document from a local file.
    /// </summary>
    public class FileDocument : IFaxDocument
    {
        public string FilePath { get; private set; }
        public string MediaType { get; private set; }

        internal FileDocument(string filePath, string mediaType)
        {
            FilePath = filePath;
            MediaType = mediaType;
        }

        public HttpContent ToHttpContent()
        {
            var content = new ByteArrayContent(File.ReadAllBytes(FilePath));
            content.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaType);
            return content;
        }
    }


    /// <summary>
    /// Represents a fax document from a file stream.
    /// </summary>
    public class FileStreamDocument : IFaxDocument
    {
        public string FileName { get; private set; }
        public string MediaType { get; private set; }
        public FileStream FileStream { get; private set; }

        internal FileStreamDocument(string fileName, FileStream fileStream, string mediaType)
        {
            FileName = fileName;
            FileStream = fileStream;
            MediaType = mediaType;
        }

        public HttpContent ToHttpContent()
        {
            var fileBytes = new byte[FileStream.Length];
            FileStream.Read(fileBytes, 0, fileBytes.Length);
            var content = new ByteArrayContent(fileBytes);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaType);
            return content;
        }
    }    
}