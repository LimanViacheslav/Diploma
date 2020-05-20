using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.BLL.SkinShop.SkinShopDTO
{
    public class ComputerDTO : CommonFieldsDTO
    {
        public string Name { get; set; }

        public ComputerComponentDTO Processor { get; set; }

        public ICollection<ComputerComponentDTO> RAM { get; set; }

        public ICollection<ComputerComponentDTO> ROM { get; set; }

        public ICollection<ComputerComponentDTO> VideoCard { get; set; }

        public ComputerComponentDTO MotherBoard { get; set; }

        public ComputerComponentDTO PowerSupply { get; set; }

        public ComputerComponentDTO SystemBlock { get; set; }

        public virtual ICollection<PropertyDTO> Properties { get; set; }
    }
}
