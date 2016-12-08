using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;

namespace GenericDotNetSupportLibrary.Helpers
{
  /// <summary>
  /// StringHelper provides basic functionality for working with strings.
  /// <remarks>
  /// The StringHelper class contains items for parsing text to various types.  It does not, however, deal with XML strings.  For XML string functions <see cref="XmlHelper"/>.
  /// </remarks>
  /// </summary>
  public partial class StringHelper
  {
    #region Parse Methods
    /// <summary>
    /// Safely parses a Boolean value from a string.
    /// <remarks>
    /// If the value is empty or consists of only white spaces the value returned will be false.  A string value of "1" will return true and a value of "0" will return false.
    /// </remarks>
    /// </summary>
    /// <param name="value">String which will be tested and parsed for a Boolean value.</param>
    /// <returns>True/False</returns>
    public static bool ParseBool(string value)
    {
      if (value == null || string.IsNullOrEmpty(value.Trim()))
        return false;
      else
      {
        value = value.Trim();

        if (value.Equals("1"))
          return true;
        else if (value.Equals("0"))
          return false;
        else
        {
          bool result = false;

          if (bool.TryParse(value, out result))
            return result;
          else
            return false;
        }
      }
    }

    /// <summary>
    /// Safely parses an integer value from a string.
    /// </summary>
    /// <param name="value">String representation of an integer</param>
    /// <returns>The parsed integer or 0 if no value could be parsed.</returns>
    public static int ParseInt(string value)
    {
      if (value == (null) || string.IsNullOrEmpty(value.Trim()))
        return default(int);
      else
      {
        int result = default(int);

        if (int.TryParse(value, out result))
          return result;

        return result;
      }
    }

    /// <summary>
    /// Safely parses a double value from a string.
    /// </summary>
    /// <param name="value">String representation of an integer</param>
    /// <returns>The parsed double or 0 if no value could be parsed.</returns>
    public static double ParseDouble(string value)
    {
      if (value == (null) || string.IsNullOrEmpty(value.Trim()))
        return default(double);
      else
      {
        double result = default(double);

        if (double.TryParse(value, out result))
          return result;

        return result;
      }
    }

    /// <summary>
    /// Safely parses a float value from a string.
    /// </summary>
    /// <param name="value">String representation of an integer</param>
    /// <returns>The parsed float or 0 if no value could be parsed.</returns>
    public static float ParseFloat(string value)
    {
      if (value == (null) || string.IsNullOrEmpty(value.Trim()))
        return default(float);
      else
      {
        float result = default(float);

        if (float.TryParse(value, out result))
          return result;

        return result;
      }
    }

    /// <summary>
    /// Safely parses an integer value from a string.
    /// </summary>
    /// <param name="value">String representation of an integer</param>
    /// <returns>The parsed integer or 0 if no value could be parsed.</returns>
    public static int? ParseNullableInt(string value)
    {
      if (value == null || string.IsNullOrEmpty(value.Trim()))
        return default(int?);
      else
      {
        int result = default(int);

        if (int.TryParse(value, out result))
          return result;
        else
          return default(int?);
      }
    }

    /// <summary>
    /// Safely parses a date time using the built in DateTime parsing method.
    /// </summary>
    /// <param name="value">String containing a date time value.</param>
    /// <returns>The DateTime value represented in the string.</returns>
    public static DateTime ParseDateTime(string value)
    {
      if (value == (null) || string.IsNullOrEmpty(value.Trim()))
        return default(DateTime);
      else
      {
        DateTime result = default(DateTime);

        if (DateTime.TryParse(value, out result))
          return result;

        return result;
      }
    }

    /// <summary>
    /// Safely parses a date time using the built in DateTime parsing method.
    /// </summary>
    /// <param name="value">String containing a date time value.</param>
    /// <returns>The DateTime value represented in the string.</returns>
    public static DateTime? ParseNullableDateTime(string value)
    {
      if (value == (null) || value.Trim().Length == 0 || string.IsNullOrEmpty(value.Trim()))
        return default(DateTime?);
      else
      {
        DateTime result = default(DateTime);

        if (DateTime.TryParse(value, out result))
          return result;
        else
        {
          long ticks = default(long);

          if (long.TryParse(value, out ticks))
            return new DateTime(ticks);
          else
            return default(DateTime?);
        }

      }
    }

    /// <summary>
    /// Attempts to convert a camel-case string into something more readable.
    /// </summary>
    /// <param name="Value">String to be converted</param>
    /// <param name="Seperator">String to be inserted inbetween upper-cased letters(typically a space or underscore).</param>
    /// <returns>Converted string.</returns>
    public static string ParseToFriendlyValue(string Value, string Seperator)
    {
      char PreviousChar = new char();
      string Buffer = "";

      for (int x = 0; x < Value.Length; x++)
      {
        if (Char.IsUpper(Value[x]) && !Char.IsUpper(PreviousChar))
          Buffer += Seperator + Value[x];
        else
          Buffer += Value[x];

        PreviousChar = Value[x];

      }

      return Buffer;

    }
    /// <summary>
    /// Attempts to convert a type model name to a readable name
    /// </summary>
    /// <param name="typeName">Type name</param>
    /// <returns></returns>
    public static string ParseToFriendlyTypeName(string typeName)
    {
      string[] separator = new string[] { "." };
      const string identifier = ",";
      const string resultingSep = " ";
      string friendlyNm = "";

      string[] typeParts;
      if (typeName != null && typeName.Contains(separator[0]))
      {
        typeParts = typeName.Split(separator, StringSplitOptions.None);

        foreach (string s in typeParts)
          if (s.Contains(identifier))
          {
            friendlyNm = s;
            break;
          }

        if (friendlyNm != "")
          friendlyNm = friendlyNm.Substring(0, friendlyNm.IndexOf(identifier.ToCharArray()[0]));

      }
      return ParseToFriendlyValue(friendlyNm, resultingSep);
    }

    /// <summary>
    /// Parses a named-parameter string into a dictionary of strings
    /// (e.g., "Item1:=Value1|,|Item2:=Value2|,|Item3:=Value3")
    /// </summary>
    /// <param name="test"></param>
    /// <returns></returns>
    public static IDictionary<string, string> Parse2Dictionary(string test)
    {
      string[] row_delimiters = new string[] { "|,|" };
      string[] col_delimiters = new string[] { ":=" };
      return Parse2Dictionary(test, row_delimiters, col_delimiters);
    }

    /// <summary>
    /// Overload of Parse2Dictionary above, allowing calling routine to specify the delimiters as simple strings.
    /// </summary>
    /// <param name="test">The string to parse</param>
    /// <param name="row_delimiters">An array of possible outer (row) delimiters.</param>
    /// <param name="col_delimiters">An array of possible inner (column) delimiters</param>
    /// <returns></returns>
    public static IDictionary<string, string> Parse2Dictionary(string test, string row_delimiter, string col_delimiter)
    {
      string[] row_delimiters = new string[1];
      string[] col_delimiters = new string[1];

      row_delimiters[0] = row_delimiter;
      col_delimiters[0] = col_delimiter;

      return Parse2Dictionary(test, row_delimiters, col_delimiters);

    }

    /// <summary>
    /// Overload of Parse2Dictionary above, allowing calling routine to specify the delimiters as arrays.
    /// </summary>
    /// <param name="test">The string to parse</param>
    /// <param name="row_delimiters">An array of possible outer (row) delimiters.</param>
    /// <param name="col_delimiters">An array of possible inner (column) delimiters</param>
    /// <returns></returns>
    public static IDictionary<string, string> Parse2Dictionary(string test, string[] row_delimiters, string[] col_delimiters)
    {

      IDictionary<string, string> items = new Dictionary<string, string>();
      try
      {
        string[] elements = test.Split(row_delimiters, System.StringSplitOptions.None);
        foreach (string item in elements)
        {
          string[] args = item.Split(col_delimiters, System.StringSplitOptions.None);
          if (args.Length > 1)
            items[args[0]] = args[1];
        }
      }
      catch { }

      return items;

    }
    #endregion

    #region Tests
    /// <summary>
    /// It determines if the string is made up of only numbers
    /// </summary>
    /// <param name="s">string to be checked</param>
    /// <returns>true for string made up of only numbers, false otherwise</returns>
    public static bool IsNumeric(string s)
    {
      if (s == null)
        return false;

      char[] cA;
      bool bNumeric = true;

      cA = s.ToCharArray();
      if (cA.Length == 0)
        bNumeric = false;
      else
      {
        if ((cA[0] == '-' || cA[0] == '+') &&
            (cA.Length > 1))
          cA[0] = '0';
      }

      foreach (char c in cA)
      {
        if (!(c >= '0' && c <= '9'))
        {
          bNumeric = false;
          break;
        }
      }
      return bNumeric;
    }

    /// <summary>
    /// This utility is used to check a set of strings to ensure none of them are null or Empty.
    /// </summary>
    /// <param name="stringList"></param>
    /// <returns></returns>
    public static bool StringsNoNullOrEmpty(params string[] stringList)
    {
      bool returnValue = true;

      foreach (string item in stringList)
      {
        if (String.IsNullOrEmpty(item))
        {
          returnValue = false;
          break;
        }
      }

      return returnValue;
    }
    #endregion

    #region Localization
    /// <summary>
    /// Method returns a localized string based on the type of enum chosen
    /// </summary>
    /// <param name="i"></param>
    /// <param name="variableName"></param>
    /// <returns></returns>
    public static string ToLocalized(int i, NumericalLocalizationEnum variableName)
    {
      switch (variableName)
      {
        case NumericalLocalizationEnum.Currency:
          return ToLocalizedCurrency(i);

        case NumericalLocalizationEnum.Number:
          return ToLocalizedNumber(i);

        case NumericalLocalizationEnum.Percentage:
          return ToLocalizedPercentage(i);

        default:
          throw new NotImplementedException(variableName.ToString());
          break;
      }
      return null;
    }

    /// <summary>
    /// Enum for Numerical localization
    /// </summary>
    public enum NumericalLocalizationEnum
    {
      /// <summary>
      /// Use this value for localizing currencies
      /// </summary>
      Currency = 0,

      /// <summary>
      ///  Use this value for localizing numbers
      /// </summary>
      Number = 1,

      /// <summary>
      /// Use this value for localizing percentages
      /// </summary>
      Percentage = 2

    }

    /// <summary>
    /// Returns a localized currency string based on the current culture. Does not support neutral cultures.
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public static string ToLocalizedCurrency(int i)
    {
      return i.ToString("c", Thread.CurrentThread.CurrentUICulture.NumberFormat);
    }

    /// <summary>
    /// Returns a localized numerical string based on the current culture. Does not support neutral cultures.
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public static string ToLocalizedNumber(int i)
    {
      return i.ToString("n", Thread.CurrentThread.CurrentUICulture.NumberFormat);
    }

    /// <summary>
    /// Returns a localized percentage string based on the current culture. Does not support neutral cultures.
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public static string ToLocalizedPercentage(float i)
    {
      NumberFormatInfo format = (NumberFormatInfo)Thread.CurrentThread.CurrentUICulture.NumberFormat.Clone();
      //remove decimal digits if no decimals exist (use floats instead of doubles because of rounding errors w/ doubles
      while (format.PercentDecimalDigits > 0 &&
        (int)Math.Floor(Convert.ToDouble(i * (int)Math.Pow(10, format.PercentDecimalDigits))) ==
        (int)(Math.Floor(Convert.ToDouble(i * (int)Math.Pow(10, format.PercentDecimalDigits - 1))) * 10))
        format.PercentDecimalDigits--;

      //.ToString("p") is expecting a decimal value, so it adds 2 trailing zeroes that we divide off
      return (i / 100).ToString("p", format);
    }

    /// <summary>
    /// Returns a localized date/tiime string based on the current culture. Does not support neutral cultures.
    /// </summary>
    /// <param name="dt">Date/Time structure</param>
    /// <param name="format">Standard format string for DateTime.ToString</param>
    /// <returns></returns>
    public static string ToLocalizedDateTime(String format, DateTime dt)
    {
      return dt.ToString(format, Thread.CurrentThread.CurrentUICulture.DateTimeFormat);
    }

    #endregion

    #region Sanitizing
    public static string StripHtml(string content, string tagsToPreserve)
    {
      content = StripTags(StripScripts(content), tagsToPreserve);
      content = content.Replace("&nbsp;", "&#160;");

      return content;
    }

    public static string StripScripts(string content)
    {
      Regex scriptFragment = new Regex("(?:<script.*?>)((\n|\r|.)*?)(?:</script>)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      return scriptFragment.Replace(content, "");
    }

    public static string StripTags(string content, string tagsToPreserve)
    {
      Regex searchExpression;

      if (String.IsNullOrEmpty(tagsToPreserve))
      {
        //Remove all tags
        searchExpression = new Regex("</?[^>]+>", RegexOptions.IgnoreCase);
        content = searchExpression.Replace(content, "");
      }
      else
      {
        //Remove (non) selected tags
        tagsToPreserve = tagsToPreserve.Replace(",", "|(/?)");

        //Eliminate all tags that we're not keeping
        searchExpression = new Regex("<(?!((" + tagsToPreserve + ")(/|\\s|>)))[^>]+>", RegexOptions.IgnoreCase);
        content = searchExpression.Replace(content, "");

        //Strip the attributes off the remaining tags
        searchExpression = new Regex("<([^>]*?)[\\s].*?>", RegexOptions.IgnoreCase);
        content = searchExpression.Replace(content, "<$1>");
      }
      return content;
    }

    public static string StripJavaScriptFromString(string badString)
    {
      string openTag = "(<|(&lt;))(";
      string invalidTagNames = "(((applet)|(body)|(embed)|(frame)|(script)|(frameset)|(iframe)|(style)|(layer)|(link)|(ilayer)|(meta)|(object))( (.(?!>|&gt;))*.?)?(>|&gt;))";
      string eventHandlers = "([a-z0-9]+(\\s+[a-z0-9]+\\s*(=((\"[^\"]*\")|('[^']*')|(\\w*)))?)*?\\s+on[a-z]+=((\"[^\"]*\")|('[^']*')))";
      string hrefJavascript = "(a(\\s+[a-z0-9]+(?:\\s*)=(?:\\s*)((\"[^\"]*\")|('[^']*')|(\\w*)))*?\\s+(href(?:\\s*)=(?:\\s*)((\"[^\"]*?javascript:[^\"]*\")|('[^']*?javascript:[^']*'))))";
      string closeTag = ")(>|(&gt;))";

      Regex _DataValidationRegex = new Regex(openTag + invalidTagNames + "|" + eventHandlers + "|" + hrefJavascript + closeTag, RegexOptions.IgnoreCase);
      string cleanString = _DataValidationRegex.Replace(badString, String.Empty);

      string cssHacks = @"\bexpression\(";
      Regex cssHackRegex = new Regex(cssHacks, RegexOptions.IgnoreCase);
      cleanString = cssHackRegex.Replace(cleanString, String.Empty);

      return cleanString;
    }

    /// <summary>
    /// This method will be used to remove _x0020_ type encoding from strings.  This is common in DataSet handling when converting to XML.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string StripEscapedHexChars(string input)
    {
      Regex regex = new Regex("_[xX][0-9a-fA-F]{4}_");

      MatchCollection mc = regex.Matches(input);
      foreach (Match m in mc)
      {
        string s = m.ToString();

        //We are matching on strings like _x0020_, we want to parse out '0020' as the payload
        string sub = s.Substring(2, 4);

        //Convert payload from Hex -> int -> char
        char x = (char)int.Parse(s.Substring(2, 4), System.Globalization.NumberStyles.HexNumber);

        //Replace the _xXXXX_ with the appropriate ASCII char
        input = input.Replace(s, x.ToString());
      }
      return input;
    }

    #endregion

    #region Conversion
    /// <summary>
    /// Attempts to convert the string passed in input
    /// into a boolean value. If the string can not be converted,
    /// false is returned.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool ToSafeBool(string input)
    {
      if (input == null)
        return false;

      try
      {
        return (Convert.ToBoolean(input));
      }
      catch (System.FormatException)
      {
        return false;
      }
    }

    /// <summary>
    /// Converts the string passed in input to a 32 bit interger. 
    /// If the string is null, zero is returned.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static int ToSafeInt32(string input)
    {
      if (input == null)
        return 0;

      try
      {
        return (Convert.ToInt32(input));
      }
      catch (System.FormatException)
      {
        return 0;
      }
    }
    /// <summary>
    /// Converts the string passed in input to a float
    /// If the string is null, zero is returned
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static float ToSafeFloat(string input)
    {
      if (input == null)
        return 0;

      try
      {
        return (Convert.ToSingle(input));
      }
      catch (System.Exception)
      {
        return 0;
      }
    }

    /// <summary>
    /// Converts the string passed in input to a 64 bit interger. 
    /// If the string is null, zero is returned.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static long ToSafeInt64(string input)
    {
      if (input == null)
        return 0;

      try
      {
        return (Convert.ToInt64(input));
      }
      catch (System.FormatException)
      {
        return 0;
      }
    }

    /// <summary>
    /// Returns the base 64 encoded version of the string.
    /// </summary>
    /// <param name="inputString"></param>
    /// <returns></returns>
    public static string ConvertToBase64String(string inputString)
    {
      return (string.IsNullOrEmpty(inputString) ? null : Convert.ToBase64String(Encoding.UTF8.GetBytes(inputString)));
    }
    #endregion

    #region Coalesce
    /// <summary>
    /// Returns null if the string is null or empty.
    /// </summary>
    /// <param name="test"></param>
    /// <returns></returns>
    public static string CoerceNullIfNullOrEmpty(string test)
    {
      if (String.IsNullOrEmpty(test))
        return null;
      else
        return test;
    }

    /// <summary>
    /// Returns the first non-empty or non-null value from the string list.
    /// </summary>
    /// <param name="stringList"></param>
    /// <returns></returns>
    public static string Coalesce(params string[] stringList)
    {
      foreach (string item in stringList)
      {
        if (!String.IsNullOrEmpty(item))
          return item;
      }
      return "";
    }
    #endregion

    #region Format
    /// <summary>
    /// Does a search-and-replace on a dictionary of macro-replaceable values.
    /// The key of each item in the dictionary is the macro token (e.g., "{{UserName}}") 
    /// and the value is the value to replace the token with (e.g, "Joe Public").
    /// This is more flexible than the standart String.Format() function.  Note that 
    /// the delimiters "{{" and "}}" are hard-coded, differing from the "{" and "}" used in the 
    /// String.Format method.
    /// </summary>
    /// <param name="test"></param>
    /// <param name="macros"></param>
    /// <returns></returns>
    public static string Format(string test, IDictionary<string, string> macros)
    {
      if (String.IsNullOrEmpty(test))
        return String.Empty;

      String[] strings = new String[3] { "{{", "", "}}" };

      foreach (string macro in macros.Keys)
      {
        strings[1] = macro;
        Regex reg = new Regex(String.Join(String.Empty, strings), RegexOptions.IgnoreCase);
        test = reg.Replace(test, macros[macro]);
      }

      return test;
    }

    /// <summary>
    /// Format the string
    /// </summary>
    /// <param name="test"></param>
    /// <param name="namedparams"></param>
    /// <returns></returns>
    public static string Format(string test, string namedparams)
    {
      IDictionary<string, string> dict = Parse2Dictionary(namedparams);
      return Format(test, dict);
    }
    #endregion

    #region ?
    /// <summary>
    /// Not Null
    /// </summary>
    /// <param name="test"></param>
    /// <returns></returns>
    public static string NotNull(string test)
    {
      return NotNull(test, "");
    }

    /// <summary>
    /// Not Null
    /// </summary>
    /// <param name="test"></param>
    /// <param name="replace"></param>
    /// <returns></returns>
    public static string NotNull(string test, string replace)
    {
      if (test == null)
        return replace;
      else
      {
        return test;
      }
    }
    /// <summary>
    /// Not Null
    /// </summary>
    /// <param name="test"></param>
    /// <returns></returns>
    public static string NotNull(object test)
    {
      return NotNull((object)test, "");
    }
    /// <summary>
    /// Not Null
    /// </summary>
    /// <param name="test"></param>
    /// <param name="replace"></param>
    /// <returns></returns>
    public static string NotNull(object test, string replace)
    {
      if (test == null)
        return replace;
      else
      {
        return test.ToString();
      }
    }
    /// <summary>
    /// Not Null
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static T NotNull<T>(object value, T defaultValue)
    {
      return (T)(value != null ? Convert.ChangeType(value, typeof(T)) : defaultValue);
    }

    /// <summary>
    /// NotBlank
    /// </summary>
    /// <param name="test"></param>
    /// <param name="replace"></param>
    /// <returns></returns>
    public static string NotBlank(string test, string replace)
    {
      return (string.IsNullOrEmpty(test)) ? replace : test;
    }
    #endregion

    /// <summary>
    /// Make Safe for JS
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string MakeSafeForJS(string input)
    {
      // escape the new line characters      
      input = input.Replace("\n", "\\n");

      // escape the double-quotes
      string doublequote = ((char)34).ToString();
      string replaceVal = "\\" + doublequote;
      input = input.Replace(doublequote.ToString(), replaceVal);

      //Added this code to replace single quotes as well because some French 
      //translations contain single quotes and they were causing javascript errors (BUG: 4936)
      string singlequote = ((char)39).ToString();
      replaceVal = "\\" + singlequote;
      input = input.Replace(singlequote.ToString(), replaceVal);

      return input;
    }

    /// <summary>
    /// Converts an integer into its english representation.
    /// </summary>
    /// <param name="number">The nunber to convert.</param>
    /// <returns>The English representation of the number.</returns>
    public static string ToEnglish(int number)
    {
      #region Array Dec
      string[] english = new string[10];
      english[0] = "";
      english[1] = "One";
      english[2] = "Two";
      english[3] = "Three";
      english[4] = "Four";
      english[5] = "Five";
      english[6] = "Six";
      english[7] = "Seven";
      english[8] = "Eight";
      english[9] = "Nine";

      string[] teen = new string[10];
      teen[0] = "Ten";
      teen[1] = "Eleven";
      teen[2] = "Twelve";
      teen[3] = "Thirteen";
      teen[4] = "Fourteen";
      teen[5] = "Fifteen";
      teen[6] = "Sixteen";
      teen[7] = "Seventeen";
      teen[8] = "Eighteen";
      teen[9] = "Nineteen";

      string[] ty = new string[10];
      ty[0] = "";
      ty[1] = "";
      ty[2] = "Twenty";
      ty[3] = "Thirty";
      ty[4] = "Forty";
      ty[5] = "Fifty";
      ty[6] = "Sixty";
      ty[7] = "Seventy";
      ty[8] = "Eighty";
      ty[9] = "Ninety";
      #endregion

      int[] list = new int[10];
      float test;				// Test for length
      int c = 0;				// A counter
      int[] place = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
      string firstpart, lastpart, midpart, mid2part, choice;

      if (number == 0)
        return "Zero";

      while (number > 0)
      {
        list[c] = number % 10;
        number /= 10;
        c++;
      }

      test = (float)c / 3;

      // Number is 99 or less
      if (test < 1)
        return (_MakeNumber(c, list));
      else if (test == 1)
      {
        // Number is 100 through 999
        place[0] = list[0];
        place[1] = list[1];
        lastpart = _MakeNumber(2, place);
        firstpart = english[list[2]];
        return (firstpart + "Hundred" + lastpart);
      }
      else if (test < 2)
      {
        // Number is 1,000 through 99,999
        if (c == 4)
          firstpart = english[list[3]];
        else
        {
          place[0] = list[3];
          place[1] = list[4];
          firstpart = _MakeNumber(2, place);
        }
        place[0] = list[0];
        place[1] = list[1];
        lastpart = _MakeNumber(2, place);
        midpart = english[list[2]];
        if (midpart == "")
          choice = "";
        else
          choice = "Hundred";
        return (firstpart + "Thousand" + midpart + choice + lastpart);
      }
      else if (test == 2)
      {
        // Number is 100,000 throug 999,999
        firstpart = english[list[5]] + "Hundred";
        place[0] = list[3];
        place[1] = list[4];
        midpart = _MakeNumber(2, place);
        mid2part = english[list[2]];
        if (mid2part == "")
          choice = "";
        else
          choice = "Hundred";
        place[0] = list[0];
        place[1] = list[1];
        lastpart = _MakeNumber(2, place);
        return (firstpart + midpart + "Thousand" + mid2part + choice + lastpart);
      }

      return ("Number is out of range!");
    }

    private static string _MakeNumber(int n, int[] list)
    {
      #region Array Dec
      string[] english = new string[10];
      english[0] = "";
      english[1] = "One";
      english[2] = "Two";
      english[3] = "Three";
      english[4] = "Four";
      english[5] = "Five";
      english[6] = "Six";
      english[7] = "Seven";
      english[8] = "Eight";
      english[9] = "Nine";

      string[] teen = new string[10];
      teen[0] = "Ten";
      teen[1] = "Eleven";
      teen[2] = "Twelve";
      teen[3] = "Thirteen";
      teen[4] = "Fourteen";
      teen[5] = "Fifteen";
      teen[6] = "Sixteen";
      teen[7] = "Seventeen";
      teen[8] = "Eighteen";
      teen[9] = "Nineteen";

      string[] ty = new string[10];
      ty[0] = "";
      ty[1] = "";
      ty[2] = "Twenty";
      ty[3] = "Thirty";
      ty[4] = "Forty";
      ty[5] = "Fifty";
      ty[6] = "Sixty";
      ty[7] = "Seventy";
      ty[8] = "Eighty";
      ty[9] = "Ninety";
      #endregion
      string word, word1, word2, dash;
      if (n == 1)
        // Single digit was passed, return the "english"
        return (english[list[0]]);
      else if (n == 2)
      {
        if (list[1] == 1)
          return (teen[list[0]]);
        else
        {
          word1 = ty[list[1]];
          word2 = english[list[0]];
          if (word2 == "")
            dash = "";
          else
            dash = "";
          word = word1 + dash + word2;
          return word;
        }
      }

      // If we fall through, return -1
      return "";
    }
    /// <summary>
    /// This method returns text with ellipsis attached if it exceeds the length specified for the parameter limit.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    public static string GetEllipsisText(string text, int limit)
    {
      string ellipsisText = text;

      if (text.Length > limit)
        ellipsisText = text.Substring(0, limit - 3) + "...";

      return ellipsisText;
    }

    /// <summary>
    /// Shortens a path to a specified number of characters.
    /// </summary>
    /// <param name="pathName">The full path to the file including the file's name.</param>
    /// <param name="maxLength">The maximum length of the string.</param>
    /// <returns>Shortened path.</returns>
    static public string ShortenPathName(string pathName, int maxLength)
    {
      if (pathName.Length <= maxLength)
        return pathName;

      string root = Path.GetPathRoot(pathName);
      if (root.Length > 3)
        root += Path.DirectorySeparatorChar;

      string[] elements = pathName.Substring(root.Length).Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

      int filenameIndex = elements.GetLength(0) - 1;

      return root + "...\\" + elements[filenameIndex];
    }

    /// <summary>
    /// Adds spaces before capital letters in a string.
    /// </summary>
    /// <param name="text">string with capital letters to have a space inserted before them.</param>
    /// <returns>A new string with spaces inserted before the capital letters</returns>
    static public string AddSpacesToSentence(string text)
    {
      if (string.IsNullOrEmpty(text) || text.Trim().Length == 0)
        return "";
      StringBuilder newText = new StringBuilder(text.Length * 2);
      newText.Append(text[0]);
      for (int i = 1; i < text.Length; i++)
      {
        if (char.IsUpper(text[i]))
          newText.Append(' ');
        newText.Append(text[i]);
      }
      return newText.ToString();
    }

    /// <summary>
    /// Uses the current thread's culture to title case a string.
    /// </summary>
    /// <param name="value">The string to Title Case.</param>
    /// <returns>A Title Case'd String</returns>
    static public string ToTitleCase(string value)
    {
      if (string.IsNullOrEmpty(value) || value.Trim().Length == 0)
        return string.Empty;
      CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
      TextInfo textInfo = cultureInfo.TextInfo;

      return textInfo.ToTitleCase(value);
    }

    /// <summary>
    /// Gets the hex string from a byte array.
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static string GetHexStringFromByte(byte[] bytes)
    {
      if (bytes == null || bytes.ToString().Trim().Length == 0)
        return string.Empty;
      StringBuilder byteBuilder = new StringBuilder();
      for (int i = 0; i < bytes.Length; i++)
        byteBuilder.Append(String.Format("{0:X2}", bytes[i]));

      return byteBuilder.ToString();
    }

    /// <summary>
    /// Tests a string for only alphanumeric characters
    /// </summary>
    /// <param name="s">string to test</param>
    /// <returns>true/false depending on the contents of the string.</returns>
    public static bool IsAlphanumeric(string s)
    {
      if (s == null || s == string.Empty)
        return false;
      Regex r = new Regex("^[a-zA-Z0-9]*$");

      return r.IsMatch(s);
    }
  }
}
