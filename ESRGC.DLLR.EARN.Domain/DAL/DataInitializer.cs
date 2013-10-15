using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using ESRGC.DLLR.EARN.Domain.DAL;
using ESRGC.DLLR.EARN.Domain.Model;

namespace ESRGC.DLLR.EARN.Domain.DAL
{
  public class DataInitializer : CreateDatabaseIfNotExists<DomainContext>
  {
    protected override void Seed(DomainContext context) {
     
      context.UserGroups.AddRange(new List<UserGroup>{
        new UserGroup { Name = "Industry partner", Description = "Seeking employees e.g healthcare representative" },
        new UserGroup { Name = "Facilitator", Description = "facilities partnership e.g. chamber of commerce" },
        new UserGroup { Name = "Training partner", Description = "Providing skilled workforce e.g. training center" }
      });

      context.Categories.AddRange(new List<Category>{
        new Category { Name = "Accommodation and Food Services" },
        new Category { Name = "Administrative and Support Services" },
        new Category { Name = "Agriculture, Forestry, Fishing, and Hunting" },
        new Category { Name = "Arts, Entertainment, and Recreation" },
        new Category { Name = "Construction" },
        new Category { Name = "Educational Services" },
        new Category { Name = "Finance and Insurance" },
        new Category { Name = "Government" },
        new Category { Name = "Health Care and Social Assistance" },
        new Category { Name = "Information" },
        new Category { Name = "Management of Companies and Enterprises" },
        new Category { Name = "Manufacturing" },
        new Category { Name = "Mining, Quarrying, and oil and Gas Extraction" },
        new Category { Name = "Professional, Scientific, and Technical Services" },
        new Category { Name = "Real Estate and Rental and Leasing" },
        new Category { Name = "Retail Trade" },
        new Category { Name = "Self-Employed" },
        new Category { Name = "Transportation and Warehousing" },
        new Category { Name = "Utilities" },
        new Category { Name = "Other Services (Except Public Administration)" }
      });
      
      //base.Seed(context);
      context.SaveChanges();
    }
  }
}
