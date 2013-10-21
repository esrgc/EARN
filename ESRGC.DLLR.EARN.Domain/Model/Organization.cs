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
    [Required(ErrorMessage = "Please enter your organization name!")]
    [Display(Name = "Name")]
    public string Name { get; set; }

    string _website, _facebook, _linkedIn, _twitter;
    [MaxLength(50)]
    [Display(Description = "www.someorganization.org")]
    [DataType(DataType.Url)]
    [RegularExpression(@"^[a-zA-Z0-9\-\.]+\.(com|org|net|mil|edu|COM|ORG|NET|MIL|EDU)$", ErrorMessage = "Invalid Url")]
    public string Website {
      get { return _website; }
      set { _website = value == null ? "" : value.Replace("http://", "").Trim(); }
    }

    [MaxLength(1000)]
    [Display(Description = "Mission statement")]
    [DataType(DataType.MultilineText)]
    [Required(ErrorMessage = "Please describe your organization")]
    public string Description { get; set; }

    [DataType(DataType.Url)]
    public string FacebookLink {
      get { return _facebook; }
      set { _facebook = value == null ? "" : value.Replace("http://", "").Trim(); }
    }
    [DataType(DataType.Url)]
    public string LinkedInLink {
      get { return _linkedIn; }
      set { _linkedIn = value == null ? "" : value.Replace("http://", "").Trim(); }
    }
    [DataType(DataType.Url)]
    public string TwitterLink {
      get { return _twitter; }
      set { _twitter = value == null ? "" : value.Replace("http://", "").Trim(); }
    }
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
