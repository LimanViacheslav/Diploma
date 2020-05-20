using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.DL.Interfaces.SkinShop
{
    public interface IAdd<T> where T : class
    {
        void Add(T item);
    }
}
