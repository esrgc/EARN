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
      return View();
    }
    //fetch messages for selected participant
    public JsonResult Fetch(int participantID) {
      var participant = _workUnit.ProfileRepository.GetEntityByID(participantID);
      var profile = CurrentAccount.Profile;
      var messages = profile.MessageBoards
        .Where(x => (x.Message.SenderID == participantID)
          || (x.Message.ReceiverID == participantID))
        .OrderBy(x=>x.Message.Created)  
        .Select(x=>new {
          id = x.MessageID,
          senderName = x.Message.Sender.Organization.Name,
          message = x.Message.Message1,
          participantID = participantID,
          participantName = participant.Organization.Name,
          date = x.Message.Created.ToShortDateString(),
          time = x.Message.Created.ToShortTimeString()
        })
        .ToList();
      return Json(messages, JsonRequestBehavior.AllowGet);
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
          lastMessage = x.MessageBoards.OrderBy(m=>m.Message.Created).Select(msg=>msg.Message).First().Message1.toShorDescription(50),
          lastMessageDate = x.MessageBoards.OrderBy(m=>m.Message.Created).Select(msg=>msg.Message).First().Created.ToShortDateString()
        })
        .ToList();

      return Json(participants, JsonRequestBehavior.AllowGet);
    }
    //[HttpPost]
    public ActionResult Send(int participantID, string message) {
      var sender = CurrentAccount.Profile;
      var recipient = _workUnit.ProfileRepository.GetEntityByID(participantID);
      if (recipient == null)
        return Json(new { status = "failed", message = "Invalid recipient. ID: " + participantID }, JsonRequestBehavior.AllowGet);
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
        _workUnit.saveChanges();
      }
      catch (Exception ex) {
        return Json(new { status = "failed", message = ex.Message }, JsonRequestBehavior.AllowGet);
      }
      return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
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
        .Where(x=>x.ProfileID != sender.ProfileID)
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
  }
}
