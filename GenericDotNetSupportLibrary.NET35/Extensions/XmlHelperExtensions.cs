using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using GenericDotNetSupportLibrary.Helpers;

namespace GenericDotNetSupportLibrary.Extensions
{
  /// <summary>
  /// Extension methods for xml helper utilities.
  /// </summary>
  public static class XmlHelperExtensions
  {
    /// <summary>
    /// Converts the base64 encoded string to its xml representation.
    /// </summary>
    /// <param name="base64EncodedString"></param>
    /// <param name="preserveWhiteSpace"></param>
    /// <returns></returns>
    public static XmlDocument ConvertBase64ToXmlDoc(this string base64EncodedString, bool preserveWhiteSpace)
    {
      return XmlHelper.ConvertBase64ToXmlDoc(base64EncodedString, preserveWhiteSpace);
    }
  }
}
