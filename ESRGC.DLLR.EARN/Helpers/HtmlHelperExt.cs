using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using ESRGC.DLLR.EARN.Domain.Model;
using ESRGC.DLLR.EARN.Domain.Helpers;

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
    public static string AddSearchFilter(
        this UrlHelper helper,
        string actionName,
        IDictionary<string, object> routeDict,
        string newFilterKey,
        object newFilterVal) {

      var routeValues = new RouteValueDictionary(routeDict);
      if (routeValues.Keys.Contains(newFilterKey)) {
        if (routeValues[newFilterKey] is IEnumerable<string>) {
          var list = (routeValues[newFilterKey] as List<string>).ToList();
          list.Add(newFilterVal as string);
          routeValues[newFilterKey] = list;
        }
        else
          routeValues[newFilterKey] = newFilterVal;
      }
      else
        routeValues.Add(newFilterKey, newFilterVal);

      //generate url
      return helper.GenerateLinkFromFilters(actionName, routeValues);

    }
    public static string AddSearchFilter(
        this UrlHelper helper,
        string actionName,
        string controller,
        IDictionary<string, object> routeDict,
        string newFilterKey,
        object newFilterVal) {

      var routeValues = new RouteValueDictionary(routeDict);
      if (routeValues.Keys.Contains(newFilterKey)) {
        if (routeValues[newFilterKey] is IEnumerable<string>) {
          var list = (routeValues[newFilterKey] as List<string>).ToList();
          list.Add(newFilterVal as string);
          routeValues[newFilterKey] = list;
        }
        else
          routeValues[newFilterKey] = newFilterVal;
      }
      else
        routeValues.Add(newFilterKey, newFilterVal);

      //generate url
      return helper.GenerateLinkFromFilters(actionName, controller, routeValues);

    }
    public static string GenerateLinkFromFilters(
        this UrlHelper helper,
        string actionName,
        IDictionary<string, object> routeDict) {

      var routeValues = new RouteValueDictionary(routeDict);
      //check if there's sub list of filter under the same key
      string extraParams = "";
      var itemTobeRemoved = new List<string>();
      foreach (var i in routeValues) {
        //checks if there are list of the same key
        if (i.Value is List<string>) {
          itemTobeRemoved.Add(i.Key);
          var paramList = i.Value as List<string>;
          //loop through and genrate url params
          foreach (var p in paramList) {
            extraParams += "&" + i.Key + "=" + p;
          }
        }
      }
      //remove list items from route dictionary
      itemTobeRemoved.ForEach(x => routeValues.Remove(x));

      //generate url
      if (routeValues.Count > 0)
        return helper.Action(actionName, routeValues) + extraParams;
      else if (extraParams.Length > 0)
        return helper.Action(actionName, routeValues) + "?" + extraParams.Substring(1, extraParams.Length - 1);
      else
        return helper.Action(actionName, routeValues);
    }

    public static string GenerateLinkFromFilters(
        this UrlHelper helper,
        string actionName,
        string controller,
        IDictionary<string, object> routeDict) {

      var routeValues = new RouteValueDictionary(routeDict);
      //check if there's sub list of filter under the same key
      string extraParams = "";
      var itemTobeRemoved = new List<string>();
      foreach (var i in routeValues) {
        //checks if there are list of the same key
        if (i.Value is List<string>) {
          itemTobeRemoved.Add(i.Key);
          var paramList = i.Value as List<string>;
          //loop through and genrate url params
          foreach (var p in paramList) {
            extraParams += "&" + i.Key + "=" + p;
          }
        }
      }
      //remove list items from route dictionary
      itemTobeRemoved.ForEach(x => routeValues.Remove(x));

      //generate url
      if (routeValues.Count > 0)
        return helper.Action(actionName, controller, routeValues) + extraParams;
      else if (extraParams.Length > 0)
        return helper.Action(actionName, controller, routeValues) + "?" + extraParams.Substring(1, extraParams.Length - 1);
      else
        return helper.Action(actionName, controller, routeValues);
    }

    public static string RemoveSearchFilter(
        this UrlHelper helper,
        string actionName,
        IDictionary<string, object> routeDict,
        string removeKey,
        string value) {
      //create new route dictionary
      var routeValues = new RouteValueDictionary(routeDict);
      //checks if value is a list
      if (routeValues.ContainsKey(removeKey)) {
        var valueAtThatKey = routeValues[removeKey];
        //if value at that key is a collection then only remove that one value
        if (valueAtThatKey is IEnumerable<string>) {
          var subList = (valueAtThatKey as List<string>).ToList();//copy to a new list
          subList.Remove(value);
          routeValues[removeKey] = subList;//reference the new list
        }
        else
          //remove key
          routeValues.Remove(removeKey);
      }
      //generate url
      return helper.GenerateLinkFromFilters(actionName, routeValues);
    }
    public static string RemoveSearchFilter(
        this UrlHelper helper,
        string actionName,
        string controller,
        IDictionary<string, object> routeDict,
        string removeKey,
        string value) {
      //create new route dictionary
      var routeValues = new RouteValueDictionary(routeDict);
      //checks if value is a list
      if (routeValues.ContainsKey(removeKey)) {
        var valueAtThatKey = routeValues[removeKey];
        //if value at that key is a collection then only remove that one value
        if (valueAtThatKey is IEnumerable<string>) {
          var subList = (valueAtThatKey as List<string>).ToList();//copy to a new list
          subList.Remove(value);
          routeValues[removeKey] = subList;//reference the new list
        }
        else
          //remove key
          routeValues.Remove(removeKey);
      }
      //generate url
      return helper.GenerateLinkFromFilters(actionName, controller, routeValues);
    }
    public static string TimeSpan(this HtmlHelper helper, DateTime? timeInPast) {
      if (timeInPast == null)
        return "unknown";
      var timeSpan = DateTime.Now - timeInPast.Value;
      if (timeSpan.Days == 0) {
        if (timeSpan.Hours == 0) {
          if (timeSpan.Minutes == 0)
            return "just now";
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
      return DataUtility.normalizePhoneNumber(phoneNumb);
    }
    //get tags in current profile
    public static List<string> getTagListString(this HtmlHelper helper, Profile profile) {
      return DataUtility.getTagListString(profile);
    }
    public static List<Tag> getTagList(this HtmlHelper helper, Profile profile) {
      return DataUtility.getTagList(profile);
    }
    public static string ToAbsoluteUrl(this string url, string protocol) {
      if (string.IsNullOrEmpty(url))
        return url;
      if (!url.Contains("http"))
        url = protocol + url;
      return url;
    }
    public static string RemoveProtocol(this string url) {
      if (!string.IsNullOrEmpty(url))
        return url.Replace("http://", "").Replace("https://", "");
      else
        return "";
    }

    /// <summary>
    /// get address geotag from profile
    /// </summary>
    /// <param name="helper"></param>
    /// <param name="profile"></param>
    /// <returns></returns>
    //public static GeoTag getAddrGeoTag(this HtmlHelper helper, Profile profile) {
    //  try {
    //    var geoTag = profile
    //      .ProfileTags
    //      .Select(x => x.Tag)
    //      .First(x => (x is GeoTag) && x.Description == "address") as GeoTag;
    //    return geoTag;
    //  }
    //  catch {
    //    return new GeoTag() { };
    //  }
    //}

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