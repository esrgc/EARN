using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.DLLR.EARN.Domain.DAL.Abstract;
using ESRGC.DLLR.EARN.Domain.Model;
using PagedList;

namespace ESRGC.DLLR.EARN.Controllers
{
  public class SearchController : BaseController
  {
    public SearchController(IWorkUnit workUnit) : base(workUnit) { }
    //
    // GET: /Search/
    public ActionResult Index(
      int? page,
      int? size,
      int? userGroupID,
      int? categoryID,
      List<string> tags) {
      //collection of current filters
      Dictionary<string, object> filters = new Dictionary<string, object>();

      //all profiles
      var profiles = _workUnit.ProfileRepository.Entities.AsQueryable();

      //filter by user group
      if (userGroupID != null) {
        profiles = profiles.Where(x => x.UserGroupID == userGroupID).AsQueryable();
        filters.Add("userGroupID", userGroupID);
      }

      //filter by category
      if (categoryID != null) {
        profiles = profiles.Where(x => x.CategoryID == categoryID).AsQueryable();
        filters.Add("categoryID", categoryID);
      }

      var result = new List<Profile>();
      var tagNames = new List<string>();
      if (tags != null) {
        //matching by tags
        tags.ForEach(x => {
          var tagName = x.ToUpper();
          tagNames.Add(tagName);
          //find the profiles that match the tag
          var resultSet = profiles.Where(
            p => p.ProfileTags
              .Select(t => t.Tag.Name)
              .Contains(tagName)
            ).ToList();
          //accummulate search results for each tag
          result = resultSet.Union(result).ToList();
        });
        filters.Add("tags", tagNames);
      }
      else
        result = profiles.ToList();


      int pageIndex = page ?? 1;
      int pageSize = size ?? 15;

      var model = result
        .OrderBy(x => x.Organization.Name) // ordered by organization name
        .ToPagedList(pageIndex, pageSize);

      //viewbag data
      ViewBag.filters = filters;

      return View(model);
    }
  }
}
