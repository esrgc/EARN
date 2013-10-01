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
      if (CurrentAccount.Profile == null) {
        return RedirectToAction("Create");
      }

      int countTag = _workUnit
        .ProfileTagRepository
        .Entities
        .Where(x => x.ProfileID == CurrentAccount.ProfileID)
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

      var communities = _workUnit.CommunityRepository.Entities.OrderBy(x => x.Name).ToList();
      var userGroups = _workUnit.UserGroupRepository.Entities.OrderBy(x => x.Name).ToList();
      return View(new CreateProfile() { Communities = communities, UserGroups = userGroups });
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
          CommunityID = profile.CommunityID,
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
      profile.Communities = _workUnit.CommunityRepository.Entities.OrderBy(x => x.Name).ToList();
      profile.UserGroups = _workUnit.UserGroupRepository.Entities.OrderBy(x => x.Name).ToList();
      return View(profile);
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
      if (CurrentAccount.Profile == null)
        return new EmptyResult();
      var profile = CurrentAccount.Profile;


      if (ModelState.IsValid) {
        //preexisting tags
        var preExistingTags = _workUnit.TagRepository.Entities.Select(x => x.Name).ToList();
        //current tags
        var currentTags = profile.ProfileTags.Select(x => x.Tag.Name).ToList();

        //list of profile tag to be removed from reference table
        var removedProfileTags = profile.ProfileTags.Where(x => !tags.Contains(x.Tag.Name)).ToList();
        //tag list to be adding to reference table
        var addedTags = tags.Where(x => !currentTags.Contains(x) && preExistingTags.Contains(x)).ToList();
        //new tags to be added to both reference table and tag table
        var newTags = tags.Where(x => !preExistingTags.Contains(x)).ToList();

        //remove tag references 
        removedProfileTags.ForEach(x => _workUnit.ProfileTagRepository.DeleteEntity(x));
        //add new references for preExisting tags but newly added to profile
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
        //now add the new tags to tag table and create new refs for them
        newTags.ForEach(x => {
          var newTag = new Tag { Name = x };
          _workUnit.TagRepository.InsertEntity(newTag);
          _workUnit.ProfileTagRepository.InsertEntity(new ProfileTag { Profile = profile, Tag = newTag });
        });

        //foreach (var tag in tags) {
        //  Tag t = null;
        //  try {
        //    t = _workUnit.TagRepository.Entities.First(x => x.Name.ToUpper() == tag.ToUpper());
        //  }
        //  catch {
        //    //tag doesn't exist
        //  }

        //  if (t == null) {//tag is new
        //    var newTag = new Tag { Name = tag.ToUpper() };
        //    _workUnit.TagRepository.InsertEntity(newTag);
        //    _workUnit.ProfileTagRepository.InsertEntity(
        //      new ProfileTag { Profile = profile, Tag = newTag }
        //    );
        //  }
        //  else { //tag already exists
        //    //check if tag is not already referenced in this current profile
        //    if (t.ProfileTags.Where(x => x.ProfileID == profile.ProfileID).Count() == 0) {
        //      //haven't been referenced so add it
        //      _workUnit.ProfileTagRepository.InsertEntity(
        //        new ProfileTag { Profile = profile, Tag = t });
        //    }
        //  }
        //}
        _workUnit.saveChanges();
        return RedirectToAction("index");
      }
      return View();
    }

    [HttpPost]
    public ActionResult ManageTagAjax(string tagName, string description) {
      return View();
    }

    public ActionResult RemoveTag(int tagID) {
      return null;
    }
  }
}
