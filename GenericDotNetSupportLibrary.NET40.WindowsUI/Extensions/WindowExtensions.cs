using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows;

namespace GenericDotNetSupportLibrary.Extensions
{
    /// <summary>
    /// Extensions to the System.Windows.Media.Visual object.  This is typically known as the Window object in WPF.
    /// </summary>
    public static class WindowExtensions
    {
        /// <summary>
        /// Shows a folder selection window.
        /// </summary>
        /// <param name="visual">The window showing the open folder dialog</param>
        /// <param name="description">The description to show in the dialog</param>
        /// <returns>The file path to the folder if OK was selected in the dialog.  Null will be returned if no folder was selected.</returns>
        public static string ShowFolderOpenDialog(this System.Windows.Media.Visual visual)
        {
            return ShowFolderOpenDialog(visual, null);
        }

        /// <summary>
        /// Shows a folder selection window.
        /// </summary>
        /// <param name="visual">The window showing the open folder dialog</param>
        /// <param name="description">The description to show in the dialog</param>
        /// <returns>The file path to the folder if OK was selected in the dialog.  Null will be returned if no folder was selected.</returns>
        public static string ShowFolderOpenDialog(this System.Windows.Media.Visual visual, string description)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (!string.IsNullOrEmpty(description))
                dialog.Description = description;
            DialogResult result = dialog.ShowDialog(visual.GetIWin32Window());

            return result == DialogResult.OK ? dialog.SelectedPath : null;
        }

        /// <summary>
        /// Gets an interface to expose Win32 HWND handles on a visual.
        /// </summary>
        /// <param name="visual">The visual which will provide the interface.</param>
        /// <returns>The interface to expose Win32 HWND handles.<seealso cref="IWin32Window"/></returns>
        public static IWin32Window GetIWin32Window(this System.Windows.Media.Visual visual)
        {
            var source = PresentationSource.FromVisual(visual) as System.Windows.Interop.HwndSource;
            IWin32Window win = new OldWindow(source.Handle);
            return win;
        }

        private class OldWindow : IWin32Window
        {
            private readonly System.IntPtr _handle;
            public OldWindow(System.IntPtr handle)
            {
                _handle = handle;
            }

            #region IWin32Window Members
            System.IntPtr System.Windows.Forms.IWin32Window.Handle
            {
                get { return _handle; }
            }
            #endregion
        }
    }
}
