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
    [DeleteEmptyConversation]
    public ActionResult Index() {
      return View(CurrentAccount);
    }

    //fetch messages for selected participant
    public JsonResult Fetch(int conversationID) {
      var convo = _workUnit.ConversationRepository
        .GetEntityByID(conversationID);

      var messages = convo.Messages
        .OrderBy(x => x.Created)
        .Select(x => new {
            id = x.MessageID,
            senderName = x.Sender.Organization.Name,
            message = x.Message1,
            date = x.Created.ToShortDateString(),
            time = x.Created.ToShortTimeString(),
            logoUrl = Url.Action("ProfileLogo", new {id = x.Sender.ProfileID }),
            profileUrl = Url.Action("ViewProfile", "Profile", new { profileID = x.Sender.ProfileID, returnUrl = Url.Action("index") }),
            isAdminMessage = x.Type == "adminMessage"
          })
        .ToList();
      var participants = convo.MessageBoards.Select(x => new { id = x.ProfileID, name = x.Profile.Organization.Name }).ToList();

      var convoDetails = getConvoDetail(convo);
      var json = new { 
        id = convo.ConversationID,
        started = convo.Started.ToString(),
        name = convoDetails.name,
        participants = participants,
        messages = messages
      };

      return Json(json, JsonRequestBehavior.AllowGet);
    }
    //return conversations and their info
    public JsonResult Conversations() {
      var profile = CurrentAccount.Profile;
      if (profile == null)
        return Json(new { status = "failed" });

      var currentName = profile.Organization.Name;//curent logged in name

        var convo = profile.getConversations()
           .OrderByDescending(x => x.LastMessageDate)
           .Select(x =>
           this.getConvoDetail(x)
         );

        return Json(convo, JsonRequestBehavior.AllowGet);
    }

    [HttpPost]
    [SendNotification]
    public ActionResult NewMessage(int[] profileIDs, string message, string type) {
      try {

        //create a new conversation or get existing convo
        var idList = profileIDs.ToList();
        idList.Add(CurrentAccount.ProfileID.Value);
        Conversation convo = null;
        try {
          var convos = _workUnit.ConversationRepository
                .Entities
                .ToList();

          convo = convos.Where(x => {
            var list = x.MessageBoards
              .Select(mb => mb.ProfileID)
              .ToList();
            //true if all ids in current convo are in the new idlist
            return (list.TrueForAll(i => idList.Contains(i)) && list.Count() == idList.Count());
          }).First();
        }
        catch {
          //not found create new convo
          convo = new Conversation() {
            LastMessageDate = DateTime.Now
          };
          _workUnit.ConversationRepository.InsertEntity(convo);
          //create message boards and generate convo's name
          idList.ForEach(x => {
            var profile = _workUnit.ProfileRepository.GetEntityByID(x);
            var mb = new MessageBoard() {
              Conversation = convo,
              Profile = profile
            };
            _workUnit.MessageBoardRepository.InsertEntity(mb);
          });
        }



        _workUnit.saveChanges();

        return Send(convo.ConversationID, message, type);
      }
      catch (Exception ex) {
        return Json(new { status = "failed", message = ex.Message }, JsonRequestBehavior.AllowGet);
      }

    }
    [HttpPost]
    [SendNotification]
    public ActionResult Send(int conversationID, string message, string type) {
      try {
        var sender = CurrentAccount.Profile;
        var conversation = _workUnit.ConversationRepository.GetEntityByID(conversationID);
        //create the message
        var m = new Message() {
          Sender = sender,
          Conversation = conversation,
          Message1 = message,
          Type = type
        };
        _workUnit.MessageRepository.InsertEntity(m);
        //update late message
        conversation.LastMessage = message;
        conversation.LastMessageDate = DateTime.Now;
        _workUnit.ConversationRepository.UpdateEntity(conversation);

        //create notiification for recipient
        conversation.MessageBoards.Select(x => x.Profile).ToList().ForEach(p => {
          if (p.ProfileID == sender.ProfileID)
            return;//send to anyone but the sender
          p.Accounts.ToList().ForEach(x => {
            var notification = new Notification() {
              Category = "Message Received",
              Message = "\"" + sender.Organization.Name + "\" has sent a new message.",
              Account = x,
              Message2 = "Message: " + message.Replace("<br />", "\r\n").toShorDescription(150),
              Message3 = "You can view and reply to this message at EARN MD CONNECT.",
              LinkToAction = string.Format(Url.Action("Index") + "#for/{0}",
                conversationID
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
      return Json(new { status = "success" , id = conversationID }, JsonRequestBehavior.AllowGet);
    }
    /// <summary>
    /// admin send a message to all profiles
    /// </summary>
    /// <returns></returns>
    //[HttpPost]
    [RoleAuthorize(Roles = "admin")]
    [SendNotification]
    public ActionResult AdminSend(string message) {
     var idList = _workUnit.ProfileRepository
       .Entities
       .Where(x=>x.ProfileID != CurrentAccount.ProfileID)
       .Select(x=>x.ProfileID)
       .ToArray();
       
      return NewMessage(idList, message, "adminMessage");
    }
    [HttpPost]
    public ActionResult Delete() {
      return View();
    }
    [HttpDelete]
    public ActionResult RemoveFromConvo(int id) {
      try {
        var convo = _workUnit.ConversationRepository.GetEntityByID(id);
        var profile = CurrentAccount.Profile;
        convo.MessageBoards
          .Where(x => x.ProfileID == profile.ProfileID)
          .ToList()
          .ForEach(mb => _workUnit.MessageBoardRepository.DeleteEntity(mb));
        _workUnit.saveChanges();

      }
      catch (Exception ex) {
        return Json(new { status = "failed", message = ex.Message });
      }
      return Json(new { status = "success" });
    }
    public ActionResult Participants(int conversationID) {
      var convo = _workUnit.ConversationRepository.GetEntityByID(conversationID);
      var json = convo.MessageBoards.Select(x => new { id = x.ProfileID, name = x.Profile.Organization.Name }).ToList();
      return Json(json, JsonRequestBehavior.AllowGet);
    }

    public ActionResult Organizations() {
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

    //Helpers
    private dynamic getConvoDetail(Conversation x) {
      var profile = CurrentAccount.Profile;
      var currentName = profile.Organization.Name;//curent logged in name
      var name = "";
      string logoUrl = "";
      
      var names = _workUnit.MessageBoardRepository
        .Entities
        .Where(mb => mb.ConversationID == x.ConversationID)
        .Where(mb => mb.Profile.Organization.Name != currentName)
        .Select(mb => mb.Profile.Organization.Name)
        .ToArray();

      switch (names.Length) {
        case 0:
          name = "";
          break;
        case 1:
          name = names[0];
          try {
            var org = _workUnit.ProfileRepository.Entities.First(p => p.Organization.Name == name);
            logoUrl = Url.Action("ProfileLogo", new { id = org.ProfileID });
          }
          catch {
            //not found               
          }
          break;
        case 2:
          name = string.Join(" and ", names);
          break;
        case 3:
          var str = names[names.Length - 1];
          str = " and " + str;
          names[names.Length - 1] = str;
          name = string.Join(", ", names);
          break;
        default:
          name = string.Join(", ", names.Take(3));
          name += ", and " + (names.Length - 3) + " others";
          break;
      }

      return new {
        id = x.ConversationID,
        lastMessage = x.LastMessage.toShorDescription(15),
        started = x.Started.ToString(),
        name = name,
        logoUrl = logoUrl,
        lastMessageDate = x.LastMessageDate.ToShortDateString()
      };
    }
  }
}
