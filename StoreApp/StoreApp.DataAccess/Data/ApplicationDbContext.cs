using Microsoft.EntityFrameworkCore;
using StoreApp.Models;

namespace StoreApp.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        #region Constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        #endregion

        #region Properties
        public DbSet<Category>? Categories { get; set; }
        #endregion

        #region Methods
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category {
                    Id = 1,
                    Name = "Action",
                    DisplayOrder = 1
                },
                new Category
                {
                    Id = 2,
                    Name = "SciFi",
                    DisplayOrder = 2
                },
                new Category
                {
                    Id = 3,
                    Name = "History",
                    DisplayOrder = 3
                },
                new Category
                {
                    Id = 4,
                    Name = "Animation",
                    DisplayOrder = 4
                });
        }
        #endregion
    }
}