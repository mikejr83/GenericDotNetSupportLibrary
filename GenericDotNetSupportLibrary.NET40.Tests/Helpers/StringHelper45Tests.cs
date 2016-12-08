using System;
using GenericDotNetSupportLibrary.Helpers;
using Xunit;

namespace SupportLibraryTests.Helpers
{
  public class StringHelper45Tests
  {
    [Fact]
    public void ParseBoolTest()
    {
      Assert.False(StringHelper.ParseBool("false"));
      Assert.False(StringHelper.ParseBool("FALSE"));
      Assert.False(StringHelper.ParseBool("0"));
      Assert.True(StringHelper.ParseBool("true"));
      Assert.True(StringHelper.ParseBool("TRUE"));
      Assert.True(StringHelper.ParseBool("1"));
      Assert.False(StringHelper.ParseBool("Garbage"));
      Assert.False(StringHelper.ParseBool("      "));
      Assert.False(StringHelper.ParseBool(String.Empty));
      Assert.False(StringHelper.ParseBool(null));
    }
    [Fact]
    public void ParseIntTest()
    {
      string value = "1";
      int expected = 1;
      int actual;
      actual = StringHelper.ParseInt(value);
      Assert.Equal(expected, actual);

      value = "550";
      expected = 550;
      actual = StringHelper.ParseInt(value);
      Assert.Equal(expected, actual);

      value = "this contains no integer value";
      expected = 0;
      actual = StringHelper.ParseInt(value);
      Assert.Equal(expected, actual);

      value = "   ";
      expected = 0;
      actual = StringHelper.ParseInt(value);
      Assert.Equal(expected, actual);

      value = (null);
      expected = 0;
      actual = StringHelper.ParseInt(value);
      Assert.Equal(expected, actual);

      value = "1000000000000000";
      expected = 0;
      actual = StringHelper.ParseInt(value);
      Assert.Equal(expected, actual);
    }
    [Fact]
    public void ParseDoubleTest()
    {
      string value = "333333333333333";
      double expected = 333333333333333;
      double actual;
      actual = StringHelper.ParseDouble(value);
      Assert.Equal(expected, actual);

      value = "1.12";
      expected = 1.12;
      actual = StringHelper.ParseDouble(value);
      Assert.Equal(expected, actual);

      value = "this contains no integer value";
      expected = default(double);
      actual = StringHelper.ParseDouble(value);
      Assert.Equal(expected, actual);

      value = "   ";
      expected = default(double);
      actual = StringHelper.ParseDouble(value);
      Assert.Equal(expected, actual);

      value = (null);
      expected = default(double);
      actual = StringHelper.ParseDouble(value);
      Assert.Equal(expected, actual);
    }
    [Fact]
    public void ParseFloatTest()
    {
      string value = "4222.114";
      float expected = 4222.114F;
      float actual;
      actual = StringHelper.ParseFloat(value);
      Assert.Equal(expected, actual);

      value = "0.32145";
      expected = 0.32145F;
      actual = StringHelper.ParseFloat(value);
      Assert.Equal(expected, actual);

      value = "1234567891011";
      expected = 1234567891011F;
      actual = StringHelper.ParseFloat(value);
      Assert.Equal(expected, actual);

      value = "   ";
      expected = 0;
      actual = StringHelper.ParseFloat(value);
      Assert.Equal(expected, actual);

      value = (null);
      expected = 0;
      actual = StringHelper.ParseFloat(value);
      Assert.Equal(expected, actual);
    }
    [Fact]
    public void ParseNullableIntTest()
    {
      string value = (null);
      int? expected = default(int?);
      int? actual;
      actual = StringHelper.ParseNullableInt(value);
      Assert.Equal(expected, actual);

      value = "1";
      expected = 1;
      actual = StringHelper.ParseNullableInt(value);
      Assert.Equal(expected, actual);

      value = "550";
      expected = 550;
      actual = StringHelper.ParseNullableInt(value);
      Assert.Equal(expected, actual);

      value = "this contains no integer value";
      expected = default(int?);
      actual = StringHelper.ParseNullableInt(value);
      Assert.Equal(expected, actual);

      value = "   ";
      expected = default(int?);
      actual = StringHelper.ParseNullableInt(value);
      Assert.Equal(expected, actual);

      value = "1000000000000000";
      expected = default(int);
      actual = StringHelper.ParseInt(value);
      Assert.Equal(expected, actual);
    }
    [Fact]
    public void ParseDateTimeTest()
    {
      string value = "5/1/2008 8:30:52 AM";
      DateTime expected = new DateTime(2008, 5, 1, 8, 30, 52);
      DateTime actual;
      actual = StringHelper.ParseDateTime(value);
      Assert.Equal(expected, actual);

      expected = DateTime.Now;
      bool arb = DateTime.TryParse(expected.ToString(), out expected);
      value = expected.ToString();
      actual = StringHelper.ParseDateTime(value);
      Assert.Equal(expected.Ticks, actual.Ticks);

      value = "01";
      expected = default(DateTime);
      actual = StringHelper.ParseDateTime(value);
      Assert.Equal(expected, actual);

      value = "  ";
      expected = default(DateTime);
      actual = StringHelper.ParseDateTime(value);
      Assert.Equal(expected, actual);

      value = (null);
      expected = default(DateTime);
      actual = StringHelper.ParseDateTime(value);
      Assert.Equal(expected, actual);
    }
    [Fact]
    public void ParseNullableDateTimeTest()
    {
      string value = "5/1/2008 8:30:52 AM";
      DateTime? expected = new DateTime(2008, 5, 1, 8, 30, 52);
      DateTime? actual;
      actual = StringHelper.ParseNullableDateTime(value);
      Assert.Equal(expected, actual);
      
      expected = DateTime.Now;
      DateTime current = expected.GetValueOrDefault();
      bool arb = DateTime.TryParse(current.ToString(), out current);
      value = expected.ToString();
      actual = StringHelper.ParseNullableDateTime(value);
      Assert.Equal(current, actual);

      long longTime = DateTime.Now.Ticks;
      value = longTime.ToString();
      expected = new DateTime(longTime);
      actual = StringHelper.ParseNullableDateTime(value);
      Assert.Equal(expected, actual);

      value = "     ";
      expected = default(DateTime?);
      actual = StringHelper.ParseNullableDateTime(value);
      Assert.Equal(expected, actual);

      value = (null);
      expected = default(DateTime?);
      actual = StringHelper.ParseNullableDateTime(value);
      Assert.Equal(expected, actual);
    }
    [Fact]
    public void ShortenPathNameTest()
    {
      string value = @"C:\\MyDir\\AnotherDir\\YetAnotherDir\\FinalDir\\ThisDir";
      string expected = @"C:\...\ThisDir";
      int maximumLength = 20;
      string actual;
      actual = StringHelper.ShortenPathName(value, maximumLength);
      Assert.Equal(expected, actual);

      value = @"C:\\MyDir\\Dir";
      expected = @"C:\\MyDir\\Dir";
      actual = StringHelper.ShortenPathName(value, maximumLength);
      Assert.Equal(expected, actual);

      value = @"MyDir\\AnotherDir\\";
      expected = @"MyDir\\AnotherDir\\";
      actual = StringHelper.ShortenPathName(value, maximumLength);
      Assert.Equal(expected, actual);

    }
    [Fact]
    public void AddSpacesToSentenceTest()
    {
      string value = "ThisStringIsInNeedOfSomeSpaces";
      string expected = "This String Is In Need Of Some Spaces";
      string actual;
      actual = StringHelper.AddSpacesToSentence(value);
      Assert.Equal(expected, actual);

      value = "       ";
      expected = string.Empty;
      actual = StringHelper.AddSpacesToSentence(value);
      Assert.Equal(expected, actual);

      value = (null);
      expected = string.Empty;
      actual = StringHelper.AddSpacesToSentence(value);
      Assert.Equal(expected, actual);
    }
    [Fact]
    public void ToTitleCaseTest()
    {
      string value = "thiS is oUr tESt STRINg";
      string expected = "This Is Our Test String";
      string actual;
      actual = StringHelper.ToTitleCase(value);
      Assert.Equal(expected, actual);

      value = "012345";
      expected = "012345";
      actual = StringHelper.ToTitleCase(value);
      Assert.Equal(expected, actual);

      value = (null);
      expected = string.Empty;
      actual = StringHelper.ToTitleCase(value);
      Assert.Equal(expected, actual);

      value = "      ";
      expected = string.Empty;
      actual = StringHelper.ToTitleCase(value);
      Assert.Equal(expected, actual);

      value = string.Empty;
      expected = string.Empty;
      actual = StringHelper.ToTitleCase(value);
      Assert.Equal(expected, actual);
      
    }
    [Fact]
    public void GetHexStringFromByte()
    {
      byte[] value = { 255, 245, 45, 0, 17 };
      string expected = "FFF52D0011";
      string actual;
      actual = StringHelper.GetHexStringFromByte(value);
      Assert.Equal(expected, actual);

      value = (null);
      expected = string.Empty;
      actual = StringHelper.GetHexStringFromByte(value);
      Assert.Equal(expected, actual);

      byte[] another = {    };
      expected = string.Empty;
      actual = StringHelper.GetHexStringFromByte(another);
      Assert.Equal(expected, actual);
    }
    [Fact]
    public void IsAlphanumeric()
    {
      string value = @"AnyAlphaNumericString0";
      bool expected = true;
      bool actual;
      actual = StringHelper.IsAlphanumeric(value);
      Assert.Equal(expected, actual);

      value = @"01234568";
      expected = true;
      actual = StringHelper.IsAlphanumeric(value);
      Assert.Equal(expected, actual);

      value = @"S p a c e s";
      expected = false;
      actual = StringHelper.IsAlphanumeric(value);
      Assert.Equal(expected, actual);

      value = @"char$*(";
      expected = false;
      actual = StringHelper.IsAlphanumeric(value);
      Assert.Equal(expected, actual);

      value = @"     ";
      expected = false;
      actual = StringHelper.IsAlphanumeric(value);
      Assert.Equal(expected, actual);

      value = (null);
      expected = false;
      actual = StringHelper.IsAlphanumeric(value);
      Assert.Equal(expected, actual);

      value = string.Empty;
      expected = false;
      actual = StringHelper.IsAlphanumeric(value);
      Assert.Equal(expected, actual);
    }
    [Fact]
    public void ParseGuidTest()
    {
      string value = @"936DA01F-9ABD-4d9d-80C7-02AF85C822A8";
      Guid expected = new Guid("936DA01F-9ABD-4d9d-80C7-02AF85C822A8");
      Guid actual;
      actual = StringHelper.ParseGuid(value);
      Assert.Equal(expected, actual);

      value = @"Something-Else123";
      expected = Guid.Empty;
      actual = StringHelper.ParseGuid(value);
      Assert.Equal(expected, actual);

      value = @"    ";
      expected = Guid.Empty;
      actual = StringHelper.ParseGuid(value);
      Assert.Equal(expected, actual);

      value = string.Empty;
      expected = Guid.Empty;
      actual = StringHelper.ParseGuid(value);
      Assert.Equal(expected, actual);

      value = (null);
      expected = Guid.Empty;
      actual = StringHelper.ParseGuid(value);
      Assert.Equal(expected, actual);
    }
  }
}
