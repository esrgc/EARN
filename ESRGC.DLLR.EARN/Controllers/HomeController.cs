using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ESRGC.DLLR.EARN.Controllers
{
  public class HomeController : BaseController
  {
    //
    // GET: /Home/

    public ActionResult Index() {
      return View();
    }

    public ActionResult ContactUs() {
      return View();
    }

    public ActionResult UnauthorizedAccess() {
      return View();
    }
    public ActionResult ProfileNotCreated() {
      return View();
    }
  }
}
