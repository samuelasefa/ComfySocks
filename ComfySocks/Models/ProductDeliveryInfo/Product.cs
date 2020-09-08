using ComfySocks.Models.ProductTransferInfo;
using ComfySocks.Models.Request;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComfySocks.Models.ProductRecivingInfo
{
    [Table("Product")]
    public partial class Product
    {
        public int ID { get; set; }
        [Required]
        public int TransferID { get; set; }
        public string ItemCode { get; set; }
        public float ProductQuantity { get; set; }

        public string Remark { get; set; }
        public int ProductInformationID { get; set; }

        public virtual ProductInformation ProductInformation { get; set; }
        public virtual Transfer Transfer { get; set; }
    }
    public partial class ProductVM 
    {
        public Product Product { get; set; }
        public string TypeOfProduct { get; set; }
        public string BatchNo { get; set; }
        public string Code { get; set; }
        public string Unit { get; set; }
        public string Remark { get; set; }
        public decimal UnitPrice { get; set; }
        public float Remaining { get; set; }
    }
}