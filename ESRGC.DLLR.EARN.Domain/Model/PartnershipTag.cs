using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESRGC.DLLR.EARN.Domain.Model
{
  public class PartnershipTag
  {
    public int PartnershipTagID { get; set; }
    public int PartnershipID { get; set; }
    public virtual Partnership Partnership { get; set; }
    public int TagID { get; set; }
    public virtual Tag Tag { get; set; }
  }
}
