﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace GenericDotNetSupportLibrary.Helpers
{
  public enum RegSAM
  {
    QueryValue = 0x0001,
    SetValue = 0x0002,
    CreateSubKey = 0x0004,
    EnumerateSubKeys = 0x0008,
    Notify = 0x0010,
    CreateLink = 0x0020,
    WOW64_32Key = 0x0200,
    WOW64_64Key = 0x0100,
    WOW64_Res = 0x0300,
    Read = 0x00020019,
    Write = 0x00020006,
    Execute = 0x00020019,
    AllAccess = 0x000f003f
  }

  

  public static partial class RegistryHelper
  {
    public static class RegHive
    {
      public static UIntPtr HKEY_LOCAL_MACHINE = new UIntPtr(0x80000002u);
      public static UIntPtr HKEY_CURRENT_USER = new UIntPtr(0x80000001u);
    }



    #region Functions
    static public string GetRegKey64(UIntPtr inHive, String inKeyName, String inPropertyName)
    {
      return GetRegKey64(inHive, inKeyName, RegSAM.WOW64_64Key, inPropertyName);
    }

    static public string GetRegKey32(UIntPtr inHive, String inKeyName, String inPropertyName)
    {
      return GetRegKey64(inHive, inKeyName, RegSAM.WOW64_32Key, inPropertyName);
    }

    static public string GetRegKey64(UIntPtr inHive, String inKeyName, RegSAM in32or64key, String inPropertyName)
    {
      //UIntPtr HKEY_LOCAL_MACHINE = (UIntPtr)0x80000002;
      int hkey = 0;

      try
      {
        uint lResult = RegistryHelperNativeMethods.RegOpenKeyEx(RegHive.HKEY_LOCAL_MACHINE, inKeyName, 0, (int)RegSAM.QueryValue | (int)in32or64key, out hkey);
        if (0 != lResult) return null;
        uint lpType = 0;
        uint lpcbData = 1024;
        StringBuilder AgeBuffer = new StringBuilder(1024);
        RegistryHelperNativeMethods.RegQueryValueEx(hkey, inPropertyName, 0, ref lpType, AgeBuffer, ref lpcbData);
        string Age = AgeBuffer.ToString();
        return Age;
      }
      finally
      {
        if (0 != hkey) RegistryHelperNativeMethods.RegCloseKey(hkey);
      }
    }
    #endregion

    #region Enums
    #endregion
  }

  internal static class RegistryHelperNativeMethods
  {
    #region Member Variables
    #region Read 64bit Reg from 32bit app
    [DllImport("Advapi32.dll", CharSet=CharSet.Unicode)]
    internal static extern uint RegOpenKeyEx(
        UIntPtr hKey,
        string lpSubKey,
        uint ulOptions,
        int samDesired,
        out int phkResult);

    [DllImport("Advapi32.dll")]
    internal static extern uint RegCloseKey(int hKey);

    [DllImport("advapi32.dll", EntryPoint = "RegQueryValueEx", CharSet = CharSet.Unicode)]
    internal static extern int RegQueryValueEx(
        int hKey, string lpValueName,
        int lpReserved,
        ref uint lpType,
        System.Text.StringBuilder lpData,
        ref uint lpcbData);
    #endregion
    #endregion
  }
}
