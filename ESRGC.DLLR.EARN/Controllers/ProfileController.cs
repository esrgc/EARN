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
        return RedirectToAction("ManageTag", "Tag");

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
