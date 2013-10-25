using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESRGC.DLLR.EARN.Domain.Model
{
  public class Notification
  {
    public int NotificationID { get; set; }
    public int AccountID { get; set; }
    public virtual Account Account { get; set; }
    public string Message { get; set; }
    public bool IsRead { get; set; }
  }
}
