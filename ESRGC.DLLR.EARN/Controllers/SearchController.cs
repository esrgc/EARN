using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.DLLR.EARN.Domain.DAL.Abstract;
using ESRGC.DLLR.EARN.Domain.Model;

namespace ESRGC.DLLR.EARN.Controllers
{
  public class SearchController : BaseController
  {
    public SearchController(IWorkUnit workUnit) : base(workUnit) { }
    //
    // GET: /Search/
    public ActionResult Index(int? userGroupID, int? categoryID, List<string> tags) {
      var profiles = _workUnit.ProfileRepository.Entities.OrderBy(x => x.Organization.Name).AsQueryable();

      //filter by user group
      if (userGroupID != null) {
        profiles = profiles.Where(x => x.UserGroupID == userGroupID).AsQueryable();
      }

      //filter by category
      if (categoryID != null) {
        profiles = profiles.Where(x => x.CategoryID == categoryID).AsQueryable();
      }

      var result = new List<Profile>();
      if (tags != null) {
        //matching by tags
        tags.ForEach(x => {
          //find the profiles that match the tag
          var resultSet = profiles.Where(p => p.ProfileTags.Select(t => t.Tag.Name).Contains(x)).ToList();
          //accummulate search results for each tag
          result = resultSet.Union(result).ToList();
        });
        ViewBag.tags = tags;
      }
      else
        result = profiles.ToList();
      


      return View(result);
    }
  }
}
