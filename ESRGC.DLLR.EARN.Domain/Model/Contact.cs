using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ESRGC.DLLR.EARN.Domain.ValidationAttributes;

namespace ESRGC.DLLR.EARN.Domain.Model
{
  public class Contact
  {
    public Contact() {
      LastUpdate = DateTime.Now;
    }
    [ScaffoldColumn(false)]
    public int ContactID { get; set; }
    
    [Display(Name = "Title")]
    public string NameTitle { get; set; }
    
    [Required(ErrorMessage = "Please enter first name")]
    [Display(Name = "First name *")]
    public string FirstName { get; set; }
    
    [Display(Name = "Middle init.")]
    public string Middle { get; set; }
    
    [Required(ErrorMessage = "Please enter last name")]
    [Display(Name = "Last name *")]
    public string LastName { get; set; }
    
    [Display(Name = "Extension")]
    public string NameExtension { get; set; }
    
    [Display(Name = "Job title")]
    public string JobTitle { get; set; }
    
    //[ScaffoldColumn(false)]
    //[Display(Name = "Company")]
    //public int? CompanyID { get; set; }
    //public virtual Company Company { get; set; }
    
    [Display(Name = "Department")]
    public string Department { get; set; }
    
    [Display(Name = "P.O. Box/Mail stop")]
    public string MailStop { get; set; }
    
    [Required]
    [Display(Name = "Street address")]
    public string StreetAddress { get; set; }
    
    [Display(Name = "Address 2")]
    public string StreetAddress2 { get; set; }
    [Required]
    [Display(Name = "City")]
    public string City { get; set; }
    [Required]
    [Display(Name = "State")]
    public string State { get; set; }
    [Required]
    [Display(Name = "Zip")]
    public string Zip { get; set; }
    
    [MaxLength(20)]
    [Required(ErrorMessage = "Phone number is required")]
    [DataType(DataType.PhoneNumber)]
    [RegularExpression(@"^[01]?[- .]?(\([2-9]\d{2}\)|[2-9]\d{2})[- .]?\d{3}[- .]?\d{4}$", ErrorMessage = "Invalid phone number")]
    [Display(Name = "Phone number *")]
    public string Phone { get; set; }
    
    [Display(Name = "Ext.")]
    [MaxLength(4)]
    public string PhoneExt { get; set; }
    
    [DataType(DataType.PhoneNumber)]
    [RegularExpression(@"^[01]?[- .]?(\([2-9]\d{2}\)|[2-9]\d{2})[- .]?\d{3}[- .]?\d{4}$", ErrorMessage = "Invalid fax number")]
    [Display(Name = "Fax")]
    public string Fax { get; set; }
    
    [DataType(DataType.PhoneNumber)]
    [RegularExpression(@"^[01]?[- .]?(\([2-9]\d{2}\)|[2-9]\d{2})[- .]?\d{3}[- .]?\d{4}$", ErrorMessage = "Invalid phone number")]
    [Display(Name = "Phone number #2")]
    public string Phone2 { get; set; }
    
    [Display(Name = "Ext. #2")]
    [MaxLength(4)]
    public string PhoneExt2 { get; set; }
    
    [MaxLength(50)]
    [DataType(DataType.EmailAddress)]
    [EmailValidation(ErrorMessage = "Invalid email address")]
    [Display(Name = "Email address")]
    [Required]
    public string Email { get; set; }
    
    [Display(Name = "Email list")]
    public string EmailList { get; set; }

    [ScaffoldColumn(false)]
    [Display(Name = "Last update")]
    public DateTime LastUpdate { get; set; }
    
    public string Floor { get; set; }

    [Display(Name = "Building")]
    public string Building { get; set; }
    
  }
}
