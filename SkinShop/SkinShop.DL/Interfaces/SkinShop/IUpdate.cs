using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.DL.Interfaces.SkinShop
{
    public interface IUpdate<T> where T : class
    {
        void Update(T item);
    }
}
