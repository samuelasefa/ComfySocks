using ComfySocks.Models.Request;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComfySocks.Models.RowDeliveryInfo
{
    [Table("RowDelivery")]
    public partial class RowDelivery
    {
        public int ID { get; set; }
        [Display(Name="Requestion No")]
        [Required]
        public int StoreRequestID { get; set; }
        public string ItemCode { get; set; }
        public double DeliveryQuantity { get; set; }

        public string Remark { get; set; }
        public int RowDeliveryInformationID { get; set; }

        public virtual RowDeliveryInformation RowDeliveryInformation { get; set; }
        public virtual StoreRequest StoreRequest { get; set; }
    }
    public partial class RowDeliveryVM 
    {
        public RowDelivery RowDelivery { get; set; }
        public string ItemDescription { get; set; }
        public string ItemType { get; set; }
        public string Code { get; set; }
        public string Unit { get; set; }

        public decimal UnitPrice { get; set; }
        public float Remaining { get; set; }
    }
}