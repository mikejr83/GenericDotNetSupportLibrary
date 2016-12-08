using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Runtime.InteropServices;

namespace GenericDotNetSupportLibrary.Helpers
{
  public static class ImpersonationHelper
  {
    /// <summary>
    /// Group Type Enum
    /// </summary>
    enum SECURITY_IMPERSONATION_LEVEL : int
    {
      SecurityAnonymous = 0,
      SecurityIdentification = 1,
      SecurityImpersonation = 2,
      SecurityDelegation = 3
    }

    

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sUsername"></param>
    /// <param name="sPassword"></param>
    /// <returns></returns>
    public static WindowsImpersonationContext ImpersonateUser(string userName, string password)
    {
      if (userName.Contains("\\"))
      {
        string[] splitUserName = userName.Split(new char[] { '\\' });

        return ImpersonateUser(splitUserName[1], splitUserName[0], password);
      }
      else
        return ImpersonateUser(userName, null, password);
    }

    /// <summary>
    /// Attempts to impersonate a user.  If successful, returns 
    /// a WindowsImpersonationContext of the new users identity.
    /// </summary>
    /// <param name="sUsername">Username you want to impersonate</param>
    /// <param name="sDomain">Logon domain</param>
    /// <param name="sPassword">User's password to logon with</param></param>
    /// <returns></returns>
    public static WindowsImpersonationContext ImpersonateUser(string sUsername, string sDomain, string sPassword)
    {
      // initialize tokens
      IntPtr pExistingTokenHandle = new IntPtr(0);
      IntPtr pDuplicateTokenHandle = new IntPtr(0);
      pExistingTokenHandle = IntPtr.Zero;
      pDuplicateTokenHandle = IntPtr.Zero;

      // if domain name was blank, assume local machine
      if (sDomain == "")
        sDomain = System.Environment.MachineName;

      try
      {
        string sResult = null;

        const int LOGON32_PROVIDER_DEFAULT = 0;

        // create token
        const int LOGON32_LOGON_INTERACTIVE = 2;
        //const int SecurityImpersonation = 2;

        // get handle to token
        bool bImpersonated = ImpersonationNativeMethods.LogonUser(sUsername, sDomain, sPassword,
          LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, ref pExistingTokenHandle);

        // did impersonation fail?
        if (false == bImpersonated)
        {
          int nErrorCode = Marshal.GetLastWin32Error();
          sResult = "LogonUser() failed with error code: " + nErrorCode + "\r\n";

          // show the reason why LogonUser failed
          throw new Exception("Error impersonating user: " + sResult);
        }

        // Get identity before impersonation
        sResult += "Before impersonation: " + WindowsIdentity.GetCurrent().Name + "\r\n";

        bool bRetVal = ImpersonationNativeMethods.DuplicateToken(pExistingTokenHandle, (int)SECURITY_IMPERSONATION_LEVEL.SecurityImpersonation, ref pDuplicateTokenHandle);

        // did DuplicateToken fail?
        if (false == bRetVal)
        {
          int nErrorCode = Marshal.GetLastWin32Error();
          ImpersonationNativeMethods.CloseHandle(pExistingTokenHandle); // close existing handle
          sResult += "DuplicateToken() failed with error code: " + nErrorCode + "\r\n";

          // show the reason why DuplicateToken failed
          throw new Exception("Error impersonating user: " + sResult);
        }
        else
        {
          // create new identity using new primary token
          WindowsIdentity newId = new WindowsIdentity(pDuplicateTokenHandle);
          WindowsImpersonationContext impersonatedUser = newId.Impersonate();

          // check the identity after impersonation
          sResult += "After impersonation: " + WindowsIdentity.GetCurrent().Name + "\r\n";

          //MessageBox.Show(this, sResult, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
          return impersonatedUser;
        }
      }
      catch (Exception)
      {
        throw;
      }
      finally
      {
        // close handle(s)
        if (pExistingTokenHandle != IntPtr.Zero)
          ImpersonationNativeMethods.CloseHandle(pExistingTokenHandle);
        if (pDuplicateTokenHandle != IntPtr.Zero)
          ImpersonationNativeMethods.CloseHandle(pDuplicateTokenHandle);
      }
    }
  }

  internal static class ImpersonationNativeMethods
  {
    // obtains user token
    [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern bool LogonUser(string pszUsername, string pszDomain, string pszPassword,
      int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

    // closes open handles returned by LogonUser
    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    internal extern static bool CloseHandle(IntPtr handle);

    // creates duplicate token handle
    [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal extern static bool DuplicateToken(IntPtr ExistingTokenHandle,
      int SECURITY_IMPERSONATION_LEVEL, ref IntPtr DuplicateTokenHandle);
  }
}
