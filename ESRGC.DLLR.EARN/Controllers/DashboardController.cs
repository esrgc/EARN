using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.DLLR.EARN.Domain.DAL.Abstract;
using ESRGC.DLLR.EARN.Domain.Model;
using ESRGC.DLLR.EARN.Filters;
using ESRGC.DLLR.EARN.Models;

namespace ESRGC.DLLR.EARN.Controllers
{
  [Authorize]
  [RoleAuthorize(Roles = "admin")]  
  public class DashboardController : BaseController
  {
    public DashboardController(IWorkUnit workUnit) : base(workUnit) { }
    
    [VerifyProfile]
    public ActionResult Index() {
      var stats = new DashboardStatistic {
        AccountTotal = _workUnit.AccountRepository.Entities.Count(),
        ProfileTotal = _workUnit.ProfileRepository.Entities.Count(),
        ContactTotal = _workUnit.ContactRepository.Entities.Count(),
        TagTotal = _workUnit.TagRepository.Entities.Where(x=>!(x is GeoTag)).Count(),
        Profiles = _workUnit.ProfileRepository.Entities.OrderBy(x => x.Organization.Name).ToList(),
        Partnerships = _workUnit.PartnershipRepository.Entities.OrderBy(x => x.Name).ToList(),
        OrganizationTotal = _workUnit.OrganizationRepository.Entities.Count(),
        Accounts = _workUnit.AccountRepository.Entities.OrderByDescending(x=>x.LastLogin).ToList()
      };
      return View(stats);
    }

  }
}
