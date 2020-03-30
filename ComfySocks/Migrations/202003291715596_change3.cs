namespace ComfySocks.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductInformation", "Approvedby", c => c.String());
            AlterColumn("dbo.ProductInformation", "From", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProductInformation", "From", c => c.Int(nullable: false));
            DropColumn("dbo.ProductInformation", "Approvedby");
        }
    }
}
