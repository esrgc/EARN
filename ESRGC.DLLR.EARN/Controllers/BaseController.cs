using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.DLLR.EARN.Domain.DAL.Abstract;
using ESRGC.DLLR.EARN.Domain.Model;

namespace ESRGC.DLLR.EARN.Controllers
{
  public class BaseController : Controller
  {
    protected IWorkUnit _workUnit = null;
    public BaseController() { }
    public BaseController(IWorkUnit workUnit) {
      _workUnit = workUnit;
    }

    [Authorize]
    public FileContentResult ProfilePicture(int pictureId) {
      return null;
    }
    
    public void updateTempDataMessage(string message) {
      TempData["message"] = message;
    }

    protected Account CurrentAccount {
      get {
        try {
          var account = _workUnit.AccountRepository.Entities.First(x => x.EmailAddress == User.Identity.Name);
          return account;
        }
        catch  {
          return null;
        }
      }
    }
  }
}
