using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRGC.GIS.Utilities;

namespace ESRGC.GIS.Geocoding
{
  public class MDLocatorWithZip : Geocoder
  {
    string _geocodeUrl, _reverseGeocodeUrl;

    public string ReverseGeocodeUrl {
      get { return _reverseGeocodeUrl; }
    }

    public string GeocodeUrl {
      get { return _geocodeUrl; }
    }
    /// <summary>
    /// MD cascading locator with zip code.
    /// Default constructor that uses Lat/Lon projection (4326)
    /// </summary>
    public MDLocatorWithZip() : this(4326) { }

    /// <summary>
    /// MD cascading locator with zip code
    /// </summary>
    /// <param name="wkid">Spatial reference</param>
    public MDLocatorWithZip(int wkid) {
      _url = "http://mdimap.us/ArcGIS/rest/services/GeocodeServices/MD.State.MDCascadingLocatorWithZIPCodes/GeocodeServer";
      _geocodeUrl = _url + "/findAddressCandidates?f=pjson";
      _reverseGeocodeUrl = _url + "/reverseGeocode?f=pjson";
      //add spatial reference
      _geocodeUrl += "&outSR=" + wkid;
      _reverseGeocodeUrl += "&outSR=" + wkid;
      _wkid = wkid;
    }
    /// <summary>
    /// geocode address and zip code
    /// </summary>
    /// <param name="address"></param>
    /// <param name="zip"></param>
    /// <returns>JSON result</returns>
    public override string geocode(string address, string zip) {
      address = address.Replace("  ", " ").Trim();//remove double white space
      zip = zip.Trim();
      //construct url
      var url = _geocodeUrl + string.Format("&street={0}&zip={1}", address, zip);

      return Net.sendRequestString(url);
    }
    /// <summary>
    /// geocode address, zipcode, and city
    /// </summary>
    /// <param name="address"></param>
    /// <param name="city"></param>
    /// <param name="zip"></param>
    /// <returns>Json result</returns>
    public override string geocode(string address, string city, string zip) {
      address = address.Replace("  ", " ").Trim();//remove double white space
      zip = zip.Trim();
      city = city.Trim();
      //construct url
      var url = _geocodeUrl + string.Format("&street={0}&zip={1}&city={2}", address, zip, city);

      return Net.sendRequestString(url);
    }
    /// <summary>
    /// reverse geocode X,Y location
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="distance">distance from center</param>
    /// <returns>JSon result</returns>
    public override string reverseGeocode(double x, double y, double distance) {
      try {
        var url = _reverseGeocodeUrl + string.Format("location={0}%2C{1}&distance={2}", x, y, distance);
        return Net.sendRequestString(url);
      }
      catch {
        return null;
      }
    }
  }
}
