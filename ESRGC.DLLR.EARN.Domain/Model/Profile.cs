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
      Connections = new List<Profile>();
      ProfileTags = new List<ProfileTag>();
      PartnershipDetails = new List<PartnershipDetail>();
      Comments = new List<Comment>();
      SentMessages = new List<Message>();
      ReceivedMessages = new List<Message>();
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
    [Display(Description = "Describe why your organization is interested in EARN MD (i.e., what you would bring to a Partnership and/or what you hope to gain).  What you provide here will be visible to other EARN MD CONNECT users when searching for potential partners.")]
    public string About { get; set; }

    //navigation properties
    public virtual ICollection<ProfileTag> ProfileTags { get; set; }
    public virtual ICollection<Profile> Connections { get; set; }
    public virtual ICollection<PartnershipDetail> PartnershipDetails { get; set; }
    public virtual ICollection<Account> Accounts { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }
    public ICollection<Message> SentMessages { get; set; }
    public ICollection<Message> ReceivedMessages { get; set; }

    //helpers
    public bool isOwnerOfPartnership(Partnership partnership) {
      return isOwnerOfPartnership(partnership.PartnershipID);
    }
    public bool isOwnerOfPartnership(int partnershipID) {
      return PartnershipDetails
        .Where(x => x.Type.ToLower() == "owner")
        .Select(x => x.PartnershipID)
        .Contains(partnershipID);
    }
    /// <summary>
    /// delete all profile details including
    /// connections, tags, comments, partnerships, and  messages
    /// </summary>
    public bool deleteDetails() {
      try {
        foreach (var c in Connections)
          Connections.Remove(c);
        foreach (var pt in ProfileTags)
          ProfileTags.Remove(pt);
        foreach (var pd in PartnershipDetails)
          PartnershipDetails.Remove(pd);
        foreach (var cm in Comments)
          Comments.Remove(cm);
        foreach (var sm in SentMessages)
          SentMessages.Remove(sm);
        foreach (var rm in ReceivedMessages)
          ReceivedMessages.Remove(rm);
        return true;
      }
      catch (Exception) {
        return false;
      }
    }
    /// <summary>
    /// Gets the account associated with this profile
    /// </summary>
    /// <returns></returns>
    public Account getAccount() {
      try {
        return Accounts.First();
      }
      catch (Exception) {
        return null;
      }
    }
    /// <summary>
    /// Returns a list of owned partnerships
    /// </summary>
    /// <returns>Null if no owned partnership found</returns>
    public List<Partnership> getOwnedPartnerships() {
      try {
        var partnerships = PartnershipDetails
            .Where(x => x.Type.ToLower() == "owner")
            .Select(x => x.Partnership).ToList();
        return partnerships;
      }
      catch (Exception) {
        return null;
      }
    }
    public bool hasOwnedPartnerships() {
      var ownPartnerships = getOwnedPartnerships();
      if (ownPartnerships == null)
        return false;
      return ownPartnerships.Count() > 0;
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
    public int partnershipCount() {
      return PartnershipDetails.Count();
    }
  }
}
