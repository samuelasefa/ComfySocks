﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ComfySocks.Models.InventoryModel;
using ComfySocks.Models.Request;
using ComfySocks.Models.ProductInfo;
using ComfySocks.Repository;
using ComfySocks.Models.ProductStock;

namespace ComfySocks.Models.Items
{
    public enum StoreType {
        ProductMaterial,
        RowMaterial,
        OfficeMaterial,
        Product
    }
    [Table("Item")]
    public class Item
    {
        public int ID { get; set; }
        [Required]
        [Display(Name ="Item Description")]
        public string Name { get; set; }
        public StoreType StoreType { get; set; }
        public int ItemTypeID { get; set; }
        public int UnitID { get; set; }
        [Required]
        [Display(Name ="ItemCode")]
        public string Code { get; set; }
        public string ApplicationUserID { get; set; }


        //referance
        public virtual RowMaterialRepositery RowMaterialRepositery { get; set; }
        public virtual AvaliableOnStock AvaliableOnStocks { get; set; }
        public virtual ProductAvialableOnStock ProductAvialableOnStock { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ItemType ItemType { get; set; }
        public virtual Unit Unit { get; set; }

        //Collection
        public virtual ICollection<TempProductStock> TempProductStocks { get; set; }
        public virtual ICollection<Stock> Stocks { get; set; }
        public virtual ICollection<StoreRequest> StoreRequest { get; set; }
    }
}