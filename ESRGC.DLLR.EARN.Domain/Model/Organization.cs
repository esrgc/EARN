using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace ESRGC.DLLR.EARN.Domain.Model
{
  public class Organization
  {
    public int OrganizationID { get; set; }
    [Required(ErrorMessage = "Please enter your organization name!")]
    [Display(Name = "Name")]
    [MaxLength(100, ErrorMessage="Maximum 100 characters")]
    public string Name { get; set; }

    string _website, _facebook, _linkedIn, _twitter;
    [MaxLength(50)]
    [Display(Description = "www.someorganization.org")]
    [DataType(DataType.Url)]
    [Url(UrlOptions.OptionalProtocol, ErrorMessage = "Invalid Url")]
    public string Website {
      get { return _website; }
      set {_website = value; }
    }

    [MaxLength(1000, ErrorMessage = "Maximum 1000 characters")]
    [Display(Description = "Brief organizational description of up to 1000 characters to accompany your profile.")]
    [DataType(DataType.MultilineText)]
    [Required(ErrorMessage = "Please provide a brief statement of purpose")]
    public string Description { get; set; }

    [DataType(DataType.Url)]
    [Url(UrlOptions.OptionalProtocol, ErrorMessage = "Invalid Url")]
    public string FacebookLink {
      get { return _facebook; }
      set { _facebook = value; }
    }
    [DataType(DataType.Url)]
    [Url(UrlOptions.OptionalProtocol, ErrorMessage = "Invalid Url")]
    public string LinkedInLink {
      get { return _linkedIn; }
      set { _linkedIn = value; }
    }
    [DataType(DataType.Url)]
    [Url(UrlOptions.OptionalProtocol, ErrorMessage = "Invalid Url")]
    public string TwitterLink {
      get { return _twitter; }
      set { _twitter = value; }
    }
    [Required(ErrorMessage = "Please enter your address")]
    [Display(Name = "Street address")]
    [MaxLength(250, ErrorMessage = "Maximum 100 characters")]
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
