using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.DLLR.EARN.Domain.DAL.Abstract;
using ESRGC.DLLR.EARN.Domain.Model;
using ESRGC.DLLR.EARN.Models;

namespace ESRGC.DLLR.EARN.Controllers
{
  public class ProfileController : BaseController
  {
    public ProfileController(IWorkUnit workUnit)
      : base(workUnit) {
    }

    public ActionResult Index() {
      return View();
    }
    public ActionResult Create() {
      var industries = _workUnit.IndustryRepository.Entities.OrderBy(x=>x.Name).ToList();
      var userGroups = _workUnit.UserGroupRepository.Entities.OrderBy(x=>x.Name).ToList();
      return View(new CreateProfile() {  Industries = industries , UserGroups = userGroups });
    }
    [HttpPost]
    public ActionResult Create(CreateProfile profile) {
      if (ModelState.IsValid) {

      }
      return View();
    }
  }
}
