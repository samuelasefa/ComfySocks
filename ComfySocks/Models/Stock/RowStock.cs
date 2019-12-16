using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ComfySocks.Models.InventoryModel;

namespace ComfySocks.Models.Stock
{
    public class RowStock
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        //reference to Item
        public virtual Item Item { get; set; }
    }
}