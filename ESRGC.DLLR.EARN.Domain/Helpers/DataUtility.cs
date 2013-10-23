using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRGC.DLLR.EARN.Domain.Model;

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
            return string.Format("{0:# (###) ###-####}", number);
          else
            return string.Format("{0:(###) ###-####}", number);
        }
        else
          return phoneNumb;
      }
      else
        return "";
    }
    public static List<string> getTagListString(Profile profile) {
      return getTagList(profile).Select(x => x.Name).ToList();
    }
    public static List<Tag> getTagList(Profile profile) {
      try {
        var list = profile.ProfileTags
                            .Select(x => x.Tag)
                            .Where(x => !(x is GeoTag))
                            .ToList();
        if (list == null)
          list = new List<Tag>();
        return list;
      }
      catch (Exception) {
        return new List<Tag>();
      }
    }
  }
}
