namespace AD_Team10.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AdjustmentVoucherDetails", "Quantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AdjustmentVoucherDetails", "Quantity", c => c.String());
        }
    }
}
