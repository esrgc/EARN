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

    public Profile() {
      Connections = new HashSet<Profile>();
      ProfileTags = new HashSet<ProfileTag>();
      PartnershipDetails = new HashSet<PartnershipDetail>();
    }
    /// <summary>
    /// Profile ID
    /// </summary>
    public int ProfileID { get; set; }

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
    [Display(Description = "Describe why your organization is interested in EARN MD (i.e., what you bring to a Partnership and/or what you hope to gain)")]
    public string About { get; set; }

    //navigation properties
    public virtual ICollection<ProfileTag> ProfileTags { get; set; }
    public virtual ICollection<Profile> Connections { get; set; }
    public virtual ICollection<PartnershipDetail> PartnershipDetails { get; set; }


    //helpers
    public bool canEditPartnership(Partnership partnership) {
      return canEditPartnership(partnership.PartnershipID);
    }
    public bool canEditPartnership(int partnershipID) {
      return PartnershipDetails
        .Where(x => x.Type.ToLower() == "owner")
        .Select(x => x.PartnershipID)
        .Contains(partnershipID);
    }
    public bool isNewToPartnership(Partnership partnership) {
      return isNewToPartnership(partnership.PartnershipID);
    }
    /// <summary>
    /// Determines whether the current profile is already associated with
    /// the provided partnership
    /// </summary>
    /// <param name="partnershipID"></param>
    /// <returns></returns>
    public bool isNewToPartnership(int partnershipID) {
      var partnerships = PartnershipDetails
        .Where(x => x.PartnershipID == partnershipID)
        .Count();
      if (partnerships > 0)
        return false;
      else
        return true;
    }
    public void addConnection(Profile connProf) {
      if (!hasConnection(connProf))
        this.Connections.Add(connProf);
    }
    public void removeConnection(Profile connProf) {
      if (hasConnection(connProf)) {
        Connections.Remove(connProf);
      }
    }
    public bool hasConnection(Profile connProf) {
      return Connections.Contains(connProf);
    }

    public bool LocationVerified() {
      if (ProfileTags == null)
        return false;
      try {
        return ProfileTags
          .Select(x => x.Tag)
          .OfType<GeoTag>()
          .First(x => x.Description.ToLower() == "address")
          .Name.Contains(Organization.StreetAddress);

      }
      catch {
        return false;
      }
    }

    public GeoTag getGeoTag() {
      try {
        return ProfileTags
              .Select(x => x.Tag)
              .First(x => (x is GeoTag) && x.Description == "address") as GeoTag;
      }
      catch {
        return null;
      }
    }

    public List<Tag> getTags() {
      var list = ProfileTags
        .Select(x => x.Tag)
        .Where(x => !(x is GeoTag))
        .OrderBy(x => x.Name)
        .ToList();
      return list ?? new List<Tag>();
    }
    public List<string> getTagNames() {
      return getTags().Select(x => x.Name).ToList();
    }
  }
}
