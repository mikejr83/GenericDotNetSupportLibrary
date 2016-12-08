using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Globalization;

namespace GenericDotNetSupportLibrary.Helpers
{
  /// <summary>
  /// Helper methods for dealing with various Windows process tasks.
  /// <remarks>
  /// This helper class contains methods for invoking Windows processes to start various tasks for the user.  These tasks include opening Notepad or an email window.
  /// </remarks>
  /// </summary>
  public class ProcessHelper
  {
    const string NOTEPAD_LOCATION = "";
    const string SUPPLEMENTAL_INSTALLER_ARGS = "/S /INSTDIR={0} /CAMConfigLocation={1}";

    /// <summary>
    /// Starts a new Notepad process with a specified file opened.
    /// </summary>
    /// <param name="fileToOpen">The filename to be passed to Notepad to be opened when the process is launched.</param>
    public static void StartNewNotepadProcess(string fileToOpen)
    {
      ProcessStartInfo processInfo = new ProcessStartInfo();
      //processInfo.Verb = "runas";
      processInfo.UseShellExecute = true;
      processInfo.RedirectStandardOutput = false;
      processInfo.WorkingDirectory = Environment.CurrentDirectory;

      processInfo.FileName = "C:\\Windows\\notepad.exe";
      processInfo.Arguments = "\"" + fileToOpen + "\"";

      Process.Start(processInfo).Dispose();
    }

    /// <summary>
    /// Starts a process with a mailto link which will cause the system to open the default email window.
    /// </summary>
    public static void StartEmailLogProcess(string logFileName)
    {
      StringBuilder sb = new StringBuilder();

     

      sb.Append("&Attach=\"" + logFileName + "\"");

      ProcessStartInfo processInfo = new ProcessStartInfo();
      processInfo.UseShellExecute = true;
      processInfo.RedirectStandardOutput = false;
      processInfo.WorkingDirectory = Environment.CurrentDirectory;
      processInfo.FileName = sb.ToString();

      Process p = Process.Start(processInfo);
      if (p != null)
        p.Dispose();
    }

    /// <summary>
    /// Emails the contents of the Output window in the AMS.
    /// </summary>
    /// <param name="outputLines">The lines from the Output window.</param>
    public static void EmailOutput(string outputLines)
    {
      StringBuilder sb = new StringBuilder();

      sb.Append("mailto:");
      sb.Append("&subject=Analytics Management Studio Output");
      sb.Append("&body=");

      sb.Append(Uri.EscapeDataString("Contents of AMS Output Window:\n\n" + outputLines));

      ProcessStartInfo processInfo = new ProcessStartInfo();
      processInfo.UseShellExecute = true;
      processInfo.RedirectStandardOutput = false;
      processInfo.WorkingDirectory = Environment.CurrentDirectory;
      processInfo.FileName = sb.ToString();

      Process p = Process.Start(processInfo);

      if (p != null)
        p.Dispose();
    }

    public static Process GetSupplementalInstallerProcess(string path, string cognosInstallDir, string camConfigLocation)
    {
      FileInfo installerFileInfo = new FileInfo(path);

      ProcessStartInfo processInfo = new ProcessStartInfo();
      processInfo.Verb = "runas";
      processInfo.UseShellExecute = true;
      processInfo.RedirectStandardOutput = false;
      processInfo.WorkingDirectory = installerFileInfo.DirectoryName;

      processInfo.FileName = path;
      processInfo.Arguments = string.Format(CultureInfo.CurrentUICulture, SUPPLEMENTAL_INSTALLER_ARGS, cognosInstallDir, camConfigLocation);

      Process sP = new Process()
      {
        StartInfo = processInfo
      };

      return sP;
    }

    public static void ExecuteProcessInHiddenWindow(string command, string args)
    {
      ProcessHelper.ExecuteProcessInHiddenWindow(command, args, string.Empty);
    }

    public static void ExecuteProcessInHiddenWindow(string command, string args, string workingDirectory)
    {
      ProcessHelper.ExecuteProcessInHiddenWindow(command, args, workingDirectory, null);
    }

    public static void ExecuteProcessInHiddenWindow(string command, string args, string workingDirectory, Action<string> handleStandardOuput)
    {
      Process p = new Process();
      ProcessStartInfo pInfo = new ProcessStartInfo();
      pInfo.CreateNoWindow = true;
      pInfo.WindowStyle = ProcessWindowStyle.Hidden;
      pInfo.ErrorDialog = false;
      pInfo.UseShellExecute = false;
      pInfo.FileName = command;
      pInfo.Arguments = args;
      if (workingDirectory.Length > 0)
      {
        pInfo.WorkingDirectory = workingDirectory;
      }
      pInfo.RedirectStandardOutput = true;
      p.StartInfo = pInfo;
      p.Start();
      if (handleStandardOuput != null)
      {
        StreamReader reader = p.StandardOutput;
        while (!reader.EndOfStream)
        {
          handleStandardOuput(reader.ReadLine().ToString());
        }
      }
      p.Close();
    }
  }
}
