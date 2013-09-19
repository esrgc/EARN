using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.DLLR.EARN.Domain.DAL.Abstract;
using ESRGC.DLLR.EARN.Domain.Model;

namespace ESRGC.DLLR.EARN.Controllers
{
  public class ProfileController : BaseController
  {
    public ProfileController(IWorkUnit workUnit) : base(workUnit) { 
    }

    public ActionResult Index() {
      return View();
    }
    public ActionResult Create() {
      return View();
    }
    [HttpPost]
    public ActionResult Create(Profile profile) {
      return View();
    }
  }
}
