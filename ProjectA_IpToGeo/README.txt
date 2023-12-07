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
-------------------------------

