﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESRGC.DLLR.EARN.Domain.Model
{
  /// <summary>
  /// Provide access to messages
  /// </summary>
  public class MessageBoard
  {
    public MessageBoard() {
      JoinedDate = DateTime.Now;
    }

    public int MessageBoardID { get; set; }

    public int? ConversationID { get; set; }
    public virtual Conversation Conversation { get; set; }

    public int ProfileID { get; set; }
    public virtual Profile Profile { get; set; }

    public DateTime JoinedDate { get; set; }
  }
}
