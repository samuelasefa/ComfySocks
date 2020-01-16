using ComfySocks.Models.ProductStock;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComfySocks.Models.Repository
{
    public class ProductMaterialRepository
    {
        [ForeignKey("TempProductStock")]
        public int ID { get; set; }

        //reference
        public virtual TempProductStock TempProductStock { get; set; }

        public float ProductMaterialAavliable { get; set; }
        public float RecentlyReducedProductMaterialAvaliable { get; set; }
    }
}