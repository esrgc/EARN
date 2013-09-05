using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ESRGC.DLLR.EARN.Domain.Model
{
    public class Company
    {
        public int CompanyID { get; set; }
        [Required]
        [Display(Name="Company name")]
        public string CompanyName { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
    }
}
