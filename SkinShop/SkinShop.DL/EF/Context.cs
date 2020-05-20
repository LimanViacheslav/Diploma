using Microsoft.AspNet.Identity.EntityFramework;
using SkinShop.DL.Entities.Identity;
using SkinShop.DL.Entities.SkinShop;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.DL.EF
{
    public class Context : IdentityDbContext<User>
    {
        static Context()
        {
            Database.SetInitializer<Context>(new DbInitializer());
        }

        public Context(string conectionString) : base(conectionString) { }

        public DbSet<Game> Games { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Favorites> Favorites { get; set; }
        public DbSet<Skin> Skins { get; set; }
        public DbSet<SkinRarity> SkinRarities { get; set; }
        public DbSet<OrderCount> OrderCounts { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Cloth> Clothes { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Computer> Computers { get; set; }
        public DbSet<ComputerComponent> ComputerComponents { get; set; }
        public DbSet<Container> Containers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Other> Others { get; set; }
        public DbSet<StringData> StringDatas { get; set; }

        public DbSet<ClientProfile> ClientProfiles { get; set; }

        //public override IDbSet<User> Users { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Skin>().HasMany(c => c.Baskets)
        //        .WithMany(s => s.Skins)
        //        .Map(t => t.MapLeftKey("SkinId")
        //        .MapRightKey("BasketId")
        //        .ToTable("SkinBasket"));
        //}
    }
}
