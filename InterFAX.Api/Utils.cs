using System;
using System.IO;
using System.Text;

namespace InterFAX.Api
{
    public static class Utils
    {
        /// <summary>
        /// Encodes a string in Base64 (primarily for basic authentication)
        /// </summary>
        /// <param name="toEncode">The string to encode.</param>
        /// <returns></returns>
        public static string Base64Encode(string toEncode)
        {
            var authBytes = Encoding.UTF8.GetBytes(toEncode);
            return Convert.ToBase64String(authBytes);
        }

        /// <summary>
        /// Copies the contents of input to output. Doesn't close either stream.
        /// </summary>
        public static void CopyStream(Stream input, Stream output)
        {
            var buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }
    }
}