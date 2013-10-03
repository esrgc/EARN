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
          new UserGroup { Name = "Training provider", Description = "Providing skilled workforce e.g. training center" }
        );
        context.SaveChanges();
      }
      if (context.Categories.Count() == 0) {
        context.Categories.AddOrUpdate(
          new Category { Name = "Accommodation and Food Services", UserGroupID = 1 },
          new Category { Name = "Administrative and Support Services", UserGroupID = 1 },
          new Category { Name = "Agriculture, Forestry, Fishing, and Hunting", UserGroupID = 1 },
          new Category { Name = "Arts, Entertainment, and Recreation", UserGroupID = 1 },
          new Category { Name = "Construction", UserGroupID = 1 },
          new Category { Name = "Educational Services", UserGroupID = 1 },
          new Category { Name = "Finance and Insurance", UserGroupID = 1 },
          new Category { Name = "Government", UserGroupID = 1 },
          new Category { Name = "Health Care and Social Assistance", UserGroupID = 1 },
          new Category { Name = "Information", UserGroupID = 1 },
          new Category { Name = "Management of Companies and Enterprises", UserGroupID = 1 },
          new Category { Name = "Manufacturing", UserGroupID = 1 },
          new Category { Name = "Mining, Quarrying, and oil and Gas Extraction", UserGroupID = 1 },
          new Category { Name = "Professional, Scientific, and Technical Services", UserGroupID = 1 },
          new Category { Name = "Real Estate and Rental and Leasing", UserGroupID = 1 },
          new Category { Name = "Retail Trade", UserGroupID = 1 },
          new Category { Name = "Self-Employed", UserGroupID = 1 },
          new Category { Name = "Transportation and Warehousing", UserGroupID = 1 },
          new Category { Name = "Utilities", UserGroupID = 1 },
          new Category { Name = "Other Services (Except Public Administration)", UserGroupID = 1 },

          new Category { Name = "Regional Council", UserGroupID = 2 },
          new Category { Name = "Economic Development Group", UserGroupID = 2 },
          new Category { Name = "Workforce Representative", UserGroupID = 2 },
          new Category { Name = "Labor Union Leader", UserGroupID = 2 },
          new Category { Name = "Chamber of Commerce", UserGroupID = 2 },
          new Category { Name = "Workforce Investment Board", UserGroupID = 2 },

          new Category { Name = "Four-Year College or University", UserGroupID = 3 },
          new Category { Name = "Two-Year, Community, and Technical Colleges", UserGroupID = 3 },
          new Category { Name = "Apprenticeship Programs", UserGroupID = 3 },
          new Category { Name = "Private Career and Technical School", UserGroupID = 3 },
          new Category { Name = "Community-Based Training Center", UserGroupID = 3 },
          new Category { Name = "Regional Higher Education Centers", UserGroupID = 3 },
          new Category { Name = "One-Stop Job Market", UserGroupID = 3 },
          new Category { Name = "Workforce Investment Board", UserGroupID = 3 },
          new Category { Name = "Other" }

        );
      }
    }
  }
}
