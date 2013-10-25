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
      return View();
    }

    public ActionResult Create() {
      return View();
    }
    [VerifyProfile]
    public ActionResult Edit() {
      var contact = CurrentAccount.Profile.Contact;
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
        return RedirectToAction("Detail", "Profile");
      }
      return View(contact);
    }
  }
}
