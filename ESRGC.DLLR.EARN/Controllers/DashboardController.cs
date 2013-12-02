using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.DLLR.EARN.Domain.DAL.Abstract;
using ESRGC.DLLR.EARN.Filters;
using ESRGC.DLLR.EARN.Models;

namespace ESRGC.DLLR.EARN.Controllers
{
  [Authorize]
  [RoleAuthorize(Roles = "admin")]
  [VerifyProfile]
  public class DashboardController : BaseController
  {
    public DashboardController(IWorkUnit workUnit) : base(workUnit) { }
    [AllowNonAdmin]
    public ActionResult NavLinks() {
      if (CurrentAccount.Role.ToLower() == "admin")
        return PartialView();
      else
        return new EmptyResult();
    }
    public ActionResult Index() {
      var stats = new DashboardStatistic {
        AccountTotal = _workUnit.AccountRepository.Entities.Count(),
        ProfileTotal = _workUnit.ProfileRepository.Entities.Count(),
        ContactTotal = _workUnit.ContactRepository.Entities.Count(),
        Profiles = _workUnit.ProfileRepository.Entities.OrderBy(x => x.Organization.Name).ToList(),
        Partnerships = _workUnit.PartnershipRepository.Entities.OrderBy(x => x.Name).ToList(),
        OrganizationTotal = _workUnit.OrganizationRepository.Entities.Count()
      };
      return View(stats);
    }

  }
}
