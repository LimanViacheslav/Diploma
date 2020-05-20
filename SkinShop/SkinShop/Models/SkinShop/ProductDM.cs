using SkinShop.DL.Entities.SkinShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkinShop.Models.SkinShop
{
    public class ProductDM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public int Sale { get; set; }

        public string Description { get; set; }

        public virtual ICollection<ImageDM> Images { get; set; }

        public int CountFavorites { get; set; }

        public int CountOrders { get; set; }

        public int CountSeen { get; set; }

        public int FromTableId { get; set; }

        public DateTime DateOfAdded { get; set; }
        
        public Goods Table { get; set; }

        public virtual ICollection<CommentDM> Comments { get; set; }
    }
}