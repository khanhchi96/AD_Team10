namespace AD_Team10.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Requisitions", "RetrievalListID", "dbo.RetrievalLists");
            DropIndex("dbo.Requisitions", new[] { "RetrievalListID" });
            DropColumn("dbo.Requisitions", "RetrievalListID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Requisitions", "RetrievalListID", c => c.Int(nullable: false));
            CreateIndex("dbo.Requisitions", "RetrievalListID");
            AddForeignKey("dbo.Requisitions", "RetrievalListID", "dbo.RetrievalLists", "RetrievalListID", cascadeDelete: true);
        }
    }
}
