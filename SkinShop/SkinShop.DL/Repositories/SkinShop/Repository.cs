using SkinShop.DL.EF;
using SkinShop.DL.Entities.SkinShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.DL.Repositories.SkinShop
{
    public class Repository<T> : AbstractRepository<T> where T : CommonFields
    {
        public Repository(Context context) : base(context) { }

        public override void Add(T item)
        {
            item.IsDeleted = false;
            _context.Entry(item).State = System.Data.Entity.EntityState.Added;
        }

        public override void Delete(int id)
        {
            T _item = Get(id);
            if (_item != null)
                _context.Entry(_item).State = System.Data.Entity.EntityState.Deleted;
        }

        public override void SoftDelete(int id)
        {
            T _item = Get(id);
            if (_item != null)
            {
                _item.IsDeleted = true;
                _context.Entry(_item).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public override void Update(T item)
        {
            _context.Entry(item).State = System.Data.Entity.EntityState.Modified;
        }

        public override ICollection<T> Show()
        {
            return _context.Set<T>().Where(x => x.IsDeleted == false).ToList();
        }

        public override IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public override T Get(int id)
        {
            return _context.Set<T>().Find(id);
        }
    }
}
