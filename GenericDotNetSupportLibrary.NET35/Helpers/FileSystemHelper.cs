using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GenericDotNetSupportLibrary.Helpers
{
  /// <summary>
  /// Provides file system helper functions
  /// </summary>
  public static class FileSystemHelper
  {
    /// <summary>
    /// Creates and returns the filename of a temporary directory.
    /// </summary>
    /// <returns>Filename path to a temporary directory.</returns>
    public static string GetTempDirectory()
    {
      string path = Path.GetRandomFileName();

      path = Path.Combine(Path.GetTempPath(), path);

      System.IO.Directory.CreateDirectory(path);

      return path;
    }

    /// <summary>
    /// When a directory is moved to a destination on the same volume, .Net's normal System.IO.Directory.Move command is used 
    /// to move the directory. If the source and destination is not on the same volume the files together with the sub-directories 
    /// are copied to the destination directory. The source directory gets deleted after the files were successfully copied.
    /// </summary>
    /// <remarks>
    /// Folder Mover enables a user to move folders/directories between different volumes/partitions. 
    /// The .Net System.IO.Directory.Move throws an IOException when trying to move folders across volumes/partitions.
    /// It's developed in C#.Net 3.5 & works with network shares as well
    /// 
    /// Problem
    /// The default behavior of .Net's System.IO.Directory.Move is for it to throw an IOException when the volume/partition's 
    /// not the same for the source and destination paths. Directories cannot easily be moved from one partition to another.
    /// 
    /// Inner working
    /// When a directory is moved to a destination on the same volume, .Net's normal System.IO.Directory.Move command is used 
    /// to move the directory. If the source and destination is not on the same volume the files together with the sub-directories 
    /// are copied to the destination directory. The source directory gets deleted after the files were successfully copied.
    /// 
    /// Copyright: jdstuart - Code used as GPL v2 - 04/10/2013
    /// Project URL: https://foldermover.codeplex.com/
    /// </remarks>
    public static class Directory
    {
      /// <summary>
      /// Move a directory to a new location. Allows for directories to be moved across volumes/partitions.
      /// </summary>
      /// <param name="sourceDirName">The directory which should be moved.</param>
      /// <param name="destDirName">The destination where the directory should be moved to. This should include the new directory name.</param>
      /// <exception cref="System.ArgumentNullException">When either <paramref name="sourceDirName"/> or <paramref name="destDirName"/> is null or empty.</exception>
      /// <exception cref="System.ArgumentException">When the <paramref name="sourceDirName"/> or <paramref name="destDirName"/> contains one or more invalid characters as defined by System.IO.Path.GetInvalidPathChars().</exception>
      /// <exception cref="System.IO.PathTooLongException">The specified path, file name or both exceed the system-defined maximum length.</exception>
      /// <exception cref="System.IO.IOException">destDirName already exists.</exception>
      /// <exception cref="System.UnauthorizedAccessException">The caller doesn't have the required permission.</exception>
      public static void Move(string sourceDirName, string destDirName)
      {
        #region Validation checks
        if (null == sourceDirName) { throw new ArgumentNullException("sourceDirName", "The source directory cannot be null."); }
        if (null == destDirName) { throw new ArgumentNullException("destDirName", "The destination directory cannot be null."); }

        sourceDirName = sourceDirName.Trim();
        destDirName = destDirName.Trim();

        if ((sourceDirName.Length == 0) || (destDirName.Length == 0)) { throw new ArgumentException("sourceDirName or destDirName is a zero-length string."); }

        char[] invalidChars = System.IO.Path.GetInvalidPathChars();
        if (sourceDirName.IndexOfAny(invalidChars) >= 0) { throw new ArgumentException("The directory contains invalid path characters.", "sourceDirName"); }
        if (destDirName.IndexOfAny(invalidChars) >= 0) { throw new ArgumentException("The directory contains invalid path characters.", "destDirName"); }

        DirectoryInfo sourceDir = new DirectoryInfo(sourceDirName);
        DirectoryInfo destDir = new DirectoryInfo(destDirName);

        if (!sourceDir.Exists) { throw new DirectoryNotFoundException("The path specified by sourceDirName is invalid: " + sourceDirName); }
        if (destDir.Exists) { throw new IOException("The path specified by destDirName already exists: " + destDirName); }
        #endregion


        if (sourceDir.Root.Name.Equals(destDir.Root.Name, StringComparison.InvariantCultureIgnoreCase))
        {
          System.IO.Directory.Move(sourceDirName, destDirName);
        }
        else
        {
          System.IO.Directory.CreateDirectory(destDirName);

          //Copy the files in the current directory.
          FileInfo[] files = sourceDir.GetFiles();
          foreach (FileInfo file in files)
          {
            string newPath = Path.Combine(destDirName, file.Name);
            file.CopyTo(newPath);
          }

          //Copy all sub directories.
          DirectoryInfo[] subDirs = sourceDir.GetDirectories();
          foreach (DirectoryInfo subDir in subDirs)
          {
            string newPath = Path.Combine(destDirName, subDir.Name);
            FileSystemHelper.Directory.Move(subDir.FullName, newPath);
          }

          System.IO.Directory.Delete(sourceDirName, true);
        }
      }
    }
  }
}
