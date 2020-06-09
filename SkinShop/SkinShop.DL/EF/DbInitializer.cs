using SkinShop.DL.Entities.SkinShop;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.DL.EF
{
    public class DbInitializer : DropCreateDatabaseIfModelChanges<Context>
    {
        protected override void Seed(Context db)
        {
            Color red = new Color() { Name = "Крастный", ColorValue = "#fc2403" };
            Color blue = new Color() { Name = "Голубой", ColorValue = "#03cefc" };
            Color gray = new Color() { Name = "Серый", ColorValue = "#8f8f8f" };
            Color gold = new Color() { Name = "Золотой", ColorValue = "#ffee00" };
            Color purple = new Color() { Name = "Фиолетовый", ColorValue = "#c4009a" };

            db.Colors.Add(red);
            db.Colors.Add(blue);
            db.Colors.Add(purple);
            db.Colors.Add(gray);
            db.Colors.Add(gold);

            SkinRarity _rare = new SkinRarity() { Id = 1, RarityName = "Rare", Colors = new List<Color>() { blue } };
            SkinRarity _common = new SkinRarity() { Id = 2, RarityName = "Common", Colors =  new List<Color>() { gray } };
            SkinRarity _legendary = new SkinRarity() { Id = 3, RarityName = "Legendary", Colors = new List<Color>() { gold } };

            db.SkinRarities.Add(_rare);
            db.SkinRarities.Add(_common);
            db.SkinRarities.Add(_legendary);

            base.Seed(db);
        }
    }
}
