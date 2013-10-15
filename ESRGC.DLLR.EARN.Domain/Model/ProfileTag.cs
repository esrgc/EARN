using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESRGC.DLLR.EARN.Domain.Model
{
  public class ProfileTag
  {
    public int ProfileTagID { get; set; }
    public int ProfileID { get; set; }
    public virtual Profile Profile { get; set; }
    public int TagID { get; set; }
    public virtual Tag Tag { get; set; }
  }
}
