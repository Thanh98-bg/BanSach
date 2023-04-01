using BanSach.DataAccess.Data;
using BanSach.DataAccess.Repository.IRepository;
using BanSach.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanSach.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product product)
        {
            Product old_product = GetFirstOrDefault(p => p.Id == product.Id);
            if (old_product != null)
            {
                old_product.Author = product.Author;
                old_product.Description = product.Description;
                old_product.Price50 = product.Price50;
                old_product.Price100 = product.Price100;
                old_product.ISBN = product.ISBN;
                old_product.Title = product.Title;
                old_product.CoverTypeId = product.CoverTypeId;
                old_product.CategoryId = product.CategoryId;
                if (old_product.ImageUrl != null)
                {
                    old_product.ImageUrl = product.ImageUrl;
                }
                _db.Products.Update(old_product);
            }
        }
    }
}
