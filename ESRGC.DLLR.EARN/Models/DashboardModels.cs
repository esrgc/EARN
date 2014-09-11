using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ESRGC.DLLR.EARN.Domain.Model;

namespace ESRGC.DLLR.EARN.Models
{
  public class DashboardStatistic
  {
    public int AccountTotal { get; set; }
    public int ProfileTotal { get; set; }
    public int ContactTotal { get; set; }
    public int OrganizationTotal { get; set; }
    public int TagTotal { get; set; }
    public List<Profile> Profiles { get; set; }
    public List<Partnership> Partnerships { get; set; }
    public List<Account> Accounts { get; set; }
    public List<Conversation> Conversations { get; set; }
    public List<Document> Documents { get; set; }
  }
}