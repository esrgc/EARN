using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using ESRGC.DLLR.EARN.Domain.Model;

namespace ESRGC.DLLR.EARN.Domain.DAL
{
  public class DomainContext: DbContext
  {
    public DomainContext()
      : base("name=DLLR.EARN") {

    }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Picture> Pictures { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<UserGroup> UserGroups { get; set; }
    public DbSet<Community> Communities { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<ProfileTag> ProfileTags { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder) {
      modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
      //modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
    }
  }
}
