using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.DLLR.EARN.Domain.DAL.Abstract;

namespace ESRGC.DLLR.EARN.Controllers
{
  public class SearchController : BaseController
  {
    public SearchController(IWorkUnit workUnit) : base(workUnit) { }
    //
    // GET: /Search/
    public ActionResult Index() {
      var profiles = _workUnit.ProfileRepository.Entities.OrderBy(x => x.Organization.Name).ToList();

      return View(profiles);
    }

  }
}
