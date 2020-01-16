using ComfySocks.Models.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComfySocks.Models.Issue
{
    [Table("StoreIssue")]
    public partial class StoreIssue
    {
        public int ID { get; set; }
        [Required(ErrorMessage ="Item ID is required")]
        [Display(Name ="Item")]
        public int ItemID { get; set; }
        
        public int StoreIssueInfoID { get; set; }

        [Required(ErrorMessage ="please enter the quantity")]
        public double Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double TotalPrice { get; set; }

       
        public float RemaningDelivery { get; set; }
        //reference
        public virtual Item Items { get; set; }
        public virtual StoreIssueInfo StoreIssueInfo { get; set; }
    }
}