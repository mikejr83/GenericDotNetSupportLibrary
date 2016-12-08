using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GenericDotNetSupportLibrary.Helpers
{
  /// <summary>
  /// Utilities for dealing with files.
  /// </summary>
  public class FileHelper
  {
    /// <summary>
    /// Reads a file as a string, ignoring locks held by other programs.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string ReadAllText(string path)
    {
      string contents = null;
      using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
      {
        using (StreamReader sr = new StreamReader(fs, true))
        {
          contents = sr.ReadToEnd();
        }
      }
      return contents;
    }
  }
}
