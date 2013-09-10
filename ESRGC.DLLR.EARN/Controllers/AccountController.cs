using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.DLLR.EARN.Models;

namespace ESRGC.DLLR.EARN.Controllers
{
  public class AccountController : Controller
  {
    //
    // GET: /Account/

    public ActionResult Index() {
      return View();
    }

    public ActionResult SignUp() {
      return View();
    }
    [HttpPost]
    public ActionResult SignUp(SignUpModel model) {
      return View();
    }

    public ActionResult SignIn() {
      return View();
    }

    [HttpPost]
    public ActionResult SignIn(SignInModel model) { 
      return View();
    }

    public ActionResult SignOut(){
      return View();
    }


  }
}
