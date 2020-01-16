using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComfySocks.Models.ProductStock
{
    [Table("ProductAvialableOnStock")]
    public partial class ProductAvialableOnStock
    {
        [ForeignKey("TempProductStock")]
        public int ID { get; set; }

        public TempProductStock TempProductStock { get; set; }

        public float ProductAvaliable { get; set; }
        public float RecentlyReducedProduct { get; set; }
    }
}