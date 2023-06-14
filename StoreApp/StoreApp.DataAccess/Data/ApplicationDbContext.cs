using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StoreApp.Models;

namespace StoreApp.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        #region Constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        #endregion

        #region Properties
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<User> Users { get; set;  }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        #endregion

        #region Methods
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category {
                    Id = 1,
                    Name = "Rock",
                    DisplayOrder = 1
                },
                new Category
                {
                    Id = 2,
                    Name = "Jazz",
                    DisplayOrder = 2
                },
                new Category
                {
                    Id = 3,
                    Name = "Classical",
                    DisplayOrder = 3
                },
                new Category
                {
                    Id = 4,
                    Name = "Hip hop",
                    DisplayOrder = 4
                });          

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Title = "Back in Black",
                    Author = "AC/DC",
                    Description = "A timeless rock album by Australian band AC / DC," +
                    "released in 1980.It features iconic tracks like 'Hells Bells', 'Back in Black', and 'You Shook Me All Night Long' showcasing the band's signature hard-hitting guitar riffs, powerful vocals, " +
                    "and energetic rhythm section. This album is considered one of the greatest rock records of all time, capturing the essence of AC/DC's high - voltage sound.",
                    ISBN = "SWD9999001",
                    ListPrice = 99,
                    Price = 90,
                    Price50 = 85,
                    Price100 = 80,
                    CategoryId = 1,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 2,
                    Title = "Nevermind",
                    Author = "Nirvana",
                    Description = "\"Nevermind\" is the breakthrough second studio album by American rock band Nirvana, released in 1991. " +
                    "It propelled the band and the grunge movement into the mainstream with its raw and honest sound.",
                    ISBN = "CAW777777701",
                    ListPrice = 40,
                    Price = 30,
                    Price50 = 25,
                    Price100 = 20,
                    CategoryId = 1,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 3,
                    Title = "Illmatic",
                    Author = "Nas",
                    Description = "\"Illmatic\" is the debut studio album by American rapper Nas, released in 1994. " +
                    "Considered one of the greatest hip hop albums of all time, it showcases Nas' lyrical prowess and storytelling ability over soulful and jazzy production",
                    ISBN = "RITO5555501",
                    ListPrice = 55,
                    Price = 50,
                    Price50 = 40,
                    Price100 = 35,
                    CategoryId = 4,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 4,
                    Title = "The Chronic",
                    Author = "Dr. Dre",
                    Description = "\"The Chronic\" is the debut solo album by American producer and rapper Dr. Dre, released in 1992.",
                    ISBN = "WS3333333301",
                    ListPrice = 70,
                    Price = 65,
                    Price50 = 60,
                    Price100 = 55,
                    CategoryId = 4,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 5,
                    Title = "Kind of Blue",
                    Author = "Ron Parker",
                    Description = "\"Kind of Blue\" is a seminal jazz album by American trumpeter Miles Davis, released in 1959. " +
                    "It is regarded as one of the greatest and most influential jazz recordings of all time.",
                    ISBN = "SOTJ1111111101",
                    ListPrice = 30,
                    Price = 27,
                    Price50 = 25,
                    Price100 = 20,
                    CategoryId = 2,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 6,
                    Title = "The Well-Tempered Clavier",
                    Author = "Johann Sebastian Bach",
                    Description = "\"The Well - Tempered Clavier\" is a collection of two sets of preludes and fugues composed by Johann Sebastian Bach. " +
                    "It is considered a cornerstone of classical keyboard music.",
                    ISBN = "FOT000000001",
                    ListPrice = 25,
                    Price = 23,
                    Price50 = 22,
                    Price100 = 20,
                    CategoryId = 3,
                    ImageUrl = ""
                });

            modelBuilder.Entity<Company>().HasData(
                new Company
                {
                    Id = 1,
                    Name = "Carturesti SRL",
                    Address = "Bulevardul General Paul Teodorescu 4", 
                    City = "București",
                    County = "București",
                    PostalCode = "061344",
                    PhoneNumber = "0723451234"
                },
                new Company
                {
                    Id = 2,
                    Name = "SC Library",
                    Address = "Str. Independentei 20",
                    City = "București",
                    County = "București",
                    PostalCode = "061324",
                    PhoneNumber = "0723321234"
                },
                new Company
                {
                    Id = 3,
                    Name = "Book Comp",
                    Address = "Bulevardul Vasile Milea 32",
                    City = "București",
                    County = "București",
                    PostalCode = "061321",
                    PhoneNumber = "0723481234"
                });
        }
        #endregion
    }
}