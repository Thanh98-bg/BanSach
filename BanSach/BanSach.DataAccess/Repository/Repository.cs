using BanSach.DataAccess.Data;
using BanSach.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BanSach.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        private DbSet<T> DbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            DbSet = _db.Set<T>();
        }
        public void Add(T item)
        {
            DbSet.Add(item);
        }

        public IEnumerable<T> GetAll()
        {
            return DbSet.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = DbSet.AsQueryable();
            return query.Where(filter).FirstOrDefault();
        }

        public void Remove(T item)
        {
            DbSet.Remove(item);
        }

        public void RemoveRange(IEnumerable<T> items)
        {
            DbSet.RemoveRange(items);
        }
    }
}
