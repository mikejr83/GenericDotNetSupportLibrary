using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml.XPath;
using System.Net;
using System.Xml.Xsl;
using System.Collections.Specialized;
using System.Xml.Linq;
using System.Xml.Schema;

namespace GenericDotNetSupportLibrary.Helpers
{
  /// <summary>
  /// Helper class for XML functions.
  /// </summary>
  public static class XmlHelper
  {
    /// <summary>
    /// Takes a string of XML, formats the XML using an XmlWriter, and saves the result to disk.
    /// </summary>
    /// <param name="documentAsText">XML string that represents the document.</param>
    /// <param name="outputFilename">The filename where the XML will be saved.</param>
    /// <param name="newLineOnAttributes">Separate each attribute on a new line. <remarks>This defaults to true.</remarks></param>
    public static void SaveFormattedXmlDocument(string documentAsText, string outputFilename, bool newLineOnAttributes = true)
    {
      XmlDocument doc = new XmlDocument();
      doc.LoadXml(documentAsText);
      XmlWriterSettings settings = new XmlWriterSettings();
      settings.Indent = true;
      settings.NewLineOnAttributes = newLineOnAttributes;
      XmlWriter wr = XmlWriter.Create(outputFilename, settings);
      doc.Save(wr);
      wr.Close();
    }

    /// <summary>
    /// Loads an existing XML document, formats the XML using an XmlWriter, and saves the result to disk.
    /// </summary>
    /// <param name="outputFilename">The file that contains the XML and will be overwritten with the formatted XML.</param>
    /// <param name="newLineOnAttributes">Separate each attribute on a new line. <remarks>This defaults to true.</remarks></param>
    public static void SaveFormattedXmlDocument(string outputFilename, bool newLineOnAttributes = true)
    {
      XmlDocument doc = new XmlDocument();
      doc.Load(outputFilename);
      XmlWriterSettings settings = new XmlWriterSettings();
      settings.Indent = true;
      settings.NewLineOnAttributes = newLineOnAttributes;
      XmlWriter wr = XmlWriter.Create(outputFilename, settings);
      doc.Save(wr);
      wr.Close();
    }

    /// <summary>
    /// Formats XML with indents for displaying visually to users.
    /// </summary>
    /// <param name="documentAsText">XML string that represents the document.</param>
    /// <param name="newLineOnAttributes">Separate each attribute on a new line. <remarks>This defaults to true.</remarks></param>
    /// <returns></returns>
    public static string FormatXml(string documentAsText, bool newLineOnAttributes = true)
    {
      string result = "";

      StringBuilder output = new StringBuilder();
      XmlWriterSettings settings = new XmlWriterSettings();
      settings.Indent = true;
      settings.NewLineOnAttributes = newLineOnAttributes;
      XmlWriter writer = XmlWriter.Create(output, settings);
      XmlDocument document = new XmlDocument();

      try
      {
        // Load the XmlDocument with the XML.
        document.LoadXml(documentAsText);

        // Write the XML into a formatting XmlTextWriter
        document.WriteContentTo(writer);
        writer.Flush();

        // Extract the text from the StreamReader.
        string formattedXML = output.ToString();

        result = formattedXML;
      }
      catch (XmlException)
      {
      }
      finally
      {
        writer.Close();
      }

      return result;
    }

    public static bool ValidateXMLFiles(string xmlFilename, string xsdFilename, out string status)
    {
      if (!File.Exists(xmlFilename))
        throw new FileNotFoundException("Unable to find XML file to validate.", xmlFilename);
      if (!File.Exists(xsdFilename))
        throw new FileNotFoundException("Unable to find schema document.", xsdFilename);

      return ValidateXML(File.ReadAllText(xmlFilename), File.ReadAllText(xsdFilename), out status);
    }

    public static bool ValidateXML(string xml, string schema, out string status)
    {
      XDocument z = XDocument.Parse(schema);
      var result = z.Root.Attributes().
              Where(a => a.IsNamespaceDeclaration).
              GroupBy(a => a.Name.Namespace == XNamespace.None ? String.Empty : a.Name.LocalName,
                      a => XNamespace.Get(a.Value)).
              ToDictionary(g => g.Key,
                           g => g.First());

      XmlReader xmlReader = XmlReader.Create(new StringReader(schema));
      XmlSchemaSet schemas = new XmlSchemaSet();
      schemas.Add(result[string.Empty].ToString(), xmlReader);

      XDocument doc = XDocument.Parse(xml);
      string validateStatus = "Validated successfully";
      StringBuilder failBuilder = new StringBuilder();
      bool success = true;
      doc.Validate(schemas, (o, e) =>
      {
        failBuilder.Append(e.Message);
        failBuilder.AppendLine();

        success = false;
      });

      status = success ? validateStatus : failBuilder.ToString();

      return success;
    }

    /// <summary>
    /// Loads and returns the XDocument from the specified location.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static XDocument LoadXDocument(string filePath)
    {
      if (!File.Exists(filePath))
        throw new ArgumentException("There is no file at location: " + filePath);

      return XDocument.Load(filePath);
    }

    /// <summary>
    /// Creates an XmlReader from the xmlDoc.
    /// </summary>
    /// <param name="xmlDoc"></param>
    /// <returns></returns>
    public static XmlReader CreateReader(XmlDocument xmlDoc)
    {
      XmlNodeReader nodeReader = new XmlNodeReader(xmlDoc);
      XmlReaderSettings readerSettings = new XmlReaderSettings();
      return XmlReader.Create(nodeReader, readerSettings);
    }

    #region XSD Related
    /// <summary>
    /// Loads the xsd from the specified location.
    /// </summary>
    /// <param name="xsdPath"></param>
    /// <returns></returns>
    public static XmlSchema LoadXsd(string xsdPath)
    {
      if (!File.Exists(xsdPath))
        throw new ArgumentException("There is no file at location: " + xsdPath);

      return XmlSchema.Read(XmlReader.Create(xsdPath), null);
    }

    /// <summary>
    /// Validates the schema and returns true if the schema is valid, else false, and the errors property 
    /// will be filled with schema validation error messages.
    /// </summary>
    /// <param name="xmlDataToValidate"></param>
    /// <param name="schema"></param>
    /// <param name="errors"></param>
    /// <returns></returns>
    public static bool ValidateAgainstSchema(XDocument xmlDataToValidate, XmlSchema schema, out StringBuilder errors)
    {
      bool isValid = false;
      errors = ValidateAgainstSchema(out isValid, xmlDataToValidate, schema);
      return isValid;
    }

    /// <summary>
    /// Validates the xml and returns a stringbuilder that contains the errors if any.
    /// </summary>
    /// <param name="errorInValidation"></param>
    /// <param name="xmlDataToValidate"></param>
    /// <param name="schema"></param>
    /// <returns></returns>
    public static StringBuilder ValidateAgainstSchema(out bool errorInValidation, XDocument xmlDataToValidate,
      XmlSchema schema)
    {
      XmlSchemaSet schemas = new XmlSchemaSet();
      schemas.Add(schema);
      errorInValidation = false;
      StringBuilder errorBuilder = new StringBuilder();
      bool isValid = true;
      xmlDataToValidate.Validate(schemas, (o, e) =>
      {
        errorBuilder.AppendLine(e.Message);
        isValid = false;
      });

      errorInValidation = isValid;
      return errorBuilder;
    }

    #endregion
    /// <summary>
    /// Returns an xml document from base64 encoded string.
    /// </summary>
    /// <param name="base64EncodedString">Base64 encoded xml string</param>
    /// <param name="preserveWhiteSpace">Set to true if the whitespace int the xml needs to be preserved</param>
    /// <returns></returns>
    public static XmlDocument ConvertBase64ToXmlDoc(string base64EncodedString, bool preserveWhiteSpace)
    {
      string xmlString = Encoding.UTF8.GetString(Convert.FromBase64String(base64EncodedString));
      XmlDocument xmlDoc = new XmlDocument();
      xmlDoc.PreserveWhitespace = preserveWhiteSpace;
      xmlDoc.LoadXml(xmlString);
      return xmlDoc;
    }

    #region XSLT Transformation
    /// <summary>
    /// Transforms the incoming xDoc and returns it.
    /// </summary>
    /// <param name="xDoc"></param>
    /// <param name="xsltTransformer"></param>
    /// <returns></returns>
    public static XDocument TransformDocument(XDocument xDoc, XslCompiledTransform xsltTransformer)
    {
      XDocument transformedDoc = new XDocument();
      using (XmlWriter writer = transformedDoc.CreateWriter())
      {
        xsltTransformer.Transform(xDoc.CreateReader(), writer);
      }

      return transformedDoc;
    }

    /// <summary>
    /// Transforms xml data via a XSL Compiled Transform.
    /// </summary>
    /// <param name="xmlData">Data document</param>
    /// <param name="xsltDoc">Stylesheet</param>
    /// <param name="enableScripts">Value indicating whether to enable script blocks</param>
    /// <returns>Transformed document</returns>
    public static XDocument TransformDocument(XDocument xmlData, XDocument xsltDoc, bool enableScripts)
    {
      XslCompiledTransform transform = new XslCompiledTransform();

      XsltSettings settings = new XsltSettings();
      settings.EnableScript = enableScripts;

      using (XmlReader reader = xsltDoc.CreateReader())
      {
        transform.Load(reader, settings, null);

        return TransformDocument(xmlData, transform);
      }
    }

    #endregion
  }
}
