﻿using ComfySocks.Models.Items;
using ComfySocks.Models.ProductTransferInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComfySocks.Models.ProductInfo
{
    [Table("Product")]
    public partial class Product
    {
        public int ID { get; set; }
        [Required]
        public int ItemID { get; set; }
        public int ProductInformationID { get; set; }

        public int ProductCode { get; set; }
        [Required]
        public float Quantity { get; set; }
        public string Remark { get; set; }
        //logical product total
        public float Total { get; set; }

        //Physical product total
        public float PPTotal { get; set; }

        public virtual ProductInformation ProductInformation { get; set; }
        public virtual Item Item { get; set; }
    }
    public class ProductViewModel
    {
        public string TypeOfProduct { get; set; }
        public string Code { get; set; }
        public string Unit { get; set; }

        public virtual Product Product { get; set; }
    }

    public class ProductVMForError
    {
        public Product Product { get; set; }

        public String Error { get; set; }
    }
}