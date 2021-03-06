﻿using ComfySocks.Models.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComfySocks.Models.SalesInfo
{
    [Table("Sales")]
    public partial class Sales
    {
        public int ID { get; set; }
        [Display(Name ="Item")]
        public int ItemID { get; set; }
        public string ProductCode { get; set; }

        public int SalesInformationID { get; set; }
        public float Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public float RemaningDelivery { get; set; }

        public string ApplicationUserID { get; set; }
        //reference
        public virtual ApplicationUser ApplicationUser{ get; set; }
        public virtual Item Item { get; set; }
        public virtual SalesInformation SalesInformation { get; set; }
        //
    }
    public class SalesVM
    {
        public int ID { get; set; }
        public string TypeOfProduct { get; set; }
        public string Code { get; set; }
        public string Unit { get; set; }

        public Sales Sales { get; set; }
    }
    public class SalesVMForError
    {
        public Sales Sales { get; set; }

        public String Error { get; set; }
    }
}