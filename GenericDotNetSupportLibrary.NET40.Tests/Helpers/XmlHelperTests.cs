using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GenericDotNetSupportLibrary.Helpers;

namespace SupportLibraryTests.Helpers
{
  [TestClass]
  public class XmlHelperTests
  {
    static string _RelyingPartyDefinitionsXML = null;
    static string _InvalidRelyingPartyDefinitionsXML = null;
    static string _RelyingPartyDefinitionsXSD = null;

    static string _RelyingPartyDefinitionsFilename = null;
    static string _InvalidRelyingPartyDefinitionFilename = null;
    static string _RelyingPartyDefinitionsXSDFilename = null;

    [ClassInitialize()]
    public static void XmlHelperTestsInitialize(TestContext testContext)
    {
      using (Stream s = typeof(XmlHelperTests).Assembly.GetManifestResourceStream("SupportLibraryTests.Resources.RelyingPartyDefinitions.xml"))
      {
        if (s != null)
          using (StreamReader r = new StreamReader(s))
          {
            _RelyingPartyDefinitionsXML = r.ReadToEnd();
          }
      }

      using (Stream s = typeof(XmlHelperTests).Assembly.GetManifestResourceStream("SupportLibraryTests.Resources.InvalidRelyingPartyDefinitions.xml"))
      {
        if (s != null)
          using (StreamReader r = new StreamReader(s))
          {
            _InvalidRelyingPartyDefinitionsXML = r.ReadToEnd();
          }
      }

      using (Stream s = typeof(XmlHelperTests).Assembly.GetManifestResourceStream("SupportLibraryTests.Resources.RelyingPartyDefinitions.xsd"))
      {
        if (s != null)
          using (StreamReader r = new StreamReader(s))
          {
            _RelyingPartyDefinitionsXSD = r.ReadToEnd();
          }
      }

      if (!string.IsNullOrEmpty(_RelyingPartyDefinitionsXML))
      {
        _RelyingPartyDefinitionsFilename = Path.GetTempFileName();
        File.WriteAllText(_RelyingPartyDefinitionsFilename, _RelyingPartyDefinitionsXML, Encoding.UTF8);
      }

      if (!string.IsNullOrEmpty(_InvalidRelyingPartyDefinitionsXML))
      {
        _InvalidRelyingPartyDefinitionFilename = Path.GetTempFileName();
        File.WriteAllText(_InvalidRelyingPartyDefinitionFilename, _InvalidRelyingPartyDefinitionsXML, Encoding.UTF8);
      }

      if (!string.IsNullOrEmpty(_RelyingPartyDefinitionsXSD))
      {
        _RelyingPartyDefinitionsXSDFilename = Path.GetTempFileName();
        File.WriteAllText(_RelyingPartyDefinitionsXSDFilename, _RelyingPartyDefinitionsXSD, Encoding.UTF8);
      }
    }

    [TestMethod]
    public void XmlHelperValidateXMLTest()
    {
      string status = string.Empty;
      bool success = XmlHelper.ValidateXML(_RelyingPartyDefinitionsXML, _RelyingPartyDefinitionsXSD, out status);

      Assert.IsTrue(success);
    }

    [TestMethod]
    public void XmlHelperValidateInvalidXMLTest()
    {
      string status = string.Empty;
      bool success = XmlHelper.ValidateXML(_InvalidRelyingPartyDefinitionsXML, _RelyingPartyDefinitionsXSD, out status);

      Assert.IsFalse(success);
    }

    [TestMethod]
    public void XmlHelperValidateXMLFileTest()
    {
      string status = string.Empty;
      bool success = XmlHelper.ValidateXMLFiles(_RelyingPartyDefinitionsFilename, _RelyingPartyDefinitionsXSDFilename, out status);

      Assert.IsTrue(success);
    }

    [TestMethod]
    public void XmlHelperValidateInvalidXMLFileTest()
    {
      string status = string.Empty;
      bool success = XmlHelper.ValidateXMLFiles(_InvalidRelyingPartyDefinitionFilename, _RelyingPartyDefinitionsXSDFilename, out status);

      Assert.IsFalse(success);
    }
  }
}
