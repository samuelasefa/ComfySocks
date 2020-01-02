using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ComfySocks.Models.InventoryModel;

namespace ComfySocks.Models.Items
{
    [Table("Item")]
    public partial class Item
    {
        public int ID { get; set; }
        [Required]
        [Display(Name ="Item Description")]
        public string Name { get; set; }
        public int StoreTypeID { get; set; }
        public int ItemTypeID { get; set; }
        public int UnitID { get; set; }
        public int Code { get; set; }
        public string ApplicationUserID { get; set; }
        //referance
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ItemType ItemType { get; set; }
        public virtual StoreType StoreType { get; set; }
        public virtual Unit Unit { get; set; }

        public virtual AvaliableOnStock AvaliableOnStock { get; set; }

        public virtual ICollection<Stock> Stocks { get; set; }
    }
}