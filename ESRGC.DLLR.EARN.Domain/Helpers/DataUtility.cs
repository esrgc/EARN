using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESRGC.DLLR.EARN.Domain.Helpers
{
  public class DataUtility
  {
    public static string normalizePhoneNumber(string phoneNumb) {
      if (!string.IsNullOrEmpty(phoneNumb)) {
        phoneNumb = phoneNumb
            .Replace(".", "")
            .Replace("-", "")
            .Replace("(", "")
            .Replace(")", "")
            .Replace(" ", "")
            .Trim();
        
          return phoneNumb;
      }
      else
        return "";
    }
    public static string formatPhoneNumber(string phoneNumb) {
      if (!string.IsNullOrEmpty(phoneNumb)) {
        phoneNumb = phoneNumb
            .Replace(".", "")
            .Replace("-", "")
            .Replace("(", "")
            .Replace(")", "")
            .Replace(" ", "")
            .Trim();
        double number;
        if (double.TryParse(phoneNumb, out number)) {
          if (phoneNumb.Length > 10)
            return string.Format("{0: # (###) ###-####}", number);
          else
            return string.Format("{0: (###) ###-####}", number);
        }
        else
          return phoneNumb;
      }
      else
        return "";
    }
  }
}
