using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using ESRGC.DLLR.EARN.Domain.DAL.Abstract;

namespace ESRGC.DLLR.EARN.Controllers
{
  public class PublicController : BaseController
  {
    public PublicController(IWorkUnit workUnit) : base(workUnit) { }
    public ActionResult Profiles(int? page, int? pageSize) {
      var profiles = _workUnit
        .ProfileRepository
        .Entities
        .OrderBy(x=>x.Organization.Name)
        .ToList();
      int index = page ?? 1;
      int size = pageSize ?? 10;
      var pagedList = profiles.ToPagedList(index, size);
      return View(pagedList);
    }

    public ActionResult Partnerships(int? page, int? pageSize) {
      var partnerships = _workUnit
        .PartnershipRepository
        .Entities
        .OrderBy(x => x.Created)
        .ToList();
      int index = page ?? 1;
      int size = pageSize ?? 10;
      var pagedList = partnerships.ToPagedList(index, size);
      return View(pagedList);
    }

  }
}
