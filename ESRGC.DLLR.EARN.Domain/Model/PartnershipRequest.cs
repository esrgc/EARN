using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESRGC.DLLR.EARN.Domain.Model
{
  public class PartnershipRequest: Request
  {
    public PartnershipRequest() {
      Type = "Partnership Request";
      Status = "new";

    }
  }
}
