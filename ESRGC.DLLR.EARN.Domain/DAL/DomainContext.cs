using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using ESRGC.DLLR.EARN.Domain.Model;

namespace ESRGC.DLLR.EARN.Domain.DAL
{
  public class DomainContext : DbContext
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
    public DbSet<Category> Categories { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<ProfileTag> ProfileTags { get; set; }
    //public DbSet<Connection> Connections { get; set; }
    public DbSet<Partnership> Partnership { get; set; }
    public DbSet<PartnershipDetail> PartnershipDetail { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder) {
      modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

      //modelBuilder.Entity<Connection>()
      //  .HasRequired(c => c.ConnectionProfile)
      //  .WithMany(p => p.Connections)
      //  .HasForeignKey(c => c.ConnectionProfileID);

      modelBuilder.Entity<Profile>()
        .HasMany(x => x.Connections)
        .WithMany()
        .Map(map => {
          map.MapLeftKey("ProfileID");
          map.MapRightKey("ConnectionProfileID");
          map.ToTable("Connection");
        });
    }
  }
}
