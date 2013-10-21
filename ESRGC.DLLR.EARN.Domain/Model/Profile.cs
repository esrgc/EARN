using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;

namespace ESRGC.DLLR.EARN.Domain.Model
{
  public class Profile
  {
    /// <summary>
    /// Profile ID
    /// </summary>
    public int ProfileID { get; set; }
    //[ScaffoldColumn(false)]
    //public double? lat { get; set; }
    //[ScaffoldColumn(false)]
    //public double? lon { get; set; }
    //public DbGeometry location { get; set; }

    [ScaffoldColumn(false)]
    public int? PictureID { get; set; }
    public virtual Picture ProfilePicture { get; set; }

    //Navigation properties
    public int ContactID { get; set; }
    public virtual Contact Contact { get; set; }

    [Display(Name = "Organization")]
    public int OrganizationID { get; set; }
    public virtual Organization Organization { get; set; }

    public int UserGroupID { get; set; }
    public virtual UserGroup UserGroup { get; set; }

    public int? CategoryID { get; set; }
    public virtual Category Category { get; set; }

    public DateTime? LastUpdate { get; set; }
    [Display(Description = "Description about why you're on this site.")]
    public string About { get; set; }
    public virtual ICollection<ProfileTag> ProfileTags { get; set; }
    public virtual ICollection<Connection> Connections { get; set; }
    public virtual ICollection<PartnershipDetail> PartnershipDetails { get; set; }

    public Connection createConnection(int ProfileID) {
      var connection = new Connection() { ProfileID = this.ProfileID, ProfileID2 = ProfileID };
      return connection;
    }

    public bool LocationVerified() {
      if (ProfileTags == null)
        return false;
      try {
        ProfileTags
          .Select(x=>x.Tag)
          .OfType<GeoTag>()
          .First(x => x.Description.ToLower() == "address");
        return true;
      }
      catch {
        return false;
      }
    }
  }
}
