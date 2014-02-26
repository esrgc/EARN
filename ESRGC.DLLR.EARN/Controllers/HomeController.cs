using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.DLLR.EARN.Domain.Model;
using ESRGC.DLLR.EARN.Filters;
using Postal;

namespace ESRGC.DLLR.EARN.Controllers
{
  public class HomeController : BaseController
  {
    //
    // GET: /Home/
    [SetGuestCookie]
    public ActionResult Index(string returnUrl) {
      ViewBag.returnUrl = returnUrl;
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
    public ViewResult CookieNotEnabled() {
      return View();
    }
    
  }
}
