using ComfySocks.Models.Items;
using ComfySocks.Models.ProductStock;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ComfySocks.Models.ProductStock
{
    public class TempProductStock
    {
        public int ID { get; set; }

        [Display(Name = "Type of Product")]
        public string ProductName { get; set; }

        public int ProductCodeID { get; set; }

        [Display(Name ="Unit")]
        public int UnitID { get; set; }

        [Required]
        public double Quantity { get; set; }

        public string ApplicationUserID { get; set; }

        //reference
        public virtual ProductCode ProductCode { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual ProductAvialableOnStock ProductAvialableOnStock { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}