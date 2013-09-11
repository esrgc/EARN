using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.DLLR.EARN.Models;

namespace ESRGC.DLLR.EARN.Controllers
{
  [Authorize]
  public class AccountController : BaseController
  {
    public ActionResult Index() {
      return View();
    }
    [AllowAnonymous]
    public ActionResult SignUp() {
      return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public ActionResult SignUp(SignUpModel model) {
      return View();
    }
    [AllowAnonymous]
    public ActionResult SignIn() {
      return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public ActionResult SignIn(SignInModel model) {
      return View();
    }

    public ActionResult SignOut() {
      return View();
    }



  }
}
