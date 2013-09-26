using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using ESRGC.DLLR.EARN.Domain.Model;

namespace ESRGC.DLLR.EARN.Helpers
{
  public static class HtmlHelperExt
  {
    /// <summary>
    /// add new search routing filter
    /// </summary>
    /// <param name="helper"></param>
    /// <param name="routeDict">Current routing values</param>
    /// <param name="newFilter">New route value</param>
    /// <returns></returns>
    public static string SearchFilter(
        this UrlHelper helper,
        string actionName,
        IDictionary<string, object> routeDict,
        string newFilterKey,
        string newFilterVal) {

      var routeValues = new RouteValueDictionary(routeDict);
      if (routeValues.Keys.Contains(newFilterKey))
        routeValues[newFilterKey] = newFilterVal;
      else
        routeValues.Add(newFilterKey, newFilterVal);
      //generate url
      return helper.Action(actionName, routeValues);
    }

    public static string CurrentSearchFilter(
        this UrlHelper helper,
        string actionName,
        IDictionary<string, object> routeDict) {

      var routeValues = new RouteValueDictionary(routeDict);
      //generate url
      return helper.Action(actionName, routeValues);
    }


    public static string ClearSearchFilter(
        this UrlHelper helper,
        string actionName,
        IDictionary<string, object> routeDict,
        string removeKey) {
      //create new route dictionary
      var routeValues = new RouteValueDictionary(routeDict);
      //remove key
      routeValues.Remove(removeKey);
      //generate url
      return helper.Action(actionName, routeValues);
    }

    public static string TimeSpan(this HtmlHelper helper, DateTime? timeInPast) {
      if (timeInPast == null)
        return "unknown";
      var timeSpan = DateTime.Now - timeInPast.Value;
      if (timeSpan.Days == 0) {
        if (timeSpan.Hours == 0) {
          if (timeSpan.Minutes == 0)
            return "Less than a minute ago";
          if (timeSpan.Minutes == 1)
            return timeSpan.Minutes + " minute ago";
          else
            return timeSpan.Minutes + " minutes ago";
        }
        if (timeSpan.Hours == 1)
          return timeSpan.Hours + " hour ago";
        else
          return timeSpan.Hours + " hours ago";
      }
      if (timeSpan.Days == 1)
        return timeSpan.Days + " day ago";
      else {
        if (timeSpan.Days < 30)
          return timeSpan.Days + " days ago";
        if (timeSpan.Days >= 30 && timeSpan.Days < 60)
          return (int)timeSpan.Days / 30 + " month ago";
        else if (timeSpan.Days > 60 && timeSpan.Days < 365)
          return (int)timeSpan.Days / 30 + " months ago";
        else if (timeSpan.Days >= 365 && timeSpan.Days < 730)
          return "over a year ago";
        else
          return (int)timeSpan.Days / 365 + " years ago";
      }

    }

    public static string DisplayPhoneText(this HtmlHelper helper, string phoneNumb) {
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

    //public static MvcHtmlString DisplayStreetAddr(
    //    this HtmlHelper helper,
    //    Contact contact
    //)
    //{

    //    if (contact.STREETNAME != null
    //            && contact.STREETNUMB != null
    //            && contact.STREETTYPE != null
    //    )
    //    {

    //        var addressLine = string.Join(" ",
    //                        contact.STREETNUMB.Trim(),
    //                        contact.PREFIXDIR ?? "",
    //                        contact.STREETNAME.Trim(),
    //                        contact.SUFFIXDIR ?? "",
    //                        contact.STREETTYPE.Trim());

    //        if (contact.FLOOR != null)
    //        {
    //            addressLine += ", " + contact.FLOOR;
    //        }
    //        if (contact.BLDG != null)
    //        {
    //            addressLine += ", " + contact.BLDG;
    //        }
    //        if (contact.SECONDARYADDRESS != null)
    //            addressLine += "<br />" + contact.SECONDARYADDRESS.Trim();
    //        if (contact.CITY != null && contact.STATE != null)
    //        {
    //            addressLine += "<br/>"
    //                + contact.CITY.Trim()
    //                + ", "
    //                + contact.STATE.Trim();
    //            if (contact.ZIP != null)
    //                addressLine += " " + contact.ZIP;
    //        }
    //        addressLine += "<br/>";
    //        return MvcHtmlString.Create(addressLine);
    //    }
    //    else
    //        return MvcHtmlString.Create("");
    //}        
  }
}