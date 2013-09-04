using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ESRGC.DLLR.EARN.Domain.DAL
{
  public class DomainContext: DbContext
  {
    public DomainContext()
      : base("name=DLLR.EARN") {

    }
    
    protected override void OnModelCreating(DbModelBuilder modelBuilder) {
      modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
      //modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
    }
  }
}
