using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESRGC.DLLR.EARN.Domain.Model
{
  public class Conversation
  {
    public Conversation() {
      Started = DateTime.Now;
    }
    public int ConversationID { get; set; }

    public DateTime Started { get; set; }
    public DateTime LastMessageDate { get; set; }
    public string LastMessage { get; set; }


    public virtual ICollection<Message> Messages { get; set; }
    public virtual ICollection<MessageBoard> MessageBoards { get; set; }
  }
}
