using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace colengo.app.Server
{
    [Table("ProductImageTaxonomyTags")]
    public class ProductImageTaxonomyTag
    {
        [Key]
        [JsonIgnore]
        public int ProductId { get; set; }

        [JsonIgnore]
        public string Taxonomy { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Alt { get; set; }

        public string? Original { get; set; }

        public string? Large { get; set; }

        public string? MediumLarge { get; set; }

        public string? Medium { get; set; }

        public string? MediumSmall { get; set; }

        public string? Small { get; set; }

        public string? Thumbnail { get; set; }

        public string? SmallThumbnail { get; set; }

        [NotMapped]
        public dynamic? Preset { get; set; }
    }
}
