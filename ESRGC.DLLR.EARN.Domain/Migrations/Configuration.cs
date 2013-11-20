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
      if (context.Tags.Count() == 0) {
        context.Tags.AddOrUpdate(
        new Tag { Name = "Career Planning" },
        new Tag { Name = "Community Organization" },
        new Tag { Name = "Economic Development" },
        new Tag { Name = "Education" },
        new Tag { Name = "Engineering" },
        new Tag { Name = "Information Systems" },
        new Tag { Name = "Information Technology" },
        new Tag { Name = "Job Creation" },
        new Tag { Name = "Job Placement" },
        new Tag { Name = "Job Training" },
        new Tag { Name = "Jobs" },
        new Tag { Name = "Training" },
        new Tag { Name = "Workforce" },
        new Tag { Name = "Workforce Development" },
        new Tag { Name = "Workforce Training" },
        new Tag { Name = "Architecture" },
        new Tag { Name = "Engineering" },
        new Tag { Name = "Design" },
        new Tag { Name = "Entertainment" },
        new Tag { Name = "Sports" },
        new Tag { Name = "Media" },
        new Tag { Name = "Financial Services" },
        new Tag { Name = "Community Services" },
        new Tag { Name = "Social Services" },
        new Tag { Name = "Construction" },
        new Tag { Name = "Farming" },
        new Tag { Name = "Fishing" },
        new Tag { Name = "Forestry" },
        new Tag { Name = "Healthcare" },
        new Tag { Name = "Legal" },
        new Tag { Name = "Management" },
        new Tag { Name = "Military Specific" },
        new Tag { Name = "Administrative Support" },
        new Tag { Name = "Personal Care" },
        new Tag { Name = "Production" },
        new Tag { Name = "Protective Service" },
        new Tag { Name = "Sales" },
        new Tag { Name = "Transportation" },
        new Tag { Name = "Food Services" },
        new Tag { Name = "Waste Management" },
        new Tag { Name = "Remediation" },
        new Tag { Name = "Agriculture" },
        new Tag { Name = "Forestry" },
        new Tag { Name = "Fishing" },
        new Tag { Name = "Hunting" },
        new Tag { Name = "Entertainment" },
        new Tag { Name = "Recreation" },
        new Tag { Name = "Construction" },
        new Tag { Name = "Education" },
        new Tag { Name = "Financial Services" },
        new Tag { Name = "Insurance" },
        new Tag { Name = "Healthcare" },
        new Tag { Name = "Manufacturing" },
        new Tag { Name = "Mineral Extraction" },
        new Tag { Name = "Oil & Gas" },
        new Tag { Name = "Public Administration" },
        new Tag { Name = "Real Estate" },
        new Tag { Name = "Retail Trade" },
        new Tag { Name = "Transportation" },
        new Tag { Name = "Warehousing" },
        new Tag { Name = "Utilities" },
        new Tag { Name = "Wholesale Trade" }
        );
        context.SaveChanges();
      };
      if (context.UserGroups.Count() == 0) {
        context.UserGroups.AddOrUpdate(
          new UserGroup { Name = "Industry", Description = "e.g. Industry Associations, Employers, Chambers of Commerce" },
          new UserGroup { Name = "Education and Training", Description = "e.g. Two- and Four-Year Institutions of Higher Education, Apprenticeship programs, K-12 programs" },
          new UserGroup { Name = "Workforce and Economic Development and Local Governmental Entities", Description = "" },
          new UserGroup { Name = "Nonprofits and Community-based Organizations", Description = "e.g. Job Training Advocacy Organizations, Veterans Groups, Advocates for People with Disabilities" },
          new UserGroup { Name = "Other Strategic Partners", Description = "e.g. Labor Unions, Philanthropic Organizations" }
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
