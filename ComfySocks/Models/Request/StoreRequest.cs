using ComfySocks.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComfySocks.Models.Request
{
    public partial class StoreRequest
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public float Quantity { get; set; }
        public string Remark { get; set; }
        public int StoreRequestInfoID { get; set; }
        public bool Deliverd { get; set; }
        //references
        public virtual Item Item { get; set; }
        public virtual StoreRequestInfo StoreRequestInfo { get; set; }
    }
    public partial class StoreRequestVM
    {

        public int ID { get; set; }
        public string ItemDescription { get; set; }
        public string Type { get; set; }
        public int Code { get; set; }
        public string Unit { get; set; }
        //referance 
        public virtual StoreRequest StoreRequest { get; set; }
    }

    public class StoreRequstVMForError
    {
        public StoreRequest StoreRequest { get; set; }

        public String   Error { get; set; }
    }
}