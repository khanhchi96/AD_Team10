namespace AD_Team10.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RetrievalListDetails", "QuantityOffered", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RetrievalListDetails", "QuantityOffered");
        }
    }
}
