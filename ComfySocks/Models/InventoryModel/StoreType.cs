using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComfySocks.Models.InventoryModel
{
    public class StoreType
    {
        public int ID { get; set; }
        [Required]
        [Display(Name ="StoreType")]
        public string Description { get; set; }
        [Required]
        [Display(Name ="ItemType")]
        public int ItemTypeID { get; set; }

        //reference to ItemType
        public ItemType ItemType { get; set; }
        public virtual ICollection<Item> Items { get; set; }


        //enum
      
    }
}