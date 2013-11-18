using System.Web;
using System.Web.Mvc;
using ESRGC.DLLR.EARN.Filters;

namespace ESRGC.DLLR.EARN
{
  public class FilterConfig
  {
    public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
      filters.Add(new HandleErrorAttribute());      
    }
  }
}