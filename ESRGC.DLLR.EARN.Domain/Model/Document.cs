using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public string Name { get; set; }
    public string MimeType { get; set; }
    public byte[] Data { get; set; }
    public DateTime Created { get; set; }

    public int? PartnershipID { get; set; }
    public virtual Partnership Partnership { get; set; }

    public int ProfileID { get; set; }
    public virtual Profile Profile { get; set; }
    [MaxLength(1000)]
    public string Description { get; set; }

    public int? FolderID { get; set; }
    public virtual Folder Folder { get; set; }
  }
}
