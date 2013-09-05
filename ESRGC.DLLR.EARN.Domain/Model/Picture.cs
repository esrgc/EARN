using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ESRGC.DLLR.EARN.Domain.Model
{
    public class Picture
    {
        public int PictureID { get; set; }
        public byte[] ImageData { get; set; }
        [ScaffoldColumn(false)]
        public string ImageMimeType { get; set; }
    }
}
