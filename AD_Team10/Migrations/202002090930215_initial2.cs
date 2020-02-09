namespace AD_Team10.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "Item_ItemID", c => c.Int());
            AddColumn("dbo.Categories", "Category_CategoryId", c => c.Int());
            AddColumn("dbo.Suppliers", "Address2", c => c.String());
            AddColumn("dbo.Suppliers", "Address3", c => c.String());
            AddColumn("dbo.Departments", "Department_DepartmentID", c => c.Int());
            AddColumn("dbo.CollectionPoints", "CollectionPoint_CollectionPointID", c => c.Int());
            AddColumn("dbo.DepUsers", "StartDate", c => c.DateTime());
            AddColumn("dbo.DepUsers", "EndDate", c => c.DateTime());
            AlterColumn("dbo.Requisitions", "ApprovalDate", c => c.DateTime());
            AlterColumn("dbo.Requisitions", "CompletedDate", c => c.DateTime());
            CreateIndex("dbo.Items", "Item_ItemID");
            CreateIndex("dbo.Categories", "Category_CategoryId");
            CreateIndex("dbo.Departments", "Department_DepartmentID");
            CreateIndex("dbo.CollectionPoints", "CollectionPoint_CollectionPointID");
            AddForeignKey("dbo.Categories", "Category_CategoryId", "dbo.Categories", "CategoryId");
            AddForeignKey("dbo.Items", "Item_ItemID", "dbo.Items", "ItemID");
            AddForeignKey("dbo.CollectionPoints", "CollectionPoint_CollectionPointID", "dbo.CollectionPoints", "CollectionPointID");
            AddForeignKey("dbo.Departments", "Department_DepartmentID", "dbo.Departments", "DepartmentID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Departments", "Department_DepartmentID", "dbo.Departments");
            DropForeignKey("dbo.CollectionPoints", "CollectionPoint_CollectionPointID", "dbo.CollectionPoints");
            DropForeignKey("dbo.Items", "Item_ItemID", "dbo.Items");
            DropForeignKey("dbo.Categories", "Category_CategoryId", "dbo.Categories");
            DropIndex("dbo.CollectionPoints", new[] { "CollectionPoint_CollectionPointID" });
            DropIndex("dbo.Departments", new[] { "Department_DepartmentID" });
            DropIndex("dbo.Categories", new[] { "Category_CategoryId" });
            DropIndex("dbo.Items", new[] { "Item_ItemID" });
            AlterColumn("dbo.Requisitions", "CompletedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Requisitions", "ApprovalDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.DepUsers", "EndDate");
            DropColumn("dbo.DepUsers", "StartDate");
            DropColumn("dbo.CollectionPoints", "CollectionPoint_CollectionPointID");
            DropColumn("dbo.Departments", "Department_DepartmentID");
            DropColumn("dbo.Suppliers", "Address3");
            DropColumn("dbo.Suppliers", "Address2");
            DropColumn("dbo.Categories", "Category_CategoryId");
            DropColumn("dbo.Items", "Item_ItemID");
        }
    }
}
