using SkinShop.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkinShop.Models.SkinShop
{
    public class CommentDM
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int ParentCommentId { get; set; }

        public int Assessment { get; set; }

        public string Text { get; set; }

        public DateTime CommentDate { get; set; }

        public virtual UserDM User { get; set; }
    }
}