using ComfySocks.Models.InventoryModel;
using ComfySocks.Models.Items;
using ComfySocks.Models.ProductInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComfySocks.Models.ProductStock
{
    public enum ProductSize {
        Small,
        Medium,
        Large
    }
    [Table("ProStock")]
    public partial class ProStock
    {
        public int ID { get; set; }
        [Required]
        public int ItemID { get; set; }

        public int ProStockInformationID { get; set; }

        [Required]
        public float Quantity { get; set; }
        [Required]
      
        public ProductSize Size { get; set; }

        public float Total { get; set; }
        //added
        public float ProwTotal { get; set; }

        public virtual ProStockInformation ProStockInformation { get; set; }
        public virtual Item Item { get; set; }
    }
    public class ProStockViewModel
    {
        public int ID { get; set; }
        public string ItemDescription { get; set; }
        public string Code { get; set; }
        public string Unit { get; set; }

        public virtual ProStock ProStock { get; set; }
    }

    public class ProStockVMForError {
        public ProStock ProStock { get; set; }

        public String Error { get; set; }
    }
}