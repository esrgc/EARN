﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using ESRGC.DLLR.EARN.Domain.DAL.Abstract;
using ESRGC.DLLR.EARN.Domain.Model;
using ESRGC.DLLR.EARN.Filters;
using PagedList;
using ESRGC.DLLR.EARN.Helpers;
using ESRGC.DLLR.EARN.Models;
using System.Configuration;

namespace ESRGC.DLLR.EARN.Controllers
{
  [Authorize]
  public class CommunicationController : BaseController
  {
    public CommunicationController(IWorkUnit workUnit) : base(workUnit) { }
    //for navigation bar to refresh (update notifications)
    public PartialViewResult NavigationBar() {
      return PartialView("navBar");
    }
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
      if (ModelState.IsValid) {
        deleteNotes.ForEach(x => _workUnit.NotificationRepository.DeleteEntity(x));
        _workUnit.saveChanges();
      }
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

    [VerifyAccount]
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
      if (ModelState.IsValid) {
        notification.Account.Notifications.ToList().ForEach(x => {
          notification.IsRead = true;
          _workUnit.NotificationRepository.UpdateEntity(x);
        });
        _workUnit.saveChanges();
      }
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
      ProfileMemberRequest pr = null;
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
        case "profile member request":
          pr = (ProfileMemberRequest)request;
          var senderProfile = pr.Sender.Profile;
          if (senderProfile == null) {
            //new account without profile -> can accept
            pr.Sender.ProfileID = pr.Receiver.ProfileID;
            pr.Sender.IsProfileOwner = false;
            _workUnit.AccountRepository.UpdateEntity(pr.Sender);
            //create notification
            notification = new Notification() {
              Account = pr.Sender,
              Category = "Profile Request Accepted",
              Message = string.Format(@"Congratulations! You are now a member of the ""{1}"" organizational profile.",
                pr.Receiver.Profile.Organization.Name),
              Message2 = "You can now edit profile information, search for partnerships and other partners.",
              LinkToAction = Url.Action("Detail", "Profile")
            };
          }
          else {
            updateTempMessage("Can not accept this request. The requesting account already belongs to an organizational profile.");
          }
          _workUnit.RequestRepository.DeleteByID(requestID);
          break;
      }
      if (notification != null) {
        _workUnit.NotificationRepository.InsertEntity(notification);
      }
      if (detail != null) {
        _workUnit.PartnershipDetailRepository.InsertEntity(detail);
        _workUnit.RequestRepository.DeleteByID(requestID);
      }
      _workUnit.saveChanges();

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
    [ValidateAntiForgeryToken]
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
    [ValidateAntiForgeryToken]
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
    [VerifyProfile]
    [HasReturnUrl]
    public ActionResult SendContactEarnEmail(string returnUrl) {
      return View();
    }
    [AllowAnonymous]
    [HasReturnUrl]
    public ActionResult SendFeedbackEmail(string returnUrl) {
      var model = new AnonymousMessageModel();
      return View(model);
    }
    [AllowAnonymous]
    [HasReturnUrl]
    public ActionResult SendTechnicalReportEmail(string returnUrl) {
      var model = new AnonymousMessageModel();
      return View(model);
    }
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    [SendCustomerMessage]
    public ActionResult SendCustomerEmail(string emailAddress, string name, string subject, string message, string returnUrl) {
      Profile currentProfile = null;
      if (!Request.IsAuthenticated) {
        if (!ModelState.IsValid) {
          updateTempMessage("Sorry! We could not send your message. Please try again.");
          return RedirectToAction("Index", "Home");
        }
        //send message without storing to database
        currentProfile = new Profile {
          Contact = new Contact {
            Email = emailAddress
          },
          Organization = new Organization {
            Name = name
          }
        };
        var m = new Message {
          Sender = currentProfile,
          Receiver = currentProfile,//this means messages go to EARN CONNECT (Customer messages)
          Title = subject,
          Header = currentProfile.Organization.Name + " has sent a message",
          Message2 = message
        };
        EmailHelper.SendCustomerEmailMessage(m);
      }
      else {
        currentProfile = CurrentAccount.Profile;
        //create a message
        var m = new Message {
          Sender = currentProfile,
          Receiver = currentProfile,//this means messages go to EARN CONNECT (Customer messages)
          Title = subject,
          Header = currentProfile.Organization.Name + " has sent a message",
          Message2 = message
        };
        _workUnit.MessageRepository.InsertEntity(m);
        _workUnit.saveChanges();
      }
      updateTempMessage("Your message has been sent to us. Please wait while we review your message. We will respond as soon as possible. Thank you!");
      return returnToUrl(returnUrl, Url.Action("Index", "Home"));
    }

    [RoleAuthorize(Roles = "admin")]
    public ActionResult EmailAnnounce() {
      return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [RoleAuthorize(Roles = "admin")]
    public ActionResult EmailAnnounce(string message, string message2, string subject, string header) {
      if (!string.IsNullOrEmpty(message)) {
        //get email list
        var emailList = _workUnit.AccountRepository
          .Entities
          .Select(x => x.EmailAddress).ToList();
        var m = new Message {
          Sender = null,
          Receiver = null,//this means messages go to EARN CONNECT (Customer messages)
          Title = subject,
          Header = header ?? "Dear EARN MD CONNECT User",
          Message1 = message,
          Message2 = message2
        };

        try {
          _workUnit.MessageRepository.InsertEntity(m);
          _workUnit.saveChanges();
        }
        catch {
          //do logging for errors or exception filter
          updateTempMessage("Message couldn't be stored to database.");
        }

        if (emailList.Count() > 0) {
          emailList.ForEach(x => {
            EmailHelper.SendAnnouncementEmail(m, x);
          });
          string earnEmail = "", esrgcEmail = "";
          try {
            earnEmail = ConfigurationManager.AppSettings["earnEmail"].ToString();
            esrgcEmail = ConfigurationManager.AppSettings["esrgcEmail"].ToString();
          }
          catch {
            earnEmail = "earn.jobs@maryland.gov";
            esrgcEmail = "esrgc@salisbury.edu";
          }
          EmailHelper.SendAnnouncementEmail(m, earnEmail);
          EmailHelper.SendAnnouncementEmail(m, esrgcEmail);
          return View("EmailSent");
        }
        else {
          updateTempMessage("No email address found");
        }
      }
      return View("EmailNotSent");
    }

    public ActionResult JoinProfileRequest(int profileID) {
      var profile = _workUnit.ProfileRepository.GetEntityByID(profileID);
      return View(profile);
    }
    [HttpPost]
    [SendNotification]
    public ActionResult SendJoinProfileRequest(int profileID, string name, string message) {
      var profile = _workUnit.ProfileRepository.GetEntityByID(profileID);
      if (profile == null) {
        updateTempMessage("Profile ID is invalid");
        return RedirectToAction("Index", "Home");
      }
      var owner = profile.getAccount();
      if (owner == null) {
        updateTempMessage("No owner found for this profile. ID " + profile.ProfileID);
        return RedirectToAction("Index", "Home");
      }
      var message1 = string.Format(
          @"{0} has requested to join your ""{1}"" profile. Contact {0} with at {2} if you need additional information.",
          name,
          profile.Organization.Name,
          CurrentAccount.EmailAddress
      );
      var message2 = @"Upon acceptance of this request, " + name +
        " will be able to have full access to your profile and edit profile information." +
        " If you do not regconize the person or the email address above, please discard this request or " +
        "contact the person for more information.";

      var notification = new Notification() {
        Category = "Request Received",
        Message = message1,
        Account = owner,
        Message2 = "Message: " + message,
        Message3 = message2,
        LinkToAction = Url.Action("Requests", "Communication")
      };
      var request = new ProfileMemberRequest() {
        Message = string.Format(
          @"{0} ({2}) has requested to join your ""{1}"" profile with the following message: {3}",
          name,
          profile.Organization.Name,
          CurrentAccount.EmailAddress,
          message
      ),
        Sender = CurrentAccount,
        Receiver = owner,
        Notification = notification,
        Profile = profile
      };
      _workUnit.RequestRepository.InsertEntity(request);
      _workUnit.saveChanges();
      updateTempMessage("Your request has been sent. You will be notified via email once the request is accepted.");
      return RedirectToAction("Index", "Home");
    }
  }
}
