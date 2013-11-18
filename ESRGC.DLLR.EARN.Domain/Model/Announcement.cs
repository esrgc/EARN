using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESRGC.DLLR.EARN.Domain.Model
{
  public class Announcement
  {
    public Announcement() {
      Created = DateTime.Now;
      ValidUntil = DateTime.Now.AddDays(30);
    }
    public int AnnouncementID { get; set; }
    public string Content { get; set; }
    public DateTime Created { get; set; }
    public DateTime ValidUntil { get; set; }
  }
}
