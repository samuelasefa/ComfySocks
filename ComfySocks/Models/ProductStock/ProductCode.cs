using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComfySocks.Models.ProductStock
{

    public enum ProductSize{
        Small,
        Medium,
        Large
    }
    [Table("ProductCode")]
    public partial class ProductCode
    {
        public int ID { get; set; }

        [Display(Name ="Product Code")]
        public int Code { get; set; }

        [Display(Name ="Product Size")]
        public ProductSize ProductSize { get; set; }
        //reference
        public virtual ICollection<TempProductStock> TempProductStock { get; set; }
    }
}