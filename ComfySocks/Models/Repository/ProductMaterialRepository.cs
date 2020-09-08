using ComfySocks.Models.Items;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComfySocks.Models.Repository
{
    public class ProductMaterialRepository
    {
        [ForeignKey("Item")]
        public int ID { get; set; }

        //reference
        public virtual Item Item { get; set; }
        [Display(Name ="Avaliable Quantity")]
        public float ProductMaterialAavliable { get; set; }
        public float RecentlyReducedProductMaterialAvaliable { get; set; }

        //reference
    }
}