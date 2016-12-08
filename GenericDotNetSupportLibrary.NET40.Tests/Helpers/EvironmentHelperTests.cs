using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericDotNetSupportLibrary.Helpers;
using Xunit;

namespace SupportLibraryTests.Helpers
{
  public class EvironmentHelperTests
  {
    [Fact(DisplayName = "Current Machine Memory Status")]
    public void CurrentMachineMemoryStatusTest()
    {

      MemoryStatus memStatus = null;

      Assert.DoesNotThrow(new Assert.ThrowsDelegate(() =>
        {
          memStatus = EnvironmentHelper.CurrentMachineMemoryStatus();
        }));

      Assert.NotNull(memStatus);
      Assert.True(memStatus.AvailablePageFile > 0);
      Assert.True(memStatus.AvailablePhysical > 0);
      Assert.True(memStatus.AvailableVirtual > 0);
      Assert.True(memStatus.TotalPageFile > 0);
      Assert.True(memStatus.TotalPhysical > 0);
      Assert.True(memStatus.TotalVirtual > 0);



    }

    [Fact(DisplayName = "Current Machine Logical Processor Information Test")]
    public void CurrentMachineLogicalProcessorInformationTest()
    {
      LogicalProcessorInformation processorInformation = null;

      Assert.DoesNotThrow(new Assert.ThrowsDelegate(() =>
      {
        processorInformation = EnvironmentHelper.CurrentMachineLogicalProcessorInformation();
      }));
    }

    [Fact(DisplayName = "Current Machine Platform")]
    public void CurrentMachinePlatformTest()
    {
      EnvironmentHelper.Platform platform = EnvironmentHelper.Platform.Unknown;

      Assert.DoesNotThrow(new Assert.ThrowsDelegate(() =>
      {
        platform = EnvironmentHelper.CurrentMachinePlatform();
      }));

      Assert.NotEqual(EnvironmentHelper.Platform.Unknown, platform);
    }
  }
}
