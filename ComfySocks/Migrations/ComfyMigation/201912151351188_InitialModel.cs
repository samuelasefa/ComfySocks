namespace ComfySocks.Migrations.ComfyMigation
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Item",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                        StoreTypeID = c.Int(nullable: false),
                        UnitID = c.Int(nullable: false),
                        Code = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.StoreType", t => t.StoreTypeID, cascadeDelete: true)
                .ForeignKey("dbo.Unit", t => t.UnitID, cascadeDelete: true)
                .Index(t => t.StoreTypeID)
                .Index(t => t.UnitID);
            
            CreateTable(
                "dbo.RowStock",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ItemID = c.Int(nullable: false),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Item", t => t.ItemID, cascadeDelete: true)
                .Index(t => t.ItemID);
            
            CreateTable(
                "dbo.StoreType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                        ItemTypeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ItemType", t => t.ItemTypeID, cascadeDelete: true)
                .Index(t => t.ItemTypeID);
            
            CreateTable(
                "dbo.ItemType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Discription = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Unit",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PurchaseItem",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PurchaseID = c.String(),
                        ItemID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Purchases_ID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Item", t => t.ItemID, cascadeDelete: true)
                .ForeignKey("dbo.Purchases", t => t.Purchases_ID)
                .Index(t => t.ItemID)
                .Index(t => t.Purchases_ID);
            
            CreateTable(
                "dbo.Purchases",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        Date = c.DateTime(nullable: false),
                        SupplierID = c.Int(nullable: false),
                        SupplierInvoiceNo = c.String(),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Vat = c.Decimal(precision: 18, scale: 2),
                        GrandTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Supplier", t => t.SupplierID, cascadeDelete: true)
                .Index(t => t.SupplierID);
            
            CreateTable(
                "dbo.Supplier",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        InvoiceNumber = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Purchases", "SupplierID", "dbo.Supplier");
            DropForeignKey("dbo.PurchaseItem", "Purchases_ID", "dbo.Purchases");
            DropForeignKey("dbo.PurchaseItem", "ItemID", "dbo.Item");
            DropForeignKey("dbo.Item", "UnitID", "dbo.Unit");
            DropForeignKey("dbo.StoreType", "ItemTypeID", "dbo.ItemType");
            DropForeignKey("dbo.Item", "StoreTypeID", "dbo.StoreType");
            DropForeignKey("dbo.RowStock", "ItemID", "dbo.Item");
            DropIndex("dbo.Purchases", new[] { "SupplierID" });
            DropIndex("dbo.PurchaseItem", new[] { "Purchases_ID" });
            DropIndex("dbo.PurchaseItem", new[] { "ItemID" });
            DropIndex("dbo.StoreType", new[] { "ItemTypeID" });
            DropIndex("dbo.RowStock", new[] { "ItemID" });
            DropIndex("dbo.Item", new[] { "UnitID" });
            DropIndex("dbo.Item", new[] { "StoreTypeID" });
            DropTable("dbo.Supplier");
            DropTable("dbo.Purchases");
            DropTable("dbo.PurchaseItem");
            DropTable("dbo.Unit");
            DropTable("dbo.ItemType");
            DropTable("dbo.StoreType");
            DropTable("dbo.RowStock");
            DropTable("dbo.Item");
        }
    }
}
