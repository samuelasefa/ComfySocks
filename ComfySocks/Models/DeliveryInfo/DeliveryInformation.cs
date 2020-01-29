using ComfySocks.Models.Request;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComfySocks.Models.DeliveryInfo
{
    [Table("DeliveryInformation")]
    public partial class DeliveryInformation
    {

        public int ID { get; set; }
        [DisplayFormat(DataFormatString = "{0:mm-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name ="De-No")]
        public string DeliveryNumber { get; set; }

        [Display(Name ="Section/Department")]
        public string From { get; set; }

        [Display(Name ="Requestion No")]
        public int StoreRequestInfoID  { get; set; }
        
        [Display(Name = "Approved by")]
        public string Approvedby { get; set; }

        [Display(Name ="Rejected By")]
        public string Rejectedby { get; set; }

        [Display(Name = "Received by")]
        public string Receivedby { get; set; }

        [Display(Name ="Issued by")]
        public string ApplictionUserID { get; set; }

        public string Status { get; set; }
        //referance
        public virtual ICollection<Delivery> Deliveries { get; set; }
        public virtual StoreRequestInformation StoreRequestInformation { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }



    }
}