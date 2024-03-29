﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace ESRGC.DLLR.EARN.Domain.Model
{
  public class Partnership
  {
    public Partnership() {
      PartnershipDetails = new List<PartnershipDetail>();
      PartnershipTags = new List<PartnershipTag>();
      Comments = new List<Comment>();
      Documents = new List<Document>();
      LastUpdate = DateTime.Now;
      Created = DateTime.Now;
    }
    string _name, _status, _description, _grantStatus;
    public int PartnershipID { get; set; }
    [Required]
    [MaxLength(100, ErrorMessage = "100 maximum characters allowed")]
    public string Name {
      get { return _name; }
      set { _name = value; LastUpdate = DateTime.Now; }
    }

    [Required]
    [MaxLength(100, ErrorMessage = "100 maximum characters allowed")]
    public string Status {
      get { return _status; }
      set { _status = value; LastUpdate = DateTime.Now; }
    }

    [Required]
    [MaxLength(500, ErrorMessage = "500 maximum characters allowed")]
    public string Description {
      get { return _description; }
      set { _description = value; LastUpdate = DateTime.Now; }
    }

    public int? PictureID { get; set; }
    [ForeignKey("PictureID")]
    public virtual Picture Logo { get; set; }


    public DateTime Created { get; set; }
    public DateTime? LastUpdate { get; set; }
    //nav properties
    public virtual ICollection<PartnershipDetail> PartnershipDetails { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }
    public virtual ICollection<PartnershipTag> PartnershipTags { get; set; }
    public virtual ICollection<Document> Documents { get; set; }

    //helpers
    /// <summary>
    /// get all partners of this partnership 
    /// including owner
    /// </summary>
    /// <returns></returns>
    public List<Profile> getAllPartners() {
      return PartnershipDetails.Select(x => x.Profile).ToList();
    }
    public List<Profile> getOwners() {
      try {
        return PartnershipDetails
           .Where(x => x.Type.ToLower() == "owner")
           .Select(x => x.Profile)
           .ToList();
      }
      catch (Exception) {
        return null;
      }
    }
    public string getOwnerNames() {
      var owners = this.getOwners().ToList();
      var names = owners.Select(x => x.Organization.Name).ToArray();
      return string.Join(",", names);
    }
    public List<Profile> getPartners() {
      return PartnershipDetails
        .Where(x => x.Type.ToLower() != "owner")
        .Select(x => x.Profile)
        .ToList();
    }
    public List<Tag> getTags() {
      return PartnershipTags.Select(x => x.Tag).ToList();
    }
    public List<string> getTagNames() {
      return PartnershipTags.Select(x => x.Tag.Name).ToList();
    }
    //public bool removePartner(int profileID) {
    //  try {
    //    var partner = PartnershipDetails.First(x => x.ProfileID == profileID);
    //    PartnershipDetails.Remove(partner);
    //    return true;
    //  }
    //  catch {
    //    return false;
    //  }
        
    //}
  }
}
