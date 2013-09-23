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
    [Required(ErrorMessage="Please enter your organization name!")]
    [Display(Name="Name")]
    public string Name { get; set; }
    [MaxLength(50)]
    [Display(Description="www.someorganization.org")]
    public string Website { get; set; }
    [MaxLength(300)]
    [Display(Description="Please describe your organization")]
    public string Description { get; set; }
    [Display(Name="Industry")]
    [Required(ErrorMessage="Please select an industry!")]
    public int IndustryID { get; set; }
    public virtual Industry Industry { get; set; }
  }
}
