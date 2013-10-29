using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESRGC.DLLR.EARN.Domain.Model
{
  public class PartnershipInvite: Request
  {
    public PartnershipInvite() {
      Type = "Partnership Invite";
      Status = "New";
    }
  }
}
