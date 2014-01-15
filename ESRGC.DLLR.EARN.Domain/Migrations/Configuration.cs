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
        new Tag { Name = "Career Planning".ToUpper() },
        new Tag { Name = "Community Organization".ToUpper() },
        new Tag { Name = "Economic Development".ToUpper() },
        new Tag { Name = "Education".ToUpper() },
        new Tag { Name = "Engineering".ToUpper() },
        new Tag { Name = "Information Systems".ToUpper() },
        new Tag { Name = "Information Technology".ToUpper() },
        new Tag { Name = "Job Creation".ToUpper() },
        new Tag { Name = "Job Placement".ToUpper() },
        new Tag { Name = "Job Training".ToUpper() },
        new Tag { Name = "Jobs".ToUpper() },
        new Tag { Name = "Training".ToUpper() },
        new Tag { Name = "Workforce".ToUpper() },
        new Tag { Name = "Workforce Development".ToUpper() },
        new Tag { Name = "Workforce Training".ToUpper() },
        new Tag { Name = "Architecture".ToUpper() },
        new Tag { Name = "Engineering".ToUpper() },
        new Tag { Name = "Design".ToUpper() },
        new Tag { Name = "Entertainment".ToUpper() },
        new Tag { Name = "Sports".ToUpper() },
        new Tag { Name = "Media".ToUpper() },
        new Tag { Name = "Financial Services".ToUpper() },
        new Tag { Name = "Community Services".ToUpper() },
        new Tag { Name = "Social Services".ToUpper() },
        new Tag { Name = "Construction".ToUpper() },
        new Tag { Name = "Farming".ToUpper() },
        new Tag { Name = "Fishing".ToUpper() },
        new Tag { Name = "Forestry".ToUpper() },
        new Tag { Name = "Healthcare".ToUpper() },
        new Tag { Name = "Legal".ToUpper() },
        new Tag { Name = "Management".ToUpper() },
        new Tag { Name = "Military Specific".ToUpper() },
        new Tag { Name = "Administrative Support".ToUpper() },
        new Tag { Name = "Personal Care".ToUpper() },
        new Tag { Name = "Production".ToUpper() },
        new Tag { Name = "Protective Service".ToUpper() },
        new Tag { Name = "Sales".ToUpper() },
        new Tag { Name = "Transportation".ToUpper() },
        new Tag { Name = "Food Services".ToUpper() },
        new Tag { Name = "Waste Management".ToUpper() },
        new Tag { Name = "Remediation".ToUpper() },
        new Tag { Name = "Agriculture".ToUpper() },
        new Tag { Name = "Forestry".ToUpper() },
        new Tag { Name = "Fishing".ToUpper() },
        new Tag { Name = "Hunting".ToUpper() },
        new Tag { Name = "Entertainment".ToUpper() },
        new Tag { Name = "Recreation".ToUpper() },
        new Tag { Name = "Construction".ToUpper() },
        new Tag { Name = "Education".ToUpper() },
        new Tag { Name = "Financial Services".ToUpper() },
        new Tag { Name = "Insurance".ToUpper() },
        new Tag { Name = "Healthcare".ToUpper() },
        new Tag { Name = "Manufacturing".ToUpper() },
        new Tag { Name = "Mineral Extraction".ToUpper() },
        new Tag { Name = "Oil & Gas".ToUpper() },
        new Tag { Name = "Public Administration".ToUpper() },
        new Tag { Name = "Real Estate".ToUpper() },
        new Tag { Name = "Retail Trade".ToUpper() },
        new Tag { Name = "Transportation".ToUpper() },
        new Tag { Name = "Warehousing".ToUpper() },
        new Tag { Name = "Utilities".ToUpper() },
        new Tag { Name = "Wholesale Trade".ToUpper() }
        );
        context.SaveChanges();
      };
      if (context.UserGroups.Count() == 0) {
        context.UserGroups.AddOrUpdate(
          new UserGroup { Name = "Industry", Description = "e.g. Industry Associations, Employers, Chambers of Commerce" },
          new UserGroup { Name = "Education and Training", Description = "e.g. Two- and Four-Year Institutions of Higher Education, Apprenticeship programs, K-12 programs" },
          new UserGroup { Name = "Workforce and Economic Development and Local Governmental Entities", Description = "" },
          new UserGroup { Name = "Nonprofits and Community-based Organizations", Description = "e.g. Job Training Advocacy Organizations, Veterans Groups, Advocates for People with Disabilities" },
          new UserGroup { Name = "Other Strategic Partners", Description = "e.g. Labor Unions, Philanthropic Organizations" },
          new UserGroup { Name = "Potential Employee Recruitment", Description = "" }
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
