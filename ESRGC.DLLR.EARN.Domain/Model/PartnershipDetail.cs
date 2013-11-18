using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESRGC.DLLR.EARN.Domain.Model
{
  public class PartnershipDetail
  {
    public int PartnershipDetailID { get; set; }
    
    public int PartnershipID { get; set; }
    public virtual Partnership Partnership { get; set; }

    public int ProfileID { get; set; }
    public virtual Profile Profile { get; set; }

    public string Type { get; set; }
  }
}
