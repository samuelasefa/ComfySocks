﻿using ComfySocks.Models.Items;
using ComfySocks.Models.ProductStock;
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
        public int TempProductStockID { get; set; }
        public int SalesInformationID { get; set; }
        public float Quantity { get; set; }
        public float UnitPrice { get; set; }

        public float RemaningDelivery { get; set; }

        public int ApplicationUserID { get; set; }
        //reference
        public virtual ApplicationUser ApplicationUser{ get; set; }
        public virtual TempProductStock TempProductStock { get; set; }
        public virtual SalesInformation SalesInformation { get; set; }
    }

    public class SalesVMForError
    {
        public Sales Sales { get; set; }

        public String Error { get; set; }
    }
}