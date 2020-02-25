using ComfySocks.Models.Items;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComfySocks.Models.ProductStock
{
    [Table("ProductAvialableOnStock")]
    public partial class ProductAvialableOnStock
    {
        [ForeignKey("Item")]
        public int ID { get; set; }

        public virtual Item Item { get; set; }

        public float ProductAvaliable { get; set; }
        public float RecentlyReducedProduct { get; set; }

        //reference
    }
}