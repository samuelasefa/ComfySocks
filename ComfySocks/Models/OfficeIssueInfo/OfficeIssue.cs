using ComfySocks.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ComfySocks.Models.OfficeIssueInfo
{
    public partial class OfficeIssue
    {
        public int ID { get; set; }
       
        public int OfficeIssueInformationID { get; set; }
        [Required]
        [Display(Name = "Item")]
        public int ItemID { get; set; }
        
        public float Quantity { get; set; }
        public string Remark { get; set; }

        //references
        public virtual Item Item { get; set; }
        public virtual OfficeIssueInformation OfficeIssueInformation { get; set; }
    }
    public partial class RowIssueVM
    {
        public string ItemDescription { get; set; }
        public string ItemType { get; set; }
        public string ItemCode { get; set; }
        public string Unit { get; set; }
        public float UnitPrice { get; set; }
        public string Remark { get; set; }
        //referance 
        public virtual OfficeIssue OfficeIssue { get; set; }
    }

    public class RowIssueVMForError
    {
        public OfficeIssue OfficeIssue { get; set; }
        public String   Error { get; set; }
    }
}