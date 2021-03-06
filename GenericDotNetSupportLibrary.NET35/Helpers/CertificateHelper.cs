﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;

namespace GenericDotNetSupportLibrary.Helpers
{
  /// <summary>
  /// Helper class for dealing with certificates.
  /// </summary>
  public class CertificateHelper
  {
    /// <summary>
    /// Gets the appropriate certificate from the store location.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="location"></param>
    /// <param name="subjectName"></param>
    /// <returns></returns>
    public static X509Certificate2 GetCertificate(StoreName name, StoreLocation location, string subjectName)
    {
      X509Store store = new X509Store(name, location);
      X509Certificate2Collection certificates = null;
      store.Open(OpenFlags.ReadOnly);

      //subjectName = subjectName.Replace(" ", string.Empty);

      try
      {
        X509Certificate2 result = null;
        //
        // Every time we call store.Certificates property, a new collection will be returned.
        //
        certificates = store.Certificates;

        for (int i = 0; i < certificates.Count; i++)
        {
          X509Certificate2 cert = certificates[i];

          if (cert.SubjectName.Name.ToLowerInvariant().Contains(subjectName.ToLowerInvariant()))
          {
            if (result != null)
            {
              throw new ApplicationException(string.Format("There are multiple certificates for subject Name {0}", subjectName));
            }

            result = new X509Certificate2(cert);
          }
        }

        if (result == null)
        {
          throw new ApplicationException(string.Format("No certificate was found for subject Name {0}", subjectName));
        }

        return result;
      }
      finally
      {
        if (certificates != null)
        {
          for (int i = 0; i < certificates.Count; i++)
          {
            X509Certificate2 cert = certificates[i];
            cert.Reset();
          }
        }

        store.Close();
      }
    }
  }
}
