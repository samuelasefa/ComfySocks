using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComfySocks.Models.Items
{
    [Table("ItemType")]
    public partial class ItemType
    {
        public int ID  { get; set; }
        [Required]
        [Display(Name ="Item Type")]
        public string Name { get; set; }
        public string ApplicationUserID { get; set; }
        //ref
        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<Item> Items { get; set; }

    }
}