using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace GenericDotNetSupportLibrary.Helpers
{
  /// <summary>
  /// Helper class for encrypting/decrypting
  /// </summary>
  public static class EncryptionHelper
  {
    /// <summary>
    /// Encrypt the file
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile"></param>
    /// <param name="key"></param>
    public static void EncryptFile(string inputFile, string outputFile, string key)
    {
      FileStream fsInput = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
      FileStream fsEncrypted = new FileStream(outputFile, FileMode.Create, FileAccess.Write);
      DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
      DES.Key = ASCIIEncoding.ASCII.GetBytes(key);
      DES.IV = ASCIIEncoding.ASCII.GetBytes(key);
      ICryptoTransform desencrypt = DES.CreateEncryptor();
      CryptoStream cryptostream = new CryptoStream(fsEncrypted, desencrypt, CryptoStreamMode.Write);
      byte[] bytearrayinput = new byte[fsInput.Length];
      fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
      cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
      cryptostream.Close();
      fsInput.Close();
      fsEncrypted.Close();
    }

    /// <summary>
    /// Decrypt the file
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile"></param>
    /// <param name="key"></param>
    public static void DecryptFile(string inputFile, string outputFile, string key)
    {
      //Print the contents of the decrypted file.
      StreamWriter fsDecrypted = new StreamWriter(outputFile);
      fsDecrypted.Write(DecryptFile(inputFile, key));
      fsDecrypted.Flush();
      fsDecrypted.Close();
    }

    public static String DecryptFile(string inputFile, string key)
    {
      DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
      //A 64 bit key and IV is required for this provider.
      //Set secret key For DES algorithm.
      DES.Key = ASCIIEncoding.ASCII.GetBytes(key);
      //Set initialization vector.
      DES.IV = ASCIIEncoding.ASCII.GetBytes(key);

      //Create a file stream to read the encrypted file back.
      FileStream fsread = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
      //Create a DES decryptor from the DES instance.
      ICryptoTransform desdecrypt = DES.CreateDecryptor();
      //Create crypto stream set to read and do a 
      //DES decryption transform on incoming bytes.
      CryptoStream cryptostreamDecr = new CryptoStream(fsread, desdecrypt, CryptoStreamMode.Read);

      StreamReader decryptedStream = new StreamReader(cryptostreamDecr);
      string decryptedString = decryptedStream.ReadToEnd();
      fsread.Close();

      return decryptedString;
    }


  }
}
