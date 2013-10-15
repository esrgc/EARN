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
  public class OrganizationController : BaseController
  {
    public OrganizationController(IWorkUnit workUnit)
      : base(workUnit) {

    }

    public ActionResult Edit(int id) {
      var organization = _workUnit.OrganizationRepository.GetEntityByID(id);
      return View(organization);
    }

    [HttpPost]
    [ActionName("Edit")]
    public ActionResult EditOrg(int id) {
      var org = _workUnit.OrganizationRepository.GetEntityByID(id) ?? new Organization();
      TryUpdateModel(org);
      if (ModelState.IsValid) {
        _workUnit.OrganizationRepository.UpdateEntity(org);
        _workUnit.saveChanges();
        return RedirectToAction("Detail", "Profile");
      }
      //errors
      return View(org);
    }
  }
}
