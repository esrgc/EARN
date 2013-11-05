using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
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
    [SendNotification]
    public ActionResult InviteToPartnership(int profileID, int partnershipID, string message, string returnUrl) {
      var profile = _workUnit.ProfileRepository.GetEntityByID(profileID);
      ViewBag.returnUrl = returnUrl;
      var senderAccount = CurrentAccount;
      var receiverAccount = profile.getAccount();
      var partnership = _workUnit.PartnershipRepository.GetEntityByID(partnershipID);
      message = message ?? "You have been invited to join our partnership";
      if (partnership != null) {
        var notification = new Notification() {
          Category = "Invite",
          Message = string.Format("You have been invited to join the \"{0}\" partnership", partnership.Name),
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
    public ActionResult SendPartnershipRequest(int partnershipID, string returnUrl) {
      var partnership = _workUnit.PartnershipRepository.GetEntityByID(partnershipID);
      ViewBag.returnUrl = returnUrl;
      return View(partnership);
    }
    [HttpPost]
    [VerifyProfile]
    [NewToPartnership]
    [SendNotification]
    public ActionResult SendPartnershipRequest(int partnershipID, string message, string returnUrl) {
      var partnership = _workUnit.PartnershipRepository.GetEntityByID(partnershipID);
      var ownerProfile = partnership.getOwner();
      var receiverAccount = ownerProfile.getAccount();
      if (receiverAccount == null) {
        updateTempDataMessage("Can not send request to this partnership! Please choose a different partnership.");
        return RedirectToAction("View", "Partnership", new { partnershipID, returnUrl });
      }
      //only send if partnership is valid
      if (partnership != null) {
        //create notification
        var notification = new Notification() {
          Category = "Request",
          Message = string.Format("An organization has requested to join your \"{0}\" partnership", partnership.Name),
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
        return RedirectToAction("View", "Partnership", new { partnershipID, returnUrl });
      }
      //error
      updateTempDataMessage("Error sending join request.");
      return RedirectToAction("View", "Partnership", new { partnershipID, returnUrl }); ;
    }
    public ActionResult Notifications() {
      var notifcations = CurrentAccount
        .Notifications
        .OrderByDescending(x => x.Created)
        .OrderBy(x => x.IsRead)
        .Take(20).ToList();
      var deleteNotes = CurrentAccount
        .Notifications
        .Where(x => {
          var timespan = DateTime.Now - x.Created;
          if (timespan.Days > 60)
            return true;
          else
            return false;
        })
        .ToList();
      deleteNotes.ForEach(x => _workUnit.NotificationRepository.DeleteEntity(x));
      _workUnit.saveChanges();
      return PartialView(notifcations);
    }

    public ActionResult NotificationCount() {
      //new notifications count
      var notifcations = CurrentAccount.Notifications.Where(x => !x.IsRead).ToList();
      return Content(notifcations.Count().ToString());
    }

    public ActionResult ViewNotification(int notificationID) {
      var notification = _workUnit.NotificationRepository.GetEntityByID(notificationID);
      ActionResult result = null;
      switch (notification.Category.ToLower()) {
        case "request accepted":
        case "invite accepted":
          var url = notification.LinkToAction;
          if (!string.IsNullOrEmpty(url))
            result = Redirect(url);
          break;
        case "request":
        case "invite":
          result = RedirectToAction("Requests", "Communication");
          break;
      }
      notification.IsRead = true;
      _workUnit.NotificationRepository.UpdateEntity(notification);
      _workUnit.saveChanges();
      if (result == null)
        return RedirectToAction("index", "home");

      return result;
    }

    public ContentResult RequestCount() {
      var requests = CurrentAccount.ReceivedRequests
        .Where(x => x.Status.ToLower() == "new").ToList();
      return Content(requests.Count().ToString());
    }

    public ActionResult Requests() {
      var requests = CurrentAccount
        .ReceivedRequests
        .ToList();
      return View(requests);
    }
    [SendNotification]
    public ActionResult AcceptRequest(int requestID) {
      var request = _workUnit.RequestRepository.GetEntityByID(requestID);
      PartnershipRequest r = null;
      Notification notification = null;
      PartnershipDetail detail = null;
      switch (request.Type.ToLower()) {
        case "partnership request":
          r = (PartnershipRequest)request;
          if (r.Partnership != null) {
            detail = new PartnershipDetail() {
              Partnership = r.Partnership,
              Profile = r.Sender.Profile,
              Type = "partner"
            };
            var message = string.Format(@"You have accepted {0} to join the ""{1}"" partnership.",
              r.Sender.Profile.Organization.Name,
              r.Partnership.Name);
            updateTempDataMessage(message);
            //create notification to sender
            notification = new Notification() {
              Account = r.Sender,
              Category = "Request Accepted",
              Message = string.Format(@"{0} has accepted your request. You are now a partner of ""{1}""",
                r.Receiver.Profile.Organization.Name,
                r.Partnership.Name),
              LinkToAction = Url.Action("View", "Partnership", new { r.PartnershipID })
            };
          }
          break;
        case "partnership invite":
          r = (PartnershipRequest)request;
          if (r.Partnership != null) {
            detail = new PartnershipDetail() {
              Partnership = r.Partnership,
              Profile = r.Receiver.Profile,
              Type = "partner"
            };
            _workUnit.PartnershipDetailRepository.InsertEntity(detail);
            var message = "You are now a partner of the \"" + r.Partnership.Name + "\" partnership.";
            updateTempDataMessage(message);
            //create notification
            notification = new Notification() {
              Account = r.Sender,
              Category = "Invite Accepted",
              Message = string.Format(@"{0} has accepted your invitation to join the ""{1}"" partnership",
                r.Receiver.Profile.Organization.Name,
                r.Partnership.Name),
              LinkToAction = Url.Action("View", "Partnership", new { r.PartnershipID })
            };
          }
          break;
      }
      if (notification != null && detail != null) {
        _workUnit.PartnershipDetailRepository.InsertEntity(detail);
        _workUnit.NotificationRepository.InsertEntity(notification);
        _workUnit.RequestRepository.DeleteByID(requestID);
        _workUnit.saveChanges();
      }
      else {
        updateTempDataMessage("Error processing request.");
      }
      return RedirectToAction("Requests");
    }
    [HttpPost]
    public ActionResult DeleteRequest(int requestID) {
      var request = _workUnit.RequestRepository.GetEntityByID(requestID);
      _workUnit.RequestRepository.DeleteByID(requestID);
      _workUnit.saveChanges();
      return RedirectToAction("Requests");
    }

  }
}
