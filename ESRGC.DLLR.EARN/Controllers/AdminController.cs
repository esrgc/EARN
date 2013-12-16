using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.DLLR.EARN.Domain.DAL.Abstract;
using ESRGC.DLLR.EARN.Domain.Model;
using ESRGC.DLLR.EARN.Filters;
using ESRGC.DLLR.EARN.Helpers;

namespace ESRGC.DLLR.EARN.Controllers
{
  [Authorize]
  [RoleAuthorize(Roles = "admin")]
  public class AdminController : BaseController
  {
    public AdminController(IWorkUnit workUnit)
      : base(workUnit) {

    }
    public ActionResult Index() {
      return View();
    }

    [AllowNonAdmin]//child action
    public ActionResult NavLinks() {
      if (CurrentAccount.Role.ToLower() == "admin")
        return PartialView();
      else
        return new EmptyResult();
    }

    
  }
}
