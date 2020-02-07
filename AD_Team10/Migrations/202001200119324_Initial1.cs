namespace AD_Team10.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RequisitionDetails", "QuantityReceived", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RequisitionDetails", "QuantityReceived");
        }
    }
}
