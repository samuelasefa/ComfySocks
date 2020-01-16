using ComfySocks.Models.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComfySocks.Repository
{
    [Table("RowMaterialRepositery")]
    public partial class RowMaterialRepositery
    {
        [ForeignKey("Item")]
        public int ID { get; set; }

        //reference
        public virtual Item Item { get; set; }

        public float RowMaterialAavliable { get; set; }
        public float RecentlyReducedRowMaterialAvaliable { get; set; }
    }
}