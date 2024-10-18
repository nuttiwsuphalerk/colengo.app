using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace colengo.app.Server
{
    [Table("ProductPrices")]
    public class ProductPrice
    {
        [IgnoreDataMember]
        [Key]
        public int ProductId { get; set; }

        public string? PriceCurrency { get; set; }

        public decimal? PriceAmount { get; set; }

        public string? OriginalPriceCurrency { get; set; }

        public decimal? OriginalPriceAmount { get; set; }

        public string? FullPriceBeforeOverallDiscountCurrency { get; set; }

        public decimal? FullPriceBeforeOverallDiscountAmount { get; set; }

        public string? PossibleDiscountPriceCurrency { get; set; }

        public decimal? PossibleDiscountPriceAmount { get; set; }
    }
}
