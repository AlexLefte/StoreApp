using StoreApp.DataAccess.Data;
using StoreApp.Models;
using StoreApp.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        #region Fields
        ApplicationDbContext _db;
        #endregion

        #region Constructor
        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        #endregion

        #region Methods

        public void Update(Company company)
        {
            _db.Companies.Update(company);
        }
        #endregion
    }
}
