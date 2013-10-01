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

      context.Communities.AddRange(new List<Community>{
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
      });
      
      //base.Seed(context);
      context.SaveChanges();
    }
  }
}
