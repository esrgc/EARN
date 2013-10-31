using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.DLLR.EARN.Domain.DAL.Abstract;
using ESRGC.DLLR.EARN.Domain.Model;
using ESRGC.DLLR.EARN.Filters;

namespace ESRGC.DLLR.EARN.Controllers
{
  [Authorize]
  public class CommunicationController : BaseController
  {
    public CommunicationController(IWorkUnit workUnit) : base(workUnit) { }
    [VerifyProfile]
    public ActionResult InviteToPartnership(int profileID, string returnUrl) {
      var profile = _workUnit.ProfileRepository.GetEntityByID(profileID);
      ViewBag.returnUrl = returnUrl;
      return View(profile);
    }
    [HttpPost]
    [VerifyProfile]
    public ActionResult InviteToPartnership(int profileID, int partnershipID, string message,string returnUrl) {
      var profile = _workUnit.ProfileRepository.GetEntityByID(profileID);
      ViewBag.returnUrl = returnUrl;
      var senderAccount = CurrentAccount;
      var receiverAccount = profile.getAccount();
      var partnership = _workUnit.PartnershipRepository.GetEntityByID(partnershipID);
      message = message ?? "You have been invited to join our partnership";
      if (partnership != null) {
        var notification = new Notification() {
          Category = "Invite",
          Message = "You have been invited to join a partnership",
          Account = receiverAccount
        };
        //create request
        var request = new PartnershipRequest() {
          Type = "Partnership Invite",
          PartnershipID = partnershipID,
          Message = message,
          Sender = senderAccount,
          Receiver = receiverAccount,
          Notification = notification
        };
        _workUnit.RequestRepository.InsertEntity(request);
        _workUnit.saveChanges();
        updateTempDataMessage("Invitation has been sent");
      }
      else {
        updateTempDataMessage("Error sending the invite.");
      }
      return View(profile);
    }
    [VerifyProfile]
    [NewToPartnership]
    public ActionResult SendPartnershipInvite(int partnershipID, int profileID) {

      return View();
    }
    
    [VerifyProfile]
    [NewToPartnership]
    public ActionResult SendPartnershipRequest(int partnershipID) {
      var partnership = _workUnit.PartnershipRepository.GetEntityByID(partnershipID);
      
      return View(partnership);
    }
    [HttpPost]
    [VerifyProfile]
    [NewToPartnership]
    public ActionResult SendPartnershipRequest(int partnershipID, string message) {
      var partnership = _workUnit.PartnershipRepository.GetEntityByID(partnershipID);
      var ownerProfile = partnership.getOwner();
      var receiverAccount = ownerProfile.getAccount();
      if (receiverAccount == null) {
        updateTempDataMessage("Can not send request to this partnership! Please choose a different partnership.");
        return RedirectToAction("View", "Partnership", new { partnershipID });
      }
      //only send if partnership is valid
      if (partnership != null) {
        //create notification
        var notification = new Notification() {
          Category = "Request",
          Message = "An organization has requested to join your partnership",
          Account = receiverAccount
        };
        //create request
        var request = new PartnershipRequest() { 
          Type = "Partnership Request",
          PartnershipID = partnershipID,
          Message = message,
          Sender = CurrentAccount,
          Receiver = receiverAccount,
          Notification = notification
        };
        
        _workUnit.RequestRepository.InsertEntity(request);
        _workUnit.saveChanges();
        updateTempDataMessage("Your request has been sent to the owner of this partnership");
        return RedirectToAction("View", "Partnership", new { partnershipID });
      }
      //error
      updateTempDataMessage("Error sending join request.");
      return RedirectToAction("View", "Partnership", new { partnershipID }); ;
    }
    public ActionResult Notifications() {
      var notifcations = CurrentAccount.Notifications.ToList();
      return new EmptyResult();
    }

    public ContentResult RequestCount() {
      var requests = CurrentAccount.ReceivedRequests
        .Where(x => x.Status.ToLower() == "new").ToList();
      return Content(requests.Count().ToString());
    }

    public ActionResult Requests() {
      var requests = CurrentAccount.ReceivedRequests.ToList();
      return View(requests);
    }
  }
}
