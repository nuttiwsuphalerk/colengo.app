using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace colengo.app.Server
{
    [Table("ProductTags")]
    public class ProductTag
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int TagId { get; set; }
    }
}
