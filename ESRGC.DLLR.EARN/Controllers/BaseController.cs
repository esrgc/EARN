using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.DLLR.EARN.Domain.DAL.Abstract;

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
  }
}
