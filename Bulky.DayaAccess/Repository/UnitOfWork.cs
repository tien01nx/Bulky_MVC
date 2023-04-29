using Bulky.DataAccess.Repository.IRepository;
using Bulky.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public ICategoryRepository Category { get; private set; }
        public IProducdtRepository Producdt { get; private set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Producdt = new ProductRepository(_db);
        }
    

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}

