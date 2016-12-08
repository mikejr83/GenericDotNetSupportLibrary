using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Windows;

namespace GenericDotNetSupportLibrary.Helpers
{
  /// <summary>
  /// This set of helper functions provide methods to open specific types of Win32 dialogs dealing with the saving and opening of files.
  /// </summary>
  public static class DialogHelper
  {
    /// <summary>
    /// Default message displayed when changes have occured and have not yet been saved.
    /// </summary>
    public const string DEFAULT_SAVE_MESSAGE = "You have unsaved changes. Would you like to stop and save these changes? Otherwise, all changes will be lost.";

    /// <summary>
    /// Shows the system open dialog box for selecting a single file.
    /// </summary>
    /// <param name="title">The title of the dialog box window.</param>
    /// <param name="type">Parameter defining the default type of file to open with the dialog. <see cref="DialogFileTypeHelper"/><seealso cref="DialogType"/></param>
    /// <param name="fileName">The parameter which will store the selected filename (including path) when the user selects a file to open.</param>
    /// <returns>True when the user successfully selects a file to open.  False if the user cancels or exits the dialog without selecting a file.</returns>
    public static bool ShowOpenDialog(string title, DialogType type, out string fileName)
    {
      return DialogHelper.ShowOpenDialog(title, type, out fileName, null);
    }

    /// <summary>
    /// Shows the system open dialog box for selecting a single file.
    /// </summary>
    /// <param name="title">The title of the dialog box window.</param>
    /// <param name="type">Parameter defining the default type of file to open with the dialog. <see cref="DialogFileTypeHelper"/><seealso cref="DialogType"/></param>
    /// <param name="fileName">The parameter which will store the selected filename (including path) when the user selects a file to open.</param>
    /// <param name="defaultFileName">Overrides the default filename to open in the dialog box specified by the DialogType parameter.<seealso cref="DialogType"/> </param>
    /// <returns>True when the user successfully selects a file to open.  False if the user cancels or exits the dialog without selecting a file.</returns>
    public static bool ShowOpenDialog(string title, DialogType type, out string fileName, string defaultFileName)
    {
      // Configure open file dialog box
      OpenFileDialog openDialog = new OpenFileDialog()
      {
        Title = title,
        FileName = defaultFileName, // Default file name
        DefaultExt = type.DefaultExt,
        Filter = type.Filter
      };

      bool success = openDialog.ShowDialog().GetValueOrDefault();

      fileName = null;

      if (success)
        fileName = openDialog.FileName;

      return success;
    }

    /// <summary>
    /// Shows the system save file dialog box for saving a single file.
    /// </summary>
    /// <param name="title">The title of the dialog box window.</param>
    /// <param name="type">Parameter defining the default type of file to save with the dialog. <see cref="DialogFileTypeHelper"/><seealso cref="DialogType"/></param>
    /// <param name="fileName">The parameter which will store the filename (including path) when the user provides the location and filename for saving.</param>
    /// <returns>True when the use successfully selects a location and filename for saving.  False if the user cancels or exits the dialog.</returns>
    public static bool ShowSaveDialog(string title, DialogType type, out string fileName)
    {
      return DialogHelper.ShowSaveDialog(title, type, out fileName, null);
    }

    /// <summary>
    /// Shows the system save file dialog box for saving a single file.
    /// </summary>
    /// <param name="title">The title of the dialog box window.</param>
    /// <param name="type">Parameter defining the default type of file to save with the dialog. <see cref="DialogFileTypeHelper"/><seealso cref="DialogType"/></param>
    /// <param name="fileName">The parameter which will store the filename (including path) when the user provides the location and filename for saving.</param>
    /// <param name="defaultFileName">Overrides the default filename to suggest for saving in the dialog box specified by the DialogType parameter.<seealso cref="DialogType"/> </param>
    /// <returns>True when the use successfully selects a location and filename for saving.  False if the user cancels or exits the dialog.</returns>
    public static bool ShowSaveDialog(string title, DialogType type, out string fileName, string defaultFileName)
    {
      // Configure open file dialog box
      SaveFileDialog saveDialog = new SaveFileDialog()
      {
        Title = title,
        FileName = defaultFileName, // Default file name
        DefaultExt = type.DefaultExt,
        Filter = type.Filter
      };

      bool success = saveDialog.ShowDialog().GetValueOrDefault();

      fileName = null;

      if (success)
        fileName = saveDialog.FileName;

      return success;
    }

    /// <summary>
    /// Displays a modal message box alerting the user to save their changes.<seealso cref="DEFAULT_SAVE_MESSAGE"/>
    /// </summary>
    /// <returns>True if the user acknowledges that they need to save.  False if the user wishes to discard his or her changes.</returns>
    public static bool AskForSaveMessage()
    {
      return DialogHelper.AskForSaveMessage(DEFAULT_SAVE_MESSAGE, "Analytics Management Studio", null);
    }

    /// <summary>
    /// Displays a modal message box alerting the user to save their changes.<seealso cref="DEFAULT_SAVE_MESSAGE"/>
    /// </summary>
    /// <param name="caption">The title of the message box.<remarks>Defaults to "Analytics Management Studio".</remarks></param>
    /// <returns>True if the user acknowledges that they need to save.  False if the user wishes to discard his or her changes.</returns>
    public static bool AskForSaveMessage(string caption)
    {
      return DialogHelper.AskForSaveMessage(DEFAULT_SAVE_MESSAGE, caption, null);
    }

    /// <summary>
    /// Displays a modal message box alerting the user to save their changes.<seealso cref="DEFAULT_SAVE_MESSAGE"/>
    /// </summary>
    /// <param name="message">The message to display to the user.<remarks>Defaults to <see cref="DEFAULT_SAVE_MESSAGE"/></remarks></param>
    /// <param name="caption">The title of the message box.<remarks>Defaults to "Analytics Management Studio".</remarks></param>
    /// <returns>True if the user acknowledges that they need to save.  False if the user wishes to discard his or her changes.</returns>
    public static bool AskForSaveMessage(string message, string caption)
    {
      return DialogHelper.AskForSaveMessage(message, caption, null);
    }

    /// <summary>
    /// Displays a modal message box alerting the user to save their changes.<seealso cref="DEFAULT_SAVE_MESSAGE"/>
    /// </summary>
    /// <param name="owner">The window requesting the message box. <remarks>If this value is left null the Application.Current.MainWindow will be used as the owner.</remarks></param>
    /// <returns>True if the user acknowledges that they need to save.  False if the user wishes to discard his or her changes.</returns>
    public static bool AskForSaveMessage(Window owner)
    {
      return DialogHelper.AskForSaveMessage(DEFAULT_SAVE_MESSAGE, "Analytics Management Studio", owner);
    }

    /// <summary>
    /// Displays a modal message box alerting the user to save their changes.<seealso cref="DEFAULT_SAVE_MESSAGE"/>
    /// </summary>
    /// <param name="message">The message to display to the user.<remarks>Defaults to <see cref="DEFAULT_SAVE_MESSAGE"/></remarks></param>
    /// <param name="caption">The title of the message box.<remarks>Defaults to "Analytics Management Studio".</remarks></param>
    /// <param name="owner">The window requesting the message box. <remarks>If this value is left null the Application.Current.MainWindow will be used as the owner.</remarks></param>
    /// <returns>True if the user acknowledges that they need to save.  False if the user wishes to discard his or her changes.</returns>
    public static bool AskForSaveMessage(string message, string caption, Window owner)
    {
      MessageBoxResult result = MessageBox.Show(owner == null ? Application.Current.MainWindow : owner, message, caption, MessageBoxButton.YesNo,
          MessageBoxImage.Question);

      return result == MessageBoxResult.Yes;
    }
  }

  /// <summary>
  /// Helper class containing access to the default dialog types.
  /// </summary>
  public static class DialogFileTypeHelper
  {
    /// <summary>
    /// <see cref="ZipDialogType"/>
    /// </summary>
    public static ZipDialogType ZipDialog
    {
      get
      {
        return new ZipDialogType();
      }
    }

    /// <summary>
    /// <see cref="XmlDialogType"/>
    /// </summary>
    public static XmlDialogType XmlDialog
    {
      get
      {
        return new XmlDialogType();
      }
    }

    /// <summary>
    /// <see cref="SqlDialogType"/>
    /// </summary>
    public static SqlDialogType SqlDialog
    {
      get
      {
        return new SqlDialogType();
      }
    }

    /// <summary>
    /// <see cref="TextDialogType"/>
    /// </summary>
    public static TextDialogType TextDialog
    {
      get
      {
        return new TextDialogType();
      }
    }

    /// <summary>
    /// <see cref="ResxDialogType"/>
    /// </summary>
    public static ResxDialogType ResxDialog
    {
      get
      {
        return new ResxDialogType();
      }
    }

    /// <summary>
    /// <see cref="DtsxDialogType"/>
    /// </summary>
    public static DtsxDialogType DtsxDialog
    {
      get
      {
        return new DtsxDialogType();
      }
    }

    /// <summary>
    /// <see cref="LogDialogType"/>
    /// </summary>
    public static LogDialogType LogDialog
    {
      get
      {
        return new LogDialogType();
      }
    }

    /// <summary>
    /// <see cref="ExeDialogType"/>
    /// </summary>
    public static ExeDialogType ExeDialog
    {
      get
      {
        return new ExeDialogType();
      }
    }

    /// <summary>
    /// <see cref="CsvDialogType"/>
    /// </summary>
    public static CsvDialogType CsvDialog
    {
      get
      {
        return new CsvDialogType();
      }
    }

    public static CertificateDialogType CertificateDialog
    {
      get { return new CertificateDialogType(); }
    }

    public static PFXCertificateDialogType PFXCertificateDialog
    {
      get { return new PFXCertificateDialogType(); }
    }

    public static CERCertificateDialogType CERCertificateDialog
    {
      get { return new CERCertificateDialogType(); }
    }
  }

  #region Default Dialog Types
  /// <summary>
  /// Defines the structure for dialog types.
  /// </summary>
  public abstract class DialogType
  {
    /// <summary>
    /// Default filter for the dialog.
    /// </summary>
    public abstract string Filter { get; }
    /// <summary>
    /// Default extension for the dialog.
    /// </summary>
    public abstract string DefaultExt { get; }

    public DialogType() { }
  }

  /// <summary>
  /// Dialog for handling zip files
  /// </summary>
  public class ZipDialogType : DialogType
  {
    /// <summary>
    /// Default filter for the dialog.
    /// </summary>
    public override string Filter
    {
      get { return "Compressed File (.zip)|*.zip"; }
    }

    /// <summary>
    /// Default extension for the dialog.
    /// </summary>
    public override string DefaultExt
    {
      get { return ".zip"; }
    }
  }

  /// <summary>
  /// Dialog for handling XML files
  /// </summary>
  public class XmlDialogType : DialogType
  {
    /// <summary>
    /// Default filter for the dialog.
    /// </summary>
    public override string Filter
    {
      get { return "XML Configuration File (.xml)|*.xml"; }
    }

    /// <summary>
    /// Default extension for the dialog.
    /// </summary>
    public override string DefaultExt
    {
      get { return ".xml"; }
    }
  }

  /// <summary>
  /// Dialog for handling .sql files
  /// </summary>
  public class SqlDialogType : DialogType
  {
    /// <summary>
    /// Default filter for the dialog.
    /// </summary>
    public override string Filter
    {
      get { return "SQL Script File (.sql)|*.sql"; }
    }

    /// <summary>
    /// Default extension for the dialog.
    /// </summary>
    public override string DefaultExt
    {
      get { return ".sql"; }
    }
  }

  /// <summary>
  /// Dialog for handling txt files
  /// </summary>
  public class TextDialogType : DialogType
  {
    /// <summary>
    /// Default filter for the dialog.
    /// </summary>
    public override string Filter
    {
      get { return "Plain Text File (.txt)|*.txt"; }
    }

    /// <summary>
    /// Default extension for the dialog.
    /// </summary>
    public override string DefaultExt
    {
      get { return ".txt"; }
    }
  }

  /// <summary>
  /// Dialog for handling .resx files
  /// </summary>
  public class ResxDialogType : DialogType
  {
    /// <summary>
    /// Default filter for the dialog.
    /// </summary>
    public override string Filter
    {
      get { return "Application Resource File (.resx)|*.resx"; }
    }

    /// <summary>
    /// Default extension for the dialog.
    /// </summary>
    public override string DefaultExt
    {
      get { return ".resx"; }
    }
  }

  /// <summary>
  /// Dialog for handling zip files (SSIS packages)
  /// </summary>
  public class DtsxDialogType : DialogType
  {
    /// <summary>
    /// Default filter for the dialog.
    /// </summary>
    public override string Filter
    {
      get { return "Integration Services Package (.dtsx)|*.dtsx"; }
    }

    /// <summary>
    /// Default extension for the dialog.
    /// </summary>
    public override string DefaultExt
    {
      get { return ".dtsx"; }
    }
  }

  /// <summary>
  /// Dialog for handling log files
  /// </summary>
  public class LogDialogType : DialogType
  {
    /// <summary>
    /// Default filter for the dialog.
    /// </summary>
    public override string Filter
    {
      get { return "Log File (.log)|*.log"; }
    }

    /// <summary>
    /// Default extension for the dialog.
    /// </summary>
    public override string DefaultExt
    {
      get { return ".log"; }
    }
  }

  /// <summary>
  /// Dialog for handling executable files
  /// </summary>
  public class ExeDialogType : DialogType
  {
    /// <summary>
    /// Default filter for the dialog.
    /// </summary>
    public override string Filter
    {
      get { return "Executable (.exe)|*.exe"; }
    }

    /// <summary>
    /// Default extension for the dialog.
    /// </summary>
    public override string DefaultExt
    {
      get { return ".exe"; }
    }
  }

  /// <summary>
  /// Dialog for handling CSV files
  /// </summary>
  public class CsvDialogType : DialogType
  {
    /// <summary>
    /// Default filter for the dialog.
    /// </summary>
    public override string Filter
    {
      get { return "Comma Separated List (.csv)|*.csv"; }
    }

    /// <summary>
    /// Default extension for the dialog.
    /// </summary>
    public override string DefaultExt
    {
      get { return ".csv"; }
    }
  }

  public class CertificateDialogType : DialogType
  {
    public override string Filter
    {
      get { return "X.509 Certificate (*.cer; *.crt)|*.cer;*.crt|Personal Information Exchange (*.pfx; *.p12)|*.pfx;*p12|PKCS #7 Certificates (*.spc; *.p7b)|*.spc;*.p7b"; }
    }

    public override string DefaultExt
    {
      get { return ".cer"; }
    }
  }

  public class PFXCertificateDialogType : DialogType
  {
    public override string Filter
    {
      get { return "Personal Information Exchange (*.pfx; *.p12)|*.pfx;*p12"; }
    }

    public override string DefaultExt
    {
      get { return ".pfx"; }
    }
  }

  public class CERCertificateDialogType : DialogType
  {
    public override string Filter
    {
      get { return "X.509 Certificate (*.cer; *.crt)|*.cer;*.crt"; }
    }

    public override string DefaultExt
    {
      get { return ".cer"; }
    }
  }
  #endregion
}
