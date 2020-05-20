using SkinShop.Models.SkinShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkinShop.Models.ViewModels
{
    public class GameDetailsVM
    {
        public GameDM Game { get; set; }

        public ProductDM Product { get; set; }

        public bool IsGameAlreadyInBasket { get; set; }

        public bool IsGameAlreadyInFavorites { get; set; }

        public List<ProductDM> OtherGames { get; set; }
    }
}