namespace ESRGC.DLLR.EARN.Domain.Migrations
{
  using System;
  using System.Data.Entity;
  using System.Data.Entity.Migrations;
  using System.Linq;
  using ESRGC.DLLR.EARN.Domain.Model;

  internal sealed class Configuration : DbMigrationsConfiguration<ESRGC.DLLR.EARN.Domain.DAL.DomainContext>
  {
    public Configuration() {
      AutomaticMigrationsEnabled = false;
      ContextKey = "ESRGC.DLLR.EARN.Domain.DAL.DomainContext";
    }

    protected override void Seed(ESRGC.DLLR.EARN.Domain.DAL.DomainContext context) {
      //  This method will be called after migrating to the latest version.

      //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
      //  to avoid creating duplicate seed data. E.g.
      //
      //    context.people.addorupdate(
      //      p => p.fullname,
      //      new person { fullname = "andrew peters" },
      //      new person { fullname = "brice lambson" },
      //      new person { fullname = "rowan miller" }
      //    );
      //
      context.UserGroups.AddOrUpdate(
        new UserGroup { Name = "Industry Member" },
        new UserGroup { Name = "Convener" },
        new UserGroup { Name = "Strategic Partner" }
        );
    }
  }
}
