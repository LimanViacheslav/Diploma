using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.DL.Entities.SkinShop
{
    public class Image : CommonFields
    {
        public string Text { get; set; }

        public byte[] Photo { get; set; }

    }
}
