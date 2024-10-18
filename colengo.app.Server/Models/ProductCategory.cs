using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace colengo.app.Server
{
    [Table("ProductCategories")]
    public class ProductCategory
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int CategoryId { get; set; }
    }
}
