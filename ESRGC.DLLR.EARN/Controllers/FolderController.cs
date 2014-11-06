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
  [RoleAuthorize(Roles = "admin")]
  public class FolderController : BaseController
  {
    public FolderController(IWorkUnit workUnit) : base(workUnit) { }

    [VerifyProfile]
    public ActionResult Create(int? FolderID) {
      ViewBag.currentFolder = null;
      //if current folder exists
      if (FolderID.HasValue) {
        //current folder
        var currentFolder = _workUnit
          .FolderRepository
          .GetEntityByID(FolderID);
        ViewBag.currentFolder = currentFolder;       
      }
      ViewBag.currentAccount = CurrentAccount;
      return View(new Folder() { ParentFolderID = FolderID});
    }

    [HttpPost]
    [ActionName("Create")]
    [VerifyProfile]
    public ActionResult CreatePost() {
      var folder = new Folder();
      TryUpdateModel(folder);
      if (ModelState.IsValid) {
        _workUnit.FolderRepository.InsertEntity(folder);
        _workUnit.saveChanges();

        return RedirectToAction("index", "Document", new { folder.FolderID }); ;
      }
      return View(folder);
      
    }
    [HttpPost]
    [VerifyProfile]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int folderID) {
      var folder = _workUnit.FolderRepository.GetEntityByID(folderID);
      if (folder == null) {
        updateTempMessage("Invalid folder ID. Please try again!");
        return RedirectToAction("Index", "Document");
      }

      if (folder.hasChildFolders() || folder.hasDocuments()) {
        updateTempMessage("Folder is not empty. Please delete all of its content first and try again!");
        return RedirectToAction("Index", "Document", new { folderID });
      }

      var parentID = folder.ParentFolderID;

      try {
        _workUnit.FolderRepository.DeleteByID(folderID);
        _workUnit.saveChanges();
        updateTempMessage("Folder deleted!");
      }
      catch (Exception ex) {
        
        throw ex;
      }

      if (parentID.HasValue)
        return RedirectToAction("index", "Document", new { folderID =  parentID});
      else
        return RedirectToAction("index", "Document");
        
    }

  }
}
