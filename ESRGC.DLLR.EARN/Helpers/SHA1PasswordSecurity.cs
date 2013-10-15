using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;

namespace ESRGC.DLLR.EARN.Helpers
{
  public class SHA1PasswordSecurity
  {
    public static byte[] encrypt(string src) {
      var encryptor = new SHA1CryptoServiceProvider();
      var tempSrc = ASCIIEncoding.ASCII.GetBytes(src);
      var hasedBytes = encryptor.ComputeHash(tempSrc);
      return hasedBytes;
    }

    public static string ByteArrayToString(byte[] arrInput) {
      int i;
      StringBuilder sOutput = new StringBuilder(arrInput.Length);
      for (i = 0; i < arrInput.Length; i++) {
        sOutput.Append(arrInput[i].ToString("X2"));
      }
      return sOutput.ToString();
    }
  }
}