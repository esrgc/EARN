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
      Created = DateTime.Now;
    }
    string _name, _status, _description, _grantStatus;
    public int PartnershipID { get; set; }
    [Required]
    [MaxLength(100, ErrorMessage = "100 maximum characters allowed")]
    public string Name {
      get { return _name; }
      set { _name = value; LastUpdate = DateTime.Now; }
    }

    [Required]
    [MaxLength(100, ErrorMessage = "100 maximum characters allowed")]
    public string Status {
      get { return _status; }
      set { _status = value; LastUpdate = DateTime.Now; }
    }

    [Required]
    [MaxLength(500, ErrorMessage = "500 maximum characters allowed")]
    public string Description {
      get { return _description; }
      set { _description = value; LastUpdate = DateTime.Now; }
    }

    [Required]
    [MaxLength(100, ErrorMessage = "100 maximum characters allowed")]

    public string GrantStatus {
      get { return _grantStatus; }
      set { _grantStatus = value; LastUpdate = DateTime.Now; }
    }


    public DateTime Created { get; set; }
    public DateTime? LastUpdate { get; set; }
    //nav properties
    public virtual ICollection<PartnershipDetail> PartnershipDetails { get; set; }

    //helpers
    public Profile getOwner() {
      try {
        return PartnershipDetails
           .Where(x => x.Type.ToLower() == "owner")
           .Select(x => x.Profile)
           .First();
      }
      catch (Exception) {
        return null;
      }
    }
    public List<Profile> getPartners() {
      return PartnershipDetails
        .Where(x => x.Type.ToLower() != "owner")
        .Select(x => x.Profile)
        .ToList();
    }
  }
}
