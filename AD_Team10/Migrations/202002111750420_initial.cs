﻿namespace AD_Team10.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdjustmentVoucherDetails",
                c => new
                    {
                        VoucherID = c.Int(nullable: false),
                        ItemID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Reason = c.String(),
                    })
                .PrimaryKey(t => new { t.VoucherID, t.ItemID })
                .ForeignKey("dbo.AdjustmentVouchers", t => t.VoucherID, cascadeDelete: true)
                .ForeignKey("dbo.Items", t => t.ItemID, cascadeDelete: true)
                .Index(t => t.VoucherID)
                .Index(t => t.ItemID);
            
            CreateTable(
                "dbo.AdjustmentVouchers",
                c => new
                    {
                        AdjustmentVoucherID = c.Int(nullable: false, identity: true),
                        AdjustmentDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        StoreEmployeeID = c.Int(nullable: false),
                        Remark = c.String(),
                    })
                .PrimaryKey(t => t.AdjustmentVoucherID)
                .ForeignKey("dbo.StoreEmployees", t => t.StoreEmployeeID, cascadeDelete: true)
                .Index(t => t.StoreEmployeeID);
            
            CreateTable(
                "dbo.StoreEmployees",
                c => new
                    {
                        StoreEmployeeID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        MiddleName = c.String(),
                        Phone = c.String(),
                        Address = c.String(),
                        Gender = c.String(),
                        Email = c.String(),
                        Designation = c.String(),
                    })
                .PrimaryKey(t => t.StoreEmployeeID);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ItemID = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        UnitOfMeasure = c.String(),
                        ReorderLevel = c.Int(nullable: false),
                        ReorderQuantity = c.Int(nullable: false),
                        UnitsInStock = c.Int(nullable: false),
                        CategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ItemID)
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.PurchaseOrderDetails",
                c => new
                    {
                        OrderID = c.Int(nullable: false),
                        ItemID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        QuantityReceived = c.Int(nullable: false),
                        Price = c.Single(nullable: false),
                    })
                .PrimaryKey(t => new { t.OrderID, t.ItemID })
                .ForeignKey("dbo.Items", t => t.ItemID, cascadeDelete: true)
                .ForeignKey("dbo.PurchaseOrders", t => t.OrderID, cascadeDelete: true)
                .Index(t => t.OrderID)
                .Index(t => t.ItemID);
            
            CreateTable(
                "dbo.PurchaseOrders",
                c => new
                    {
                        PurchaseOrderID = c.Int(nullable: false, identity: true),
                        SupplierID = c.Int(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                        CompletedDate = c.DateTime(),
                        OrderStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PurchaseOrderID)
                .ForeignKey("dbo.Suppliers", t => t.SupplierID, cascadeDelete: true)
                .Index(t => t.SupplierID);
            
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        SupplierID = c.Int(nullable: false, identity: true),
                        SupplierCode = c.String(),
                        SupplierName = c.String(),
                        ContactName = c.String(),
                        Phone = c.String(),
                        Fax = c.String(),
                        Address = c.String(),
                        GSTNumber = c.String(),
                    })
                .PrimaryKey(t => t.SupplierID);
            
            CreateTable(
                "dbo.SupplierItems",
                c => new
                    {
                        SupplierID = c.Int(nullable: false),
                        ItemID = c.Int(nullable: false),
                        Price = c.Single(nullable: false),
                    })
                .PrimaryKey(t => new { t.SupplierID, t.ItemID })
                .ForeignKey("dbo.Items", t => t.ItemID, cascadeDelete: true)
                .ForeignKey("dbo.Suppliers", t => t.SupplierID, cascadeDelete: true)
                .Index(t => t.SupplierID)
                .Index(t => t.ItemID);
            
            CreateTable(
                "dbo.RequisitionDetails",
                c => new
                    {
                        RequisitionID = c.Int(nullable: false),
                        ItemID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        QuantityReceived = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RequisitionID, t.ItemID })
                .ForeignKey("dbo.Items", t => t.ItemID, cascadeDelete: true)
                .ForeignKey("dbo.Requisitions", t => t.RequisitionID, cascadeDelete: true)
                .Index(t => t.RequisitionID)
                .Index(t => t.ItemID);
            
            CreateTable(
                "dbo.Requisitions",
                c => new
                    {
                        RequisitionID = c.Int(nullable: false, identity: true),
                        ApprovalDate = c.DateTime(),
                        RequisitionDate = c.DateTime(nullable: false),
                        CompletedDate = c.DateTime(),
                        Status = c.Int(nullable: false),
                        Remark = c.String(),
                        EmployeeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RequisitionID)
                .ForeignKey("dbo.DeptEmployees", t => t.EmployeeID, cascadeDelete: true)
                .Index(t => t.EmployeeID);
            
            CreateTable(
                "dbo.DeptEmployees",
                c => new
                    {
                        DeptEmployeeID = c.Int(nullable: false, identity: true),
                        DepartmentID = c.Int(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        MiddleName = c.String(),
                        Phone = c.String(),
                        Address = c.String(),
                        Gender = c.String(),
                        Email = c.String(),
                        Designation = c.String(),
                    })
                .PrimaryKey(t => t.DeptEmployeeID)
                .ForeignKey("dbo.Departments", t => t.DepartmentID, cascadeDelete: true)
                .Index(t => t.DepartmentID);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentID = c.Int(nullable: false, identity: true),
                        DepartmentCode = c.String(),
                        DepartmentName = c.String(),
                        CollectionPointID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DepartmentID)
                .ForeignKey("dbo.CollectionPoints", t => t.CollectionPointID, cascadeDelete: true)
                .Index(t => t.CollectionPointID);
            
            CreateTable(
                "dbo.CollectionPoints",
                c => new
                    {
                        CollectionPointID = c.Int(nullable: false, identity: true),
                        CollectionPointName = c.String(),
                    })
                .PrimaryKey(t => t.CollectionPointID);
            
            CreateTable(
                "dbo.DeptUsers",
                c => new
                    {
                        DeptUserID = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        Role = c.Int(nullable: false),
                        DeptEmployeeID = c.Int(nullable: false),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DeptUserID)
                .ForeignKey("dbo.DeptEmployees", t => t.DeptEmployeeID, cascadeDelete: true)
                .Index(t => t.DeptEmployeeID);
            
            CreateTable(
                "dbo.RequisitionRetrievals",
                c => new
                    {
                        RequisitionID = c.Int(nullable: false),
                        RetrievalListID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RequisitionID, t.RetrievalListID })
                .ForeignKey("dbo.Requisitions", t => t.RequisitionID, cascadeDelete: true)
                .ForeignKey("dbo.RetrievalLists", t => t.RetrievalListID, cascadeDelete: true)
                .Index(t => t.RequisitionID)
                .Index(t => t.RetrievalListID);
            
            CreateTable(
                "dbo.RetrievalLists",
                c => new
                    {
                        RetrievalListID = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RetrievalListID);
            
            CreateTable(
                "dbo.RetrievalListDetails",
                c => new
                    {
                        RetrievalListID = c.Int(nullable: false),
                        DepartmentID = c.Int(nullable: false),
                        ItemID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        QuantityOffered = c.Int(nullable: false),
                        QuantityReceived = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RetrievalListID, t.DepartmentID, t.ItemID })
                .ForeignKey("dbo.Departments", t => t.DepartmentID, cascadeDelete: true)
                .ForeignKey("dbo.Items", t => t.ItemID, cascadeDelete: true)
                .ForeignKey("dbo.RetrievalLists", t => t.RetrievalListID, cascadeDelete: true)
                .Index(t => t.RetrievalListID)
                .Index(t => t.DepartmentID)
                .Index(t => t.ItemID);
            
            CreateTable(
                "dbo.StoreUsers",
                c => new
                    {
                        StoreUserID = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        Role = c.Int(nullable: false),
                        StoreEmployeeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StoreUserID)
                .ForeignKey("dbo.StoreEmployees", t => t.StoreEmployeeID, cascadeDelete: true)
                .Index(t => t.StoreEmployeeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StoreUsers", "StoreEmployeeID", "dbo.StoreEmployees");
            DropForeignKey("dbo.RequisitionRetrievals", "RetrievalListID", "dbo.RetrievalLists");
            DropForeignKey("dbo.RetrievalListDetails", "RetrievalListID", "dbo.RetrievalLists");
            DropForeignKey("dbo.RetrievalListDetails", "ItemID", "dbo.Items");
            DropForeignKey("dbo.RetrievalListDetails", "DepartmentID", "dbo.Departments");
            DropForeignKey("dbo.RequisitionRetrievals", "RequisitionID", "dbo.Requisitions");
            DropForeignKey("dbo.DeptUsers", "DeptEmployeeID", "dbo.DeptEmployees");
            DropForeignKey("dbo.AdjustmentVoucherDetails", "ItemID", "dbo.Items");
            DropForeignKey("dbo.RequisitionDetails", "RequisitionID", "dbo.Requisitions");
            DropForeignKey("dbo.Requisitions", "EmployeeID", "dbo.DeptEmployees");
            DropForeignKey("dbo.DeptEmployees", "DepartmentID", "dbo.Departments");
            DropForeignKey("dbo.Departments", "CollectionPointID", "dbo.CollectionPoints");
            DropForeignKey("dbo.RequisitionDetails", "ItemID", "dbo.Items");
            DropForeignKey("dbo.PurchaseOrderDetails", "OrderID", "dbo.PurchaseOrders");
            DropForeignKey("dbo.PurchaseOrders", "SupplierID", "dbo.Suppliers");
            DropForeignKey("dbo.SupplierItems", "SupplierID", "dbo.Suppliers");
            DropForeignKey("dbo.SupplierItems", "ItemID", "dbo.Items");
            DropForeignKey("dbo.PurchaseOrderDetails", "ItemID", "dbo.Items");
            DropForeignKey("dbo.Items", "CategoryID", "dbo.Categories");
            DropForeignKey("dbo.AdjustmentVoucherDetails", "VoucherID", "dbo.AdjustmentVouchers");
            DropForeignKey("dbo.AdjustmentVouchers", "StoreEmployeeID", "dbo.StoreEmployees");
            DropIndex("dbo.StoreUsers", new[] { "StoreEmployeeID" });
            DropIndex("dbo.RetrievalListDetails", new[] { "ItemID" });
            DropIndex("dbo.RetrievalListDetails", new[] { "DepartmentID" });
            DropIndex("dbo.RetrievalListDetails", new[] { "RetrievalListID" });
            DropIndex("dbo.RequisitionRetrievals", new[] { "RetrievalListID" });
            DropIndex("dbo.RequisitionRetrievals", new[] { "RequisitionID" });
            DropIndex("dbo.DeptUsers", new[] { "DeptEmployeeID" });
            DropIndex("dbo.Departments", new[] { "CollectionPointID" });
            DropIndex("dbo.DeptEmployees", new[] { "DepartmentID" });
            DropIndex("dbo.Requisitions", new[] { "EmployeeID" });
            DropIndex("dbo.RequisitionDetails", new[] { "ItemID" });
            DropIndex("dbo.RequisitionDetails", new[] { "RequisitionID" });
            DropIndex("dbo.SupplierItems", new[] { "ItemID" });
            DropIndex("dbo.SupplierItems", new[] { "SupplierID" });
            DropIndex("dbo.PurchaseOrders", new[] { "SupplierID" });
            DropIndex("dbo.PurchaseOrderDetails", new[] { "ItemID" });
            DropIndex("dbo.PurchaseOrderDetails", new[] { "OrderID" });
            DropIndex("dbo.Items", new[] { "CategoryID" });
            DropIndex("dbo.AdjustmentVouchers", new[] { "StoreEmployeeID" });
            DropIndex("dbo.AdjustmentVoucherDetails", new[] { "ItemID" });
            DropIndex("dbo.AdjustmentVoucherDetails", new[] { "VoucherID" });
            DropTable("dbo.StoreUsers");
            DropTable("dbo.RetrievalListDetails");
            DropTable("dbo.RetrievalLists");
            DropTable("dbo.RequisitionRetrievals");
            DropTable("dbo.DeptUsers");
            DropTable("dbo.CollectionPoints");
            DropTable("dbo.Departments");
            DropTable("dbo.DeptEmployees");
            DropTable("dbo.Requisitions");
            DropTable("dbo.RequisitionDetails");
            DropTable("dbo.SupplierItems");
            DropTable("dbo.Suppliers");
            DropTable("dbo.PurchaseOrders");
            DropTable("dbo.PurchaseOrderDetails");
            DropTable("dbo.Categories");
            DropTable("dbo.Items");
            DropTable("dbo.StoreEmployees");
            DropTable("dbo.AdjustmentVouchers");
            DropTable("dbo.AdjustmentVoucherDetails");
        }
    }
}
