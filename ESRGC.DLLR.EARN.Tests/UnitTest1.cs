﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using ESRGC.DLLR.EARN.Controllers;
using ESRGC.DLLR.EARN.Domain.DAL;
using ESRGC.DLLR.EARN.Domain.DAL.Concrete;
using ESRGC.GIS.Geocoding;
using ESRGC.GIS.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ESRGC.DLLR.EARN.Tests
{
  [TestClass]
  public class UnitTest1
  {
    [TestMethod]
    public void TestMethod1() {
      //arrage

      //act
      var req = Net.sendRequestString("http://mdimap.us/ArcGIS/rest/services/GeocodeServices/MD.State.MDCascadingLocatorWithZIPCodes/GeocodeServer/findAddressCandidates?Street=1455+E+Sandy+Acres+Dr&City=&ZIP=&SingleLine=&outFields=&outSR=4326&f=pjson");

      //assert

      Console.WriteLine(req);
      Assert.IsNotNull(req);

    }
    [TestMethod]
    public void TestParseJSon() {
      //arrange
      string url = "http://mdimap.us/ArcGIS/rest/services/GeocodeServices/MD.State.MDCascadingLocatorWithZIPCodes/GeocodeServer/findAddressCandidates?Street=1455+E+Sandy+Acres+Dr&City=&ZIP=&SingleLine=&outFields=&outSR=4326&f=pjson";
      //act
      var result = Net.sendRequestString(url);
      var resultObj = Net.deserializeJson(result);
      //assert
      Console.WriteLine(resultObj.ToString());
      //Assert.AreEqual(4326, resultObj.spatialReference.wkid);

    }

    [TestMethod]
    public void TestAddressGeocode() {
      //arrange
      var address = "1455 E Sandy Acres Dr";
      var city = "Salisbury";
      var zip = "21804";
      var geocoder = new MDLocatorWithZip();//default lat/lon
      //act
      var result = geocoder.geocode(address, city, zip);
      var resultObj = Net.deserializeJson(result);
      var candidates = resultObj.candidates as IEnumerable<dynamic>;

      Console.WriteLine(geocoder.Url);

      foreach (var candidate in candidates) {
        if (candidate.score == 100)
          Console.WriteLine(candidate);
      }
    }

    [TestMethod]
    public void testProfileGeoTag() { 
      //arrange
      var controller = new ProfileController(new WorkUnit(new DomainContext()));
      controller.addAddressGeoTag(1);
    }
  }
}