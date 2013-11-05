using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace ESRGC.DLLR.EARN.Domain.Model
{
  public class Comment
  {
    public Comment() {
      Created = DateTime.Now;
      Updated = DateTime.Now;
    }

    public int CommentID { get; set;  }
    [MaxLength(1000)]
    public string Content { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }

    public int AuthorID { get; set; }
    public virtual Profile Author { get; set; }

    public int PartnershipID { get; set; }
    public virtual Partnership Partnership { get; set; }
    
  }
}
