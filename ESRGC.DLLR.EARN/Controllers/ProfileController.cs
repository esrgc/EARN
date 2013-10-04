using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.DLLR.EARN.Domain.DAL.Abstract;
using ESRGC.DLLR.EARN.Domain.Model;
using ESRGC.DLLR.EARN.Models;

namespace ESRGC.DLLR.EARN.Controllers
{
  [Authorize]
  public class ProfileController : BaseController
  {
    public ProfileController(IWorkUnit workUnit)
      : base(workUnit) {
    }

    public ActionResult Index() {
      var profile = CurrentAccount.Profile;
      if (profile == null) {
        return RedirectToAction("Create");
      }

      int countTag = _workUnit
        .ProfileTagRepository
        .Entities
        .Where(x => x.ProfileID == profile.ProfileID)
        .Count();

      if (countTag == 0)
        return RedirectToAction("ManageTag");

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
        return RedirectToAction("Index");
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

        return RedirectToAction("index");
      }
      //error
      profile.Categories = _workUnit.CategoryRepository.Entities.OrderBy(x => x.Name).ToList();
      profile.UserGroups = _workUnit.UserGroupRepository.Entities.OrderBy(x => x.Name).ToList();
      return View(profile);
    }

    public PartialViewResult SubcategoryDropdown(int userGroupID) {
      var subcats = _workUnit
        .CategoryRepository
        .Entities
        .Where(x => x.UserGroupID == userGroupID || x.UserGroupID == null)
        .ToList();

      return PartialView(subcats);
    }

    /// <summary>
    /// return a list of pre-existing tags
    /// these are the tags that user accummulate
    /// </summary>
    /// <returns></returns>
    public JsonResult Tags() {
      var tags = _workUnit.TagRepository.Entities.OfType<Tag>().Select(x => x.Name).ToArray();
      return Json(tags, JsonRequestBehavior.AllowGet);
    }

    public ActionResult ManageTag() {
      if (CurrentAccount.Profile == null)
        return RedirectToAction("Create");
      if (CurrentAccount.Profile.ProfileTags != null)
        ViewBag.currentTags = CurrentAccount.Profile.ProfileTags.Select(x => x.Tag).ToList();

      return View();
    }

    [HttpPost]
    public ActionResult ManageTag(ICollection<string> tags) {
      if (tags == null) {
        return RedirectToAction("index");
      }
      var profile = CurrentAccount.Profile;
      if (profile == null)
        return new EmptyResult();


      if (ModelState.IsValid) {
        //preexisting tags
        var preExistingTags = _workUnit.TagRepository.Entities.Select(x => x.Name).ToList();
        //new tags to be added to both reference table and tag table
        var newTags = tags.Where(x => !preExistingTags.Contains(x)).ToList();

        //now add the new tags to tag table and create new refs for them 
        //this list is exclusive from the current tag list
        if (newTags != null) {
          newTags.ForEach(x => {
            var newTag = new Tag { Name = x };
            _workUnit.TagRepository.InsertEntity(newTag);
            _workUnit.ProfileTagRepository.InsertEntity(new ProfileTag { Profile = profile, Tag = newTag });
          });
        }
        List<string> addedTags = null;
        //remove tags marked as removed and add current added tags
        if (profile.ProfileTags != null) {
          //list of profile tag to be removed from reference table
          var removedProfileTags = profile.ProfileTags.Where(x => !tags.Contains(x.Tag.Name)).ToList();
          //remove tag references 
          if (removedProfileTags != null) {
            removedProfileTags.ForEach(x => _workUnit.ProfileTagRepository.DeleteEntity(x));
          }
          //now get the current tags
          var currentTags = profile.ProfileTags.Select(x => x.Tag.Name).ToList();
          //tag list to be adding to reference table
          addedTags = tags.Where(x => !currentTags.Contains(x) && preExistingTags.Contains(x)).ToList();
        }
        //no new tags but profile tags container is empty
        //so add the preExisting tags to profile
        else {
          addedTags = tags.Where(x => !newTags.Contains(x)).ToList();
        }

        //now add the references to profiles
        if (addedTags != null) {
          addedTags.ForEach(x => {
            try {
              //get the tag 
              var t = _workUnit.TagRepository.Entities.First(k => k.Name == x);
              //create new reference
              var reference = new ProfileTag { Profile = profile, Tag = t };
              _workUnit.ProfileTagRepository.InsertEntity(reference);
            }
            catch {
              //tag doesn't exist  
            }
          });
        }

        _workUnit.saveChanges();
        return RedirectToAction("index");
      }
      return View();
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
      return RedirectToAction("index");
    }
  }
}
