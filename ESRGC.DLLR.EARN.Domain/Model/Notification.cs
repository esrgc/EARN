using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESRGC.DLLR.EARN.Domain.Model
{
  public class Notification
  {
    public Notification() {
      IsRead = false;
      Created = DateTime.Now;
      Requests = new List<Request>();
      EmailSent = false;
    }

    public int NotificationID { get; set; }
    public int AccountID { get; set; }
    public virtual Account Account { get; set; }
    public string Message { get; set; }
    public string Category { get; set; }
    public bool IsRead { get; set; }

    public virtual ICollection<Request> Requests { get; set; }

    public DateTime Created { get; set; }
    public string LinkToAction { get; set; }
    public bool EmailSent { get; set; }
  }
}
