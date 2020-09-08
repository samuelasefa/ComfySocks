using ComfySocks.Models.OfficeRequest;
using ComfySocks.Models.Request;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComfySocks.Models.OfficeDeliveryInfo
{
    [Table("OfficeDeliveryInformation")]
    public partial class OfficeDeliveryInformation
    {

        public int ID { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name ="No")]
        public string OfficeDeliveryNumber { get; set; }

        [Display(Name ="Section/Department")]
        public string Section { get; set; }

        [Display(Name ="Requestion No")]
        public int OfficeMaterialRequestInformationID { get; set; }
      
        [Display(Name = "Received by")]
        public string Receivedby { get; set; }
        [Display(Name = "Approved by")]
        public string ApprovedBy { get; set; }

        [Display(Name ="Issued by")]
        public string Issuedby { get; set; }
        public string ApplictionUserID { get; set; }

        public string Status { get; set; }
        //referance
        public virtual ICollection<OfficeDelivery> OfficeDeliveries { get; set; }
        public virtual OfficeMaterialRequestInformation OfficeMaterialRequestInformation { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }



    }
}