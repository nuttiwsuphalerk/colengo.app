using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace colengo.app.Server
{
    [Table("Brands")]
    public class Brand
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? UserIdentifier { get; set; }
    }
}
