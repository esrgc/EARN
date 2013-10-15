using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;

namespace ESRGC.DLLR.EARN.Domain.Model
{
  public class GeoTag: Tag
  {
    public DbGeometry Geometry { get; set; }
  }
}
