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
        //"Planning Grant in Progress",
        "Planning Grant Awarded",
        "Implementation Grant Proposed",
        "Implementation Grant Awarded",
        "Other"
      };
    }

  }
}