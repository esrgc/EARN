using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace ESRGC.DLLR.EARN.Domain.Model
{
  public class Category
  {
    [Display(Name="Category")]
    public int CategoryID { get; set; }
    public string Name { get; set; }

    public int? UserGroupID { get; set; }
    public virtual UserGroup UserGroup { get; set; }

    public virtual ICollection<Profile> Profiles { get; set; }
  }
}
