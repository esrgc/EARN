using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESRGC.DLLR.EARN.Domain.Model
{
  public class History
  {
    public History() { }

    public DateTime TimeStamp { get; set; }
    public string Action { get; set; }
    public int AccountTotal { get; set; }
    public int ProfileTotal { get; set; }
    public int PartnershipTotal { get; set; }
  }
}
