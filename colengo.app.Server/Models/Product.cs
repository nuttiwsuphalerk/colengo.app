using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace colengo.app.Server
{
    [Table("Products")]
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        public string? Name { get; set; }

        public string? Title { get; set; }

        public string? ThumbnailImage { get; set; }

        [NotMapped]
        public ProductPriceModel? Price { get; set; }

        [NotMapped]
        public ProductPriceModel? OriginalPrice { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public int? BrandId { get; set; }

        [NotMapped]
        public Brand? Brand { get; set; }

        public int? Sold { get; set; }

        public bool? AllowMultipleConfigs { get; set; }

        public string? Url { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? OverallCampaignEndDate { get; set; }

        public decimal? ReviewScore { get; set; }

        public int? ReviewCount { get; set; }

        public bool? Has3DAssets { get; set; }

        [NotMapped]
        public ProductPriceModel? FullPriceBeforeOverallDiscount { get; set; }

        [NotMapped]
        public ProductPriceModel? PossibleDiscountPrice { get; set; }

        public string? Layout { get; set; }

        public string? Location { get; set; }

        [NotMapped]
        public List<Category>? Categories { get; set; }

        [NotMapped]
        public List<Tag>? Tags { get; set; }

        [NotMapped]
        public List<ProductImage>? Images { get; set; }

        [NotMapped]
        public Dictionary<string, List<ProductImageTaxonomyTag>>? ImageTaxonomyTags { get; set; }
    }

    public class ProductPriceModel
    {
        public string? Currency { get; set; }

        public decimal? Amount { get; set; }
    }
}
