using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace GenericDotNetSupportLibrary.Helpers
{
  public static class EnvironmentHelper
  {
    public enum Platform
    {
      X86,
      X64,
      IA64,
      Unknown
    }

    public static MemoryStatus CurrentMachineMemoryStatus()
    {
      MemoryNativeMethods.MEMORYSTATUSEX memStatus = new MemoryNativeMethods.MEMORYSTATUSEX();
      if (MemoryNativeMethods.GlobalMemoryStatusEx(memStatus))
      {
        return new MemoryStatus(memStatus.ullTotalPhys, memStatus.ullAvailPhys, memStatus.ullTotalPageFile, memStatus.ullAvailPageFile,
          memStatus.ullTotalVirtual, memStatus.ullAvailVirtual, memStatus.ullAvailExtendedVirtual);
      }

      throw new InvalidOperationException();
    }

    public static LogicalProcessorInformation CurrentMachineLogicalProcessorInformation()
    {
      return new LogicalProcessorInformation(ProcessorNativeMethods.CurrentMachineLogicalProcessorInformation());
    }

    public static Platform CurrentMachinePlatform()
    {
      return PlatformNativeMethods.GetPlatform();
    }

    #region NativeMethods Classes
    internal static class MemoryNativeMethods
    {
      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
      public class MEMORYSTATUSEX
      {
        public uint dwLength;
        public uint dwMemoryLoad;
        public ulong ullTotalPhys;
        public ulong ullAvailPhys;
        public ulong ullTotalPageFile;
        public ulong ullAvailPageFile;
        public ulong ullTotalVirtual;
        public ulong ullAvailVirtual;
        public ulong ullAvailExtendedVirtual;
        public MEMORYSTATUSEX()
        {
          this.dwLength = (uint)Marshal.SizeOf(typeof(MemoryNativeMethods.MEMORYSTATUSEX));
        }
      }



      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
      public static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);


    }

    internal static class ProcessorNativeMethods
    {
      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
      public struct PROCESSORCORE
      {
        public byte Flags;
      };

      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
      public struct NUMANODE
      {
        public uint NodeNumber;
      }

      public enum PROCESSOR_CACHE_TYPE
      {
        CacheUnified,
        CacheInstruction,
        CacheData,
        CacheTrace
      }

      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
      public struct CACHE_DESCRIPTOR
      {
        public byte Level;
        public byte Associativity;
        public ushort LineSize;
        public uint Size;
        public PROCESSOR_CACHE_TYPE Type;
      }

      [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Auto)]
      public struct SYSTEM_LOGICAL_PROCESSOR_INFORMATION_UNION
      {
        [FieldOffset(0)]
        public PROCESSORCORE ProcessorCore;
        [FieldOffset(0)]
        public NUMANODE NumaNode;
        [FieldOffset(0)]
        public CACHE_DESCRIPTOR Cache;
        [FieldOffset(0)]
        private UInt64 Reserved1;
        [FieldOffset(8)]
        private UInt64 Reserved2;
      }

      public enum LOGICAL_PROCESSOR_RELATIONSHIP
      {
        RelationProcessorCore,
        RelationNumaNode,
        RelationCache,
        RelationProcessorPackage,
        RelationGroup,
        RelationAll = 0xffff
      }

      public struct SYSTEM_LOGICAL_PROCESSOR_INFORMATION
      {
        public UIntPtr ProcessorMask;
        public LOGICAL_PROCESSOR_RELATIONSHIP Relationship;
        public SYSTEM_LOGICAL_PROCESSOR_INFORMATION_UNION ProcessorInformation;
      }

      [DllImport(@"kernel32.dll", SetLastError = true)]
      public static extern bool GetLogicalProcessorInformation(
          IntPtr Buffer,
          ref uint ReturnLength
      );

      private const int ERROR_INSUFFICIENT_BUFFER = 122;

      public static SYSTEM_LOGICAL_PROCESSOR_INFORMATION[] CurrentMachineLogicalProcessorInformation()
      {
        uint ReturnLength = 0;
        GetLogicalProcessorInformation(IntPtr.Zero, ref ReturnLength);
        if (Marshal.GetLastWin32Error() == ERROR_INSUFFICIENT_BUFFER)
        {
          IntPtr Ptr = Marshal.AllocHGlobal((int)ReturnLength);
          try
          {
            if (GetLogicalProcessorInformation(Ptr, ref ReturnLength))
            {
              int size = Marshal.SizeOf(typeof(SYSTEM_LOGICAL_PROCESSOR_INFORMATION));
              int len = (int)ReturnLength / size;
              SYSTEM_LOGICAL_PROCESSOR_INFORMATION[] Buffer = new SYSTEM_LOGICAL_PROCESSOR_INFORMATION[len];
              IntPtr Item = Ptr;
              for (int i = 0; i < len; i++)
              {
                Buffer[i] = (SYSTEM_LOGICAL_PROCESSOR_INFORMATION)Marshal.PtrToStructure(Item, typeof(SYSTEM_LOGICAL_PROCESSOR_INFORMATION));
                Item = new IntPtr(Item.ToInt32() + size);
              }
              return Buffer;
            }
          }
          finally
          {
            Marshal.FreeHGlobal(Ptr);
          }
        }
        return null;
      }
    }

    internal static class PlatformNativeMethods
    {
      internal const ushort PROCESSOR_ARCHITECTURE_INTEL = 0;
      internal const ushort PROCESSOR_ARCHITECTURE_IA64 = 6;
      internal const ushort PROCESSOR_ARCHITECTURE_AMD64 = 9;
      internal const ushort PROCESSOR_ARCHITECTURE_UNKNOWN = 0xFFFF;

      [StructLayout(LayoutKind.Sequential)]
      internal struct SYSTEM_INFO
      {
        public ushort wProcessorArchitecture;
        public ushort wReserved;
        public uint dwPageSize;
        public IntPtr lpMinimumApplicationAddress;
        public IntPtr lpMaximumApplicationAddress;
        public UIntPtr dwActiveProcessorMask;
        public uint dwNumberOfProcessors;
        public uint dwProcessorType;
        public uint dwAllocationGranularity;
        public ushort wProcessorLevel;
        public ushort wProcessorRevision;
      };

      [DllImport("kernel32.dll")]
      internal static extern void GetNativeSystemInfo(ref SYSTEM_INFO lpSystemInfo);

      [DllImport("kernel32.dll")]
      internal static extern void GetSystemInfo(ref SYSTEM_INFO lpSystemInfo);

      public static Platform GetPlatform()
      {
        SYSTEM_INFO sysInfo = new SYSTEM_INFO();

        if (System.Environment.OSVersion.Version.Major > 5 ||
      (System.Environment.OSVersion.Version.Major == 5 && System.Environment.OSVersion.Version.Minor >= 1))
        {
          GetNativeSystemInfo(ref sysInfo);
        }
        else
        {
          GetSystemInfo(ref sysInfo);
        }

        switch (sysInfo.wProcessorArchitecture)
        {
          case PROCESSOR_ARCHITECTURE_IA64:
            return Platform.IA64;

          case PROCESSOR_ARCHITECTURE_AMD64:
            return Platform.X64;

          case PROCESSOR_ARCHITECTURE_INTEL:
            return Platform.X86;

          default:
            return Platform.Unknown;
        }
      }
    }
    #endregion
  }

  public class MemoryStatus
  {
    public ulong TotalPhysical { get; private set; }
    public ulong AvailablePhysical { get; private set; }
    public ulong TotalPageFile { get; private set; }
    public ulong AvailablePageFile { get; private set; }
    public ulong TotalVirtual { get; private set; }
    public ulong AvailableVirtual { get; private set; }
    public ulong AvailableExtendedVirtual { get; private set; }

    public MemoryStatus(ulong totalPhysical, ulong availablePhysical, ulong totalPageFile, ulong availablePageFile, ulong totalVirtual, ulong availableVirtual, ulong availableExtendedVirtual)
    {
      this.TotalPhysical = totalPhysical;
      this.AvailablePhysical = availablePhysical;
      this.TotalPageFile = totalPageFile;
      this.AvailablePageFile = availablePageFile;
      this.TotalVirtual = totalVirtual;
      this.AvailableVirtual = availableVirtual;
      this.AvailableExtendedVirtual = availableExtendedVirtual;
    }
  }

  public class LogicalProcessorInformation
  {
    public int PhysicalProcessorSockets { get; private set; }
    public int ProcessorCores { get; private set; }
    public int LogicalProcessors { get; private set; }

    internal LogicalProcessorInformation(EnvironmentHelper.ProcessorNativeMethods.SYSTEM_LOGICAL_PROCESSOR_INFORMATION[] information)
    {
      foreach (EnvironmentHelper.ProcessorNativeMethods.SYSTEM_LOGICAL_PROCESSOR_INFORMATION info in information)
      {
        switch (info.Relationship)
        {
          case EnvironmentHelper.ProcessorNativeMethods.LOGICAL_PROCESSOR_RELATIONSHIP.RelationProcessorCore:
            this.ProcessorCores++;

            int lshift = sizeof(ulong) * 8 - 1;
            ulong bitTest = (ulong)1 << lshift;
            int bitSetCount = 0;
            for (int i = 0; i <= lshift; i++)
            {
              bitSetCount += ((info.ProcessorMask.ToUInt64() & bitTest) > 0 ? 1 : 0);
              bitTest /= 2;
            }

            this.LogicalProcessors += bitSetCount;
            break;

          case EnvironmentHelper.ProcessorNativeMethods.LOGICAL_PROCESSOR_RELATIONSHIP.RelationProcessorPackage:
            this.PhysicalProcessorSockets++;
            break;
        }
      }
    }
  }
}
