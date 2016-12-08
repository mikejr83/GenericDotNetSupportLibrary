using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GenericDotNetSupportLibrary.Helpers
{
  public static class UrlHelper
  {
    /// <summary>
    /// Combines multiple paths for a Relying Party
    /// </summary>
    /// <param name="rpUrl">The root Url of the Relying Party</param>
    /// <param name="paths">An array of paths to combine with the RelyingParty Url.  
    /// If any of these paths are absolute (beginning with http:|https:|tcp:), 
    /// they will become the root, and previous paths will be ignored.</param>
    /// <returns></returns>
    public static string CombineForRelyingParty(string rpUrl, params string[] paths)
    {
      rpUrl = StringHelper.NotNull<string>(rpUrl, "");
      string combinedPath = "";

      //loop through the paths backwards to save time - in case one of the is an absolute Url
      foreach (string path in paths.Reverse<string>())
      {
        if (!String.IsNullOrEmpty(path))
        {
          combinedPath = Combine(path, combinedPath);
          if (Regex.IsMatch(path, "^http|https|net.tcp"))
            return combinedPath;
        }
      }
      return Combine(rpUrl, combinedPath);
    }

    public static string Combine(params string[] paths)
    {
      string combinedPath = paths[0];
      foreach (string path in paths.Skip<string>(1))
      {
        if (!String.IsNullOrEmpty(path))
        {
          if (combinedPath.EndsWith("/") && path.StartsWith("/"))
            combinedPath += path.Substring(1);
          else if (combinedPath.EndsWith("/") || path.StartsWith("/"))
            combinedPath += path;
          else
            combinedPath += "/" + path;
        }
      }
      return combinedPath;
    }
  }
}
