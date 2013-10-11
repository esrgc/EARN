using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.DLLR.EARN.Domain.DAL.Abstract;
using ESRGC.DLLR.EARN.Domain.Model;

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
      var tags = _workUnit.TagRepository.Entities.OfType<Tag>().Select(x => x.Name).ToArray();
      return Json(tags, JsonRequestBehavior.AllowGet);
    }
    /// <summary>
    /// manage tag for current profile
    /// </summary>
    /// <returns></returns>
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
        return RedirectToAction("Detail", "profile");
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
        return RedirectToAction("index", "profile");
      }
      return View();
    }
  }
}
