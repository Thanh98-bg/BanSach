using BanSach.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanSach.DataAccess.Repository.IRepository
{
    public interface IShoppingCartRepository:IRepository<ShoppingCart>
    {
        int IncrementCount(ShoppingCart cart, int count);
        int DecrementCount(ShoppingCart cart, int count);
    }
}
