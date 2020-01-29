using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComfySocks.Models.InventoryModel
{
    [Table("Supplier")]
    public partial class Supplier
    {
        public int ID { get; set; }
        [Required]
        [Display(Name ="Supplier's Name")]
        public string Name { get; set; }
        [Display(Name ="Supplier's Invoice No.")]
        public string No { get; set; }

        //referance
        public virtual ICollection<StockInformation> StockInformation { get; set; }


    }
}