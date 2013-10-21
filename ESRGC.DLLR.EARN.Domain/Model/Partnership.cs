using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESRGC.DLLR.EARN.Domain.Model
{
  public class Partnership
  {
    public int PartnershipID { get; set; }

    public string Name { get; set; }
    public string Status { get; set; }
    public string Description { get; set; }
    public string GrantStatus { get; set; }
    public virtual ICollection<PartnershipDetail> PartnershipDetails { get; set; }
  }
}
