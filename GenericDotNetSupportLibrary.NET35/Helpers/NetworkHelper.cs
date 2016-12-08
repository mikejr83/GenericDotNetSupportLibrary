using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace GenericDotNetSupportLibrary.Helpers
{
  public class NetworkHelper
  {
    /// <summary>
    /// Gets the ip addresses of the current machine.
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<string> GetIPv4Addresses()
    {
      IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());

      return ipHostInfo.AddressList.Select(a => a.ToString());
    }

    public static string GetIPv4Address()
    {
      IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());

      return ipHostInfo.AddressList.Where(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).Select(a => a.ToString()).FirstOrDefault();
    }

    public static string GetIPv6Address()
    {
      IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());

      return ipHostInfo.AddressList.Where(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6).Select(a => a.ToString()).FirstOrDefault();
    }

    public static string FindFQDN()
    {
      string fqdn = null;

      try
      {
        var ipProperties = IPGlobalProperties.GetIPGlobalProperties();
        fqdn = string.Format("{0}.{1}", ipProperties.HostName, ipProperties.DomainName);
      }
      catch { }

      if (string.IsNullOrEmpty(fqdn) || fqdn == ".")
        fqdn = "localhost";

      return fqdn;
    }
  }
}
