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
    //nav properties
    public virtual ICollection<PartnershipDetail> PartnershipDetails { get; set; }
  }
}
