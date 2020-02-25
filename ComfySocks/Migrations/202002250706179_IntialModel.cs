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
                        StoreType = c.Int(nullable: false),
                        ItemTypeID = c.Int(nullable: false),
                        UnitID = c.Int(nullable: false),
                        Code = c.String(nullable: false),
                        ApplicationUserID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.ItemType", t => t.ItemTypeID)
                .ForeignKey("dbo.Unit", t => t.UnitID)
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
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
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
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.OfficeIssueInformations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        OfficeIssueNumber = c.String(),
                        ApprovedBy = c.String(nullable: false),
                        From = c.String(),
                        ApplicationUserID = c.String(maxLength: 128),
                        StoreID = c.Int(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.Store", t => t.StoreID)
                .Index(t => t.ApplicationUserID)
                .Index(t => t.StoreID);
            
            CreateTable(
                "dbo.OfficeIssues",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OfficeIssueInformationID = c.Int(nullable: false),
                        ItemID = c.Int(nullable: false),
                        Quantity = c.Single(nullable: false),
                        Remark = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Item", t => t.ItemID)
                .ForeignKey("dbo.OfficeIssueInformations", t => t.OfficeIssueInformationID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.OfficeIssueInformationID)
                .Index(t => t.ItemID)
                .Index(t => t.ApplicationUser_Id);
            
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
                "dbo.ProductInformation",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        ProductNumber = c.String(),
                        StoreID = c.Int(nullable: false),
                        ApplicationUserID = c.String(maxLength: 128),
                        Deliverdby = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.Store", t => t.StoreID)
                .Index(t => t.StoreID)
                .Index(t => t.ApplicationUserID);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TransferProductID = c.Int(nullable: false),
                        ProductInformationID = c.Int(nullable: false),
                        Quantity = c.Single(nullable: false),
                        Remark = c.String(),
                        Total = c.Single(nullable: false),
                        PPTotal = c.Single(nullable: false),
                        Transfer_ID = c.Int(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ProductInformation", t => t.ProductInformationID)
                .ForeignKey("dbo.Transfer", t => t.Transfer_ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ProductInformationID)
                .Index(t => t.Transfer_ID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Transfer",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TransferInformationID = c.Int(nullable: false),
                        ItemID = c.Int(nullable: false),
                        Quantity = c.Single(nullable: false),
                        Remark = c.String(),
                        Total = c.Single(nullable: false),
                        PPT = c.Single(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Item", t => t.ItemID)
                .ForeignKey("dbo.TransferInformation", t => t.TransferInformationID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.TransferInformationID)
                .Index(t => t.ItemID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.TransferInformation",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        From = c.String(),
                        TempProductStockInfoID = c.Int(nullable: false),
                        FPTNo = c.String(),
                        StoreID = c.Int(nullable: false),
                        Recivedby = c.String(),
                        Approvedby = c.String(),
                        ApplicationUserID = c.String(nullable: false, maxLength: 128),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Store", t => t.StoreID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .Index(t => t.StoreID)
                .Index(t => t.ApplicationUserID);
            
            CreateTable(
                "dbo.PurchaseInformations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        PRNo = c.String(),
                        To = c.String(),
                        StoreID = c.Int(nullable: false),
                        StoreName = c.String(),
                        ItemType = c.String(),
                        Status = c.String(),
                        isNormal = c.Boolean(nullable: false),
                        isUrgent = c.Boolean(nullable: false),
                        isVeryUrgent = c.Boolean(nullable: false),
                        ApplicationUserID = c.String(maxLength: 128),
                        Checkedby = c.String(),
                        Approvedby = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.Store", t => t.StoreID)
                .Index(t => t.StoreID)
                .Index(t => t.ApplicationUserID);
            
            CreateTable(
                "dbo.Purchases",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ItemID = c.Int(nullable: false),
                        PurchaseInformtionID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        SRNo = c.String(),
                        Remark = c.String(),
                        PurchaseInformation_ID = c.Int(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Item", t => t.ItemID)
                .ForeignKey("dbo.PurchaseInformations", t => t.PurchaseInformation_ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ItemID)
                .Index(t => t.PurchaseInformation_ID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Stock",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ItemID = c.Int(nullable: false),
                        StockInformationID = c.Int(nullable: false),
                        UnitPrice = c.Single(nullable: false),
                        Quantity = c.Single(nullable: false),
                        StoreID = c.Int(nullable: false),
                        Total = c.Single(nullable: false),
                        ProwTotal = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Item", t => t.ItemID)
                .ForeignKey("dbo.StockInformation", t => t.StockInformationID)
                .ForeignKey("dbo.Store", t => t.StoreID)
                .Index(t => t.ItemID)
                .Index(t => t.StockInformationID)
                .Index(t => t.StoreID);
            
            CreateTable(
                "dbo.StockInformation",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        StoreNumber = c.String(),
                        SupplierID = c.Int(nullable: false),
                        ApplicationUserID = c.String(maxLength: 128),
                        Reciviedby = c.String(),
                        Approvedby = c.String(),
                        SubTotal = c.Single(nullable: false),
                        Tax = c.Single(nullable: false),
                        GrandTotal = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.Supplier", t => t.SupplierID)
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
                "dbo.OfficeMaterialRequests",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OfficeMaterialRequestInformationID = c.Int(nullable: false),
                        ItemID = c.Int(nullable: false),
                        Quantity = c.Single(nullable: false),
                        Remark = c.String(),
                        Deliverd = c.Boolean(nullable: false),
                        RemaningDelivery = c.Single(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Item", t => t.ItemID)
                .ForeignKey("dbo.OfficeMaterialRequestInformations", t => t.OfficeMaterialRequestInformationID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.OfficeMaterialRequestInformationID)
                .Index(t => t.ItemID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.OfficeMaterialRequestInformations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        StoreRequestNumber = c.String(),
                        ApprovedBy = c.String(nullable: false),
                        From = c.String(),
                        ApplicationUserID = c.String(nullable: false, maxLength: 128),
                        StoreID = c.Int(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Store", t => t.StoreID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .Index(t => t.ApplicationUserID)
                .Index(t => t.StoreID);
            
            CreateTable(
                "dbo.ProductAvialableOnStock",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        ProductAvaliable = c.Single(nullable: false),
                        RecentlyReducedProduct = c.Single(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Item", t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.ProductionOrderInfo",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CustomerID = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        OrderNumber = c.String(),
                        Status = c.String(),
                        From = c.String(),
                        To = c.String(),
                        ApplicationUserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.Customer", t => t.CustomerID)
                .Index(t => t.CustomerID)
                .Index(t => t.ApplicationUserID);
            
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TinNumber = c.Int(nullable: false),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(),
                        City = c.String(),
                        SubCity = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SalesInformation",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        FsNo = c.String(),
                        InvoiceNumber = c.Int(nullable: false),
                        CustomerID = c.Int(nullable: false),
                        ApplicationUserID = c.String(maxLength: 128),
                        Checkedby = c.String(),
                        Reciviedby = c.String(),
                        Approvedby = c.String(),
                        Status = c.String(),
                        SubTotal = c.Single(nullable: false),
                        Tax = c.Single(nullable: false),
                        GrandTotal = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.Customer", t => t.CustomerID)
                .Index(t => t.CustomerID)
                .Index(t => t.ApplicationUserID);
            
            CreateTable(
                "dbo.Sales",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ItemID = c.Int(nullable: false),
                        SalesInformationID = c.Int(nullable: false),
                        Quantity = c.Single(nullable: false),
                        UnitPrice = c.Single(nullable: false),
                        RemaningDelivery = c.Single(nullable: false),
                        ApplicationUserID = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.Item", t => t.ItemID)
                .ForeignKey("dbo.SalesInformation", t => t.SalesInformationID)
                .Index(t => t.ItemID)
                .Index(t => t.SalesInformationID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.ProductionOrder",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.Int(nullable: false),
                        ItemID = c.Int(nullable: false),
                        ProductSize = c.Int(nullable: false),
                        Quantity = c.Single(nullable: false),
                        Date = c.DateTime(),
                        ProductionOrderInfoID = c.Int(nullable: false),
                        deliverd = c.Boolean(nullable: false),
                        RemaningDelivery = c.Single(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Item", t => t.ItemID)
                .ForeignKey("dbo.ProductionOrderInfo", t => t.ProductionOrderInfoID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ItemID)
                .Index(t => t.ProductionOrderInfoID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.ProductlogicalAvaliable",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        LogicalProductAvaliable = c.Single(nullable: false),
                        RecentlyReduced = c.Single(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Item", t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.ProductMaterialRepositories",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        ProductMaterialAavliable = c.Single(nullable: false),
                        RecentlyReducedProductMaterialAvaliable = c.Single(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Item", t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.ProStockInformation",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        StoreNumber = c.String(),
                        ApplicationUserID = c.String(maxLength: 128),
                        Reciviedby = c.String(),
                        Approvedby = c.String(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .Index(t => t.ApplicationUserID);
            
            CreateTable(
                "dbo.ProStock",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ItemID = c.Int(nullable: false),
                        ProStockInformationID = c.Int(nullable: false),
                        Quantity = c.Single(nullable: false),
                        Size = c.Int(nullable: false),
                        Total = c.Single(nullable: false),
                        ProwTotal = c.Single(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Item", t => t.ItemID)
                .ForeignKey("dbo.ProStockInformation", t => t.ProStockInformationID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ItemID)
                .Index(t => t.ProStockInformationID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.RowIssueInformations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        RowIssueNumber = c.String(),
                        ApprovedBy = c.String(nullable: false),
                        RequestNumber = c.String(),
                        From = c.String(),
                        ApplicationUserID = c.String(maxLength: 128),
                        StoreID = c.Int(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.Store", t => t.StoreID)
                .Index(t => t.ApplicationUserID)
                .Index(t => t.StoreID);
            
            CreateTable(
                "dbo.RowIssues",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RowIssueInformationID = c.Int(nullable: false),
                        ItemID = c.Int(nullable: false),
                        Quantity = c.Single(nullable: false),
                        Remark = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Item", t => t.ItemID)
                .ForeignKey("dbo.RowIssueInformations", t => t.RowIssueInformationID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.RowIssueInformationID)
                .Index(t => t.ItemID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.RowMaterialRepositery",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        RowMaterialAavliable = c.Single(nullable: false),
                        RecentlyReducedRowMaterialAvaliable = c.Single(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Item", t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.SalesDelivery",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SalesDeliveryInformationID = c.Int(nullable: false),
                        SalesID = c.Int(nullable: false),
                        Quantity = c.Single(nullable: false),
                        Remark = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Sales", t => t.SalesID)
                .ForeignKey("dbo.SalesDeliveryInformation", t => t.SalesDeliveryInformationID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.SalesDeliveryInformationID)
                .Index(t => t.SalesID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.SalesDeliveryInformation",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        DeliveryNumber = c.String(),
                        From = c.String(),
                        SalesInformationID = c.Int(nullable: false),
                        Receivedby = c.String(),
                        ApplictionUserID = c.String(),
                        Status = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.SalesInformation", t => t.SalesInformationID)
                .Index(t => t.SalesInformationID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.StoreRequestInformations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        StoreRequestNumber = c.String(),
                        ApprovedBy = c.String(nullable: false),
                        From = c.String(),
                        ApplicationUserID = c.String(nullable: false, maxLength: 128),
                        StoreID = c.Int(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Store", t => t.StoreID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .Index(t => t.ApplicationUserID)
                .Index(t => t.StoreID);
            
            CreateTable(
                "dbo.StoreRequests",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StoreRequestInformationID = c.Int(nullable: false),
                        ItemID = c.Int(nullable: false),
                        Quantity = c.Single(nullable: false),
                        Remark = c.String(),
                        Deliverd = c.Boolean(nullable: false),
                        RemaningDelivery = c.Single(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Item", t => t.ItemID)
                .ForeignKey("dbo.StoreRequestInformations", t => t.StoreRequestInformationID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.StoreRequestInformationID)
                .Index(t => t.ItemID)
                .Index(t => t.ApplicationUser_Id);
            
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
            DropForeignKey("dbo.Transfer", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.TransferInformation", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.StoreRequests", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.StoreRequestInformations", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.StoreRequests", "StoreRequestInformationID", "dbo.StoreRequestInformations");
            DropForeignKey("dbo.StoreRequests", "ItemID", "dbo.Item");
            DropForeignKey("dbo.StoreRequestInformations", "StoreID", "dbo.Store");
            DropForeignKey("dbo.SalesDelivery", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.SalesDeliveryInformation", "SalesInformationID", "dbo.SalesInformation");
            DropForeignKey("dbo.SalesDelivery", "SalesDeliveryInformationID", "dbo.SalesDeliveryInformation");
            DropForeignKey("dbo.SalesDeliveryInformation", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.SalesDelivery", "SalesID", "dbo.Sales");
            DropForeignKey("dbo.RowMaterialRepositery", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.RowMaterialRepositery", "ID", "dbo.Item");
            DropForeignKey("dbo.RowIssues", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.RowIssueInformations", "StoreID", "dbo.Store");
            DropForeignKey("dbo.RowIssues", "RowIssueInformationID", "dbo.RowIssueInformations");
            DropForeignKey("dbo.RowIssues", "ItemID", "dbo.Item");
            DropForeignKey("dbo.RowIssueInformations", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Purchases", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProStock", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProStock", "ProStockInformationID", "dbo.ProStockInformation");
            DropForeignKey("dbo.ProStock", "ItemID", "dbo.Item");
            DropForeignKey("dbo.ProStockInformation", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Product", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProductMaterialRepositories", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProductMaterialRepositories", "ID", "dbo.Item");
            DropForeignKey("dbo.ProductlogicalAvaliable", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProductlogicalAvaliable", "ID", "dbo.Item");
            DropForeignKey("dbo.ProductionOrder", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProductionOrder", "ProductionOrderInfoID", "dbo.ProductionOrderInfo");
            DropForeignKey("dbo.ProductionOrder", "ItemID", "dbo.Item");
            DropForeignKey("dbo.Sales", "SalesInformationID", "dbo.SalesInformation");
            DropForeignKey("dbo.Sales", "ItemID", "dbo.Item");
            DropForeignKey("dbo.Sales", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.SalesInformation", "CustomerID", "dbo.Customer");
            DropForeignKey("dbo.SalesInformation", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProductionOrderInfo", "CustomerID", "dbo.Customer");
            DropForeignKey("dbo.ProductionOrderInfo", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProductAvialableOnStock", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProductAvialableOnStock", "ID", "dbo.Item");
            DropForeignKey("dbo.OfficeMaterialRequestInformations", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.OfficeMaterialRequests", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.OfficeMaterialRequestInformations", "StoreID", "dbo.Store");
            DropForeignKey("dbo.OfficeMaterialRequests", "OfficeMaterialRequestInformationID", "dbo.OfficeMaterialRequestInformations");
            DropForeignKey("dbo.OfficeMaterialRequests", "ItemID", "dbo.Item");
            DropForeignKey("dbo.OfficeIssues", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.OfficeIssueInformations", "StoreID", "dbo.Store");
            DropForeignKey("dbo.Stock", "StoreID", "dbo.Store");
            DropForeignKey("dbo.StockInformation", "SupplierID", "dbo.Supplier");
            DropForeignKey("dbo.Stock", "StockInformationID", "dbo.StockInformation");
            DropForeignKey("dbo.StockInformation", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Stock", "ItemID", "dbo.Item");
            DropForeignKey("dbo.PurchaseInformations", "StoreID", "dbo.Store");
            DropForeignKey("dbo.Purchases", "PurchaseInformation_ID", "dbo.PurchaseInformations");
            DropForeignKey("dbo.Purchases", "ItemID", "dbo.Item");
            DropForeignKey("dbo.PurchaseInformations", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProductInformation", "StoreID", "dbo.Store");
            DropForeignKey("dbo.Product", "Transfer_ID", "dbo.Transfer");
            DropForeignKey("dbo.Transfer", "TransferInformationID", "dbo.TransferInformation");
            DropForeignKey("dbo.TransferInformation", "StoreID", "dbo.Store");
            DropForeignKey("dbo.Transfer", "ItemID", "dbo.Item");
            DropForeignKey("dbo.Product", "ProductInformationID", "dbo.ProductInformation");
            DropForeignKey("dbo.ProductInformation", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Store", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.OfficeIssues", "OfficeIssueInformationID", "dbo.OfficeIssueInformations");
            DropForeignKey("dbo.OfficeIssues", "ItemID", "dbo.Item");
            DropForeignKey("dbo.OfficeIssueInformations", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ItemType", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Item", "ItemTypeID", "dbo.ItemType");
            DropForeignKey("dbo.Item", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AvaliableOnStocks", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Unit", new[] { "ApplicationUserID" });
            DropIndex("dbo.StoreRequests", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.StoreRequests", new[] { "ItemID" });
            DropIndex("dbo.StoreRequests", new[] { "StoreRequestInformationID" });
            DropIndex("dbo.StoreRequestInformations", new[] { "StoreID" });
            DropIndex("dbo.StoreRequestInformations", new[] { "ApplicationUserID" });
            DropIndex("dbo.SalesDeliveryInformation", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.SalesDeliveryInformation", new[] { "SalesInformationID" });
            DropIndex("dbo.SalesDelivery", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.SalesDelivery", new[] { "SalesID" });
            DropIndex("dbo.SalesDelivery", new[] { "SalesDeliveryInformationID" });
            DropIndex("dbo.RowMaterialRepositery", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.RowMaterialRepositery", new[] { "ID" });
            DropIndex("dbo.RowIssues", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.RowIssues", new[] { "ItemID" });
            DropIndex("dbo.RowIssues", new[] { "RowIssueInformationID" });
            DropIndex("dbo.RowIssueInformations", new[] { "StoreID" });
            DropIndex("dbo.RowIssueInformations", new[] { "ApplicationUserID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.ProStock", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ProStock", new[] { "ProStockInformationID" });
            DropIndex("dbo.ProStock", new[] { "ItemID" });
            DropIndex("dbo.ProStockInformation", new[] { "ApplicationUserID" });
            DropIndex("dbo.ProductMaterialRepositories", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ProductMaterialRepositories", new[] { "ID" });
            DropIndex("dbo.ProductlogicalAvaliable", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ProductlogicalAvaliable", new[] { "ID" });
            DropIndex("dbo.ProductionOrder", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ProductionOrder", new[] { "ProductionOrderInfoID" });
            DropIndex("dbo.ProductionOrder", new[] { "ItemID" });
            DropIndex("dbo.Sales", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Sales", new[] { "SalesInformationID" });
            DropIndex("dbo.Sales", new[] { "ItemID" });
            DropIndex("dbo.SalesInformation", new[] { "ApplicationUserID" });
            DropIndex("dbo.SalesInformation", new[] { "CustomerID" });
            DropIndex("dbo.ProductionOrderInfo", new[] { "ApplicationUserID" });
            DropIndex("dbo.ProductionOrderInfo", new[] { "CustomerID" });
            DropIndex("dbo.ProductAvialableOnStock", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ProductAvialableOnStock", new[] { "ID" });
            DropIndex("dbo.OfficeMaterialRequestInformations", new[] { "StoreID" });
            DropIndex("dbo.OfficeMaterialRequestInformations", new[] { "ApplicationUserID" });
            DropIndex("dbo.OfficeMaterialRequests", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.OfficeMaterialRequests", new[] { "ItemID" });
            DropIndex("dbo.OfficeMaterialRequests", new[] { "OfficeMaterialRequestInformationID" });
            DropIndex("dbo.StockInformation", new[] { "ApplicationUserID" });
            DropIndex("dbo.StockInformation", new[] { "SupplierID" });
            DropIndex("dbo.Stock", new[] { "StoreID" });
            DropIndex("dbo.Stock", new[] { "StockInformationID" });
            DropIndex("dbo.Stock", new[] { "ItemID" });
            DropIndex("dbo.Purchases", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Purchases", new[] { "PurchaseInformation_ID" });
            DropIndex("dbo.Purchases", new[] { "ItemID" });
            DropIndex("dbo.PurchaseInformations", new[] { "ApplicationUserID" });
            DropIndex("dbo.PurchaseInformations", new[] { "StoreID" });
            DropIndex("dbo.TransferInformation", new[] { "ApplicationUserID" });
            DropIndex("dbo.TransferInformation", new[] { "StoreID" });
            DropIndex("dbo.Transfer", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Transfer", new[] { "ItemID" });
            DropIndex("dbo.Transfer", new[] { "TransferInformationID" });
            DropIndex("dbo.Product", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Product", new[] { "Transfer_ID" });
            DropIndex("dbo.Product", new[] { "ProductInformationID" });
            DropIndex("dbo.ProductInformation", new[] { "ApplicationUserID" });
            DropIndex("dbo.ProductInformation", new[] { "StoreID" });
            DropIndex("dbo.Store", new[] { "ApplicationUserID" });
            DropIndex("dbo.OfficeIssues", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.OfficeIssues", new[] { "ItemID" });
            DropIndex("dbo.OfficeIssues", new[] { "OfficeIssueInformationID" });
            DropIndex("dbo.OfficeIssueInformations", new[] { "StoreID" });
            DropIndex("dbo.OfficeIssueInformations", new[] { "ApplicationUserID" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.ItemType", new[] { "ApplicationUserID" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Item", new[] { "ApplicationUserID" });
            DropIndex("dbo.Item", new[] { "UnitID" });
            DropIndex("dbo.Item", new[] { "ItemTypeID" });
            DropIndex("dbo.AvaliableOnStocks", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AvaliableOnStocks", new[] { "ID" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Unit");
            DropTable("dbo.StoreRequests");
            DropTable("dbo.StoreRequestInformations");
            DropTable("dbo.SalesDeliveryInformation");
            DropTable("dbo.SalesDelivery");
            DropTable("dbo.RowMaterialRepositery");
            DropTable("dbo.RowIssues");
            DropTable("dbo.RowIssueInformations");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.ProStock");
            DropTable("dbo.ProStockInformation");
            DropTable("dbo.ProductMaterialRepositories");
            DropTable("dbo.ProductlogicalAvaliable");
            DropTable("dbo.ProductionOrder");
            DropTable("dbo.Sales");
            DropTable("dbo.SalesInformation");
            DropTable("dbo.Customer");
            DropTable("dbo.ProductionOrderInfo");
            DropTable("dbo.ProductAvialableOnStock");
            DropTable("dbo.OfficeMaterialRequestInformations");
            DropTable("dbo.OfficeMaterialRequests");
            DropTable("dbo.Supplier");
            DropTable("dbo.StockInformation");
            DropTable("dbo.Stock");
            DropTable("dbo.Purchases");
            DropTable("dbo.PurchaseInformations");
            DropTable("dbo.TransferInformation");
            DropTable("dbo.Transfer");
            DropTable("dbo.Product");
            DropTable("dbo.ProductInformation");
            DropTable("dbo.Store");
            DropTable("dbo.OfficeIssues");
            DropTable("dbo.OfficeIssueInformations");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.ItemType");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Item");
            DropTable("dbo.AvaliableOnStocks");
        }
    }
}
