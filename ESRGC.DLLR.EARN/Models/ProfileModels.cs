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
    public ICollection<Community> Communities { get; set; }
    public ICollection<UserGroup> UserGroups { get; set; }
    public int UserGroupID { get; set; }
    public int CommunityID { get; set; }
    public Contact Contact { get; set; }
  }
}