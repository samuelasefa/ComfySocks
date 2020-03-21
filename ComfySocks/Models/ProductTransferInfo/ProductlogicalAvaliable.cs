using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ComfySocks.Models.Items;

namespace ComfySocks.Models.ProductTransferInfo
{
    [Table("ProductlogicalAvaliable")]
    public partial class ProductlogicalAvaliable
    {
        [ForeignKey("Item")]
        public int ID { get; set; }

        //referance
        public virtual Item Item { get; set; }
        public float LogicalProductAvaliable { get; set; }
        public float RecentlyReduced { get; set; }
    }
}