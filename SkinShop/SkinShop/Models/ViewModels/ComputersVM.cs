using SkinShop.Models.SkinShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkinShop.Models.ViewModels
{
    public class ComputersVM
    {
        public IEnumerable<ProductDM> Computers { get; set; }

        public List<string> Processors { get; set; }

        public List<string> RAMs { get; set; }

        public List<string> ROMs { get; set; }

        public List<string> VideoCards { get; set; }

        public List<string> MotherBoards { get; set; }

        public List<string> PowerSupplies { get; set; }

        public List<string> SystemBlocks { get; set; }

        public int MinPrice { get; set; }

        public int MaxPrice { get; set; }

        public PageInfo PageInfo { get; set; }
    }
}