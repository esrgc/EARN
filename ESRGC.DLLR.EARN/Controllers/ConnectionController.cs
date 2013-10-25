using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.DLLR.EARN.Domain.DAL.Abstract;
using ESRGC.DLLR.EARN.Filters;

namespace ESRGC.DLLR.EARN.Controllers
{
  [Authorize]
  [VerifyAccount]
  public class ConnectionController : BaseController
  {
    public ConnectionController(IWorkUnit workUnit)
      : base(workUnit) {
    }

    public ActionResult Index() {
      return View();
    }
    public ActionResult ListConnections(int profileID) {
      var profile = _workUnit.ProfileRepository.GetEntityByID(profileID);
      var connections = profile.Connections.ToList();
      return PartialView(connections);
    }
    [HttpGet]
    [VerifyProfile]
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
        updateTempDataMessage("The connection \""+ connectProfile.Organization.Name +"\" has been added to your profile");
        //ViewBag.returnUrl = returnUrl;
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
    [VerifyProfile]
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
        updateTempDataMessage("The connection \"" + connectProfile.Organization.Name + "\" has been removed");
        //ViewBag.returnUrl = returnUrl;
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
