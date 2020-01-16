using ComfySocks.Models.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComfySocks.Models.InventoryModel
{
    public class AvaliableOnStock
    {
        [ForeignKey("Item")]
        public int ID { get; set; }

        public virtual Item Item { get; set; }

        public float Avaliable { get; set; }
        public float RecentlyReduced { get; set; }
    }
}