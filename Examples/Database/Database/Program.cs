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
				string mySQLusername="nutzer";
				string mySQLpassword="geheim";
				string mySQLDbName="daten";

				MySQL mySQL=new MySQL();
				mySQL.Connect(mySQLserver, mySQLDbName, mySQLusername, mySQLpassword);

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
				string postgreSQLusername="postgres";
				string postgreSQLpassword="geheim";
				string postgreSQLDbName="test";

				PostgreSQL postgresql=new PostgreSQL();
				postgresql.Connect(postgreSQLserver, postgreSQLDbName, postgreSQLusername, postgreSQLpassword);

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
