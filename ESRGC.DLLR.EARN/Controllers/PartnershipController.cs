using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.DLLR.EARN.Domain.DAL.Abstract;
using ESRGC.DLLR.EARN.Domain.Model;

namespace ESRGC.DLLR.EARN.Controllers
{
  [Authorize]
  public class PartnershipController : BaseController
  {
    public PartnershipController(IWorkUnit workUnit) : base(workUnit) { }
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
      var partnership = _workUnit.PartnershipRepository.GetEntityByID(partnershipID);
      return View(partnership);
    }
    /// <summary>
    /// list partnerships for provided profile
    /// </summary>
    /// <param name="profileID"></param>
    /// <returns></returns>
    public ActionResult ListPartnerships(int profileID) {
      var profile = _workUnit.ProfileRepository.GetEntityByID(profileID);
      var partnerships = profile.PartnershipDetails.Select(x => x.Partnership).ToList();
      ViewBag.currentProfile = CurrentAccount.Profile;
      return PartialView(partnerships);
    }
    /// <summary>
    /// create a new partnership
    /// </summary>
    /// <returns></returns>
    public ActionResult Create() {
      return View();
    }
    [HttpPost]
    public ActionResult Create(Partnership partnership) {
      if (ModelState.IsValid) {
        var partnershipDetail = new PartnershipDetail() {
          Profile = CurrentAccount.Profile,
          Type = "Convener",
          Partnership = partnership
        };
        _workUnit.PartnershipDetailRepository.InsertEntity(partnershipDetail);
        _workUnit.saveChanges();
        return RedirectToAction("Detail", new { partnership.PartnershipID });
      }
      //error
      return View(partnership);
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
