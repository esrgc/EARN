using System;
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
  public class ContactController : BaseController
  {

    public ContactController(IWorkUnit workUnit)
      : base(workUnit) {

    }

    public ActionResult Index() {
      return View(CurrentAccount.Contact);
    }

    public ActionResult Create(string returnUrl) {
      if (CurrentAccount.Contact != null) {
        updateTempMessage("You already have a contact information.");
        return RedirectToAction("Index");
      }
      ViewBag.returnUrl = returnUrl;
      return View(new Contact() { Email = CurrentAccount.EmailAddress });
    }
    [HttpPost]
    [ActionName("Create")]
    public ActionResult CreateContact(string returnUrl) {
      var contact = new Contact();
      TryUpdateModel(contact);
      if (ModelState.IsValid) {
        var currentAccount = CurrentAccount;
        currentAccount.Contact = contact;
        _workUnit.AccountRepository.UpdateEntity(currentAccount);
        _workUnit.ContactRepository.InsertEntity(contact);
        _workUnit.saveChanges();
        return RedirectToAction("Detail", "Profile");
      }
      if (!string.IsNullOrEmpty(returnUrl)) {
        return returnToUrl(returnUrl, Url.Action("index"));
      }
      return View(contact);
    }
    
    public ActionResult Edit() {
      var contact = CurrentAccount.Contact;
      return View(contact);
    }

    [HttpPost]
    [ActionName("Edit")]
    public ActionResult EditContact(int id) {
      var contact = _workUnit.ContactRepository.GetEntityByID(id);
      TryUpdateModel(contact);
      if (ModelState.IsValid) {
        _workUnit.ContactRepository.UpdateEntity(contact);
        _workUnit.saveChanges();
        return RedirectToAction("index");
      }
      return View(contact);
    }
  }
}
