using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComfySocks.Models.Items
{
    [Table("StoreType")]
    public partial class StoreType
    {
        public int ID { get; set; }
        [Required]
        [Display(Name ="Stock Name")]
        public string Name { get; set; }
        public string ApplicationUserID { get; set; }
        //REFRANCE
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Item> Items { get; set; }

    }
}