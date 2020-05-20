using SkinShop.DL.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.DL.Interfaces.Identity
{
    public interface IClientManager
    {
        void Create(ClientProfile item);
        User GetUser(string id);
        User FindUser(Func<User, bool> predicate);
        IEnumerable<User> GetUsers();
        ICollection<User> FindUsers(Func<User, bool> predicate);
        void Update(User item);
    }
}
