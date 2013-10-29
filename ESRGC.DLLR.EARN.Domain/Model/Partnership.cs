using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace ESRGC.DLLR.EARN.Domain.Model
{
  public class Partnership
  {
    public Partnership() {
      PartnershipDetails = new List<PartnershipDetail>();
      LastUpdate = DateTime.Now;
    }
    public int PartnershipID { get; set; }
    [Required]
    [MaxLength(100, ErrorMessage="100 maximum characters allowed")]
    public string Name { get; set; }
    [Required]
    [MaxLength(100, ErrorMessage = "100 maximum characters allowed")]
    public string Status { get; set; }
    [Required]
    [MaxLength(500, ErrorMessage = "500 maximum characters allowed")]
    public string Description { get; set; }
    [Required]
    [MaxLength(100, ErrorMessage = "100 maximum characters allowed")]
    public string GrantStatus { get; set; }

    public DateTime? LastUpdate { get; set; }
    //nav properties
    public virtual ICollection<PartnershipDetail> PartnershipDetails { get; set; }

    //helpers
    public List<Profile> getOwners() {
      return PartnershipDetails
        .Where(x => x.Type.ToLower() == "owner")
        .Select(x => x.Profile)
        .ToList();
    }
    public List<Profile> getPartners() {
      return PartnershipDetails
        .Where(x => x.Type.ToLower() != "owner")
        .Select(x => x.Profile)
        .ToList();
    }
  }
}
