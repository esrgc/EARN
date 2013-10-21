using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace ESRGC.DLLR.EARN.Domain.Model
{
  public class Connection
  {
    public int ConnectionID { get; set; }
    public int ProfileID { get; set; }
    public virtual Profile Profile { get; set; }
    public int ProfileID2 { get; set; }
    public virtual Profile Profile2 { get; set; }
  }
}
