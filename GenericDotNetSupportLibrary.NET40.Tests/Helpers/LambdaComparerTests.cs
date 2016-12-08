using System;
using System.Collections.Generic;
using System.Linq;
using GenericDotNetSupportLibrary.Helpers;
using Xunit;

namespace SupportLibraryTests.Helpers
{
  public class LambdaComparerTests
  {
    [Fact]
    public void LambdaComparerTest35()
    {
      List<TestObject> randomObjects = new List<TestObject>()
      {
        new TestObject(123), new TestObject(456), new TestObject(56), new TestObject(1), new TestObject(1000)
      };
      LambdaComparer<TestObject> testComparer = new LambdaComparer<TestObject>((first, second) =>
      {
        return first.Value.CompareTo(second.Value);
      });
      randomObjects.Sort(testComparer);

      Assert.True(randomObjects[0].Value == 1);
      Assert.True(randomObjects[4].Value == 1000);
      Assert.True(randomObjects[1].Value == 56);
      Assert.True(randomObjects.FirstOrDefault().Value == 1);


      randomObjects = new List<TestObject>()
      {
        new TestObject("123"), new TestObject("empty"), new TestObject(default(string)), new TestObject("apple"), new TestObject("doubt")
      };
      testComparer = new LambdaComparer<TestObject>((first, second) =>
      {
        if (String.IsNullOrEmpty(first.StringValue) && String.IsNullOrEmpty(second.StringValue))
          return 0;
        else if (String.IsNullOrEmpty(first.StringValue))
          return -1;
        else if (String.IsNullOrEmpty(second.StringValue))
          return 1;
        else
          return first.StringValue.LastOrDefault().CompareTo(second.StringValue.LastOrDefault());
      });
      randomObjects.Sort(testComparer);

      Assert.Equal(randomObjects.LastOrDefault().StringValue, "empty");
      Assert.True(randomObjects.FirstOrDefault().StringValue == default(string));
      Assert.Equal(randomObjects[1].StringValue, "123");
    }
    [Fact]
    public void LambdaEqualityComparerTest()
    {
      List<TestObject> randomObjects = new List<TestObject>()
      {
        new TestObject(123), new TestObject(456), new TestObject(null), new TestObject(1), new TestObject(1000)
      };
      List<TestObject> moreObjects = new List<TestObject>()
      {
        new TestObject(123), new TestObject(456), new TestObject(null), new TestObject(1), new TestObject(1000)
      };
      LambdaEqualityComparer<TestObject> testComparer = new LambdaEqualityComparer<TestObject>((first, second) =>
      {
        return first.Value.Equals(second.Value);
      });
      Assert.True(testComparer.Equals(randomObjects.FirstOrDefault(), moreObjects.FirstOrDefault()));
      Assert.True(testComparer.Equals(randomObjects.LastOrDefault(), moreObjects.LastOrDefault()));
      Assert.True(testComparer.Equals(randomObjects[2], moreObjects[2]));

      randomObjects = new List<TestObject>()
      {
        new TestObject("123"), new TestObject("empty"), new TestObject(default(string)), new TestObject("apple"), new TestObject("doubt")
      };
      moreObjects = new List<TestObject>()
      {
        new TestObject("empty"), new TestObject("123"), new TestObject("apple"), new TestObject("doubt"), new TestObject(default(string))
      };
      LambdaComparer<TestObject> lambdaComparer = new LambdaComparer<TestObject>((first, second) =>
      {
        if (String.IsNullOrEmpty(first.StringValue) && String.IsNullOrEmpty(second.StringValue))
          return 0;
        else if (String.IsNullOrEmpty(first.StringValue))
          return 1;
        else if (String.IsNullOrEmpty(second.StringValue))
          return -1;
        else
          return first.StringValue.CompareTo(second.StringValue);
      });
      randomObjects.Sort(lambdaComparer);
      moreObjects.Sort(lambdaComparer);

      testComparer = new LambdaEqualityComparer<TestObject>((first, second) =>
      {
        if (first.StringValue == default(string) && second.StringValue == default(string))
          return true;
        return first.StringValue.Equals(second.StringValue);
      });
      Assert.True(testComparer.Equals(randomObjects.FirstOrDefault(), moreObjects.FirstOrDefault()));
      Assert.True(testComparer.Equals(randomObjects.LastOrDefault(), moreObjects.LastOrDefault()));
      Assert.True(testComparer.Equals(randomObjects[2], moreObjects[2]));
    }
    class TestObject
    {
      public int Value { get; set; }
      public string StringValue { get; set; }
      public TestObject(int value)
      {
        this.Value = value;
      }
      public TestObject(string stringValue)
      {
        this.StringValue = stringValue;
      }
    }
  }
}
