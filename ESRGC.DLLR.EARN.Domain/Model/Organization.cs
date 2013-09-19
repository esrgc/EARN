using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ESRGC.DLLR.EARN.Domain.Model
{
  public class Organization
  {
    public int OrganizationID { get; set; }
    [Required]
    public string Name { get; set; }
    [MaxLength(50)]
    public string Website { get; set; }
    [MaxLength(300)]
    public string Description { get; set; }

    public int IndustryID { get; set; }
    public virtual Industry Industry { get; set; }
  }
}
