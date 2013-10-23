using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ESRGC.DLLR.EARN.Controllers
{
  public class PartnershipController : BaseController
  {
    //
    // GET: /Partnership/
    //for search
    public ActionResult Index() {
      return View();
    }
    /// <summary>
    /// View partnership detail
    /// </summary>
    /// <param name="partnershipID"></param>
    /// <returns></returns>
    public ActionResult Detail(int partnershipID) {
      return View();
    }
    /// <summary>
    /// create a new partnership
    /// </summary>
    /// <returns></returns>
    public ActionResult Create() {
      return View();
    }
    /// <summary>
    /// Manage partnership
    /// </summary>
    /// <param name="partnershipID"></param>
    /// <returns></returns>
    public ActionResult Edit(int partnershipID) {
      return View();
    }
    /// <summary>
    /// Delete a partnership
    /// </summary>
    /// <param name="partnershipID"></param>
    /// <returns></returns>
    public ActionResult Delete(int partnershipID) {
      return View();
    }


  }
}
