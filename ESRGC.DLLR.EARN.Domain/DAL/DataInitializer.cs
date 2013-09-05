using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using ESRGC.DLLR.EARN.Domain.DAL;

namespace ESRGC.DLLR.EARN.Domain.DAL
{
  public class DataInitializer : CreateDatabaseIfNotExists<DomainContext>
  {
    protected override void Seed(DomainContext context) {
      context.SaveChanges();
      base.Seed(context);
    }
  }
}
