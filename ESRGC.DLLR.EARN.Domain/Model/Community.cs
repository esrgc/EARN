using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESRGC.DLLR.EARN.Domain.Model
{
  public class Community
  {
    public int CommunityID { get; set; }
    public string Name { get; set; }

    public virtual ICollection<Organization> Organizations { get; set; }
  }
}
