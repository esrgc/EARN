using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using ESRGC.DLLR.EARN.Domain.DAL;
using ESRGC.DLLR.EARN.Domain.Model;

namespace ESRGC.DLLR.EARN.Domain.Migrations
{
  
  internal sealed class Configuration : DbMigrationsConfiguration<DomainContext>
  {
    public Configuration() {
      AutomaticMigrationsEnabled = false;
      ContextKey = "ESRGC.DLLR.EARN.Domain.DAL.DomainContext";
    }

    protected override void Seed(DomainContext context) {
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
      
      context.UserGroups.RemoveRange(context.UserGroups.ToList());
      context.Industries.RemoveRange(context.Industries.ToList());
      context.UserGroups.AddOrUpdate(
        new UserGroup { Name = "Industry member", Description = "Seeking employees e.g healthcare representative" },
        new UserGroup { Name = "Convener", Description = "facilities partnership e.g. chamber of commerce" },
        new UserGroup { Name = "Strategic partner", Description = "Providing skilled workforce e.g. training center" }
      );
      context.Industries.AddOrUpdate(
        new Industry { Name = "Accommodation and Food Services" },
        new Industry { Name = "Administrative and Support Services" },
        new Industry { Name = "Agriculture, Forestry, Fishing, and Hunting" },
        new Industry { Name = "Arts, Entertainment, and Recreation" },
        new Industry { Name = "Construction" },
        new Industry { Name = "Educational Services" },
        new Industry { Name = "Finance and Insurance" },
        new Industry { Name = "Government" },
        new Industry { Name = "Health Care and Social Assistance" },
        new Industry { Name = "Information" },
        new Industry { Name = "Management of Companies and Enterprises" },
        new Industry { Name = "Manufacturing" },
        new Industry { Name = "Mining, Quarrying, and oil and Gas Extraction" },
        new Industry { Name = "Professional, Scientific, and Technical Services" },
        new Industry { Name = "Real Estate and Rental and Leasing" },
        new Industry { Name = "Retail Trade" },
        new Industry { Name = "Self-Employed" },
        new Industry { Name = "Transportation and Warehousing" },
        new Industry { Name = "Utilities" },
        new Industry { Name = "Other Services (Except Public Administration)" }
      );
    }
  }
}
