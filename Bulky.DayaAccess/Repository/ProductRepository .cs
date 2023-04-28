using Bulky.DataAccess.Repository.IRepository;
using Bulky.DataAccess;
using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProducdtRepository
    {

        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
       
     

      
        public void Update(Product product)
        {
            _db.Products.Update(product);
        }
    }
}
