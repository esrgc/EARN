﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.DLLR.EARN.Domain.DAL.Abstract;
using ESRGC.DLLR.EARN.Domain.Model;
using ESRGC.DLLR.EARN.Filters;
using ESRGC.DLLR.EARN.Models;
using ESRGC.GIS.Geocoding;
using ESRGC.GIS.Utilities;
using PagedList;

namespace ESRGC.DLLR.EARN.Controllers
{
  [Authorize]
  [VerifyAccount]
  [VerifyProfile]
  public class ProfileController : BaseController
  {
    public ProfileController(IWorkUnit workUnit)
      : base(workUnit) {
    }
    public ActionResult Index() {
      return View();
    }
    public ActionResult DisplayShortProfile(int profileID) {
      if (CurrentAccount != null) {
        ViewBag.currentProfile = CurrentAccount.Profile;
        var profile = _workUnit.ProfileRepository.GetEntityByID(profileID);
        return PartialView("shortProfilePartial", profile);
      }
      return new EmptyResult();
    }
    [VerifyProfile]
    public ActionResult Detail() {
      var profile = CurrentAccount.Profile;

      //check if address tag exists
      int addressTagCount = _workUnit
        .ProfileTagRepository
        .Entities
        .Where(x => x.ProfileID == profile.ProfileID)
        .Select(x => x.Tag)
        .OfType<GeoTag>()
        .Where(x => x.Description.ToLower() == "address")
        .Count();

      if (addressTagCount == 0)
        addUpdateAddrGeoTag(profile.ProfileID);

      //count tags
      //int countTag = profile.getTags().Count();
      //if (countTag == 0) {
      //  updateTempMessage("There are currently no tags on your profile. Other organizations will be more likely to find you when tags are available.");
      //  return RedirectToAction("ManageTag", "Tag");
      //}

      return View(CurrentAccount.Profile);
    }

    [AllowNonProfile]
    [HasPendingProfileRequest]//prevent searching while having pending profile request
    public ActionResult Find(string name, int? page, int? pageSize, string f = "html") {
      if (CurrentAccount.Profile != null) {
        updateTempMessage("Your organization profile already exists.");
        return RedirectToAction("Detail");
      }
      int index = page ?? 1;
      int size = pageSize ?? 10;
      var result = _workUnit.ProfileRepository.Entities;
      if (!String.IsNullOrEmpty(name))
        result = result.Where(x => x.Organization.Name.ToLower().Contains(name.ToLower()));
      //if ajax request return json
      var json = result.Select(x => new {
        ID = x.ProfileID,
        Name = x.Organization.Name,
        Website = x.Organization.Website,
        Group = x.UserGroup.Name,
      }).ToList()
      .ToPagedList(index, size);
      
      if (Request.IsAjaxRequest() || f.ToLower() == "json") {
        return Json(json, JsonRequestBehavior.AllowGet);
      }
      
      var pagedList = result.ToList().ToPagedList(index, size);
      return View(pagedList);
    }
    [AllowNonProfile]
    public ActionResult Join(int profileID) {
      var profile = _workUnit.ProfileRepository.GetEntityByID(profileID);
      if (profile == null) {
        updateTempMessage("Invalid profile ID");
        return RedirectToAction("Index", "Home");
      }


      return View();
    }

    [AllowNonProfile]
    [HasPendingProfileRequest]
    public ActionResult Create() {
      //if (CurrentAccount.Profile != null) {
      //  updateTempDataMessage("Profile already created!");
      //  return RedirectToAction("Index");
      //}

      var categories = _workUnit.CategoryRepository.Entities.OrderBy(x => x.Name).ToList();
      var userGroups = _workUnit.UserGroupRepository.Entities.OrderBy(x => x.UserGroupID).ToList();
      return View(new CreateProfile() { Categories = categories, UserGroups = userGroups });
    }

    [HttpPost]
    [AllowNonProfile]
    public ActionResult Create(CreateProfile profile) {
      if (CurrentAccount.Profile != null) {
        updateTempMessage("Profile already created!");
        return RedirectToAction("Detail");
      }
      if (ModelState.IsValid) {
        //insert organization
        _workUnit.OrganizationRepository.InsertEntity(profile.Organization);
        _workUnit.ContactRepository.InsertEntity(profile.Contact);
        _workUnit.saveChanges();

        var p = new Profile() {
          Contact = profile.Contact,
          Organization = profile.Organization,
          UserGroupID = profile.UserGroupID,
          CategoryID = profile.CategoryID,
          LastUpdate = DateTime.Now
        };

        _workUnit.ProfileRepository.InsertEntity(p);
        _workUnit.saveChanges();

        var account = CurrentAccount;
        if (account != null) {
          account.Profile = p;
          account.IsProfileOwner = true;
          _workUnit.AccountRepository.UpdateEntity(account);
          _workUnit.saveChanges();
        }

        return RedirectToAction("ManageTag", "Tag");
      }
      //error
      profile.Categories = _workUnit.CategoryRepository.Entities.OrderBy(x => x.Name).ToList();
      profile.UserGroups = _workUnit.UserGroupRepository.Entities.OrderBy(x => x.Name).ToList();
      return View(profile);
    }
    //temporary removed from the form
    //public PartialViewResult SubcategoryDropdown(int userGroupID) {
    //  var subcats = _workUnit
    //    .CategoryRepository
    //    .Entities
    //    .Where(x => x.UserGroupID == userGroupID || x.UserGroupID == null)
    //    .ToList();

    //  return PartialView(subcats);
    //}
    //public profile view
    [HasReturnUrl]
    public ActionResult ViewProfile(int profileID, string returnUrl) {
      var profile = _workUnit.ProfileRepository.GetEntityByID(profileID);
      return View(profile);
    }

    [HttpPost]
    public ActionResult EditAbout(string about) {
      try {
        var profile = CurrentAccount.Profile;
        profile.About = about;
        _workUnit.ProfileRepository.UpdateEntity(profile);
        _workUnit.saveChanges();
        updateTempMessage("Your about section has been saved.");
      }
      catch (Exception) {
        updateTempMessage("Error saving about text");
      }
      return RedirectToAction("Detail");
    }

    public ActionResult UploadImage() {
      var profile = CurrentAccount.Profile;
      if (profile == null)
        return RedirectToAction("Detail");
      return View(profile);
    }
    [HttpPost]
    public ActionResult UploadImage(int profileID, HttpPostedFileBase dataInput) {
      var profile = _workUnit.ProfileRepository.GetEntityByID(profileID);
      if (dataInput == null)
        ModelState.AddModelError("", "No data input. Please select a file to upload");
      else {
        var picture = new Picture() {
          ImageMimeType = dataInput.ContentType,
          ImageData = new byte[dataInput.ContentLength]
        };
        //read the input stream and store to picture object 
        dataInput.InputStream.Read(picture.ImageData, 0, dataInput.ContentLength);

        //store picture to database
        _workUnit.PictureRepository.InsertEntity(picture);
        if (profile.PictureID != null)//delete current picture
          _workUnit.PictureRepository.DeleteByID(profile.PictureID);
        _workUnit.saveChanges();
        profile.PictureID = picture.PictureID;
        _workUnit.saveChanges();
        //changes done return to detail page
        return RedirectToAction("Detail");
      }
      //error has occurred   
      return View(profile);
    }
    [VerifyProfile]
    public ActionResult Delete() {
      return View();
    }
    [HttpPost]
    [VerifyProfile]
    [ActionName("Delete")]
    public ActionResult DeleteProfile() {
      if (!CurrentAccount.IsProfileOwner) {
        updateTempMessage("Sory, you can not delete this profile. Only the profile creator/owner can delete.");
        return RedirectToAction("Settings", "Account");
      }
      var profile = CurrentAccount.Profile;

      if (profile == null) {
        updateTempMessage("Invalid profile ID");
        return RedirectToAction("Index", "Home");
      }

      if (profile.hasOwnedPartnerships()) {
        updateTempMessage("Your organizational profile is currently involved with one or more partnertships as an administrator. Please re-assign admin role before deleting your profile.");
        return RedirectToAction("Detail");
      }         

      //delete profile tags
      var profileTags = _workUnit.ProfileTagRepository
        .Entities
        .Where(x => x.ProfileID == profile.ProfileID)
        .ToList();
      foreach (var tag in profileTags) {
        _workUnit.ProfileTagRepository.DeleteEntity(tag);
      }

      //CurrentAccount.ProfileID = null;
      //_workUnit.AccountRepository.UpdateEntity(CurrentAccount);
      profile.deleteDetails();
      _workUnit.ProfileRepository.DeleteEntity(profile);
      if (profile.Organization != null) {
        _workUnit.OrganizationRepository.DeleteEntity(profile.Organization);
      }
      //delete contact
      if (profile.Contact != null) {
        _workUnit.ContactRepository.DeleteEntity(profile.Contact);
      }
     
      _workUnit.saveChanges();
      updateTempMessage("Your profile has been deleted.");
      return RedirectToAction("Index", "Home");
    }
    [VerifyProfile]
    public ActionResult Disable() {
      return View();
    }
  }
}
