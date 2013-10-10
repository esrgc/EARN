using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ESRGC.DLLR.EARN.Domain.Model;

namespace ESRGC.DLLR.EARN.Models
{
  public class CreateProfile
  {

    public Organization Organization { get; set; }
    public ICollection<Category> Categories { get; set; }
    public ICollection<UserGroup> UserGroups { get; set; }
    [Required(ErrorMessage = "Please choose your user group")]
    public int UserGroupID { get; set; }
    public int? CategoryID { get; set; }
    public Contact Contact { get; set; }
  }
}