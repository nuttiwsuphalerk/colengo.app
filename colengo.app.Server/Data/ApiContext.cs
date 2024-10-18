
using Microsoft.EntityFrameworkCore;

namespace colengo.app.Server.Data
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } = null!;

        public DbSet<Brand> Brands { get; set; } = null!;

        public DbSet<Category> Categories { get; set; } = null!;

        public DbSet<ProductCategory> ProductCategories { get; set; } = null!;

        public DbSet<ProductImage> ProductImages { get; set; } = null!;

        public DbSet<ProductImageTaxonomyTag> ProductImageTaxonomyTags { get; set; } = null!;

        public DbSet<ProductPrice> ProductPrices { get; set; } = null!;

        public DbSet<ProductTag> ProductTags { get; set; } = null!;

        public DbSet<Tag> Tags { get; set; } = null!;

    }
}
