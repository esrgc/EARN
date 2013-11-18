using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using ESRGC.DLLR.EARN.Domain.DAL.Abstract;
using ESRGC.DLLR.EARN.Domain.Model;
using ESRGC.DLLR.EARN.Filters;
using PagedList;

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
          Category = "Invite Recieved",
          Message = string.Format("You have been invited to join the \"{0}\" partnership", partnership.Name),
          Account = receiverAccount,
          LinkToAction = Url.Action("Requests", "Communication")
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
        updateTempMessage("Invitation has been sent");
      }
      else {
        updateTempMessage("Error sending the invite.");
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
        updateTempMessage("Can not send request to this partnership! Please choose a different partnership.");
        return RedirectToAction("View", "Partnership", new { partnershipID, returnUrl });
      }
      //only send if partnership is valid
      if (partnership != null) {
        //create notification
        var notification = new Notification() {
          Category = "Request Received",
          Message = string.Format("{0} has requested to join your \"{1}\" partnership",
            CurrentAccount.Profile.Organization.Name,
            partnership.Name),
          Account = receiverAccount,
          Message2 = @"You will be able to choose whether to accept this request, 
and/or view this user’s Organizational Profile for more information.",
          LinkToAction = Url.Action("Requests", "Communication")
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
        updateTempMessage("Your request has been sent to the admin of this partnership");
        return RedirectToAction("View", "Partnership", new { partnershipID, returnUrl });
      }
      //error
      updateTempMessage("Error sending join request.");
      return RedirectToAction("View", "Partnership", new { partnershipID, returnUrl }); ;
    }
    [VerifyAccount]
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
    [VerifyAccount]
    public ActionResult NotificationCount() {
      //new notifications count
      var notifcations = CurrentAccount.Notifications.Where(x => !x.IsRead).ToList();
      return Content(notifcations.Count().ToString());
    }
    [VerifyAccount]
    public ActionResult MarkAllNotificationsAsRead(string returnUrl) {
      var notifications = _workUnit.NotificationRepository.Entities
        .Where(x => x.AccountID == CurrentAccount.AccountID)
        .ToList();
      notifications.ForEach(x => {
        x.IsRead = true;
        _workUnit.NotificationRepository.UpdateEntity(x);
      });
      _workUnit.saveChanges();
      return returnToUrl(Request.UrlReferrer.LocalPath, Url.Action("Detail", "Profile"));
    }
    public ActionResult ViewNotification(int notificationID) {
      var notification = _workUnit.NotificationRepository.GetEntityByID(notificationID);
      ActionResult result = null;
      switch (notification.Category.ToLower()) {
        case "request":
        case "invite":
          result = RedirectToAction("Requests", "Communication");
          break;
        case "request accepted":
        case "invite accepted":
        default:
          var url = notification.LinkToAction;
          if (!string.IsNullOrEmpty(url))
            result = Redirect(url);
          break;
      }
      //mark all as read
      notification.Account.Notifications.ToList().ForEach(x => {
        notification.IsRead = true;
        _workUnit.NotificationRepository.UpdateEntity(x);
      });
      _workUnit.saveChanges();
      if (result == null)
        return RedirectToAction("index", "home");

      return result;
    }
    [VerifyAccount]
    public ContentResult RequestCount() {
      var requests = CurrentAccount.ReceivedRequests
        .Where(x => x.Status.ToLower() == "new").ToList();
      return Content(requests.Count().ToString());
    }
    [VerifyAccount]
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
            //check if the requested organization is already a partner
            if (r.Partnership.getPartners().Contains(r.Sender.Profile)) {
              updateTempMessage("\"" + r.Sender.Profile.Organization.Name
                + "\"is already a partner of this partnership");
              break;
            }
            //form a partner reference
            detail = new PartnershipDetail() {
              Partnership = r.Partnership,
              Profile = r.Sender.Profile,
              Type = "partner"
            };
            var message = string.Format(@"You have accepted {0} to join the ""{1}"" partnership.",
              r.Sender.Profile.Organization.Name,
              r.Partnership.Name);
            updateTempMessage(message);
            //create notification to sender
            notification = new Notification() {
              Account = r.Sender,
              Category = "Partnership Request Accepted",
              Message = string.Format(@"{0} has accepted your request. You are now a partner of ""{1}""",
                r.Receiver.Profile.Organization.Name,
                r.Partnership.Name),
              Message2 = "You can now visit the \"" + r.Partnership.Name
                + "\" partnership’s profile, and communicate with your partners!",
              LinkToAction = Url.Action("Detail", "Partnership", new { r.PartnershipID })
            };
          }
          break;
        case "partnership invite":
          r = (PartnershipRequest)request;
          if (r.Partnership != null) {
            //check if the invited organization is already a partner
            if (r.Partnership.getPartners().Contains(r.Receiver.Profile)) {
              updateTempMessage("You are already a partner of this partnership");
              break;
            }
            //form a partner reference
            detail = new PartnershipDetail() {
              Partnership = r.Partnership,
              Profile = r.Receiver.Profile,
              Type = "partner"
            };
            _workUnit.PartnershipDetailRepository.InsertEntity(detail);
            var message = "You are now a partner of the \"" + r.Partnership.Name + "\" partnership.";
            updateTempMessage(message);
            //create notification
            notification = new Notification() {
              Account = r.Sender,
              Category = "Partnership Invite Accepted",
              Message = string.Format(@"{0} is now a new partner of your ""{1}"" partnership",
                r.Receiver.Profile.Organization.Name,
                r.Partnership.Name),
              LinkToAction = Url.Action("Detail", "Partnership", new { r.PartnershipID })
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

      return RedirectToAction("Requests");
    }
    [HttpPost]
    public ActionResult DeleteRequest(int requestID) {
      var request = _workUnit.RequestRepository.GetEntityByID(requestID);
      _workUnit.RequestRepository.DeleteByID(requestID);
      _workUnit.saveChanges();
      return RedirectToAction("Requests");
    }
    [VerifyProfile]
    [VerifyProfilePartnership]
    public ActionResult ListComments(int partnershipID, int? page, int? size) {
      try {
        var partnership = _workUnit.PartnershipRepository.GetEntityByID(partnershipID);
        int pageIndex = page ?? 1;
        int pageSize = size ?? 15;
        var comments = partnership.Comments.ToPagedList(pageIndex, pageSize);
        return PartialView(comments);
      }
      catch (Exception) {
        return new EmptyResult();
      }
    }
    [HttpPost]
    [VerifyProfile]
    [VerifyProfilePartnership]
    [SendNotification]
    public ActionResult PostComment(int partnershipID, string comment, string returnUrl) {
      var partnership = _workUnit.PartnershipRepository.GetEntityByID(partnershipID);
      var c = new Comment() {
        Partnership = partnership,
        Content = comment,
        Author = CurrentAccount.Profile
      };
      _workUnit.CommentRepository.InsertEntity(c);

      //create notifications
      foreach (var p in partnership.PartnershipDetails.Select(x => x.Profile).ToList()) {
        if (p.ProfileID != CurrentAccount.ProfileID) {
          var account = p.getAccount();
          var notification = new Notification() {
            Account = account,
            LinkToAction = Url.Action("Detail", "Partnership", new { partnershipID, returnUrl }),
            Category = "New Comment",
            Message = CurrentAccount.Profile.Organization.Name
            + " has posted a new comment on the partnership \""
            + partnership.Name + "\"",
            Message2 = comment
          };
          _workUnit.NotificationRepository.InsertEntity(notification);
        }
      }
      _workUnit.saveChanges();
      //return to previous url
      if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
          && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\")) {
        return Redirect(returnUrl);
      }
      else {
        return RedirectToAction("Detail", "Partnership", new { partnershipID });
      }
    }
    [HttpPost]
    [VerifyProfile]
    public ActionResult DeleteComment(int commentID, string returnUrl) {
      try {
        var comment = _workUnit.CommentRepository.GetEntityByID(commentID);
        var partnershipID = comment.PartnershipID;
        _workUnit.CommentRepository.DeleteEntity(comment);
        _workUnit.saveChanges();
        updateTempMessage("Your comment has been deleted.");
        //return to previous url
        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\")) {
          return Redirect(returnUrl);
        }
        else {
          return RedirectToAction("Detail", "Partnership", new { partnershipID });
        }
      }
      catch (Exception) {
        updateTempMessage("An error has occured while deleting your comment.");
        return RedirectToAction("Index", "Partnership");
      }
    }
  }
}
