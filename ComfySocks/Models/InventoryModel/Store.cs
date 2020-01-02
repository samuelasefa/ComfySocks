using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComfySocks.Models.InventoryModel
{
    [Table("Store")]
    public partial class Store
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Store Name")]
        public string Name { get; set; }

        [Display(Name = "Store Loction")]
        public string Location { get; set; }

        public string ApplicationUserID { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Stock> Stocks { get; set; }
    }
}