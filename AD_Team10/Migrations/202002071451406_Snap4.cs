namespace AD_Team10.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Snap4 : DbMigration
    {
        public override void Up()
        {
            //AdjustmentVoucherDetails                                                                                           
            Sql("INSERT INTO AdjustmentVoucherDetails(VoucherID,ItemID,Quantity,Reason) Values(1,20,'-3','Damaged')");
            Sql("INSERT INTO AdjustmentVoucherDetails(VoucherID,ItemID,Quantity,Reason) Values(1,33,'+5','Gift pack')");
            Sql("INSERT INTO AdjustmentVoucherDetails(VoucherID,ItemID,Quantity,Reason) Values(2,32,'-2','Damaged')");
            Sql("INSERT INTO AdjustmentVoucherDetails(VoucherID,ItemID,Quantity,Reason) Values(3,11,'-10','Damaged')");
            Sql("INSERT INTO AdjustmentVoucherDetails(VoucherID,ItemID,Quantity,Reason) Values(4,44,'+2','Gift pack')");
            Sql("INSERT INTO AdjustmentVoucherDetails(VoucherID,ItemID,Quantity,Reason) Values(5,23,'-1','Damaged')");
            Sql("INSERT INTO AdjustmentVoucherDetails(VoucherID,ItemID,Quantity,Reason) Values(6,12,'-1','Damaged')");
            Sql("INSERT INTO AdjustmentVoucherDetails(VoucherID,ItemID,Quantity,Reason) Values(6,46,'+4','Gift pack')");
            Sql("INSERT INTO AdjustmentVoucherDetails(VoucherID,ItemID,Quantity,Reason) Values(7,78,'-1','Damaged')");
            Sql("INSERT INTO AdjustmentVoucherDetails(VoucherID,ItemID,Quantity,Reason) Values(8,52,'+10','Gift pack')");
            Sql("INSERT INTO AdjustmentVoucherDetails(VoucherID,ItemID,Quantity,Reason) Values(9,22,'-1','Damaged')");
            Sql("INSERT INTO AdjustmentVoucherDetails(VoucherID,ItemID,Quantity,Reason) Values(10,5,'+2','Gift pack')");


            //PurchaseOrderDetails                                                                                                
            Sql("INSERT INTO PurchaseOrderDetails(OrderID,ItemID,Quantity,QuantityReceived,Price) Values(1,26,100,100,1.65)");
            Sql("INSERT INTO PurchaseOrderDetails(OrderID,ItemID,Quantity,QuantityReceived,Price) Values(1,42,60,60,7.00)");
            Sql("INSERT INTO PurchaseOrderDetails(OrderID,ItemID,Quantity,QuantityReceived,Price) Values(1,33,150,150,0.90)");
            Sql("INSERT INTO PurchaseOrderDetails(OrderID,ItemID,Quantity,QuantityReceived,Price) Values(1,20,50,50,2.80)");
            Sql("INSERT INTO PurchaseOrderDetails(OrderID,ItemID,Quantity,QuantityReceived,Price) Values(2,14,400,400,0.45)");
            Sql("INSERT INTO PurchaseOrderDetails(OrderID,ItemID,Quantity,QuantityReceived,Price) Values(2,32,150,150,0.90)");
            Sql("INSERT INTO PurchaseOrderDetails(OrderID,ItemID,Quantity,QuantityReceived,Price) Values(2,76,80,80,2.20)");
            Sql("INSERT INTO PurchaseOrderDetails(OrderID,ItemID,Quantity,QuantityReceived,Price) Values(3,11,400,400,0.15)");
            Sql("INSERT INTO PurchaseOrderDetails(OrderID,ItemID,Quantity,QuantityReceived,Price) Values(3,41,20,20,90.00)");
            Sql("INSERT INTO PurchaseOrderDetails(OrderID,ItemID,Quantity,QuantityReceived,Price) Values(3,25,50,50,2.00)");
            Sql("INSERT INTO PurchaseOrderDetails(OrderID,ItemID,Quantity,QuantityReceived,Price) Values(3,79,20,20,1.50)");
            Sql("INSERT INTO PurchaseOrderDetails(OrderID,ItemID,Quantity,QuantityReceived,Price) Values(4,52,50,50,3.00)");
            Sql("INSERT INTO PurchaseOrderDetails(OrderID,ItemID,Quantity,QuantityReceived,Price) Values(4,44,60,60,8.40)");
            Sql("INSERT INTO PurchaseOrderDetails(OrderID,ItemID,Quantity,QuantityReceived,Price) Values(5,23,50,50,2.65)");
            Sql("INSERT INTO PurchaseOrderDetails(OrderID,ItemID,Quantity,QuantityReceived,Price) Values(5,48,60,60,10.00)");
            Sql("INSERT INTO PurchaseOrderDetails(OrderID,ItemID,Quantity,QuantityReceived,Price) Values(5,18,50,50,0.50)");
            Sql("INSERT INTO PurchaseOrderDetails(OrderID,ItemID,Quantity,QuantityReceived,Price) Values(5,77,80,80,2.40)");
            Sql("INSERT INTO PurchaseOrderDetails(OrderID,ItemID,Quantity,QuantityReceived,Price) Values(6,46,60,60,9.60)");
            Sql("INSERT INTO PurchaseOrderDetails(OrderID,ItemID,Quantity,QuantityReceived,Price) Values(6,12,400,400,0.35)");
            Sql("INSERT INTO PurchaseOrderDetails(OrderID,ItemID,Quantity,QuantityReceived,Price) Values(6,35,80,80,4.80)");
            Sql("INSERT INTO PurchaseOrderDetails(OrderID,ItemID,Quantity,QuantityReceived,Price) Values(7,64,50,50,12.49)");
            Sql("INSERT INTO PurchaseOrderDetails(OrderID,ItemID,Quantity,QuantityReceived,Price) Values(7,73,20,20,1.80)");
            Sql("INSERT INTO PurchaseOrderDetails(OrderID,ItemID,Quantity,QuantityReceived,Price) Values(7,42,60,60,7.00)");
            Sql("INSERT INTO PurchaseOrderDetails(OrderID,ItemID,Quantity,QuantityReceived,Price) Values(7,78,80,80,2.00)");
            Sql("INSERT INTO PurchaseOrderDetails(OrderID,ItemID,Quantity,QuantityReceived,Price) Values(8,62,50,50,6.00)");
            Sql("INSERT INTO PurchaseOrderDetails(OrderID,ItemID,Quantity,QuantityReceived,Price) Values(9,22,50,50,3.00)");
            Sql("INSERT INTO PurchaseOrderDetails(OrderID,ItemID,Quantity,QuantityReceived,Price) Values(9,54,50,50,12.30)");
            Sql("INSERT INTO PurchaseOrderDetails(OrderID,ItemID,Quantity,QuantityReceived,Price) Values(10,63,50,50,8.50)");
            Sql("INSERT INTO PurchaseOrderDetails(OrderID,ItemID,Quantity,QuantityReceived,Price) Values(10,82,20,20,15.55)");
            Sql("INSERT INTO PurchaseOrderDetails(OrderID,ItemID,Quantity,QuantityReceived,Price) Values(10,5,30,30,1.35)");
            Sql("INSERT INTO PurchaseOrderDetails(OrderID,ItemID,Quantity,QuantityReceived,Price) Values(11,80,20,18,3.00)");
            Sql("INSERT INTO PurchaseOrderDetails(OrderID,ItemID,Quantity,QuantityReceived,Price) Values(11,57,50,50,15.20)");
            Sql("INSERT INTO PurchaseOrderDetails(OrderID,ItemID,Quantity,QuantityReceived,Price) Values(11,55,50,45,12.30)");
            Sql("INSERT INTO PurchaseOrderDetails(OrderID,ItemID,Quantity,QuantityReceived,Price) Values(12,25,50,50,2.00)");
            Sql("INSERT INTO PurchaseOrderDetails(OrderID,ItemID,Quantity,QuantityReceived,Price) Values(12,34,150,130,0.90)");
            Sql("INSERT INTO PurchaseOrderDetails(OrderID,ItemID,Quantity,QuantityReceived,Price) Values(12,2,30,25,2.00)");


        }

        public override void Down()
        {
        }
    }
}
