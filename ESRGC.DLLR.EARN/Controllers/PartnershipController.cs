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
  public class PartnershipController : BaseController
  {
    public PartnershipController(IWorkUnit workUnit) : base(workUnit) { }
    //
    // GET: /Partnership/
    //for search
    [VerifyProfile]
    public ActionResult Index(int? page, int? size) {
      var partnerships = _workUnit
        .PartnershipRepository
        .Entities
        .OrderBy(x => x.Name)
        .ToList();
      int pageIndex = page ?? 1, pageSize = size ?? 15;
      var pagedList = partnerships.ToPagedList(pageIndex, pageSize);
      return View(pagedList);
    }
    /// <summary>
    /// View partnership detail
    /// </summary>
    /// <param name="partnershipID"></param>
    /// <returns></returns>
    [VerifyProfile]
    [VerifyProfilePartnership]
    public ActionResult Detail(int partnershipID, string returnUrl, int? page, int? size) {
      ViewBag.returnUrl = returnUrl;
      var partnership = _workUnit.PartnershipRepository.GetEntityByID(partnershipID);
      int commentPageIndex = page ?? 1;
      int commentPageSize = size ?? 10;
      var comments = partnership.Comments.ToPagedList(commentPageIndex, commentPageSize);
      ViewBag.comments = comments;
      return View(partnership);
    }
    [VerifyProfile]
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
    public ActionResult ListPartnerships(int profileID, string returnUrl) {
      var profile = _workUnit.ProfileRepository.GetEntityByID(profileID);
      var partnerships = profile.PartnershipDetails.Select(x => x.Partnership).ToList();
      ViewBag.currentProfile = CurrentAccount.Profile;
      ViewBag.returnUrl = returnUrl;
      return PartialView(partnerships);
    }
    /// <summary>
    /// create a new partnership
    /// </summary>
    /// <returns></returns>
    [VerifyProfile]
    public ActionResult Create() {
      return View();
    }
    [HttpPost]
    [VerifyProfile]
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
          LinkToAction = Url.Action("Detail", new { partnership.PartnershipID})
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
    [VerifyProfile]
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
    [VerifyProfile]
    [CanEditPartnership]
    [HasReturnUrl]
    public ActionResult Delete(int partnershipID, string returnUrl) {
      var partnership = _workUnit.PartnershipRepository.GetEntityByID(partnershipID);
      return View(partnership);
    }
    [HttpPost]
    [VerifyProfile]
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

    public ActionResult InvalidAccessToPartnership() {
      return View();
    }
    public ActionResult InvalidPartnershipRequest() {
      return View();
    }
  }
}
