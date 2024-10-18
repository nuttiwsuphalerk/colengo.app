using System.ComponentModel.DataAnnotations.Schema;

namespace colengo.app.Server
{
    public class ProductMasterPaging
    {
        public int TotalPages { get; set; }

        public int TotalItems { get; set; }

        public int PageSize { get; set; }

        public List<Product> Products { get; set; }
    }

    public class ProductModelPaging
    {
        public int total { get; set; }

        public dynamic? products { get; set; }
    }
}
