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
        return RedirectToAction("AddTag");

      return View(CurrentAccount.Profile);
    }
    public ActionResult Create() {
      //if (CurrentAccount.Profile != null) {
      //  updateTempDataMessage("Profile already created!");
      //  return RedirectToAction("Index");
      //}

      var industries = _workUnit.IndustryRepository.Entities.OrderBy(x => x.Name).ToList();
      var userGroups = _workUnit.UserGroupRepository.Entities.OrderBy(x => x.Name).ToList();
      return View(new CreateProfile() { Industries = industries, UserGroups = userGroups });
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
          IndustryID = profile.IndustryID
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
      return View();
    }

    public ActionResult AddTag() {
      if (CurrentAccount.Profile == null)
        return RedirectToAction("Create");

      ViewBag.preExistingTags = _workUnit.TagRepository.Entities.ToList();
      ViewBag.currentTags = CurrentAccount.Profile.ProfileTags.ToList();
     
      return View();
    }

    [HttpPost]
    public ActionResult AddTag(ICollection<string> tags) {
      if (CurrentAccount.Profile == null)
        return new EmptyResult();

      if (ModelState.IsValid) {
        foreach (var tag in tags) {
          int count = _workUnit
            .TagRepository
            .Entities
            .Where(x => x.Name.ToUpper() == tag.ToUpper()).Count();
          if (count == 0) {//tag is new
            var newTag = new Tag { Name = tag.ToUpper() };
            _workUnit.TagRepository.InsertEntity(newTag);
            _workUnit.ProfileTagRepository.InsertEntity(new ProfileTag { Profile = CurrentAccount.Profile, Tag = newTag });
          }
        }
        _workUnit.saveChanges();
        return RedirectToAction("index");
      }
      return View();
    }

    [HttpPost]
    public ActionResult AddTagAjax(string tagName, string description) {
      return View();
    }
  }
}
