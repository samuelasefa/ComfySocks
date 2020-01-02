namespace ComfySocks.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IntialModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AvaliableOnStocks",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Avaliable = c.Single(nullable: false),
                        RecentlyReduced = c.Single(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.Item", t => t.ID)
                .Index(t => t.ID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Item",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        StoreTypeID = c.Int(nullable: false),
                        ItemTypeID = c.Int(nullable: false),
                        UnitID = c.Int(nullable: false),
                        Code = c.Int(nullable: false),
                        ApplicationUserID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.ItemType", t => t.ItemTypeID, cascadeDelete: true)
                .ForeignKey("dbo.StoreType", t => t.StoreTypeID, cascadeDelete: true)
                .ForeignKey("dbo.Unit", t => t.UnitID, cascadeDelete: true)
                .Index(t => t.StoreTypeID)
                .Index(t => t.ItemTypeID)
                .Index(t => t.UnitID)
                .Index(t => t.ApplicationUserID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ItemType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ApplicationUserID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .Index(t => t.ApplicationUserID);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.StoreRequestInfo",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        StoreRequestNumber = c.String(),
                        RequestedBy = c.String(nullable: false),
                        ApprovedBy = c.String(nullable: false),
                        ApplicationUserID = c.String(maxLength: 128),
                        StoreID = c.String(),
                        Status = c.String(),
                        Store_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.Store", t => t.Store_ID)
                .Index(t => t.ApplicationUserID)
                .Index(t => t.Store_ID);
            
            CreateTable(
                "dbo.Store",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Location = c.String(),
                        ApplicationUserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .Index(t => t.ApplicationUserID);
            
            CreateTable(
                "dbo.Stock",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ItemID = c.Int(nullable: false),
                        StockReferanceID = c.Int(nullable: false),
                        Quantity = c.Single(nullable: false),
                        StoreID = c.Int(nullable: false),
                        Total = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Item", t => t.ItemID, cascadeDelete: true)
                .ForeignKey("dbo.StockReferance", t => t.StockReferanceID, cascadeDelete: true)
                .ForeignKey("dbo.Store", t => t.StoreID, cascadeDelete: true)
                .Index(t => t.ItemID)
                .Index(t => t.StockReferanceID)
                .Index(t => t.StoreID);
            
            CreateTable(
                "dbo.StockReferance",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        StoreNumber = c.String(),
                        SupplierID = c.Int(nullable: false),
                        Invoice = c.String(),
                        ApplicationUserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.Supplier", t => t.SupplierID, cascadeDelete: true)
                .Index(t => t.SupplierID)
                .Index(t => t.ApplicationUserID);
            
            CreateTable(
                "dbo.Supplier",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        No = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.StoreRequests",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ItemID = c.Int(nullable: false),
                        Quantity = c.Single(nullable: false),
                        Remark = c.String(),
                        StoreRequestInfoID = c.Int(nullable: false),
                        Deliverd = c.Boolean(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Item", t => t.ItemID, cascadeDelete: true)
                .ForeignKey("dbo.StoreRequestInfo", t => t.StoreRequestInfoID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ItemID)
                .Index(t => t.StoreRequestInfoID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.StoreType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ApplicationUserID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .Index(t => t.ApplicationUserID);
            
            CreateTable(
                "dbo.Unit",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ApplicationUserID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .Index(t => t.ApplicationUserID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AvaliableOnStocks", "ID", "dbo.Item");
            DropForeignKey("dbo.Unit", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Item", "UnitID", "dbo.Unit");
            DropForeignKey("dbo.StoreType", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Item", "StoreTypeID", "dbo.StoreType");
            DropForeignKey("dbo.StoreRequests", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.StoreRequests", "StoreRequestInfoID", "dbo.StoreRequestInfo");
            DropForeignKey("dbo.StoreRequests", "ItemID", "dbo.Item");
            DropForeignKey("dbo.StoreRequestInfo", "Store_ID", "dbo.Store");
            DropForeignKey("dbo.Stock", "StoreID", "dbo.Store");
            DropForeignKey("dbo.StockReferance", "SupplierID", "dbo.Supplier");
            DropForeignKey("dbo.Stock", "StockReferanceID", "dbo.StockReferance");
            DropForeignKey("dbo.StockReferance", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Stock", "ItemID", "dbo.Item");
            DropForeignKey("dbo.Store", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.StoreRequestInfo", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ItemType", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Item", "ItemTypeID", "dbo.ItemType");
            DropForeignKey("dbo.Item", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AvaliableOnStocks", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Unit", new[] { "ApplicationUserID" });
            DropIndex("dbo.StoreType", new[] { "ApplicationUserID" });
            DropIndex("dbo.StoreRequests", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.StoreRequests", new[] { "StoreRequestInfoID" });
            DropIndex("dbo.StoreRequests", new[] { "ItemID" });
            DropIndex("dbo.StockReferance", new[] { "ApplicationUserID" });
            DropIndex("dbo.StockReferance", new[] { "SupplierID" });
            DropIndex("dbo.Stock", new[] { "StoreID" });
            DropIndex("dbo.Stock", new[] { "StockReferanceID" });
            DropIndex("dbo.Stock", new[] { "ItemID" });
            DropIndex("dbo.Store", new[] { "ApplicationUserID" });
            DropIndex("dbo.StoreRequestInfo", new[] { "Store_ID" });
            DropIndex("dbo.StoreRequestInfo", new[] { "ApplicationUserID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.ItemType", new[] { "ApplicationUserID" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Item", new[] { "ApplicationUserID" });
            DropIndex("dbo.Item", new[] { "UnitID" });
            DropIndex("dbo.Item", new[] { "ItemTypeID" });
            DropIndex("dbo.Item", new[] { "StoreTypeID" });
            DropIndex("dbo.AvaliableOnStocks", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AvaliableOnStocks", new[] { "ID" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Unit");
            DropTable("dbo.StoreType");
            DropTable("dbo.StoreRequests");
            DropTable("dbo.Supplier");
            DropTable("dbo.StockReferance");
            DropTable("dbo.Stock");
            DropTable("dbo.Store");
            DropTable("dbo.StoreRequestInfo");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.ItemType");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Item");
            DropTable("dbo.AvaliableOnStocks");
        }
    }
}
