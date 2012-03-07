using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCL.Database.MySQL;
using CSCL.Database.SQLite;
using CSCL.Database.PostgreSQL;

namespace Database
{
	class Program
	{
		static void Main(string[] args)
		{
			////MySQL
			{
				Console.WriteLine("MySQL");

				//Connect to MySQL Database
				string mySQLserver="seeseekey.net";
				int mySQLPort=3306;
				string mySQLusername="nutzer";
				string mySQLpassword="geheim";
				string mySQLDbName="daten";

				MySQL mySQL=new MySQL(mySQLserver, mySQLPort, mySQLDbName, mySQLusername, mySQLpassword);
				mySQL.Connect();

				//get table names
				List<string> tables=mySQL.GetTables();

				foreach(string table in tables)
				{
					Console.WriteLine(table);
				}
			}

			////PostgreSQL
			{
				Console.WriteLine("PostgreSQL");

				//Connect to PostgreSQL Database
				string postgreSQLserver="seeseekey.net";
				int postgreSQLPort=5432;
				string postgreSQLusername="postgres";
				string postgreSQLpassword="geheim";
				string postgreSQLDbName="test";

				PostgreSQL postgresql=new PostgreSQL(postgreSQLserver, postgreSQLPort, postgreSQLDbName, postgreSQLusername, postgreSQLpassword);
				postgresql.Connect();

				//get table names
				List<string> tables=postgresql.GetTables();

				foreach(string table in tables)
				{
					Console.WriteLine(table);
				}
			}

			//SQLite
			{
				Console.WriteLine("SQLite");

				//Connect/Open to SQlite Database
				string sqliteDB=@"D:\test.db";
				SQLite sqlite=new SQLite(sqliteDB);

				//get table names
				List<string> tables=sqlite.GetTables();

				foreach(string table in tables)
				{
					Console.WriteLine(table);
				}
			}

			//Ende
			Console.ReadLine();
		}
	}
}
