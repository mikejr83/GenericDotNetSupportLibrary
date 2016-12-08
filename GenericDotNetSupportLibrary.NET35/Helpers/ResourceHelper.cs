using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace GenericDotNetSupportLibrary.Helpers
{
    /// <summary>
    /// Helper methods for working with resource files
    /// </summary>
    public static class ResourceHelper
    {
        /// <summary>
        /// Gets text from an embedded resource
        /// </summary>
        /// <param name="path">The path to the embedded resource</param>
        /// <param name="assembly">The assembly containing the embedded resource.<remarks>If this value is null the calling assembly is used.</remarks></param>
        /// <returns>The string read from the resource.</returns>
        public static string GetTextFromEmbeddedResource(string path, Assembly assembly = null)
        {
            string contents = null;

            Stream fileStream = null;
            if (assembly == null)
                fileStream = Assembly.GetCallingAssembly().GetManifestResourceStream(path);
            else
                fileStream = assembly.GetManifestResourceStream(path);

            StreamReader reader = new StreamReader(fileStream);

            contents = reader.ReadToEnd();

            return contents;
        }
    }
}
