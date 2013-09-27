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
      var preExistingTags = _workUnit.TagRepository.Entities.ToList();
      return View(preExistingTags);
    }
    [HttpPost]
    public ActionResult AddTag(ICollection<string> tags) {
      return View();
    }

    [HttpPost]
    public ActionResult AddTagAjax(string tagName, string description) {
      return View();
    }
  }
}
