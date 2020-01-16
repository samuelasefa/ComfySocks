using ComfySocks.Models.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ComfySocks.Models.InventoryModel;
using ComfySocks.Models.Order;

namespace ComfySocks.Models.ProductTransferInfo
{
    [Table("ProductTransfer")]
    public partial class Transfer
    {
        public int ID { get; set; }

        [Display(Name ="Type of Product")]
        public int TempProductStockID { get; set; }

        [Display(Name ="PT-ID")]
        public int TransferInformationID { get; set; }

        public float Quantity{ get; set; }
        public string Remark { get; set; }


        //reference
        //public virtual TempProductStock TempProductStock { get; set; }
        public virtual ProductionOrder ProductionOrder { get; set; }
        public virtual TransferInformation TransferInformation { get; set; }
    }
}