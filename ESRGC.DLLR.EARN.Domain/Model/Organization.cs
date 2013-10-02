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
    [DataType(DataType.Url)]
    public string Website { get; set; }

    [MaxLength(1000)]
    [Display(Description="Please describe your organization")]
    [DataType(DataType.MultilineText)]
    [Required(ErrorMessage="Please describe your organization")]
    public string Description { get; set; }
    
    [DataType(DataType.Url)]
    public string FacebookLink { get; set; }

    [DataType(DataType.Url)]
    public string LinkedInLink { get; set; }

    [DataType(DataType.Url)]
    public string TwitterLink { get; set; }
  }
}
