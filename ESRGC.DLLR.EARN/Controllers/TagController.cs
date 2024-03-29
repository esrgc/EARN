﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.DLLR.EARN.Domain.DAL.Abstract;
using ESRGC.DLLR.EARN.Domain.Model;
using ESRGC.DLLR.EARN.Filters;

namespace ESRGC.DLLR.EARN.Controllers
{
  [Authorize]
  public class TagController : BaseController
  {
    public TagController(IWorkUnit workUnit) : base(workUnit) { }

    /// <summary>
    /// return a list of pre-existing tags
    /// these are the tags that user accummulate
    /// </summary>
    /// <returns></returns>
    public JsonResult Tags() {
      var tags = _workUnit
        .TagRepository
        .Entities
        .Where(x => !(x is GeoTag))
        .OrderBy(x => x.Name)
        .Select(x => x.Name)
        .ToArray();
      return Json(tags, JsonRequestBehavior.AllowGet);
    }
    /// <summary>
    /// manage tag for current profile
    /// </summary>
    /// <returns></returns>
    public ActionResult ManageTag() {
      if (CurrentAccount.Profile == null)
        return RedirectToAction("Create", "Profile");

      return View(CurrentAccount.Profile);
    }

    [HttpPost]
    public ActionResult ManageTag(ICollection<string> tags) {
      if (tags == null) {
        return RedirectToAction("Detail", "Profile");
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
          //tag list to be added to reference table
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
        return RedirectToAction("Detail", "Profile");
      }
      return View();
    }

    [HttpPost]
    [VerifyProfile]
    [VerifyProfilePartnership]
    public ActionResult ManagePartnershipTag(int partnershipID, ICollection<string> tags) {
      if (tags == null) {
        tags = new List<string>();//this will allow deleting all tags
      }
      //no need to check for null value for partnership.
      //because the partnership is always valid at this point 
      //validated by VerifyProfilePartnership filter
      var partnership = _workUnit.PartnershipRepository.GetEntityByID(partnershipID);

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
            //_workUnit.partnershipTagRepository.InsertEntity(new partnershipTag { partnership = partnership, Tag = newTag });
            _workUnit.PartnershipTagRepository.InsertEntity(new PartnershipTag { Partnership = partnership, Tag = newTag });
          });
        }
        List<string> addedTags = null;
        //remove tags marked as removed and add current added tags
        if (partnership.PartnershipTags.Count > 0) {
          //list of partnership tag to be removed from reference table
          var removedPartnershipTags = partnership.PartnershipTags.Where(x => !tags.Contains(x.Tag.Name)).ToList();
          //remove tag references 
          if (removedPartnershipTags != null) {
            removedPartnershipTags.ForEach(x => _workUnit.PartnershipTagRepository.DeleteEntity(x));
          }
          //now get the current tags
          var currentTags = partnership.PartnershipTags.Select(x => x.Tag.Name).ToList();
          //tag list to be added to reference table
          addedTags = tags.Where(x => !currentTags.Contains(x) && preExistingTags.Contains(x)).ToList();
        }
        //no new tags but partnership tags container is empty
        //so add the preExisting tags to partnership
        else {
          addedTags = tags.Where(x => !newTags.Contains(x)).ToList();
        }

        //now add the references to partnerships
        if (addedTags != null) {
          addedTags.ForEach(x => {
            try {
              //get the tag 
              var t = _workUnit.TagRepository.Entities.First(k => k.Name == x);
              //create new reference
              var reference = new PartnershipTag { Partnership = partnership, Tag = t };
              _workUnit.PartnershipTagRepository.InsertEntity(reference);
            }
            catch {
              //tag doesn't exist  
            }
          });
        }
        updateTempMessage("Tags have been saved.");
        _workUnit.saveChanges();
        return RedirectToAction("Detail", "Partnership", new { partnershipID });
      }
      updateTempMessage("Failed to edit tags for partnership");
      return RedirectToAction("Detail", "Partnership", new { partnershipID });
    }
  }
}
