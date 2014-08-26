using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESRGC.DLLR.EARN.Domain.Model
{
  public class Request
  {
    public Request() {
      Created = DateTime.Now;
    }

    public int RequestID { get; set; }
    public string Type { get; set; }
    public string Message { get; set; }
    public string Status { get; set; }

    public int SenderID { get; set; }
    public virtual Profile Sender { get; set; }
    public int ReceiverID { get; set; }
    public virtual Profile Receiver { get; set; }

    
    public DateTime Created { get; set; }
  }
}
