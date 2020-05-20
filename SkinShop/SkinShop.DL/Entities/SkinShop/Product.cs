using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.DL.Entities.SkinShop
{
    public enum Goods
    {
        Skin = 1,
        Game,
        Computer,
        Cloth,
        ComputerComponent,
        Container,
        Other
    }

    public class Product: CommonFields
    {
        public string Name { get; set; }

        public double Price { get; set; }

        public int Sale { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Image> Images { get; set; }

        public int CountFavorites { get; set; }

        public int CountOrders { get; set; }

        public int CountSeen { get; set; }

        public int FromTableId { get; set; }

        public DateTime DateOfAdded { get; set; }

        public Goods Table { get; set; }
    }
}
