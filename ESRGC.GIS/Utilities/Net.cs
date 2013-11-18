using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ESRGC.GIS.Utilities
{
  public static class Net
  {
    public static Stream sendRequest(string url) {
      try {
        //format url
        url = url.Replace(" ", "+");
        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
        //default get method
        webRequest.Method = "GET";
        webRequest.ContentType = "text";
        //send request
        HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
        Stream stream = webResponse.GetResponseStream();
        StreamReader responseStreamReader = new StreamReader(stream);
        //read to memory
        string result = responseStreamReader.ReadToEnd();
        MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
        //close response stream
        webResponse.Close();

        return ms;
      }
      catch (Exception ex) {
        throw ex;
      }
    }

    public static string sendRequestString(string url) {
      try {
        //format url
        url = url.Replace(" ", "+");
        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
        //default get method
        webRequest.Method = "GET";
        webRequest.ContentType = "text";
        //send request
        HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
        Stream stream = webResponse.GetResponseStream();
        StreamReader responseStreamReader = new StreamReader(stream);
        //read to memory
        string result = responseStreamReader.ReadToEnd();
        //close response stream
        webResponse.Close();

        return result;
      }
      catch (Exception ex) {
        throw ex;
      }
    }

    public static dynamic deserializeJson(string json) {
      try {
        var jsonObj = JObject.Parse(json);
        return jsonObj;
      }
      catch {
        return null;
      }
    }
  }
}
