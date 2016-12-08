using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericDotNetSupportLibrary.Extensions.Environment;
using Xunit;

namespace GenericDotNetSupportLibrary.Tests.Extensions.Evironment
{
  public class ByteExtensionsTests
  {
    [Fact(DisplayName = "Byte Extensions Conversion Tests")]
    public void BytesConversionsTests()
    {
      UInt64 bytesAsGigabytes = (UInt64)(4 * Math.Pow(1024, 3));

      Assert.Equal(4, (double)bytesAsGigabytes.BytesToGigabytes(), 2);

      UInt64 bytesAsMegabytes = (UInt64)(2 * Math.Pow(1024, 2));

      Assert.Equal(2, (double)bytesAsMegabytes.BytesToMegabytes(), 2);
    }
  }
}
