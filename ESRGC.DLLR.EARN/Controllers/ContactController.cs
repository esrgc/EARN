using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.DLLR.EARN.Domain.DAL.Abstract;
using ESRGC.DLLR.EARN.Domain.Model;

namespace ESRGC.DLLR.EARN.Controllers
{
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

    public ActionResult Edit(int id) {
      var contact = _workUnit.ContactRepository.GetEntityByID(id);
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
