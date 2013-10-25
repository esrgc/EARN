using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ESRGC.DLLR.EARN.Helpers
{
  public static class ViewDataHelper
  {
    public static List<string> getPartnershipStatusList(this HtmlHelper helper) {
      return new List<string>() { 
        "Initiating",
        "Partnering/drafting proposal",
        "Partnered/proposed"
      };
    }
    public static List<string> getGrantStatusList(this HtmlHelper helper) {
      return new List<string>() { 
        "Pre-draft",
        "Drafting",
        "Submitted",
        "Awarded"
      };
    }
  }
}