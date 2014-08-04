using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESRGC.DLLR.EARN.Domain.Model
{
  public class ProfileMemberRequest : Request
  {
    public ProfileMemberRequest() {
      Type = "Profile Member Request";
      Status = "new";
    }

    public int ProfileID { get; set; }
    public virtual Profile Profile { get; set; }
  }
}
