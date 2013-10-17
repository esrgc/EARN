using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.DLLR.EARN.Domain.DAL.Abstract;
using ESRGC.DLLR.EARN.Domain.Model;
using ESRGC.DLLR.EARN.Models;
using ESRGC.GIS.Geocoding;
using ESRGC.GIS.Utilities;

namespace ESRGC.DLLR.EARN.Controllers
{
  [Authorize]
  public class ProfileController : BaseController
  {
    public ProfileController(IWorkUnit workUnit)
      : base(workUnit) {
    }
    public ActionResult Index() {
      return View();
    }

    public ActionResult Detail() {
      var profile = CurrentAccount.Profile;
      if (profile == null) {
        return RedirectToAction("Create");
      }

      int countTag = _workUnit
        .ProfileTagRepository
        .Entities
        .Where(x => x.ProfileID == profile.ProfileID)
        .Select(x => x.Tag)
        .OfType<Tag>()
        .Count();

      if (countTag == 0)
        return RedirectToAction("ManageTag", "Tag");

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

      return View(CurrentAccount.Profile);
    }

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
    public ActionResult Create(CreateProfile profile) {
      if (CurrentAccount.Profile != null) {
        updateTempDataMessage("Profile already created!");
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
          _workUnit.AccountRepository.UpdateEntity(account);
          _workUnit.saveChanges();
        }        

        return RedirectToAction("Detail");
      }
      //error
      profile.Categories = _workUnit.CategoryRepository.Entities.OrderBy(x => x.Name).ToList();
      profile.UserGroups = _workUnit.UserGroupRepository.Entities.OrderBy(x => x.Name).ToList();
      return View(profile);
    }
    //temporary removed from the form
    public PartialViewResult SubcategoryDropdown(int userGroupID) {
      var subcats = _workUnit
        .CategoryRepository
        .Entities
        .Where(x => x.UserGroupID == userGroupID || x.UserGroupID == null)
        .ToList();

      return PartialView(subcats);
    }
    //public profile view
    public ActionResult ViewProfile(int profileID, string returnUrl) {
      var profile = _workUnit.ProfileRepository.GetEntityByID(profileID);
      ViewBag.returnUrl = returnUrl;
      return View(profile);
    }

    [HttpPost]
    public ActionResult EditAbout(string about) {
      try {
        var profile = CurrentAccount.Profile;
        profile.About = about;
        _workUnit.ProfileRepository.UpdateEntity(profile);
        _workUnit.saveChanges();
      }
      catch (Exception) {
        updateTempDataMessage("Error saving about text");
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
  }
}
