using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace ESRGC.DLLR.EARN.Domain.Model
{
  public class Folder
  {
    public Folder() {
      Created = DateTime.Now;
    }
    public int FolderID { get; set; }
    [Required]
    public string Name { get; set; }
    public DateTime Created { get; set; }

    public string Description { get; set; }

    public virtual ICollection<Document> Documents { get; set; }

    public int? ParentFolderID { get; set; }
    public virtual Folder ParentFolder { get; set; }

    public virtual ICollection<Folder> ChildFolders { get; set; }

    public bool isRootFolder() {
      return ParentFolderID == null;
    }

    public Stack<Folder> getFolderHierarchy() {
      var stack = new Stack<Folder>();
      var parent = this.ParentFolder;

      while (parent != null) {
        stack.Push(parent);
        parent = parent.ParentFolder;
      }

      return stack;
    }

    public bool hasDocuments() {
      return Documents == null? false: Documents.Count() > 0;
    }
    public bool hasChildFolders() {
      return ChildFolders == null ? false : ChildFolders.Count() > 0;
    }
    
  }
}
