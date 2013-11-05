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
    public DbSet<Request> Requests { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Comment> Comments { get; set; }
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
      //sender foreign key
      modelBuilder.Entity<Account>()
        .HasMany(x => x.SentRequests)
        .WithRequired(x => x.Sender)
        .HasForeignKey(x => x.SenderID)
        .WillCascadeOnDelete(false);
      //receiver foreign key
      //modelBuilder.Entity<Account>()
      //  .HasMany(x => x.ReceivedRequests)
      //  .WithRequired(x => x.Receiver)
      //  .HasForeignKey(x => x.ReceiverID);
      //or
      modelBuilder.Entity<Request>()
        .HasRequired(x => x.Receiver)
        .WithMany(x => x.ReceivedRequests)
        .HasForeignKey(x => x.ReceiverID)
        .WillCascadeOnDelete(false);
      //Profile - Comment relationship (one to many)
      modelBuilder.Entity<Comment>()
        .HasRequired(x => x.Author)
        .WithMany(x => x.Comments)
        .HasForeignKey(x => x.AuthorID);
    }
  }
}
