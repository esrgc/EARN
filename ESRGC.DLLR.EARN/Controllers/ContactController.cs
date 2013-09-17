using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.DLLR.EARN.Domain.DAL.Abstract;

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
  }
}
