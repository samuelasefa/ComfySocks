namespace ComfySocks.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class anychange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductInformation", "From", c => c.Int(nullable: false));
            AddColumn("dbo.ProductInformation", "Status", c => c.String());
            AddColumn("dbo.Product", "ItemID", c => c.Int(nullable: false));
            DropColumn("dbo.Product", "TransferProductID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Product", "TransferProductID", c => c.Int(nullable: false));
            DropColumn("dbo.Product", "ItemID");
            DropColumn("dbo.ProductInformation", "Status");
            DropColumn("dbo.ProductInformation", "From");
        }
    }
}
