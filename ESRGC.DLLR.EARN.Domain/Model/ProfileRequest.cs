using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESRGC.DLLR.EARN.Domain.Model
{
  public class ProfileRequest
  {
    public ProfileRequest() {
      Created = DateTime.Now;
    }

    public int ProfileRequestID { get; set; }
    public string Type { get; set; }
    public string Message { get; set; }
    public string Status { get; set; }

    public int SenderID { get; set; }
    public virtual Account Sender { get; set; }
    public int ReceiverID { get; set; }
    public virtual Account Receiver { get; set; }

    public int ProfileID { get; set; }
    public virtual Profile Profile { get; set; }
    
    public DateTime Created { get; set; }
  }
}
