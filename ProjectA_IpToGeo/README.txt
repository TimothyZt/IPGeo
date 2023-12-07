-------------------------------First Commit
Project A Ip_To_Geo Created;

-------------------------------Second Commit
Add 2 Model Classes:

	(1) The GeoliteCityIpv4_Int model class corresponds to the data type of the database entity table.

	(2) The GeoliteCityIpv4_String model class corresponds to the data type of the data source file, and its main purpose is to extract source file data.

Build the Efcore operating environment and add some libraries.

Configure the connection string required by Mysql in the appsettings.json file.

Register the DbContext service required by the project in the Program.cs file.

Implement 2 algorithms:  Ip_to_Decimal And Decimal_To_Ip

Implemented an HttpGet request method:
	input a single IP, and query all the corresponding information. If the returned value is empty, it means it is outside the collection range.

-------------------------------Third Commit

Add 1 Service: UpdateIpGeoService.cs 

	Implemented the method of requesting the data source to download the data set and decompressing all downloaded .gz type compressed packages.

-------------------------------Forth Commit

Add 1 Model mapping class：GeoMap.cs  

Use csvhelper to read and write data from large csv files, convert the appropriate type and insert the data into the database.

Use the bulkInsert method in the Efcore.Extansion library to replace the commonly used mydbcontext.Add() and mydbcontext.savechanges();

Use Raw SQL language to dynamically create and delete database tables.