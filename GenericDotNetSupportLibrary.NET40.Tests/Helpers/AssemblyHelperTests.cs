using System;
using GenericDotNetSupportLibrary.Helpers;
using Xunit;

namespace SupportLibraryTests.Helpers
{
  public class AssemblyHelperTests
  {
    [Fact]
    public void FindAssemblyIdentifierTest()
    {
      Assert.Equal<Guid>(AssemblyHelper.FindAssemblyIdentifier(typeof(AssemblyHelperTests).Assembly), new Guid("b24abba5-612d-4b2c-9f05-46262e2631d3"));
    }
  }
}
