using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace GenericDotNetSupportLibrary.Helpers
{
  /// <summary>
  /// Contains functionality for dealing with the Windows Registry
  /// </summary>
  public static partial class RegistryHelper
  {
    /// <summary>
    /// Reads a value from the base key HKEY_LOCAL_MACHINE in the registry
    /// </summary>
    /// <param name="keyPath">path to the key</param>
    /// <param name="valueName">the name of the value in the key to read</param>
    /// <returns>the value of the named value in the registry key</returns>
    public static string ReadFromHKLM(string keyPath, string valueName)
    {
      return RegistryHelper.ReadFromHKLM(keyPath, valueName, true);
    }

    /// <summary>
    /// Reads a value from the base key HKEY_LOCAL_MACHINE in the registry
    /// </summary>
    /// <param name="keyPath">path to the key</param>
    /// <param name="valueName">the name of the value in the key to read</param>
    /// <param name="read64bit">read from the registry in 64bit mode</param>
    /// <returns>the value of the named value in the registry key</returns>
    public static string ReadFromHKLM(string keyPath, string valueName, bool read64bit)
    {
      string value = null;
      RegistryKey localKey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, read64bit ? RegistryView.Registry64 : RegistryView.Default);

      if (localKey != null)
      {
        RegistryKey subKey = localKey.OpenSubKey(keyPath);

        object valueFromKey = null;

        if (subKey != null)
          valueFromKey = subKey.GetValue(valueName);

        if (valueFromKey != null && valueFromKey is string)
          value = valueFromKey as string;
        else if (valueFromKey != null)
          value = valueFromKey.ToString();

      }
      return value;
    }

    public static IEnumerable<string> ReadSubKeyNamesFromHKLM(string parentKeyPath)
    {
      return ReadSubKeyNamesFromHKLM(parentKeyPath, true);
    }

    public static IEnumerable<string> ReadSubKeyNamesFromHKLM(string parentKeyPath, bool read64Bit)
    {
      RegistryKey localKey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, read64Bit ? RegistryView.Registry64 : RegistryView.Default);

      List<string> keyNames = new List<string>();

      if (localKey != null)
      {
        RegistryKey parentKey = localKey.OpenSubKey(parentKeyPath);

        if (parentKey != null)
          keyNames = new List<string>(parentKey.GetSubKeyNames());
      }

      return keyNames;
    }

    public static RegistryKey LoadSubKeyFromHKLM(string subKeyPath)
    {
      return LoadSubKeyFromHKLM(subKeyPath, true);
    }

    public static RegistryKey LoadSubKeyFromHKLM(string subKeyPath, bool read64Bit)
    {
      RegistryKey localKey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, read64Bit ? RegistryView.Registry64 : RegistryView.Default);

      List<string> keyNames = new List<string>();

      RegistryKey subKey = null;

      if (localKey != null)
      {
        subKey = localKey.OpenSubKey(subKeyPath, true);
      }

      return subKey;
    }

    public static RegistryKey CreateSubKeyInHKLM(string subKeyPath)
    {
      return CreateSubKeyInHKLM(subKeyPath, true);
    }

    public static RegistryKey CreateSubKeyInHKLM(string subKeyPath, bool read64Bit)
    {
      RegistryKey localKey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, read64Bit ? RegistryView.Registry64 : RegistryView.Default);

      return localKey.CreateSubKey(subKeyPath);
    }

    /// <summary>
    /// Writes to the a key in the base key HKEY_LOCAL_MACHINE in the registry
    /// </summary>
    /// <param name="keyPath">path to the key</param>
    /// <param name="valueName">the name of the named value in the key to write</param>
    /// <param name="value">value to write into the registry key</param>
    public static void WriteToHKLM(string keyPath, string valueName, string value)
    {
      RegistryHelper.WriteToHKLM(keyPath, valueName, value, true);
    }

    /// <summary>
    /// Writes to the a key in the base key HKEY_LOCAL_MACHINE in the registry
    /// </summary>
    /// <param name="keyPath">path to the key</param>
    /// <param name="valueName">the name of the named value in the key to write</param>
    /// <param name="value">value to write into the registry key</param>
    /// <param name="registyViewIs64bit">the mode in which the registry should be viewed</param>
    public static void WriteToHKLM(string keyPath, string valueName, string value, bool registyViewIs64bit)
    {
      if (value == null) return;

      RegistryKey localKey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, registyViewIs64bit ? RegistryView.Registry64 : RegistryView.Default);

      if (localKey != null)
      {
        RegistryKey subKey = Registry.LocalMachine.OpenSubKey(keyPath, true);

        if (subKey == null)
        {
          localKey.CreateSubKey(keyPath);
          subKey = Registry.LocalMachine.OpenSubKey(keyPath, true);
        }

        if (subKey != null)
        {
          subKey.SetValue(valueName, value, RegistryValueKind.String);
        }

        subKey.Dispose();
      }
    }

    /// <summary>
    /// Reads a value from the base key HKEY_CURRENT_USER in the registry
    /// </summary>
    /// <param name="keyPath">path to the key</param>
    /// <param name="valueName">the name of the value in the key to read</param>
    /// <param name="value">value to write into the registry key</param>
    public static void WriteToHKCU(string keyPath, string valueName, string value)
    {
      RegistryHelper.WriteToHKCU(keyPath, valueName, value, true);
    }

    /// <summary>
    /// Reads a value from the base key HKEY_CURRENT_USER in the registry
    /// </summary>
    /// <param name="keyPath">path to the key</param>
    /// <param name="valueName">the name of the value in the key to read</param>
    /// <param name="value">value to write into the registry key</param>
    /// <param name="read64bit">read from the registry in 64bit mode</param>
    public static void WriteToHKCU(string keyPath, string valueName, string value, bool registyViewIs64bit)
    {
      if (value == null) return;

      RegistryKey localKey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.CurrentUser, registyViewIs64bit ? RegistryView.Registry64 : RegistryView.Default);


      if (localKey != null)
      {
        RegistryKey subKey = localKey.OpenSubKey(keyPath);

        if (subKey == null)
          subKey = localKey.CreateSubKey(keyPath);

        if (subKey != null)
        {
          subKey.SetValue(valueName, value, RegistryValueKind.String);
          subKey.Close();
        }
      }
    }
  }
}
