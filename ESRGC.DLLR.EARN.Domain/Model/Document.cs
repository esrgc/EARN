using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESRGC.DLLR.EARN.Domain.Model
{
  public class Document
  {
    public Document() {
      Created = DateTime.Now;
    }
    public int DocumentID { get; set; }
    public string MimeType { get; set; }
    public byte[] Data { get; set; }
    public DateTime Created { get; set; }

    public int PartnershipID { get; set; }
    public virtual Partnership Partnership { get; set; }
  }
}
