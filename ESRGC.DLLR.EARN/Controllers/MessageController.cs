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
        .Where(x => (x.Message.SenderID == participantID)
         || (x.Message.ReceiverID == participantID));
      if (CurrentAccount.Role.ToLower() == "user") {
        messages = messages.Where(x => x.Type == "message");
      }
      else if (CurrentAccount.Role.ToLower() == "admin") {
        //takes it all
      }
      else
        return Json(new { message = "Failed to fetch messages" }, JsonRequestBehavior.AllowGet);

     var messageList = messages
        .OrderBy(x => x.Message.Created)
        .Select(x => new {
          id = x.MessageID,
          senderName = x.Message.Sender.Organization.Name,
          message = x.Message.Message1,
          participantID = participantID,
          participantName = participant.Organization.Name,
          date = x.Message.Created.ToShortDateString(),
          time = x.Message.Created.ToShortTimeString(),
          profileUrl = Url.Action("ViewProfile", "Profile", new { profileID = participantID })
        })
      .ToList();
      return Json(messageList, JsonRequestBehavior.AllowGet);

    }
    public JsonResult Participants() {
      var profile = CurrentAccount.Profile;
      //get participants as receivers
      var participants1 = _workUnit.MessageBoardRepository
        .Entities
        .Where(x => x.Message.SenderID == profile.ProfileID)
        .Select(x => x.Message.Receiver).ToList();
      //get participants as senders
      var participants2 = _workUnit.MessageBoardRepository
        .Entities
        .Where(x => x.Message.ReceiverID == profile.ProfileID)
        .Select(x => x.Message.Sender).ToList();
      var participants = participants1.Union(participants2)
        .Select(x => new {
          id = x.ProfileID,
          name = x.Organization.Name,
          logoUrl = Url.Action("ProfileLogo", new { Id = x.ProfileID }),
          lastMessage = x.MessageBoards.OrderByDescending(m => m.Message.Created).First().Message.Message1.toShorDescription(50),
          lastMessageDate = x.MessageBoards.OrderByDescending(m => m.Message.Created).First().Message.Created.ToShortDateString(),
          lastMessageTime = x.MessageBoards.OrderByDescending(m => m.Message.Created).First().Message.Created.ToShortTimeString(),
          dateTime = x.MessageBoards.OrderByDescending(m => m.Message.Created).First().Message.Created //for sorting
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
          Message1 = message
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
