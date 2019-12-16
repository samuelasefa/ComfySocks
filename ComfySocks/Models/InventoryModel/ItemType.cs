using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComfySocks.Models.InventoryModel
{
    public class ItemType
    {
        public int ID { get; set; }
        [Required]
        [Display(Name ="ItemType")]
        public string Discription { get; set; }

        public virtual ICollection<StoreType> StoreTypes { get; set; }
    }
}