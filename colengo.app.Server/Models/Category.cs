using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace colengo.app.Server
{
    [Table("Categories")]
    public class Category
    {
        [Key]
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string? Name { get; set; }

        public string? Title { get; set; }
    }
}
