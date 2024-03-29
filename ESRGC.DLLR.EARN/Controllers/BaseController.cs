﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.DLLR.EARN.Domain.DAL.Abstract;
using ESRGC.DLLR.EARN.Domain.Model;
using ESRGC.DLLR.EARN.Filters;
using ESRGC.GIS.Geocoding;
using ESRGC.GIS.Utilities;


namespace ESRGC.DLLR.EARN.Controllers
{
  public class BaseController : Controller
  {
    protected IWorkUnit _workUnit = null;
    public BaseController() { }
    public BaseController(IWorkUnit workUnit) {
      _workUnit = workUnit;
    }

    public IWorkUnit WorkUnit {
      get {
        return _workUnit;
      }
    }
    [AllowNonProfile]
    public ActionResult ProfilePicture(int pictureId) {
      try {
        var pic = _workUnit.PictureRepository.GetEntityByID(pictureId);

        return File(pic.ImageData, pic.ImageMimeType);
      }
      catch (Exception) {
        var physicalPath = Server.MapPath("~/Client/images/default-logo.png");

        using (var fileStream = System.IO.File.OpenRead(physicalPath)) {
          var data = new byte[fileStream.Length];
          var buffer = fileStream.Read(data, 0, (int)fileStream.Length);
          return File(data, "image/png");
        }
      }
    }
    [AllowNonProfile]
    public ActionResult ProfileLogo(int id) {
      var profile = _workUnit.ProfileRepository.GetEntityByID(id);
      if (profile == null)
        return new EmptyResult();
      if (profile.ProfilePicture != null) {
        try {
          var pic = profile.ProfilePicture;
          return File(pic.ImageData, pic.ImageMimeType);
        }
        catch (Exception) {
          return null;
        }
      }
      else {
        var physicalPath = Server.MapPath("~/Client/images/default-logo.png");

        using (var fileStream = System.IO.File.OpenRead(physicalPath)) {
          var data = new byte[fileStream.Length];
          var buffer = fileStream.Read(data, 0, (int)fileStream.Length);
          return File(data, "image/png");
        }
      }
    }
    [AllowNonProfile]
    public ActionResult PartnershipLogo(int id) {
      var partnership = _workUnit.PartnershipRepository.GetEntityByID(id);
      if (partnership == null)
        return new EmptyResult();
      if (partnership.Logo != null) {
        try {
          var pic = partnership.Logo;
          return File(pic.ImageData, pic.ImageMimeType);
        }
        catch (Exception) {
          return null;
        }
      }
      else {
        var physicalPath = Server.MapPath("~/Client/images/empty-partnership-logo.png");

        using (var fileStream = System.IO.File.OpenRead(physicalPath)) {
          var data = new byte[fileStream.Length];
          var buffer = fileStream.Read(data, 0, (int)fileStream.Length);
          return File(data, "image/png");
        }
      }
    }
    //////////////////////////
    //helpers 
    ///////////////////////
    //attempt to geocode the profile address
    protected void addUpdateAddrGeoTag(int profileId) {
      var profile = _workUnit.ProfileRepository.GetEntityByID(profileId);
      if (profile == null)
        return;

      var address = profile.Organization.StreetAddress;
      var zip = profile.Organization.Zip;
      var city = profile.Organization.City;
      var state = profile.Organization.State;
      //if any field is empty do nothing
      if (string.IsNullOrEmpty(address) ||
        string.IsNullOrEmpty(zip) ||
        string.IsNullOrEmpty(city))
        return;

      //geocode
      var geocoder = new MDLocatorWithZip();
      var jsonResult = geocoder.geocode(address, city, zip);
      var resultObj = Net.deserializeJson(jsonResult);
      if (resultObj == null)
        return;
      var result = resultObj.candidates as IEnumerable<dynamic>;
      if (result == null)
        return;

      var bestScore = geocoder.AcceptableScore;
      dynamic location = null;
      //parse result (get the entry with highest score and above acceptable score 65)
      foreach (var candidate in result) {
        if (candidate.score > bestScore) {
          bestScore = candidate.score;
          location = candidate.location;
        }
      }
      //create geo tag
      if (location != null) {
        var tagName = string.Format("{0}, {1}, {2} {3}", address, city, state, zip);
        var wkt = string.Format("POINT({0} {1})", location.x, location.y);
        //create geo tag
        var geoTag = new GeoTag() {
          Name = tagName,
          Geometry = DbGeometry.PointFromText(wkt, geocoder.SpatialReference),
          Description = "address"
        };
        //remove all old references
        try {
          var oldAddrTagRefs = profile
                .ProfileTags
                .Where(x => (x.Tag is GeoTag) && x.Tag.Description == "address")
                .ToList();
          oldAddrTagRefs.ForEach(x => _workUnit.ProfileTagRepository.DeleteEntity(x));
          _workUnit.saveChanges();
        }
        catch (ArgumentNullException) {
          //no item found
        }
        //update new tags 
        try {
          var currentTag = _workUnit.TagRepository.Entities.OfType<GeoTag>().First(x => x.Name.ToUpper() == tagName.ToUpper());
          //update the geometry if it already exists
          currentTag.Geometry = geoTag.Geometry;
          if (currentTag.Description == "")
            currentTag.Description = "address";
          //update to database
          _workUnit.TagRepository.UpdateEntity(currentTag);
          //check if it already has referenced this profile
          if (!profile.ProfileTags.Select(x => x.Tag).Contains(currentTag)) {
            var profileTag = new ProfileTag() {
              Tag = currentTag,
              Profile = profile
            };
            _workUnit.ProfileTagRepository.InsertEntity(profileTag);
          }
        }
        catch {
          //create ProfileTag
          var profileTag = new ProfileTag() {
            Tag = geoTag,
            Profile = profile
          };
          _workUnit.ProfileTagRepository.InsertEntity(profileTag);
        }
        finally {
          _workUnit.saveChanges();
        }
      }
    }

    public void updateTempMessage(string message) {
      TempData["message"] = message;
    }

    public MvcHtmlString messageSupportLink() {
      var supportProfiles = _workUnit.ProfileRepository.Entities
        .Where(x => x.EARNSupport == true)
        .Select(x => x.Organization.Name)
        .ToList();
      return new MvcHtmlString(Url.Action("Index", "Message") + "#new/" + string.Join(",", supportProfiles));
    }

    protected Account CurrentAccount {
      get {
        try {
          var account = _workUnit.AccountRepository.Entities.First(x => x.EmailAddress.ToLower() == User.Identity.Name.ToLower());
          return account;
        }
        catch (Exception) {
          return null;
        }
      }
    }


    protected RedirectResult returnToUrl(string returnUrl, string defaultUrl) {
      //return to previous url
      if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
          && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\")) {
        return Redirect(returnUrl);
      }
      else {
        return Redirect(defaultUrl);
      }
    }

    protected bool notifyProfile(Notification notification, Profile profile) {
      //notification.
      profile.Accounts.ToList()
        .ForEach(x => {
          notification.Account = x;

        });
      return true;
    }
  }
}
