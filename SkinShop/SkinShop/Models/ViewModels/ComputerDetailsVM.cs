using SkinShop.Models.SkinShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkinShop.Models.ViewModels
{
    public class ComputerDetailsVM
    {
        public ComputerDM Computer { get; set; }

        public ProductDM Product { get; set; }

        public ProductDM Processor { get; set; }
        public List<ProductDM> Rams { get; set; }
        public List<ProductDM> Roms { get; set; }
        public List<ProductDM> VideoCards { get; set; }
        public ProductDM PowerBlock { get; set; }
        public ProductDM SystemBlock { get; set; }
        public ProductDM MotherBoard { get; set; }

        public bool IsCompAlreadyInBasket { get; set; }

        public bool IsCompAlreadyInFavorites { get; set; }

        public List<ProductDM> OtherComputers { get; set; }
    }
}