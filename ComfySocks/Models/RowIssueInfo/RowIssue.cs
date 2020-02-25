using ComfySocks.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ComfySocks.Models.RowIssueInfo
{
    public partial class RowIssue
    {
        public int ID { get; set; }
       
        public int RowIssueInformationID { get; set; }
        [Required]
        [Display(Name = "Item")]
        public int ItemID { get; set; }
        
        public float Quantity { get; set; }
        public string Remark { get; set; }

        //references
        public virtual Item Item { get; set; }
        public virtual RowIssueInformation RowIssueInformation { get; set; }
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
        public virtual RowIssue RowIssue { get; set; }
    }

    public class RowIssueVMForError
    {
        public RowIssue RowIssue { get; set; }
        public String   Error { get; set; }
    }
}