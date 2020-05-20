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

            Game cs = new Game()
            {
                IsThingGame = true,
                Name = "CS GO",
                SystemRequirements = "Минимальные системные требования: /br" +
                "Процессор: Intel® Core™ 2 Duo E6600 / AMD Phenom™ X3 8750 " +
                "Видеокарта: Видеокарта должна иметь объем видеопамяти не меньше 256 Мб " +
                "Оперативная память: 2 Гб " +
                "Операционная система: Windows® 7 / Vista / XP " +
                "Место на диске: 15 ГБ " +
                "Рекомендуемые системные требования: " +
                "Официальных рекомендованных системных требований нет.Ниже представлено наше видение требований, достаточных для игры в FHD разрешении на максимальных настройках." +
                " Процессор: Intel® Core™ i3 " +
                "Видеокарта: NVIDIA® GeForce® GTX 1050 2 ГБ и любая видеокарта новее с видеопамятью от 1 Гб " +
                "Оперативная память: 4 Гб " +
                "Операционная система: Windows 7 / 8 / 10 " +
                "Место на диске: 15 ГБ"
            };
            db.Games.Add(cs);

            Product csProduct = new Product()
            {
                FromTableId = 1,
                Name = "CS GO",
                Price = 200,
                Sale = 0,
                Table = Goods.Game
            };

            db.Products.Add(csProduct);

            Skin ak = new Skin()
            {
                Game = cs,
                SkinRarity = _legendary,
                Name = "Ak 47"
            };

            Product akProduct = new Product()
            {
                FromTableId = 1,
                Table = Goods.Skin,
                Name = "Ak 47",
                Price = 100,
                Sale = 10
            };

            db.Products.Add(akProduct);
            db.Skins.Add(ak);

            base.Seed(db);
        }
    }
}
