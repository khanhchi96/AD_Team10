namespace AD_Team10.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    //Author: Sivakumar Jayachelvi Nivvya
    public partial class initial1 : DbMigration
    {
        public override void Up()
        {
            //Categories
            Sql("INSERT INTO Categories(CategoryName) Values('Clip')");//1
            Sql("INSERT INTO Categories(CategoryName) Values('Envelope')");//2
            Sql("INSERT INTO Categories(CategoryName) Values('Eraser')");
            Sql("INSERT INTO Categories(CategoryName) Values('Exercise')");
            Sql("INSERT INTO Categories(CategoryName) Values('File')");
            Sql("INSERT INTO Categories(CategoryName) Values('Folder')");
            Sql("INSERT INTO Categories(CategoryName) Values('Pad')");
            Sql("INSERT INTO Categories(CategoryName) Values('Paper')");
            Sql("INSERT INTO Categories(CategoryName) Values('Pen')");
            Sql("INSERT INTO Categories(CategoryName) Values('Puncher')");
            Sql("INSERT INTO Categories(CategoryName) Values('Ruler')");
            Sql("INSERT INTO Categories(CategoryName) Values('Scissors')");
            Sql("INSERT INTO Categories(CategoryName) Values('Sharpener')");
            Sql("INSERT INTO Categories(CategoryName) Values('Shorthand')");
            Sql("INSERT INTO Categories(CategoryName) Values('Stapler')");
            Sql("INSERT INTO Categories(CategoryName) Values('Tacks')");
            Sql("INSERT INTO Categories(CategoryName) Values('Tape')");
            Sql("INSERT INTO Categories(CategoryName) Values('Transparency')");
            Sql("INSERT INTO Categories(CategoryName) Values('Tray')");

            //CollectionPoints
            Sql("INSERT INTO CollectionPoints(CollectionPointName) Values('Stationery Store')");
            Sql("INSERT INTO CollectionPoints(CollectionPointName) Values('Management School')");
            Sql("INSERT INTO CollectionPoints(CollectionPointName) Values('Medical School')");
            Sql("INSERT INTO CollectionPoints(CollectionPointName) Values('Engineering School')");
            Sql("INSERT INTO CollectionPoints(CollectionPointName) Values('Science School')");
            Sql("INSERT INTO CollectionPoints(CollectionPointName) Values('University Hospital')");

            //StoreEmployees
            Sql("INSERT INTO StoreEmployees(FirstName,LastName,MiddleName,Phone,Address,Gender,Email,Designation) Values('Esther','Tan','','8888 9999','Blk 321C Kranji Road,#02-302,Singapore 650 321','M','esther@u.logic.edu','Clerk')");
            Sql("INSERT INTO StoreEmployees(FirstName,LastName,MiddleName,Phone,Address,Gender,Email,Designation) Values('Xiao','Zhan','Zhan','8834 7777','Blk 321C Kranji Road,#02-302,Singapore 650 321','M','xiaozhanzhan@u.logic.edu','Supervisor')");
            Sql("INSERT INTO StoreEmployees(FirstName,LastName,MiddleName,Phone,Address,Gender,Email,Designation) Values('Angela','Baby','','8834 6666','Blk 321C Kranji Road,#02-302,Singapore 650 321','M','baby@u.logic.edu','Manager')");
            Sql("INSERT INTO StoreEmployees(FirstName,LastName,MiddleName,Phone,Address,Gender,Email,Designation) Values('Yusof','bin','Ishak','8834 7761','Blk 321C Kranji Road,#02-302,Singapore 650 321','F','yusofbin@u.logic.edu','Clerk')");
            Sql("INSERT INTO StoreEmployees(FirstName,LastName,MiddleName,Phone,Address,Gender,Email,Designation) Values('Fandi','Ahmad','','8904 2673','Blk 234D Woodlands Drive, #12-312,Singapore 604 234,','M','fandiahmad@u.logic.edu','Clerk')");
            Sql("INSERT INTO StoreEmployees(FirstName,LastName,MiddleName,Phone,Address,Gender,Email,Designation) Values('Ang','Peng','Siong','8746 9035','Blk 546A Clementi Road, #04-420,Singapore 671 546','M','angpeng@u.logic.edu','Clerk')");
            Sql("INSERT INTO StoreEmployees(FirstName,LastName,MiddleName,Phone,Address,Gender,Email,Designation) Values('Jeanette','Aw','','88097723','Blk 664B Midview Drive, #12-803, Singapore 602 664','F','jeanetteaw@u.logic.edu','Clerk')");
            Sql("INSERT INTO StoreEmployees(FirstName,LastName,MiddleName,Phone,Address,Gender,Email,Designation) Values('Vivian','Balakrishnan','','8349 2317','Blk 555A Sembawang Road, #08-424,Singapore 673 555','M','vivibala@u.logic.edu','Clerk')");


            //Suppliers
            Sql("INSERT INTO Suppliers(SupplierCode, SupplierName, ContactName, Phone, Fax, Address, GSTNumber) Values('ALPA','Alpha Office Supplies','Ms Irene Tan','461 9928','461 2238','Blk 1128, Ang Mo Kio Industrial Park,#02-1108 Ang Mo Kio Street 62, Singapore 622262','MR-8500440-2')");
            Sql("INSERT INTO Suppliers(SupplierCode, SupplierName, ContactName, Phone, Fax, Address, GSTNumber) Values('CHEP','Cheap Stationer','Mr Soh Kway Koh','354 3234','474 2434','Blk 34, Clementi Road,#07-02 Ban Ban Soh Building,Singapore 110525','MR-8502434-9')");
            Sql("INSERT INTO Suppliers(SupplierCode, SupplierName, ContactName, Phone, Fax, Address, GSTNumber) Values('BANE','Banes Shop','Mr Loh Ah Pek','478 1234','479 2434','Blk 124, Alexandra Road,#03-04 Banes Building,Singapore 550315','MR-8200420-2')");
            Sql("INSERT INTO Suppliers(SupplierCode, SupplierName, ContactName, Phone, Fax, Address, GSTNumber) Values('OMEG','Omega Stationery Supplier','Mr Ronnie Ho','767 1233','767 1234','Blk 11, Hillview Avenue,#03-04,Singapore 679036','MR-8555330-1')");
            Sql("INSERT INTO Suppliers(SupplierCode, SupplierName, ContactName, Phone, Fax, Address, GSTNumber) Values('YUNN','Yunnan General Supplier','Ms Ang Peng Siong','3456 1974','4728 5362','1 Irving Pl, #08-01 The Commerze@Irving, Singapore 369546','MR-8367239-6')");
            Sql("INSERT INTO Suppliers(SupplierCode, SupplierName, ContactName, Phone, Fax, Address, GSTNumber) Values('MILN','Millen Stationery Supplier','Mr Taufik Batisah','4902 7813','3901 2783','#06-76, Midview City, 22 Sin Ming Lane, Singapore 573969','MR-8529040-4')");
            Sql("INSERT INTO Suppliers(SupplierCode, SupplierName, ContactName, Phone, Fax, Address, GSTNumber) Values('LION','Lion Stationery','Ms Felicia Chin','8234 5682','3489 6715','20 Maxwell Rd, #09-17 Maxwell House, Singapore 069113','MR-8290420-6')");
            Sql("INSERT INTO Suppliers(SupplierCode, SupplierName, ContactName, Phone, Fax, Address, GSTNumber) Values('EVGN','Evergreeen Office Supplies','Ms Kym Ng','4509 3592','4782 3840','60 Paya Lebar Rd, 09-36, Singapore 409051','MR-8389330-1')");
            Sql("INSERT INTO Suppliers(SupplierCode, SupplierName, ContactName, Phone, Fax, Address, GSTNumber) Values('OFNS','Office N Supplies','Mr Heng Swee Keat','4996 3471','4732 8210','1 Scotts Road #24-10, Shaw Centre, Singapore 228208','MR-8455330-2')");
            Sql("INSERT INTO Suppliers(SupplierCode, SupplierName, ContactName, Phone, Fax, Address, GSTNumber) Values('ALMK','Allmarks Stationery Company','Ms Maia Lee','3389 4714','3390 4572','4 Kaki Bukit Pl, Eunos Techpark, Singapore 415979','MR-8401134-8')");


        }

        public override void Down()
        {
        }
    }
}
