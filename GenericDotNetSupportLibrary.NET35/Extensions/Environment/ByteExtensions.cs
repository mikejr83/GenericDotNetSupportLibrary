using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericDotNetSupportLibrary.Extensions.Environment
{
  public static class ByteExtensions
  {
    public static double BytesToKilobytes(this UInt64 bytes)
    {
      return bytes / 1024d;
    }

    public static double BytesToMegabytes(this UInt64 bytes)
    {
      return bytes / 1024d / 1024d;
    }

    public static double BytesToGigabytes(this UInt64 bytes)
    {
      return bytes / 1024d / 1024d / 1024d;
    }

    public static double BytesToKilobytes(this Double bytes)
    {
      return bytes / 1024d;
    }

    public static double BytesToMegabytes(this Double bytes)
    {
      return bytes / 1024d / 1024d;
    }

    public static double BytesToGigabytes(this Double bytes)
    {
      return bytes / 1024d / 1024d / 1024d;
    }
  }
}
