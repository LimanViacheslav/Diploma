using SkinShop.DL.EF;
using SkinShop.DL.Entities.Identity;
using SkinShop.DL.Interfaces.Identity;
using SkinShop.DL.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.DL.Repositories.Identity
{
    public class ClientManager : IClientManager
    {
        public Context Database { get; set; }
        public ClientManager(Context db)
        {
            Database = db;
        }

        public void Create(ClientProfile item)
        {
            if (item != null)
            {
                Database.ClientProfiles.Add(item);
                Database.SaveChanges();
            }
        }

        public User GetUser(string id)
        {
            return Database.Users.Find(id);
        }

        public IEnumerable<User> GetUsers()
        {
            return Database.Set<User>().Where(x => x.IsDeleted == false).ToList();
        }

        public User FindUser(Func<User, bool> predicate)
        {
            return Database.Set<User>().Where(predicate).FirstOrDefault();
        }

        public ICollection<User> FindUsers(Func<User, bool> predicate)
        {
            return Database.Set<User>().Where(predicate).ToList();
        }

        public void Update(User item)
        {
            Database.Entry(item).State = System.Data.Entity.EntityState.Modified;
        }
    }
}
