namespace AD_Team10.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchaseOrderDetails", "QuantityReceived", c => c.Int(nullable: false));
            AddColumn("dbo.PurchaseOrders", "CompletedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Requisitions", "CompletedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Requisitions", "CompletedDate");
            DropColumn("dbo.PurchaseOrders", "CompletedDate");
            DropColumn("dbo.PurchaseOrderDetails", "QuantityReceived");
        }
    }
}
