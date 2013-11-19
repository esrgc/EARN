﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESRGC.DLLR.EARN.Domain.Model
{
  public class Message
  {
    public Message() {
      IsRead = false;
      Created = DateTime.Now;
      EmailSent = false;
    }
    public int MessageID { get; set; }
    public int SenderID { get; set; }
    public Profile Sender { get; set; }

    public int ReceiverID { get; set; }
    public Profile Receiver { get; set; }
    public string Title { get; set; }
    public string Header { get; set; }
    public string Message1 { get; set; }
    public string Message2 { get; set; }
    public string Message3 { get; set; }
    public string FootNote { get; set; }
    public bool IsRead { get; set; }

    public DateTime Created { get; set; }
    public string LinkToAction { get; set; }
    public bool EmailSent { get; set; }
  }
}
