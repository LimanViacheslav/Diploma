using SkinShop.DL.EF;
using SkinShop.DL.Interfaces.SkinShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.DL.Repositories.SkinShop
{
    public abstract class AbstractRepository<T> : IAdd<T>, IDelete, IShow<T>, IUpdate<T> where T : class
    {
        protected Context _context;

        public AbstractRepository(Context context)
        {
            _context = context;
        }

        public abstract void Add(T item);

        public abstract void Delete(int id);

        public abstract void SoftDelete(int id);

        public abstract ICollection<T> Show();

        public abstract T Get(int id);

        public abstract void Update(T item);

        public abstract IEnumerable<T> Find(Func<T, bool> predicate);
    }
}
