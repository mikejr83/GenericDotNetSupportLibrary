using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GenericDotNetSupportLibrary.Helpers;

namespace GenericDotNetSupportLibrary.Tests.Helpers
{
  [TestClass]
  public class StringHelperUnitTests
  {
    [TestMethod]
    public void TestMethod1()
    {
    }

    [TestMethod]
    [TestCategory("StringHelper")]
    public void NotNull_Test()
    {
      int i1 = StringHelper.NotNull<int>(null, 103);
      Assert.IsTrue(i1 == 103, "Unexpected value for NotNull test");

      int i2 = StringHelper.NotNull<int>("1234", 103);
      Assert.IsTrue(i2 == 1234, "Unexpected value for NotNull test");

      int i3 = StringHelper.NotNull<int>("0", 103);
      Assert.IsTrue(i3 == 0, "Unexpected value for NotNull test");
    }


    [TestMethod]
    [TestCategory("StringHelper")]
    public void NotBlank_Test()
    {
      string testVal = "ss";
      string returnVal = StringHelper.NotBlank(testVal, "replace");
      Assert.IsTrue(returnVal == "ss", "Unexpected returnVal");

      returnVal = StringHelper.NotBlank(string.Empty, "replace");
      Assert.IsTrue(returnVal == "replace", "Unexpected return val");

      string testVal1 = "1234";
      returnVal = StringHelper.NotBlank(testVal1, "replace");
      Assert.IsTrue(returnVal == "1234", "unexpected returnVal");
    }



    /// <summary>
    /// if string is non-empty or not-null, the string is returned, else it is null
    /// </summary>
    [TestMethod]
    [TestCategory("StringHelper")]
    public void CoerceNullIfNullOrEmpty_Test()
    {
      string testVal = "test";
      string returnVal = StringHelper.CoerceNullIfNullOrEmpty(testVal);
      Assert.IsTrue(returnVal == "test", "Unexpected returnVal");

      testVal = null;
      string returnVal1 = StringHelper.CoerceNullIfNullOrEmpty(testVal);
      Assert.IsTrue(returnVal1 == null, "Unexpected returnVal");

      testVal = "";
      string returnVal2 = StringHelper.CoerceNullIfNullOrEmpty(testVal);
      Assert.IsTrue(returnVal2 == null, "Unexpected returnVal");

      testVal = "1234";
      string returnVal3 = StringHelper.CoerceNullIfNullOrEmpty(testVal);
      Assert.IsTrue(returnVal3 == "1234", "unexpected returnVal");
    }



    /// <summary>
    /// checks if the first non-null string in a set of strings is returned. if there are no non-null strings, then empty
    /// string is returned
    /// </summary>
    [TestMethod]
    [TestCategory("StringHelper")]
    public void Coalesce_Test()
    {
      String[] array1 = new String[3];
      array1[0] = null;
      array1[1] = "test1";
      array1[2] = null;
      foreach (string item1 in array1)
      {
        string returnVal1 = StringHelper.Coalesce(item1);
        if (item1 != null)
        {
          Assert.IsTrue(returnVal1 == "test1", "Unexpected returnVal");
        }
      }

      String[] array2 = new String[3];
      array2[0] = "test";
      array2[1] = null;
      array2[2] = null;
      foreach (string item2 in array2)
      {
        string returnVal2 = StringHelper.Coalesce(item2);
        if (item2 != null)
        {
          Assert.IsTrue(returnVal2 == "test", "Unexpected returnVal");
        }
      }

      String[] array3 = new String[2];
      array3[0] = null;
      array3[1] = null;
      foreach (string item3 in array3)
      {
        string returnVal3 = StringHelper.Coalesce(item3);
        Assert.IsTrue(returnVal3 == "", "Unexpected returnVal");
      }

    }


    /// <summary>
    /// if there is a null value in a set of strings then return false. Else return true
    /// </summary>
    [TestMethod]
    [TestCategory("StringHelper")]
    public void StringsNoNullOrEmpty_Test()
    {
      String[] array1 = new String[3];
      array1[0] = null;
      array1[1] = "test1";
      array1[2] = "test2";
      foreach (string item1 in array1)
      {
        bool returnVal1 = StringHelper.StringsNoNullOrEmpty(item1);
        Assert.IsTrue(returnVal1 == false, "Unexpected returnVal");
        break;
      }

      String[] array2 = new String[2];
      array2[0] = "test1";
      array2[1] = "test2";
      foreach (string item2 in array2)
      {
        bool returnVal2 = StringHelper.StringsNoNullOrEmpty(item2);
        Assert.IsTrue(returnVal2 == true, "Unexpected returnVal");
      }
    }

    /// <summary>
    /// checks to see if the dictionary word is replaced by its corresponding value
    /// </summary>
    [TestMethod]
    [TestCategory("StringHelper")]
    public void Format_Test()
    {
      //for single dictionary pair value
      IDictionary<string, string> macros1 = new Dictionary<string, string>();
      macros1.Add("username1", "value1");
      string sentence1 = "Hello {{username1}}";
      string returnVal1 = StringHelper.Format(sentence1, macros1);
      Assert.IsTrue(returnVal1 == "Hello value1", "unexpected returnVal");

      //for multiple dictionary pair values
      IDictionary<string, string> macros2 = new Dictionary<string, string>();
      macros2.Add("username1", "value1");
      macros2.Add("username2", "value2");
      macros2.Add("username3", "value3");
      macros2.Add("password1", "pwd1");
      string sentence2 = "Hello {{username3}} your password is {{password1}}";
      string returnVal2 = StringHelper.Format(sentence2, macros2);
      Assert.IsTrue(returnVal2 == "Hello value3 your password is pwd1", "unexpected returnVal");


      string test1 = null;
      string returnVal3 = StringHelper.Format(test1, macros1);
      Assert.IsTrue(returnVal3 == "", "unexpected returnVal");
    }


    /// <summary>
    /// checks whether the dictionary-parsed items are displayed correctly
    /// </summary>
    [TestMethod]
    [TestCategory("StringHelper")]
    public void Parse2Dictionary_Test()
    {
      //for multiple values
      string test1 = "Item1:=Value1|,|Item2:=Value2|,|Item3:=Value3";
      IDictionary<string, string> returnVal1 = new Dictionary<string, string>();
      string[] row_delimiters = new string[] { "|,|" };
      string[] col_delimiters = new string[] { ":=" };
      returnVal1 = StringHelper.Parse2Dictionary(test1);
      string[] elements = test1.Split(row_delimiters, System.StringSplitOptions.None);
      foreach (string item in elements)
      {
        string[] args = item.Split(col_delimiters, System.StringSplitOptions.None);
        Assert.IsTrue(returnVal1[args[0]] == args[1], "unexpected returnVal");
      }


      //for one value
      string test2 = "Item1:=Value1";
      IDictionary<string, string> returnVal2 = new Dictionary<string, string>();
      returnVal2 = StringHelper.Parse2Dictionary(test2);
      elements = test2.Split(col_delimiters, System.StringSplitOptions.None);
      Assert.IsTrue(returnVal2[elements[0]] == "Value1", "unexpected returnVal");

    }


    /// <summary>
    /// checks the correctness of all the ToLocalized values according to currency, number and percentage
    /// </summary>
    [TestMethod]
    [TestCategory("StringHelper")]
    public void ToLocalized_Test()
    {
      int i = 50;
      string returnVal1 = StringHelper.ToLocalized(i, StringHelper.NumericalLocalizationEnum.Currency);
      Assert.IsTrue(returnVal1 == "$50.00", "unexpected returnVal");

      string returnVal2 = StringHelper.ToLocalized(i, StringHelper.NumericalLocalizationEnum.Number);
      Assert.IsTrue(returnVal2 == "50.00", "unexpected returnVal");

      string returnVal3 = StringHelper.ToLocalized(i, StringHelper.NumericalLocalizationEnum.Percentage);
      Assert.IsTrue(returnVal3 == "50 %", "unexpected returnVal");
    }



    /// <summary>
    /// represents the integer as localized currency based on the current culture
    /// </summary>
    [TestMethod]
    [TestCategory("StringHelper")]
    public void ToLocalizedCurrency_Test()
    {
      float d = 10.98F;
      string returnVal = StringHelper.ToLocalizedCurrency(Convert.ToInt32(d));
      Assert.IsTrue(returnVal == "$11.00", "unexpected returnVal");

      int i = 50;
      string returnVal1 = StringHelper.ToLocalizedCurrency(i);
      Assert.IsTrue(returnVal1 == "$50.00", "unexpected returnVal");

      i = 0;
      string returnVal2 = StringHelper.ToLocalizedCurrency(i);
      Assert.IsTrue(returnVal2 == "$0.00", "unexpected returnVal");

    }



    /// <summary>
    /// represents the integer according to the current culture
    /// </summary>
    [TestMethod]
    [TestCategory("StringHelper")]
    public void ToLocalizedNumber_Test()
    {
      int i = 50;
      string returnVal1 = StringHelper.ToLocalizedNumber(i);
      Assert.IsTrue(returnVal1 == "50.00", "unexpected returnVal");

      float d = 10.98F;
      string returnVal = StringHelper.ToLocalizedNumber(Convert.ToInt32(d));
      Assert.IsTrue(returnVal == "11.00", "unexpected returnVal");

      i = 0;
      string returnVal2 = StringHelper.ToLocalizedNumber(i);
      Assert.IsTrue(returnVal2 == "0.00", "unexpected returnVal");
    }


    /// <summary>
    /// checks whether the integer is represented as percentage according to the current culture
    /// </summary>
    [TestMethod]
    [TestCategory("StringHelper")]
    public void ToLocalizedPercentage_Test()
    {
      float d = 10.98F;
      string returnVal = StringHelper.ToLocalizedPercentage(d);
      Assert.IsTrue(returnVal == "10.98 %", "unexpected returnVal");

      int i = 50;
      string returnVal1 = StringHelper.ToLocalizedPercentage(i);
      Assert.IsTrue(returnVal1 == "50 %", "unexpected returnVal");

      i = 0;
      string returnVal2 = StringHelper.ToLocalizedPercentage(i);
      Assert.IsTrue(returnVal2 == "0 %", "unexpected returnVal");
    }



    /// <summary>
    /// when the format and the cultureinfo are provided, the test ensures that the datetime format returned is correct
    /// </summary>
    [TestMethod]
    [TestCategory("StringHelper")]
    public void ToLocalizedDateTime_Test()
    {
      CultureInfo en = new CultureInfo("en-US");
      string format1 = en.DateTimeFormat.ShortDatePattern;
      DateTime dt1 = new DateTime(2001, 7, 13);
      string returnVal1 = StringHelper.ToLocalizedDateTime(format1, dt1);
      Assert.IsTrue(returnVal1 == "7/13/2001", "unexpected returnVal");

      CultureInfo br = new CultureInfo("pt-BR");
      string format2 = br.DateTimeFormat.ShortDatePattern; ;
      DateTime dt2 = new DateTime(2001, 7, 13);
      string returnVal2 = StringHelper.ToLocalizedDateTime(format2, dt2);
      Assert.IsTrue(returnVal2 == "13/07/2001", "unexpected returnVal");

      CultureInfo hr = new CultureInfo("de-DE");
      string format3 = hr.DateTimeFormat.ShortDatePattern; ;
      DateTime dt3 = new DateTime(2008, 4, 10);
      string returnVal3 = StringHelper.ToLocalizedDateTime(format3, dt3);
      Assert.IsTrue(returnVal3 == "10.04.2008", "unexpected returnVal");

      CultureInfo en1 = new CultureInfo("en-US");
      string format4 = en1.DateTimeFormat.LongDatePattern;
      DateTime dt4 = new DateTime(2008, 4, 10);
      string returnVal4 = StringHelper.ToLocalizedDateTime(format4, dt4);
      Assert.IsTrue(returnVal4 == "Thursday, April 10, 2008", "unexpected returnVal");

      string format5 = en1.DateTimeFormat.FullDateTimePattern;
      DateTime dt5 = new DateTime(2001, 7, 13, 5, 0, 0);
      string returnVal5 = StringHelper.ToLocalizedDateTime(format5, dt5);
      Assert.IsTrue(returnVal5 == "Friday, July 13, 2001 5:00:00 AM", "unexpected returnVal");
    }



    /// <summary>
    /// checks whether the boolean value passed as a string is returned as a boolean. Else returns false
    /// </summary>
    [TestMethod]
    [TestCategory("StringHelper")]
    public void ToSafeBool_Test()
    {
      string item = "true";
      bool returnVal1 = StringHelper.ToSafeBool(item);
      Assert.IsTrue(returnVal1 == true, "unexpected returnVal");

      item = "false";
      bool returnVal2 = StringHelper.ToSafeBool(item);
      Assert.IsTrue(returnVal2 == false, "unexpected returnVal");

      item = null;
      bool returnVal3 = StringHelper.ToSafeBool(item);
      Assert.IsTrue(returnVal3 == false, "unexpected returnVal");

      item = "test";
      bool returnVal4 = StringHelper.ToSafeBool(item);
      Assert.IsTrue(returnVal4 == false, "unexpected returnVal");
    }


    /// <summary>
    /// checks whether the integer passed as a string is returned in number format. Else return 0
    /// </summary>
    [TestMethod]
    [TestCategory("StringHelper")]
    public void ToSafeInt32_Test()
    {
      string item = "123";
      int returnVal1 = StringHelper.ToSafeInt32(item);
      Assert.IsTrue(returnVal1 == 123, "unexpected returnVal");

      item = "-123";
      int returnVal = StringHelper.ToSafeInt32(item);
      Assert.IsTrue(returnVal == -123, "unexpected returnVal");

      item = null;
      int returnVal2 = StringHelper.ToSafeInt32(item);
      Assert.IsTrue(returnVal2 == 0, "unexpected returnVal");

      item = "test";
      int returnVal3 = StringHelper.ToSafeInt32(item);
      Assert.IsTrue(returnVal3 == 0, "unexpected returnVal");

      //to check the range of 'int'
      item = "2147483647";
      int returnVal4 = StringHelper.ToSafeInt32(item);
      Assert.IsTrue(returnVal4 == 2147483647, "unexpected returnVal");

    }



    /// <summary>
    /// checks whether the float passed as a string is returned as a float number (according to floating number 
    /// representation). Else return 0
    /// </summary>
    [TestMethod]
    [TestCategory("StringHelper")]
    public void ToSafeFloat_Test()
    {
      string item = "1.1";
      float returnVal1 = StringHelper.ToSafeFloat(item);
      Assert.IsTrue(returnVal1 == 1.1F, "unexpected returnVal");

      item = "-1.1";
      float returnVal = StringHelper.ToSafeFloat(item);
      Assert.IsTrue(returnVal == -1.1F, "unexpected returnVal");

      item = null;
      float returnVal2 = StringHelper.ToSafeFloat(item);
      Assert.IsTrue(returnVal2 == 0, "unexpected returnVal");

      item = "test";
      float returnVal3 = StringHelper.ToSafeFloat(item);
      Assert.IsTrue(returnVal3 == 0, "unexpected returnVal");
    }



    /// <summary>
    /// checks whether the long number passed as a string is returned in number format. Else return 0
    /// </summary>
    [TestMethod]
    [TestCategory("StringHelper")]
    public void ToSafeInt64_Test()
    {
      string item = "124";
      long returnVal1 = StringHelper.ToSafeInt64(item);
      Assert.IsTrue(returnVal1 == 124, "unexpected returnVal");

      item = "-124";
      long returnVal = StringHelper.ToSafeInt64(item);
      Assert.IsTrue(returnVal == -124, "unexpected returnVal");

      item = null;
      long returnVal2 = StringHelper.ToSafeInt64(item);
      Assert.IsTrue(returnVal2 == 0, "unexpected returnVal");

      item = "test";
      long returnVal3 = StringHelper.ToSafeInt64(item);
      Assert.IsTrue(returnVal3 == 0, "unexpected returnVal");

      //to check the range of 'long'
      item = "-922337203685477508";
      long returnVal4 = StringHelper.ToSafeInt64(item);
      Assert.IsTrue(returnVal4 == -922337203685477508, "unexpected returnVal");
    }



    /// <summary>
    /// checks whether newline, double quotes and single quotes are escaped correctly
    /// </summary>
    [TestMethod]
    [TestCategory("StringHelper")]
    public void MakeSafeForJS_Test()
    {
      string input1 = "test\ntest";
      string returnVal = StringHelper.MakeSafeForJS(input1);
      Assert.IsTrue(returnVal == "test\\ntest", "unexpected returnVal");

      string input2 = "test\"test\"test";
      returnVal = StringHelper.MakeSafeForJS(input2);
      Assert.IsTrue(returnVal == "test\\\"test\\\"test", "unexpected returnVal");

      string input3 = "test\'test\'test";
      returnVal = StringHelper.MakeSafeForJS(input3);
      Assert.IsTrue(returnVal == "test\\\'test\\\'test", "unexpected returnVal");

      string input4 = "\n";
      returnVal = StringHelper.MakeSafeForJS(input4);
      Assert.IsTrue(returnVal == "\\n", "unexpected returnVal");

      string input5 = "\"";
      returnVal = StringHelper.MakeSafeForJS(input5);
      Assert.IsTrue(returnVal == "\\\"", "unexpected returnVal");

      string input6 = "\'";
      returnVal = StringHelper.MakeSafeForJS(input6);
      Assert.IsTrue(returnVal == "\\\'", "unexpected returnVal");

      string input7 = "";
      returnVal = StringHelper.MakeSafeForJS(input7);
      Assert.IsTrue(returnVal == "", "unexpected returnVal");
    }



    /// <summary>
    /// tests whether a string in readable format, with separator, is returned when a camel-case word is passed
    /// </summary>
    [TestMethod]
    [TestCategory("StringHelper")]
    public void ParseToFriendlyValue_Test()
    {

      string s1 = "tEsTsS";
      string separator = " ";
      string returnVal1 = StringHelper.ParseToFriendlyValue(s1, separator);
      Assert.IsTrue(returnVal1 == "t Es Ts S", "unexpected returnVal");

      string s2 = "tEsTsS";
      separator = "_";
      string returnVal2 = StringHelper.ParseToFriendlyValue(s2, separator);
      Assert.IsTrue(returnVal2 == "t_Es_Ts_S", "unexpected returnVal");

      string s3 = "tests";
      string returnVal3 = StringHelper.ParseToFriendlyValue(s3, separator);
      Assert.IsTrue(returnVal3 == "tests", "unexpected returnVal");

      separator = "";
      string s4 = "TESTS";
      string returnVal4 = StringHelper.ParseToFriendlyValue(s4, separator);
      Assert.IsTrue(returnVal4 == "TESTS", "unexpected returnVal");

      string s5 = "";
      string returnVal5 = StringHelper.ParseToFriendlyValue(s5, separator);
      Assert.IsTrue(returnVal5 == "", "unexpected returnVal");

    }


    /// <summary>
    /// checks to see if a readable name is returned in place of a typename
    /// </summary>
    [TestMethod]
    [TestCategory("StringHelper")]
    public void ParseToFriendlyTypeName_Test()
    {
      string s1 = "";
      string returnVal1 = StringHelper.ParseToFriendlyTypeName(s1);
      Assert.IsTrue(returnVal1 == " Property Dynamic Value", "unexpected returnVal");

      s1 = null;
      string returnVal2 = StringHelper.ParseToFriendlyTypeName(s1);
      Assert.IsTrue(returnVal2 == "", "unexpected returnVal");
    }



    /// <summary>
    /// checks whether the passed string is a number. Returns true if it is, else false
    /// </summary>
    [TestMethod]
    [TestCategory("StringHelper")]
    public void IsNumeric_Test()
    {
      string s = "12345";
      bool returnVal1 = StringHelper.IsNumeric(s);
      Assert.IsTrue(returnVal1 == true, "unexpected returnVal");

      s = "1234.50";
      bool returnVal = StringHelper.IsNumeric(s);
      Assert.IsTrue(returnVal == false, "unexpected returnVal");

      string str = null;
      bool returnVal2 = StringHelper.IsNumeric(str);
      Assert.IsTrue(returnVal2 == false, "unexpected returnVal");

      str = "test";
      bool returnVal3 = StringHelper.IsNumeric(str);
      Assert.IsTrue(returnVal3 == false, "unexpected returnVal");
    }


    /// <summary>
    /// checks whether the HTML tags are stripped from the string 
    /// </summary>
    [TestMethod]
    [TestCategory("StringHelper")]
    public void StripHtml_Test()
    {
      string content = "<p>test</p> <b>W</b> <i>test1</i>";
      string tagstoPreserve = "";
      string returnVal1 = StringHelper.StripHtml(content, tagstoPreserve);
      Assert.IsTrue(returnVal1 == "test W test1", "unexpected returnVal");

      content = "";
      string returnVal2 = StringHelper.StripHtml(content, tagstoPreserve);
      Assert.IsTrue(returnVal2 == "", "unexpected returnVal");
    }



    /// <summary>
    /// removes the scripts from the given string
    /// </summary>
    [TestMethod]
    [TestCategory("StringHelper")]
    public void StripScripts_Test()
    {
      string content = "123<script type=\"text/javascript\">\n\t\tfunction utmx_section(){}function utmx(){}\n\t\t(function()})();\n\t</script>456";
      string returnVal1 = StringHelper.StripScripts(content);
      Assert.IsTrue(returnVal1 == "123456", "unexpected returnVal");

      content = "abc<script test>  </script>def";
      string returnVal3 = StringHelper.StripScripts(content);
      Assert.IsTrue(returnVal3 == "abcdef", "unexpected returnVal");

      content = "";
      string returnVal2 = StringHelper.StripScripts(content);
      Assert.IsTrue(returnVal2 == "", "unexpected returnVal");
    }


    /// <summary>
    /// checks whether the HTML tags are stripped from a string 
    /// </summary>
    [TestMethod]
    [TestCategory("StringHelper")]
    public void StripTags_Test()
    {
      string content = "<p>test</p> <script type=\"text/javascript\">X</script> <b>W</b> <i>test1</i>";
      string tagstoPreserve = "";
      string returnVal1 = StringHelper.StripTags(content, tagstoPreserve);
      Assert.IsTrue(returnVal1 == "test X W test1", "unexpected returnVal");


      content = "<a href=\"new_page.html\" OnClick=\"javascript:MyFunction();\">Text to Click</a>";
      string returnVal = StringHelper.StripTags(content, tagstoPreserve);
      Assert.IsTrue(returnVal == "Text to Click", "unexpected returnVal");


      content = "";
      string returnVal2 = StringHelper.StripTags(content, tagstoPreserve);
      Assert.IsTrue(returnVal2 == "", "unexpected returnVal");
    }


    [TestMethod]
    [TestCategory("StringHelper")]
    public void StripJavaScriptFromString_Test()
    {
      string content = "<a href=\"new_page.html\" OnClick=\"javascript:MyFunction();\">Text to Click</a>";
      string returnVal1 = StringHelper.StripJavaScriptFromString(content);
      Assert.IsTrue(returnVal1 == "Text to Click</a>", "unexpected returnVal");

      content = "<ul><li><a href=\"/wiki/Portal:Arts\" OnClick=\"javascript:MyFunction();\">Arts</a></li></ul>";
      string returnVal2 = StringHelper.StripJavaScriptFromString(content);
      Assert.IsTrue(returnVal2 == "<ul><li>Arts</a></li></ul>", "unexpected returnVal");

      content = "";
      string returnVal3 = StringHelper.StripJavaScriptFromString(content);
      Assert.IsTrue(returnVal3 == "", "unexpected returnVal");
    }


    /// <summary>
    /// removes the hex characters in the string and prints the rest with the corresponding character in between
    /// </summary>
    [TestMethod]
    [TestCategory("StringHelper")]
    public void StripEscapedHexChars_Test()
    {
      string input = "1010_x0020_10101";
      string returnVal1 = StringHelper.StripEscapedHexChars(input);
      Assert.IsTrue(returnVal1 == "1010 10101", "unexpected returnVal");

      input = "test_x0021_test";
      string returnVal2 = StringHelper.StripEscapedHexChars(input);
      Assert.IsTrue(returnVal2 == "test!test", "unexpected returnVal");

      input = "1010_x0026_1010";
      string returnVal3 = StringHelper.StripEscapedHexChars(input);
      Assert.IsTrue(returnVal3 == "1010&1010", "unexpected returnVal");

      input = "";
      string returnVal4 = StringHelper.StripEscapedHexChars(input);
      Assert.IsTrue(returnVal4 == "", "unexpected returnVal");

      input = "test_x002F_test";
      string returnVal5 = StringHelper.StripEscapedHexChars(input);
      Assert.IsTrue(returnVal5 == "test/test", "unexpected returnVal");

      input = "test_x002B_test";
      string returnVal6 = StringHelper.StripEscapedHexChars(input);
      Assert.IsTrue(returnVal6 == "test+test", "unexpected returnVal");
    }



    /// <summary>
    /// converts the number to its english equivalent
    /// </summary>
    [TestMethod]
    [TestCategory("StringHelper")]
    public void ToEnglish_Test()
    {
      int h = 0;
      string returnVal1 = StringHelper.ToEnglish(h);
      Assert.IsTrue(returnVal1 == "Zero", "unexpected returnVal");

      int i = 1;
      string returnVal2 = StringHelper.ToEnglish(i);
      Assert.IsTrue(returnVal2 == "One", "unexpected returnVal");

      int j = 14;
      string returnVal3 = StringHelper.ToEnglish(j);
      Assert.IsTrue(returnVal3 == "Fourteen", "unexpected returnVal");

      int k = 50;
      string returnVal4 = StringHelper.ToEnglish(k);
      Assert.IsTrue(returnVal4 == "Fifty", "unexpected returnVal");

      int c = 88;
      string returnVal = StringHelper.ToEnglish(c);
      Assert.IsTrue(returnVal == "EightyEight", "unexpected returnVal");

      int l = 100;
      string returnVal5 = StringHelper.ToEnglish(l);
      Assert.IsTrue(returnVal5 == "OneHundred", "unexpected returnVal");

      int m = 1000;
      string returnVal6 = StringHelper.ToEnglish(m);
      Assert.IsTrue(returnVal6 == "OneThousand", "unexpected returnVal");

      int n = 9999;
      string returnVal7 = StringHelper.ToEnglish(n);
      Assert.IsTrue(returnVal7 == "NineThousandNineHundredNinetyNine", "unexpected returnVal");

      int a = 999999;
      string returnVal8 = StringHelper.ToEnglish(a);
      Assert.IsTrue(returnVal8 == "NineHundredNinetyNineThousandNineHundredNinetyNine", "unexpected returnVal");

      int p = 1000000;
      string returnVal9 = StringHelper.ToEnglish(p);
      Assert.IsTrue(returnVal9 == "Number is out of range!", "unexpected returnVal");
    }



    /// <summary>
    /// limits the word according to the ellipsis condition
    /// </summary>
    [TestMethod]
    [TestCategory("StringHelper")]
    public void GetEllipsisText_Test()
    {
      string text = "this is a test sentence";
      int limit = 10;
      string returnVal1 = StringHelper.GetEllipsisText(text, limit);
      Assert.IsTrue(returnVal1 == "this is...", "unexpected returnVal");

      limit = 5;
      string returnVal = StringHelper.GetEllipsisText(text, limit);
      Assert.IsTrue(returnVal == "th...", "unexpected returnVal");

      text = "";
      string returnVal2 = StringHelper.GetEllipsisText(text, limit);
      Assert.IsTrue(returnVal2 == "", "unexpected returnVal");
    }
  }
}
