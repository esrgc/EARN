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
  public class DocumentController : BaseController
  {
    public DocumentController(IWorkUnit workUnit) : base(workUnit) { }

    [VerifyProfile]
    [VerifyProfilePartnership]
    public ActionResult List(int partnershipID) {
      var partnership = _workUnit.PartnershipRepository.GetEntityByID(partnershipID);
      return PartialView(partnership);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [VerifyProfile]
    [VerifyProfilePartnership]
    [SendNotification]
    public ActionResult Upload(int partnershipID, HttpPostedFileBase data, string description, string returnUrl) {
      var partnership = _workUnit.PartnershipRepository.GetEntityByID(partnershipID);
      if (data == null) {
        updateTempMessage("No data was uploaded");
        return RedirectToAction("Detail", "Partnership", new { partnershipID, returnUrl });
      }
      if (ModelState.IsValid) {
        var document = new Document {
          Name = data.FileName,
          Profile = CurrentAccount.Profile,
          Partnership = partnership,
          Data = new byte[data.ContentLength],
          MimeType = data.ContentType,
          Description = description
        };
        //read data
        data.InputStream.Read(document.Data, 0, data.ContentLength);
        //store it
        _workUnit.DocumentRepository.InsertEntity(document);

        //notify partners
        partnership.getAllPartners()
          .Where(x => x.ProfileID != CurrentAccount.ProfileID)
          .ToList()
          .ForEach(x => {
          var notification = new Notification {
            Account = x.getAccount(),
            Category = "Document Uploaded",
            LinkToAction = Url.Action("Detail", "Partnership", new { partnershipID }),
            Message = CurrentAccount.Profile.Organization.Name + " has uploaded a new document (" + document.Name + ")."
          };
          _workUnit.NotificationRepository.InsertEntity(notification);
        });

        _workUnit.saveChanges();
        updateTempMessage("Document uploaded successfully.");
      }
      else
        updateTempMessage("There was an error uploading document.");

      return RedirectToAction("Detail", "Partnership", new { partnershipID, returnUrl });
    }

    [VerifyProfile]
    [VerifyProfilePartnership]
    public FileContentResult Download(int documentID, int partnershipID) {
      var doc = _workUnit.DocumentRepository.GetEntityByID(documentID);
      if (doc == null) {
        return null;
      }
      return File(doc.Data, doc.MimeType, doc.Name);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [VerifyProfilePartnership]
    [SendNotification]
    public ActionResult Delete(int partnershipID, int documentID, string returnUrl) {
      var document = _workUnit.DocumentRepository.GetEntityByID(documentID);
      if (document.ProfileID != CurrentAccount.ProfileID) {
        updateTempMessage("You are not allowed to delete this document: "
          + document.Name
          + ". Only the owner/uploader can do so.");
      }
      else {
        _workUnit.DocumentRepository.DeleteEntity(document);
        
        var partnership = _workUnit.PartnershipRepository.GetEntityByID(partnershipID);
        //notify partners
        partnership.getAllPartners()
          .Where(x=>x.ProfileID != CurrentAccount.ProfileID)
          .ToList()
          .ForEach(x => {
          var notification = new Notification {
            Account = x.getAccount(),
            Category = "Document Deleted",
            LinkToAction = Url.Action("Detail", "Partnership", new { partnershipID }),
            Message = CurrentAccount.Profile.Organization.Name + " has deleted a document (" + document.Name + ")."
          };
          _workUnit.NotificationRepository.InsertEntity(notification);
        });
        _workUnit.saveChanges();
        updateTempMessage("Document deleted");
      }
      if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
             && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\")) {
        return Redirect(returnUrl);
      }
      else {
        return RedirectToAction("Detail", "Partnership", new { partnershipID, returnUrl });
      }
    }
  }
}
