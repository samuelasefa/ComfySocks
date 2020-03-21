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
    [Table("Transfer")]
    public partial class Transfer
    {
        public int ID { get; set; }

        [Display(Name ="PT-ID")]
        public int TransferInformationID { get; set; }

        public int ItemID { get; set; }
        public string ProductCode { get; set; }

        public float Quantity{ get; set; }
        public string Remark { get; set; }

        public float Total { get; set; }
        public float PPT { get; set; }
        //reference
        public virtual Item Item { get; set; }
        public virtual TransferInformation TransferInformation { get; set; }
    }

    public class TransferVM
    {
        public string TypeOfProduct { get; set; }
        public string Code { get; set; }
        public string Unit { get; set; }

        public Transfer Transfer { get; set; }
    }

    public class TransferViewModelForError
    {
        public Transfer Transfer { get; set; }

        public String Error { get; set; }
    }
    
}