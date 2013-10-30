using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.DLLR.EARN.Domain.DAL.Abstract;
using ESRGC.DLLR.EARN.Filters;

namespace ESRGC.DLLR.EARN.Controllers
{
  [Authorize]
  public class CommunicationController : BaseController
  {
    public CommunicationController(IWorkUnit workUnit) : base(workUnit) { }

    [VerifyProfile]
    [NewToPartnership]
    public ActionResult SendPartnershipInvite(int partnershipID) {

      return View();
    }
    [VerifyProfile]
    [NewToPartnership]
    public ActionResult SendPartnershipRequest(int partnershipID) {
      
      return View();
    }
  }
}
