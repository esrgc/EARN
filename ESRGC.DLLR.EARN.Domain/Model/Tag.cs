using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace ESRGC.DLLR.EARN.Domain.Model
{
  public class Tag
  {
    public Tag(){
      ProfileTags = new List<ProfileTag>();
      PartnershipTags = new List<PartnershipTag>();
    }
    public int TagID { get; set; }
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }

    public virtual ICollection<ProfileTag> ProfileTags { get; set; }
    public virtual ICollection<PartnershipTag> PartnershipTags { get; set; }
  }
}
