using ComfySocks.Models.InventoryModel;
using System.ComponentModel.DataAnnotations;

namespace ComfySocks.Models.PurchaseModel
{
    public class PurchaseItem
    {
        public int ID { get; set; }
        public string PurchaseID { get; set; }
        [Display(Name ="Item")]
        public int ItemID { get; set; }
        [Range(0,1000000, ErrorMessage ="Not an acceptable Quantity")]
        public int Quantity { get; set; }

        [Range(0.00, 99999999.99)]
        public decimal UnitPrice { get; set; }
        [Range(0.00, 99999999.99)]
        public decimal TotalPrice { get; set; }

        //referance to Item and Purchase
        public virtual Item Item { get; set; }
        public virtual Purchases Purchases{ get; set; }

    }
}