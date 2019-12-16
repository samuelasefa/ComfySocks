using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ComfySocks.Models.Stock;

namespace ComfySocks.Models.InventoryModel
{
    public class Item
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Item Description")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "StoreType")]
        public int? StoreTypeID { get; set; }

        [Required]
        [Display(Name = "OUM")]
        public int? UnitID { get; set; }

        [Required]
        [Display(Name = "Item Code")]
        public string Code { get; set; }

        //referance entity
        public virtual StoreType StoreType { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual ICollection<RowStock> RowStocks { get; set; }
    }

    //public enum StoreType{
    //    OfficeMateril,
    //    RowMateril,
    //    ProductMateril,
    //    MachineryMateril
    //}
}