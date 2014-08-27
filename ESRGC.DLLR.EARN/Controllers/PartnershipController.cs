using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.DLLR.EARN.Domain.DAL.Abstract;
using ESRGC.DLLR.EARN.Domain.Model;
using ESRGC.DLLR.EARN.Filters;
using PagedList;
namespace ESRGC.DLLR.EARN.Controllers
{
  [Authorize]
  [VerifyProfile]
  public class PartnershipController : BaseController
  {
    public PartnershipController(IWorkUnit workUnit) : base(workUnit) { }
    //
    // GET: /Partnership/
    //for search

    public ActionResult Index(string name, int? page, int? size) {
      var partnerships = _workUnit
        .PartnershipRepository
        .Entities
        .OrderBy(x => x.Name)
        .ToList();

      if (!string.IsNullOrEmpty(name)) {
        partnerships = partnerships.Where(x => x.Name.ToLower().Contains(name.ToLower())).ToList();
      }
      ViewBag.name = name;
      int pageIndex = page ?? 1, pageSize = size ?? 10;
      var pagedList = partnerships.ToPagedList(pageIndex, pageSize);
      return View(pagedList);
    }
    /// <summary>
    /// View partnership detail
    /// </summary>
    /// <param name="partnershipID"></param>
    /// <returns></returns>

    [VerifyProfilePartnership]
    public ActionResult Detail(int partnershipID, string returnUrl, int? size) {
      ViewBag.returnUrl = returnUrl;
      var partnership = _workUnit.PartnershipRepository.GetEntityByID(partnershipID);
      int commentPageSize = size ?? 10;
      var comments = partnership.Comments
        .OrderByDescending(x => x.Created)
        .Take(commentPageSize)
        .Reverse()
        .ToList();

      string loadMoreUrl = "";
      int remainSize = partnership.Comments.Count() - commentPageSize;
      if (remainSize >= 10)
        loadMoreUrl = Url.Action("Detail", new { partnershipID, returnUrl, size = commentPageSize + 10 });
      else
        loadMoreUrl = Url.Action("Detail", new { partnershipID, returnUrl, size = commentPageSize + remainSize });

      ViewBag.comments = comments;
      ViewBag.commentCount = partnership.Comments.Count();
      ViewBag.loadMoreUrl = loadMoreUrl;
      return View(partnership);
    }

    public ActionResult View(int partnershipID, string returnUrl) {
      ViewBag.returnUrl = returnUrl;
      var partnership = _workUnit.PartnershipRepository.GetEntityByID(partnershipID);
      return View(partnership);
    }

    /// <summary>
    /// list partnerships for provided profile
    /// </summary>
    /// <param name="profileID"></param>
    /// <returns></returns>

    [HasReturnUrl]
    public ActionResult ListPartnerships(int profileID, string returnUrl) {
      var profile = _workUnit.ProfileRepository.GetEntityByID(profileID);
      var partnerships = profile.PartnershipDetails.Select(x => x.Partnership).ToList();
      return PartialView(partnerships);
    }
    /// <summary>
    /// create a new partnership
    /// </summary>
    /// <returns></returns>

    public ActionResult Create() {
      return View();
    }
    [HttpPost]

    [SendNotification]
    public ActionResult Create(Partnership partnership) {
      if (ModelState.IsValid) {
        var partnershipDetail = new PartnershipDetail() {
          Profile = CurrentAccount.Profile,
          Type = "Owner",
          Partnership = partnership
        };
        _workUnit.PartnershipDetailRepository.InsertEntity(partnershipDetail);
        _workUnit.saveChanges();
        //notifications
        var notification = new Notification {
          Account = CurrentAccount,
          Category = "New Partnership Profile Created",
          Header = "Thank you for creating a Partnership Profile on EARN MD CONNECT!",
          Message = @"You are now the Administrator of your Partnership Profile. 
As the Administrator, you are the only user able 1) to control the Partnership Profile’s membership, 2) 
to invite other organizations to join your Partnership Profile, and 3) 
to decide whether to approve users who request to join your Partnership Profile.

If you are the appropriate Administrator for your partnership’s EARN MD CONNECT Partnership Profile, you’re ready to get started!
",
          Message2 = @"Next, please visit your EARN MD CONNECT Partnership Profile and invite 
your partners to join the Partnership Profile on EARN MD CONNECT, communicate with your partners, 
and search for any other Partnership Profiles that may be of interest to you.",
          Message3 = @"Please note: 1) to become a member of an EARN MD CONNECT Partnership Profile, 
a user must first have, or create an Organizational Profile; 2) the Partnership Profile’s Title, 
Description, Target Industry, region, and membership will be visible to other EARN MD CONNECT users; 3) 
full Partnership Profiles, including the communication function, are only accessible to that Partnership 
Profile’s members, and 4) the communication feature is meant to support communication between partners, 
but should not be used to share proprietary or sensitive content.",
          LinkToAction = Url.Action("Detail", new { partnership.PartnershipID })
        };
        _workUnit.NotificationRepository.InsertEntity(notification);
        _workUnit.saveChanges();
        return RedirectToAction("Detail", new { partnership.PartnershipID });
      }
      //error
      return View(partnership);
    }
    /// <summary>
    /// Manage partnership
    /// </summary>
    /// <param name="partnershipID"></param>
    /// <returns></returns>

    [CanEditPartnership]
    [HasReturnUrl]
    public ActionResult Edit(int partnershipID, string returnUrl) {
      var partnership = _workUnit.PartnershipRepository.GetEntityByID(partnershipID);
      return View(partnership);
    }
    [HttpPost]
    [ActionName("Edit")]
    [CanEditPartnership]
    [SendNotification]
    public ActionResult EditPartnership(int partnershipID, string returnUrl) {
      var partnership = _workUnit.PartnershipRepository.GetEntityByID(partnershipID) ?? new Partnership();
      TryUpdateModel(partnership);
      if (ModelState.IsValid) {
        partnership.LastUpdate = DateTime.Now;
        _workUnit.PartnershipRepository.UpdateEntity(partnership);
        //notifications
        partnership.getAllPartners()
          .Where(x => x.ProfileID != CurrentAccount.ProfileID)
          .ToList()
          .ForEach(x => {
            var notification = new Notification {
              Category = "Partnership Edited",
              Account = x.getAccount(),
              LinkToAction = Url.Action("Detail", new { partnershipID }),
              Message = partnership.Name + " has been mofified by " + CurrentAccount.Profile.Organization.Name + ".",
              Message2 = "Status: " + partnership.Status + "."
            };
            _workUnit.NotificationRepository.InsertEntity(notification);
          });
        _workUnit.saveChanges();
        return RedirectToAction("Detail", new { partnershipID, returnUrl });
      }
      //error redisplay
      return View(partnership);
    }
    /// <summary>
    /// Delete a partnership
    /// </summary>
    /// <param name="partnershipID"></param>
    /// <returns></returns>

    [CanEditPartnership]
    [HasReturnUrl]
    public ActionResult Delete(int partnershipID, string returnUrl) {
      var partnership = _workUnit.PartnershipRepository.GetEntityByID(partnershipID);
      return View(partnership);
    }
    [HttpPost]

    [CanEditPartnership]
    [SendNotification]
    [ActionName("Delete")]
    public ActionResult DeletePartnership(int partnershipID, string returnUrl) {
      if (ModelState.IsValid) {
        var partnership = _workUnit.PartnershipRepository.GetEntityByID(partnershipID);
        foreach (var detail in partnership.PartnershipDetails.ToList()) {
          _workUnit.PartnershipDetailRepository.DeleteEntity(detail);
        }
        //comments
        partnership.Comments
          .ToList()
          .ForEach(x => { _workUnit.CommentRepository.DeleteEntity(x); });
        //tags
        partnership.PartnershipTags
          .ToList()
          .ForEach(x => { _workUnit.PartnershipTagRepository.DeleteEntity(x); });
        //delete documents
        partnership.Documents
          .ToList()
          .ForEach(x => { _workUnit.DocumentRepository.DeleteEntity(x); });
        _workUnit.PartnershipRepository.DeleteEntity(partnership);
        //notifications
        partnership.getAllPartners()
          .Where(x => x.ProfileID != CurrentAccount.ProfileID)
          .ToList()
          .ForEach(x => {
            var notification = new Notification {
              Category = "Partnership Deleted",
              Account = x.getAccount(),
              Message = partnership.Name + " has been deleted by " + CurrentAccount.Profile.Organization.Name + ".",
            };
            _workUnit.NotificationRepository.InsertEntity(notification);
          });
        _workUnit.saveChanges();
        updateTempMessage("Partnership deleted.");
        return RedirectToAction("Detail", "Profile");
      }
      updateTempMessage("Error deleting partnership");
      //return to previous url
      if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
          && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\")) {
        return Redirect(returnUrl);
      }
      else {
        return RedirectToAction("Index");
      }

    }

    [VerifyProfilePartnership]
    public ActionResult LeavePartnership(int partnershipID) {
      var partnership = _workUnit.PartnershipRepository.GetEntityByID(partnershipID);

      return View(partnership);
    }

    [HttpPost]

    [VerifyProfilePartnership]
    [ValidateAntiForgeryToken]
    [SendNotification]
    [ActionName("LeavePartnership")]
    public ActionResult LeavePartnershipPost(int partnershipID, string returnUrl) {
      var partnership = _workUnit.PartnershipRepository.GetEntityByID(partnershipID);
      var currentProfile = CurrentAccount.Profile;
      PartnershipDetail partner = null;
      try {
        partner = _workUnit.PartnershipDetailRepository.Entities
           .First(x => x.ProfileID == currentProfile.ProfileID && x.PartnershipID == partnershipID);
        _workUnit.PartnershipDetailRepository.DeleteByID(partner.PartnershipDetailID);
      }
      catch (Exception) {
        //not found
      }

      if (partner != null) {
        //notify other partners
        partnership.getAllPartners()
          .Where(x => x.ProfileID != currentProfile.ProfileID)
          .ToList()
          .ForEach(x => {
            x.Accounts.ToList().ForEach(a => {
              //notifications
              var notification = new Notification {
                Account = a,
                Category = "Partnership Update",
                Message = currentProfile.Organization.Name + " has left the \"" + partnership.Name + "\" partnership.",
                LinkToAction = Url.Action("Detail", new { partnershipID })
              };
              _workUnit.NotificationRepository.InsertEntity(notification);
            });
          });

        _workUnit.saveChanges();
        updateTempMessage("You have left the \"" + partnership.Name + "\" partnership");
        return RedirectToAction("MyPartnerships");
      }
      else
        updateTempMessage("Error leaving partnership.");

      return returnToUrl(returnUrl, Url.Action("Detail", new { partnershipID }));
    }


    [CanEditPartnership]
    [HasReturnUrl]
    public ActionResult RemovePartner(int partnershipID, int profileID, string returnUrl) {
      var partnership = _workUnit.PartnershipRepository.GetEntityByID(partnershipID);
      var profile = _workUnit.ProfileRepository.GetEntityByID(profileID);
      if (profile == null) {
        updateTempMessage("Invalid partnership ID");
        return RedirectToAction("Detail", new { partnershipID });
      }
      ViewBag.profile = profile;
      return View(partnership);
    }

    [HttpPost]
    [CanEditPartnership]
    [ValidateAntiForgeryToken]
    [SendNotification]
    [ActionName("RemovePartner")]
    public ActionResult RemovePartnerPost(int partnershipID, int profileID, string returnUrl) {
      var partnership = _workUnit.PartnershipRepository.GetEntityByID(partnershipID);
      var profile = _workUnit.ProfileRepository.GetEntityByID(profileID);
      var currentProfile = CurrentAccount.Profile;
      PartnershipDetail partner = null;
      try {
        partner = _workUnit.PartnershipDetailRepository.Entities
           .First(x => x.ProfileID == profileID && x.PartnershipID == partnershipID);
        _workUnit.PartnershipDetailRepository.DeleteByID(partner.PartnershipDetailID);
      }
      catch (Exception) {
        //not found
      }

      if (partner != null) {
        //notify other partners
        partnership.getAllPartners()
          .Where(x => x.ProfileID != profileID && x.ProfileID != currentProfile.ProfileID)
          .ToList()
          .ForEach(x => {
            //notifications
            var notification = new Notification {
              Account = x.getAccount(),
              Category = "Partnership Update",
              Message = profile.Organization.Name + " is no longer a partner of the \"" + partnership.Name + "\" partnership",
              LinkToAction = Url.Action("Detail", new { partnershipID })
            };
            _workUnit.NotificationRepository.InsertEntity(notification);
          });
        //notify the removed partner
        var n = new Notification {
          Account = profile.getAccount(),
          Category = "Partnership Update",
          Message = "You are no longer a partner of the \"" + partnership.Name + "\" partnership",
          Message2 = "The administrator has removed you emailAddress the partnership.",
          Message3 = "There is no further action neccessary. You can request to join other partnerships."
        };
        _workUnit.NotificationRepository.InsertEntity(n);
        _workUnit.saveChanges();
        updateTempMessage(profile.Organization.Name + " has been removed emailAddress \"" + partnership.Name + "\" partnership");
      }
      else
        updateTempMessage("Error removing partner emailAddress partnership.");

      return returnToUrl(returnUrl, Url.Action("Detail", new { partnershipID }));
    }

    //[NewToPartnership]
    //[HasReturnUrl]
    //public ActionResult ContactAdmin(int partnershipID, string returnUrl) {
    //  var partnership = _workUnit.PartnershipRepository.GetEntityByID(partnershipID);
    //  return View(partnership);
    //}
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //[NewToPartnership]
    //[SendMessaage]
    //public ActionResult ContactAdmin(int partnershipID, string message, string returnUrl) {
    //  var partnership = _workUnit.PartnershipRepository.GetEntityByID(partnershipID);
    //  var currentProfile = CurrentAccount.Profile;
    //  //create a message
    //  var m = new Message {
    //    Sender = currentProfile,
    //    Receiver = partnership.getOwners(),
    //    Title = "Partnership Message",
    //    Header = currentProfile.Organization.Name + " has sent you a message",
    //    Message1 = "This message is regarding the \"" + partnership.Name + "\" partnership.",
    //    Message2 = message
    //  };
    //  _workUnit.MessageRepository.InsertEntity(m);
    //  _workUnit.saveChanges();
    //  updateTempMessage("Your message has been sent to the Administrator. A copy of this message was also emailed to you via"
    //    + " your contact email (" + currentProfile.Contact.Email + ").");
    //  return RedirectToAction("View", new { partnershipID, returnUrl });
    //}

    public ActionResult MyPartnerships() {
      return View();
    }
    public ActionResult InvalidAccessToPartnership() {
      return View();
    }
    public ActionResult InvalidPartnershipRequest() {
      return View();
    }
    [CanEditPartnership]
    public ActionResult MakeAdmin(int profileId, int partnershipID) {
      var profile = _workUnit.ProfileRepository.GetEntityByID(profileId);
      if (profile == null) {
        updateTempMessage("Invalid organization partnership");
        return RedirectToAction("Detail", new { partnershipID });
      }
      var partnership = _workUnit.PartnershipRepository.GetEntityByID(partnershipID);
      if (partnership == null) {
        updateTempMessage("Invalid Partnership partnership");
        return RedirectToAction("Detail", new { partnershipID });
      }
      ViewBag.profile = profile;
      ViewBag.partnership = partnership;
      return View();
    }

    [HttpPost]
    [CanEditPartnership]
    [SendNotification]
    [ActionName("MakeAdmin")]
    public ActionResult MakeAdminPost(int profileId, int partnershipID) {
      var currentProfile = CurrentAccount.Profile;
      var partnershipDetail = currentProfile.PartnershipDetails.First(x => x.PartnershipID == partnershipID);
      //make current profile 
      //partnershipDetail.Type = "partner";
      //_workUnit.PartnershipDetailRepository.UpdateEntity(partnershipDetail);
      //now look for the new admin 
      try {
        var newAdmin = _workUnit.PartnershipDetailRepository
            .Entities
            .First(x => x.ProfileID == profileId && x.PartnershipID == partnershipID);
        newAdmin.Type = "Owner";
        _workUnit.PartnershipDetailRepository.UpdateEntity(newAdmin);

        //get the new admin's profile
        var profile = _workUnit.ProfileRepository.GetEntityByID(profileId);
        profile.Accounts.ToList().ForEach(x => {
          //create notification
          var notif = new Notification() {
            Account = x,
            Category = "Partnership Admin Assigned",
            Message = string.Format("{0} has assigned your organization to be the administrator of the {1} partnership.",
              currentProfile.Organization.Name,
              partnershipDetail.Partnership.Name
            ),
            Message2 = "You can now edit the partnership status and details, delete the partnerhsip partnership, approve new partners, and invite organizations to join the partnership.",
            LinkToAction = Url.Action("Detail", new { partnershipID })
          };
          _workUnit.NotificationRepository.InsertEntity(notif);
        });
        //done now save to the database
        _workUnit.saveChanges();
        return RedirectToAction("Detail", new { partnershipID });
      }
      catch {

        updateTempMessage("Error retreiving partnership information for partnership id " + profileId + ".");
        return RedirectToAction("Detail", new { partnershipID });
      }

    }

    [CanEditPartnership]
    public ActionResult RemoveAdmin(int profileId, int partnershipID) {
      var profile = _workUnit.ProfileRepository.GetEntityByID(profileId);
      if (profile == null) {
        updateTempMessage("Invalid organization partnership");
        return RedirectToAction("Detail", new { partnershipID });
      }
      var partnership = _workUnit.PartnershipRepository.GetEntityByID(partnershipID);
      if (partnership == null) {
        updateTempMessage("Invalid Partnership partnership");
        return RedirectToAction("Detail", new { partnershipID });
      }

      if (partnership.getOwners().Count == 1) {
        updateTempMessage("You are the only admin in this partnership. Please assign another organization to be an admin before removing your admin privilege");
        return RedirectToAction("Detail", new { partnershipID });
      }

      ViewBag.profile = profile;
      ViewBag.partnership = partnership;
      return View();
    }

    [HttpPost]
    [CanEditPartnership]
    [ActionName("RemoveAdmin")]
    public ActionResult RemoveAdminPost(int profileId, int partnershipID) {
      var currentProfile = CurrentAccount.Profile;
      var partnershipDetail = currentProfile.PartnershipDetails.First(x => x.PartnershipID == partnershipID);
     
      try {
       
        //make current profile 
        partnershipDetail.Type = "partner";
        _workUnit.PartnershipDetailRepository.UpdateEntity(partnershipDetail);
       
        //done now save to the database
        _workUnit.saveChanges();

        return RedirectToAction("Detail", new { partnershipID });
      }
      catch {

        updateTempMessage("Error retreiving partnership information for partnership id " + profileId + ".");
        return RedirectToAction("Detail", new { partnershipID });
      }

    }

    [CanEditPartnership]
    public ActionResult UploadImage(int partnershipID) {
      var partnership = _workUnit.PartnershipRepository.GetEntityByID(partnershipID);
      if (partnership == null)
        return RedirectToAction("Detail");
      return View(partnership);
    }
    [HttpPost]
    [CanEditPartnership]
    public ActionResult UploadImage(int partnershipID, HttpPostedFileBase dataInput) {
      var partnership = _workUnit.PartnershipRepository.GetEntityByID(partnershipID);
      if (dataInput == null)
        ModelState.AddModelError("", "No data input. Please select a file to upload");
      else {
        var picture = new Picture() {
          ImageMimeType = dataInput.ContentType,
          ImageData = new byte[dataInput.ContentLength]
        };
        //read the input stream and store to picture object 
        dataInput.InputStream.Read(picture.ImageData, 0, dataInput.ContentLength);

        //store picture to database
        _workUnit.PictureRepository.InsertEntity(picture);
        if (partnership.PictureID != null)//delete current picture
          _workUnit.PictureRepository.DeleteByID(partnership.PictureID);
        partnership.PictureID = picture.PictureID;
        _workUnit.PartnershipRepository.UpdateEntity(partnership);
        _workUnit.saveChanges();
        //changes done return to detail page
        return RedirectToAction("Detail", new { partnershipID });
      }
      //error has occurred   
      return View(partnership);
    }
  }
}
