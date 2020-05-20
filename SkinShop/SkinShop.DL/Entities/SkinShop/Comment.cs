using SkinShop.DL.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.DL.Entities.SkinShop
{
    public class Comment: CommonFields
    {
        public int ProductId { get; set; }

        public int ParentCommentId { get; set; }

        public int Assessment { get; set; }

        public string Text { get; set; }

        public DateTime CommentDate { get; set; }

        public virtual User User { get; set; }
    }
}
