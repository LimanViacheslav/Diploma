using SkinShop.BLL.Identity.IdentityDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.BLL.SkinShop.SkinShopDTO
{
    public class CommentDTO : CommonFieldsDTO
    {
        public int ProductId { get; set; }

        public int ParentCommentId { get; set; }

        public int Assessment { get; set; }

        public string Text { get; set; }

        public DateTime CommentDate { get; set; }

        public virtual UserDTO User { get; set; }
    }
}
