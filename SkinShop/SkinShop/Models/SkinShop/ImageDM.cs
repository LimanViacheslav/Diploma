using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkinShop.Models.SkinShop
{
    public class ImageDM
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public byte[] Photo { get; set; }
    }
}