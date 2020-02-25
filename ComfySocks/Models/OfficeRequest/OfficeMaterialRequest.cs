using ComfySocks.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComfySocks.Models.OfficeRequest
{
        public partial class OfficeMaterialRequest
        {
            public int ID { get; set; }
            public int OfficeMaterialRequestInformationID { get; set; }
            public int ItemID { get; set; }
            public float Quantity { get; set; }
            public string Remark { get; set; }
            public bool Deliverd { get; set; }


            public float RemaningDelivery { get; set; }
            //references
            public virtual Item Item { get; set; }
            public virtual OfficeMaterialRequestInformation OfficeMaterialRequestInformation { get; set; }
        }
        public partial class OfficeMaterialRequestVM
        {
            public int ID { get; set; }
            public string ItemDescription { get; set; }
            public string Type { get; set; }
            public string Code { get; set; }
            public string Unit { get; set; }
            public string Remark { get; set; }
            //referance 
            public virtual OfficeMaterialRequest OfficeMaterialRequest { get; set; }
        }

        public class OfficeMaterialRequstVMForError
        {
            public OfficeMaterialRequest OfficeMaterialRequest { get; set; }
            public String Error { get; set; }
        }
}