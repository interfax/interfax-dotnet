using System;
using System.Collections.Generic;
using System.Xml;

namespace Interfax.ClientLib.Utils
{
    /// <summary>
    /// Helper - get media type by file type
    /// </summary>
    public class MediaTypeFinder
    {
        private static Dictionary<string, string> _mapping = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        /// Static constructor - load xml into a dictionary
        /// </summary>
        static MediaTypeFinder()
        {
            var doc = new XmlDocument();
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var st = assembly.GetManifestResourceStream(assembly.GetName().Name + ".Utils.MediaTypes.xml");
            doc.Load(st);

            var mappings = doc.GetElementsByTagName("MediaTypeMapping");

            //iterate all and load to dictionary
            foreach (XmlNode node in mappings)
            {
                var element = (XmlElement)node;
                var fileType = element.GetElementsByTagName("FileType")[0].InnerText;
                if (_mapping.ContainsKey(fileType))
                    continue; // ignore duplicates
                var mediaType = element.GetElementsByTagName("MediaType")[0].InnerText;
                _mapping.Add(fileType, mediaType);
            }

        }

        /// <summary>
        /// Get media type by file type
        /// </summary>
        /// <param name="fileType">file type</param>
        /// <returns>media type</returns>
        public static string GetMediaType(string fileType)
        {
            if (_mapping.ContainsKey(fileType))
                return _mapping[fileType];
            return "application/octet-stream"; // default for any unknown file type
        }
    }
}
