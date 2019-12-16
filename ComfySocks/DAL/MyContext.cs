using ComfySocks.Models.InventoryModel;
using ComfySocks.Models.PurchaseModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using ComfySocks.Models.Stock;

namespace ComfySocks.DAL
{
    public class MyContext:DbContext
    {
        public MyContext() : base("DefaultConnection")
        {}
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemType> ItemTypes { get; set; }
        public DbSet<StoreType> StoreTypes { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Purchases> Purchases{ get; set; }
        public DbSet<PurchaseItem> PurchaseItems { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<RowStock> RowStock { get; set; }


        //avoid pluralizing table names in database

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}