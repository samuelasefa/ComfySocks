using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComfySocks.Models.InventoryModel
{
    public class Unit
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Unit")]
        public string Description { get; set; }

        //relation ship to ItemType
        public virtual ICollection<Item> Items { get; set; }
    }
}
