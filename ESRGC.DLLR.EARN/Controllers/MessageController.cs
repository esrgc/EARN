using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.DLLR.EARN.Domain.DAL.Abstract;
using ESRGC.DLLR.EARN.Domain.Model;
using ESRGC.DLLR.EARN.Filters;
using ESRGC.DLLR.EARN.Helpers;

namespace ESRGC.DLLR.EARN.Controllers
{
  [Authorize]
  [VerifyProfile]
  public class MessageController : BaseController
  {

    public MessageController(IWorkUnit workUnit) : base(workUnit) { }
    //
    // GET: /Message/

    public ActionResult Index() {
      return View(CurrentAccount);
    }

    //fetch messages for selected participant
    public JsonResult Fetch(int participantID) {
      var participant = _workUnit.ProfileRepository.GetEntityByID(participantID);
      var profile = CurrentAccount.Profile;
      var messages = profile.MessageBoards
        .Where(x => ((x.Message.SenderID == participantID) 
          || (x.Message.ReceiverID == participantID) 
          || (x.Message.ReceiverID == null && (x.Message.SenderID == participantID || x.Message.SenderID == profile.ProfileID))))
        .OrderBy(x => x.Message.Created)
        .Select(x => new {
          id = x.MessageID,
          type = x.Type,
          senderName = x.Message.Sender.Organization.Name,
          message = x.Message.Message1,
          participantID = participantID,
          participantName = participant.Organization.Name,
          date = x.Message.Created.ToShortDateString(),
          time = x.Message.Created.ToShortTimeString(),
          profileUrl = Url.Action("ViewProfile", "Profile", new { profileID = x.Message.Sender.ProfileID, returnUrl = Url.Action("index") }),
          isAdminMessage = x.Message.ReceiverID == null
        })
      .ToList();
      return Json(messages, JsonRequestBehavior.AllowGet);

    }

    public JsonResult Participants() {
      var profile = CurrentAccount.Profile;
      //get participants that this profile sent messages to
      var p1 = _workUnit.MessageBoardRepository
        .Entities
        .Where(x => x.Message.SenderID == profile.ProfileID)
        .Where(x => x.Type != "adminMessage")
        .Select(x => x.Message.Receiver).ToList();
      //get participants that this profile received messages from
      var p2 = _workUnit.MessageBoardRepository
        .Entities
        .Where(x => x.Message.ReceiverID == profile.ProfileID)
        .Select(x => x.Message.Sender).ToList();
     
      //var annountments = profile.MessageBoards
      //  .Where(x => x.Type == "adminMessage")
        
      
      var participantList = p1.Union(p2);

      var participants = participantList.Select(x => {
        var lastMsg = x.MessageBoards
          .Where(m=>m.Message.SenderID == profile.ProfileID || m.Message.ReceiverID == profile.ProfileID)
          .OrderByDescending(m => m.Message.Created)
          .First().Message;

        var returnData = new {
          id = x.ProfileID,
          name = x.Organization.Name,
          logoUrl = Url.Action("ProfileLogo", new { Id = x.ProfileID }),
          lastMessage = lastMsg.Message1.toShorDescription(50),
          lastMessageDate = lastMsg.Created.ToShortDateString(),
          lastMessageTime = lastMsg.Created.ToShortTimeString(),
          dateTime = lastMsg.Created //for sorting
        };
        return returnData;
      })
        .OrderByDescending(x => x.dateTime)
        .ToList();
      return Json(participants, JsonRequestBehavior.AllowGet);

    }

    //[HttpPost]
    //[SendNotification]
    //public ActionResult Send(string name, string message) {
    //  //get participant id
    //  var id = _workUnit.ProfileRepository.Entities
    //    .First(x => x.Organization.Name == name).ProfileID;
    //  return Send(id, message);
    //}
    [HttpPost]
    [SendNotification]
    public ActionResult Send(int? participantID, string name, string message) {
      var sender = CurrentAccount.Profile;
      int id;
      //find id when name is used
      if (participantID == null) {
        //get participant id
        id = _workUnit.ProfileRepository.Entities
          .First(x => x.Organization.Name.ToLower() == name.ToLower())
          .ProfileID;
      }
      else
        id = participantID.Value;

      var recipient = _workUnit.ProfileRepository.GetEntityByID(id);

      if (recipient == null)
        return Json(new { status = "failed", message = "Invalid recipient. ID or name: " + participantID }, JsonRequestBehavior.AllowGet);
      //save name for return 
      if (string.IsNullOrEmpty(name))
        name = recipient.Organization.Name;

      //send the message
      try {
        //create new message object
        var msg = new Message() {
          Sender = sender,
          Receiver = recipient,
          Message1 = message
        };
        _workUnit.MessageRepository.InsertEntity(msg);
        //create message boards
        var senderMsgBoard = new MessageBoard() {
          Message = msg,
          Type = "message",
          Profile = sender
        };
        _workUnit.MessageBoardRepository.InsertEntity(senderMsgBoard);
        var receiverMsgBoard = new MessageBoard() {
          Message = msg,
          Type = "message",
          Profile = recipient
        };
        _workUnit.MessageBoardRepository.InsertEntity(receiverMsgBoard);

        //create notiification for recipient
        recipient.Accounts.ToList().ForEach(x => {
          var notification = new Notification() {
            Category = "Message Received",
            Message = sender.Organization.Name + " has sent a new message.",
            Account = x,
            Message2 = "Message: " + message.Replace("<br />", "\r\n").toShorDescription(150),
            Message3 = "You can view and reply to this message at EARN MD CONNECT.",
            LinkToAction = string.Format(Url.Action("Index") + "#for/{0}/{1}",
              sender.Organization.Name,
              sender.ProfileID
            )
          };
          _workUnit.NotificationRepository.InsertEntity(notification);
        });

        _workUnit.saveChanges();
      }
      catch (Exception ex) {
        return Json(new { status = "failed", message = ex.Message }, JsonRequestBehavior.AllowGet);
      }
      return Json(new { status = "success", id = id, name = name }, JsonRequestBehavior.AllowGet);
    }
    /// <summary>
    /// admin send a message to all profiles
    /// </summary>
    /// <returns></returns>
    //[HttpPost]
    [RoleAuthorize(Roles = "admin")]
    [SendNotification]
    public ActionResult AdminSend(string message) {
      var sender = CurrentAccount.Profile;
      var recipients = _workUnit.ProfileRepository.Entities
        .Where(x => x.ProfileID != sender.ProfileID)
        .ToList();
      if (recipients == null)
        return Json(new { status = "failed", message = "Couldn't get recipient profiles" }, JsonRequestBehavior.AllowGet);
      try {
        //create new message object
        var msg = new Message() {
          Message1 = message,
          Sender = sender
        };
        _workUnit.MessageRepository.InsertEntity(msg);
        //create message boards
        var senderMsgBoard = new MessageBoard() {
          Message = msg,
          Type = "adminMessage",
          Profile = sender
        };
        _workUnit.MessageBoardRepository.InsertEntity(senderMsgBoard);
        recipients.ForEach(recipient => {
          var receiverMsgBoard = new MessageBoard() {
            Message = msg,
            Type = "adminMessage",
            Profile = recipient
          };
          _workUnit.MessageBoardRepository.InsertEntity(receiverMsgBoard);
          //create notifications
          recipient.Accounts.ToList().ForEach(x => {
            var notification = new Notification() {
              Category = "Announement Message Received",
              Message = sender.Organization.Name + " has sent a new announcement message.",
              Account = x,
              Message2 = "Message: " + message.Replace("<br />", "\r\n").toShorDescription(150),
              Message3 = "You can view and reply to this message at EARN MD CONNECT.",
              LinkToAction = string.Format(Url.Action("Index") + "#for/{0}/{1}",
                sender.Organization.Name,
                sender.ProfileID
              )
            };
            _workUnit.NotificationRepository.InsertEntity(notification);
          });
        });
        _workUnit.saveChanges();
      }
      catch (Exception ex) {
        return Json(new { status = "failed", message = ex.Message }, JsonRequestBehavior.AllowGet);
      }
      return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
    }
    [HttpPost]
    public ActionResult Delete() {
      return View();
    }

    public ActionResult Connections() {
      var connections = _workUnit
        .ProfileRepository
        .Entities
        .Where(x => x.ProfileID != CurrentAccount.ProfileID)
        .ToList();

      var json = connections.Select(x => new {
        id = x.ProfileID,
        name = x.Organization.Name,
        logoUrl = Url.Action("ProfileLogo", new { Id = x.ProfileID })
      }).OrderBy(x => x.name).ToList();

      return Json(json, JsonRequestBehavior.AllowGet);
    }


  }
}
