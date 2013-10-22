using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.DLLR.EARN.Domain.DAL.Abstract;

namespace ESRGC.DLLR.EARN.Controllers
{
  [Authorize]
  public class ConnectionController : BaseController
  {
    public ConnectionController(IWorkUnit workUnit)
      : base(workUnit) {
    }

    public ActionResult Index() {
      return View();
    }

    [HttpGet]
    public ActionResult AddConnection(int profileID, string returnUrl) {
      if (CurrentAccount.Profile == null) {
        updateTempDataMessage("You haven't created a currentProfile. Please create one before adding connection");
        return RedirectToAction("Index", "Home");
      }

      var currentProfile = CurrentAccount.Profile;
      try {
        var connectProfile = _workUnit.ProfileRepository.GetEntityByID(profileID);
        currentProfile.addConnection(connectProfile);
        _workUnit.saveChanges();
        updateTempDataMessage("A connection has been added to your profile");
      }
      catch {
        updateTempDataMessage("Could not add connection. An error has occured. Please try again later."); 
      }

      //return to previous url
      if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
          && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\")) {
        return Redirect(returnUrl);
      }
      else {
        return RedirectToAction("Detail", "Profile");
      }
    }

    [HttpGet]
    public ActionResult RemoveConnection(int profileID, string returnUrl) {
      if (CurrentAccount.Profile == null) {
        updateTempDataMessage("You haven't created a currentProfile. Please create one before adding connection");
        return RedirectToAction("Index", "Home");
      }

      var currentProfile = CurrentAccount.Profile;
      try {
        var connectProfile = _workUnit.ProfileRepository.GetEntityByID(profileID);
        currentProfile.removeConnection(connectProfile);
        _workUnit.saveChanges();
        updateTempDataMessage("A connection has been removed from your profile");
      }
      catch {
        updateTempDataMessage("Could not remove connection. An error has occured. Please try again later.");
      }

      //return to previous url
      if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
          && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\")) {
        return Redirect(returnUrl);
      }
      else {
        return RedirectToAction("Detail", "Profile");
      }
    }
    
  }
}
