namespace AD_Team10.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial4 : DbMigration
    {
        public override void Up()
        {
            //Departments
            Sql("INSERT INTO Departments(DepartmentCode,DepartmentName,CollectionPointID) Values('ENGL','English',1)");
            Sql("INSERT INTO Departments(DepartmentCode,DepartmentName,CollectionPointID) Values('CPSC','Computer Science',4)");
            Sql("INSERT INTO Departments(DepartmentCode,DepartmentName,CollectionPointID) Values('COMM','Commerce',1)");
            Sql("INSERT INTO Departments(DepartmentCode,DepartmentName,CollectionPointID) Values('REGR','Registrar',1)");
            Sql("INSERT INTO Departments(DepartmentCode,DepartmentName,CollectionPointID) Values('ZOOL','Zoology',5)");
            Sql("INSERT INTO Departments(DepartmentCode,DepartmentName,CollectionPointID) Values('MEDC','Medicine',3)");
            Sql("INSERT INTO Departments(DepartmentCode,DepartmentName,CollectionPointID) Values('BIZZ','Business',4)");
            Sql("INSERT INTO Departments(DepartmentCode,DepartmentName,CollectionPointID) Values('BOTN','Botany',5)");
            Sql("INSERT INTO Departments(DepartmentCode,DepartmentName,CollectionPointID) Values('INTC','Information Technology',4)");
            Sql("INSERT INTO Departments(DepartmentCode,DepartmentName,CollectionPointID) Values('ECON','Economics',1)");

            //StoreUsers
            Sql("INSERT INTO StoreUsers(Username,Password,Role,StoreEmployeeID) Values('esther.tan','1234',0,1)");
            Sql("INSERT INTO StoreUsers(Username,Password,Role,StoreEmployeeID) Values('xiao.zhan','2345',1,2)");
            Sql("INSERT INTO StoreUsers(Username,Password,Role,StoreEmployeeID) Values('angela.baby','3456',2,3)");
            Sql("INSERT INTO StoreUsers(Username,Password,Role,StoreEmployeeID) Values('jeanet','4567',0,4)");
            Sql("INSERT INTO StoreUsers(Username,Password,Role,StoreEmployeeID) Values('vivi','5678',0,5)");

            // Item
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Clips Double 1','Dozen',50,30,60,1)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Clips Double 2','Dozen',50,30,40,1)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Clips Double 3/4','Dozen',100,100,90,1)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Clips Paper Large','Box',50,30,58,1)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Clips Paper Medium','Box',100,100,30,1)");//5
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Clips Paper Small','Box',50,30,52,1)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Envelope Brown(3x6)','Each',600,400,610,2)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Envelope Brown(3x6) w/ Window','Each',600,400,700,2)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Envelope Brown(5x7)','Each',600,400,629,2)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Envelope Brown(5x7) w/ Window','Each',600,400,400,2)");//10
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Envelope White(3x6)','Each',600,400,650,2)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Envelope White(3x6) w/ Window','Each',600,400,300,2)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Envelope White(5x7)','Each',600,400,608,2)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Envelope White(5x7) w/ Window','Each',600,400,602,2)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Eraser (hard)','Each',50,20,60,3)");//15
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Eraser (soft)','Each',50,20,53,3)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Eraser long(soft)','Each',50,20,60,3)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Exercise Book (100 pg)','Each',100,50,120,4)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Exercise Book (120 pg)','Each',100,50,139,4)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Exercise Book A4 Hardcover (100 pg)','Each',100,50,145,4)");//20
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Exercise Book A4 Hardcover (120 pg)','Each',100,50,111,4)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Exercise Book A4 Hardcover (200 pg)','Each',100,50,123,4)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Exercise Book Hardcover (100 pg)','Each',100,50,101,4)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Exercise Book Hardcover (120 pg)','Each',100,50,123,4)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('File Separator','Set',100,50,130,5)");//25
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('File-Blue Plain','Each',200,100,234,5)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('File-Blue with Logo','Each',200,150,201,5)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('File-Brown w/o Logo','Each',200,150,341,5)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('File-Brown with Logo','Each',200,150,245,5)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Folder Plastic Blue','Each',200,150,180,6)");//30
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Folder Plastic Clear','Each',200,150,203,6)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Folder Plastic Green','Each',200,150,253,6)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Folder Plastic Pink','Each',200,150,288,6)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Folder Plastic Yellow','Each',200,150,250,6)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Highlighter Blue','Box',100,80,128,9)");//35
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Highlighter Green','Box',100,80,95,9)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Highlighter Pink','Box',100,80,139,9)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Highlighter Yellow','Box',100,80,112,9)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Hole Puncher 2 holes','Each',50,20,52,10)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Hole Puncher 3 holes','Each',50,20,60,10)");//40
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Hole Puncher Adjustable','Each',50,20,51,10)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Pad Postit Memo 1x2','Packet',100,60,101,7)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Pad Postit Memo 1/2x1','Packet',100,60,111,7)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Pad Postit Memo 1/2x2','Packet',100,60,123,7)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Pad Postit Memo 2x3','Packet',100,60,132,7)");//45
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Pad Postit Memo 2x1','Packet',100,60,109,7)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Pad Postit Memo 2x4','Packet',100,60,122,7)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Pad Postit Memo 3/4x2','Packet',100,60,105,7)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Paper Photostat A3','Box',500,500,600,8)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Paper Photostat A4','Box',500,500,700,8)");//50
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Pen Ballpoint Black','Dozen',100,50,123,9)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Pen Ballpoint Blue','Dozen',100,50,134,9)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Pen Ballpoint Red','Dozen',100,50,127,9)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Pen Felt Tip Black','Dozen',100,50,113,9)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Pen Felt Tip Blue','Dozen',100,50,137,9)");//55
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Pen Felt Tip Red','Dozen',100,50,125,9)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Pen Transparency Permanent','Packet',100,50,111,9)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Pen Transparency Soluble','Packet',100,50,133,9)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Pen Whiteboard Marker Black','Box',100,50,104,9)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Pen Whiteboard Marker Blue','Box',100,50,139,9)");//60
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Pen Whiteboard Marker Green','Box',100,50,111,9)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Pen Whiteboard Marker Red','Box',100,50,108,9)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Pencil 2B','Dozen',100,50,126,9)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Pencil 2B with Eraser End','Dozen',100,50,110,9)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Pencil 4H','Dozen',100,50,104,9)");//65
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Pencil B','Dozen',100,50,135,9)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Pencil B with Eraser End','Dozen',100,50,117,9)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Ruler 12','Dozen',50,20,51,11)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Ruler 6','Dozen',50,20,60,11)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Scissors 5','Each',50,20,58,12)");//70
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Scissors 6','Each',50,20,55,12)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Scissors 7','Each',50,20,60,12)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Scotch Tape','Each',50,20,61,17)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Scotch Tape Dispenser','Each',50,20,53,17)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Sharpener','Each',50,20,53,13)");//75
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Shorthand Book (100 pg)','Each',100,80,120,14)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Shorthand Book (120 pg)','Each',100,80,105,14)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Shorthand Book (80 pg)','Each',100,80,123,14)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Stapler No. 28','Each',50,20,53,15)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Stapler No. 34','Each',50,20,60,15)");//80
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Stapler No. 29','Box',50,20,56,15)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Stapler No. 36','Box',50,20,67,15)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Thumb Tacks Large','Box',10,10,11,16)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Thumb Tacks Medium','Box',10,10,17,16)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Thumb Tacks Small','Box',10,10,13,16)");//85
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Transparency Blue','Box',100,200,128,18)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Transparency Clear','Box',500,400,543,18)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Transparency Green','Box',100,200,143,18)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Transparency Red','Box',100,200,127,18)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Transparency Reverse Blue','Box',100,200,109,18)");//90
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Transparency Cover 3M','Box',500,400,526,18)");
            Sql("INSERT INTO Items(Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID) Values('Trays In/Out','Set',20,10,23,19)");

        }

        public override void Down()
        {
        }
    }
}
