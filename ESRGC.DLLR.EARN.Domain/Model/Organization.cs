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
    [Display(Description="Mission statement")]
    [DataType(DataType.MultilineText)]
    [Required(ErrorMessage="Please describe your organization")]
    public string Description { get; set; }
    
    [DataType(DataType.Url)]
    public string FacebookLink { get; set; }

    [DataType(DataType.Url)]
    public string LinkedInLink { get; set; }

    [DataType(DataType.Url)]
    public string TwitterLink { get; set; }

    [Required(ErrorMessage = "Please enter your address")]
    [Display(Name = "Street address")]
    public string StreetAddress { get; set; }

    [Display(Name = "Address 2")]
    public string StreetAddress2 { get; set; }
    [Required(ErrorMessage = "Please enter a city name")]

    [Display(Name = "City")]
    public string City { get; set; }

    [Required(ErrorMessage = "Please enter your state e.i. MD")]
    [Display(Name = "State")]
    [MaxLength(2, ErrorMessage = "Please enter only abbreviation form of state e.i. MD, VA...ect.")]
    public string State { get; set; }

    [Required(ErrorMessage = "Please enter zip code")]
    [MaxLength(5)]
    [Display(Name = "Zip")]
    public string Zip { get; set; }

    public string Department { get; set; }

    [Display(Name = "Building")]
    public string Building { get; set; }
  }
}
