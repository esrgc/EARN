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
      
      //context.UserGroups.RemoveRange(context.UserGroups.ToList());
      //context.Communities.RemoveRange(context.Communities.ToList());
      if (context.UserGroups.Count() == 0) {
        context.UserGroups.AddOrUpdate(
          new UserGroup { Name = "Industry partner", Description = "Seeking employees e.g healthcare representative" },
          new UserGroup { Name = "Facilitator", Description = "facilities partnership e.g. chamber of commerce" },
          new UserGroup { Name = "Training partner", Description = "Providing skilled workforce e.g. training center" }
        );
      }
      if (context.Communities.Count() == 0) {
        context.Communities.AddOrUpdate(
          new Community { Name = "Accommodation and Food Services" },
          new Community { Name = "Administrative and Support Services" },
          new Community { Name = "Agriculture, Forestry, Fishing, and Hunting" },
          new Community { Name = "Arts, Entertainment, and Recreation" },
          new Community { Name = "Construction" },
          new Community { Name = "Educational Services" },
          new Community { Name = "Finance and Insurance" },
          new Community { Name = "Government" },
          new Community { Name = "Health Care and Social Assistance" },
          new Community { Name = "Information" },
          new Community { Name = "Management of Companies and Enterprises" },
          new Community { Name = "Manufacturing" },
          new Community { Name = "Mining, Quarrying, and oil and Gas Extraction" },
          new Community { Name = "Professional, Scientific, and Technical Services" },
          new Community { Name = "Real Estate and Rental and Leasing" },
          new Community { Name = "Retail Trade" },
          new Community { Name = "Self-Employed" },
          new Community { Name = "Transportation and Warehousing" },
          new Community { Name = "Utilities" },
          new Community { Name = "Other Services (Except Public Administration)" }
        );
      }
    }
  }
}
