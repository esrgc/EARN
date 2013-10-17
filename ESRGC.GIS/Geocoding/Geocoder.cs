using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESRGC.GIS.Geocoding
{
  /// <summary>
  /// Derived class implements the specific geocoder based on a specific API 
  /// </summary>
  public abstract class Geocoder
  {
    protected string _url;//request url
    protected int _wkid;//spatial reference
    protected int _acceptableScore = 65;

    public Geocoder() { }
    public Geocoder(string url, int wkid) {
      _url = url.Replace(" ", "+");
      _wkid = wkid;
    }
    public string Url { get { return _url; } }
    public int SpatialReference { get { return _wkid; } }
    public int AcceptableScore { get { return _acceptableScore; } set { _acceptableScore = value; } }

    /// <summary>
    /// geocode address and zip code
    /// </summary>
    /// <param name="address">Address used to geocode</param>
    /// <param name="zip">Zip code to geocode</param>
    /// <returns>Json string of the result</returns>
    public abstract string geocode(string address, string zip);

    /// <summary>
    /// geocode address and zip code
    /// </summary>
    /// <param name="address">Address used to geocode</param>
    /// <param name="zip">Zip code to geocode</param>
    /// <param name="city">City constraint</param>
    /// <returns>Json string of the result</returns>
    public abstract string geocode(string address, string city, string zip);
    /// <summary>
    /// reverse geocode from XY
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="distance"></param>
    /// <returns>Json string of the result</returns>
    public abstract string reverseGeocode(double x, double y, double distance);
  }
}
