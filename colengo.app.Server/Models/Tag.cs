using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace colengo.app.Server
{
    [Table("Tags")]
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? ParentName { get; set; }

        public string? UserIdentifier { get; set; }

        public string? CollectionName { get; set; }

        public int? CollectionId { get; set; }

        public string? ThumbnailImage { get; set; }
    }
}
