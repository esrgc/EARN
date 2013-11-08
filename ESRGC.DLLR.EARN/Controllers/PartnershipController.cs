﻿using System;
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
      int commentPageSize = size ?? 20;
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
    public ActionResult ListPartnerships(int profileID,string returnUrl) {
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
    public ActionResult Create() {
      return View();
    }
    [HttpPost]
    [VerifyProfile]
    public ActionResult Create(Partnership partnership) {
      if (ModelState.IsValid) {
        var partnershipDetail = new PartnershipDetail() {
          Profile = CurrentAccount.Profile,
          Type = "Owner",
          Partnership = partnership
        };
        _workUnit.PartnershipDetailRepository.InsertEntity(partnershipDetail);
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
    public ActionResult EditPartnership(int partnershipID, string returnUrl) {
      var partnership = _workUnit.PartnershipRepository.GetEntityByID(partnershipID) ?? new Partnership();
      TryUpdateModel(partnership);
      if (ModelState.IsValid) {
        partnership.LastUpdate = DateTime.Now;
        _workUnit.PartnershipRepository.UpdateEntity(partnership);
        _workUnit.saveChanges();
        return RedirectToAction("Detail", new { partnershipID , returnUrl});
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
    [ActionName("Delete")]
    public ActionResult DeletePartnership(int partnershipID, string returnUrl) {
      if (ModelState.IsValid) {
        var partnership = _workUnit.PartnershipRepository.GetEntityByID(partnershipID);
        foreach (var detail in partnership.PartnershipDetails.ToList()) {
          _workUnit.PartnershipDetailRepository.DeleteEntity(detail);
        }
        _workUnit.PartnershipRepository.DeleteEntity(partnership);
        _workUnit.saveChanges();
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
