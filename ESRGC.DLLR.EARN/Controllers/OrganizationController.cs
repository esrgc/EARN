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
  public class OrganizationController : BaseController
  {
    public OrganizationController(IWorkUnit workUnit)
      : base(workUnit) {

    }
    [VerifyProfile]
    public ActionResult Edit() {      
      var organization = CurrentAccount.Profile.Organization;
      return View(organization);
    }

    [HttpPost]
    [ActionName("Edit")]
    public ActionResult EditOrg(int organizationID) {
      var org = _workUnit.OrganizationRepository.GetEntityByID(organizationID) ?? new Organization();
      TryUpdateModel(org);
      if (ModelState.IsValid) {
        _workUnit.OrganizationRepository.UpdateEntity(org);
        _workUnit.saveChanges();

        //since address might have been updated so update the geotag
        if(CurrentAccount.Profile != null && CurrentAccount.ProfileID.HasValue)
          addUpdateAddrGeoTag(CurrentAccount.ProfileID.Value);

        return RedirectToAction("Detail", "Profile");
      }
      //errors
      return View(org);
    }
  }
}
