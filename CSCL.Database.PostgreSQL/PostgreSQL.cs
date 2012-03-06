using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CSCL.Database;
using NpgsqlTypes;
using Npgsql;

namespace CSCL.Database.PostgreSQL
{
	public class PostgreSQL : Database
	{
		//Eigenschaften
		public string Host { get; set; }
		public int Port { get; set; }
		public string Database { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }

		//Variablen
		NpgsqlConnection IntlPostgreSQLConnection=null;
		NpgsqlTransaction IntllPostgreSQLTransaction=null;

		//Konstruktor
		public PostgreSQL()
		{
		}

		public PostgreSQL(string host, int port, string database, string username, string passwort)
		{
			Host=host;
			Port=port;
			Database=database;
			Username=username;
			Password=passwort;
		}

		//Verbindung
		/// <summary>
		/// Öffnet eine SQLite Datenbank
		/// </summary>
		/// <param name="filename"></param>
		public override bool Connect()
		{
			try
			{
				if(Connected) Disconnect(); //Datenbank schließen falls noch eine geöffnet ist

				string cs=GetConnectionString(Host, Database, Username, Password);

				IntlPostgreSQLConnection=new NpgsqlConnection();
				IntlPostgreSQLConnection.ConnectionString=cs;

				IntlPostgreSQLConnection.Open();

				Connected=true;
			}
			catch
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Schließt die PostgreSQL Datenbank
		/// </summary>
		public override void Disconnect()
		{
			if(Connected)
			{
				IntlPostgreSQLConnection.Close();
				IntlPostgreSQLConnection.Dispose();
				IntlPostgreSQLConnection=null;
				Connected=false;
			}
		}

		//Abfragen
		#region Abfragen
		/// <summary>
		/// Führt ein Kommando aus
		/// </summary>
		/// <param name="sqlCommand"></param>
		public override int ExecuteNonQuery(string sqlCommand)
		{
			try
			{
				NpgsqlCommand InstSQLiteCommand=new NpgsqlCommand(sqlCommand, IntlPostgreSQLConnection);
				return InstSQLiteCommand.ExecuteNonQuery();
			}
			catch(NpgsqlException sqlex)
			{
				throw sqlex;
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Führt ein SQLiteCommand aus
		/// </summary>
		/// <param name="sqlCommand"></param>
		/// <returns></returns>
		private int ExecuteNonQuery(NpgsqlCommand sqlCommand)
		{
			try
			{
				sqlCommand.Connection=IntlPostgreSQLConnection;
				return sqlCommand.ExecuteNonQuery();
			}
			catch(NpgsqlException sqlex)
			{
				throw new Exception(sqlex.Message);
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Führt eine Query aus und gibt sie zurück
		/// </summary>
		/// <param name="sqlCommand"></param>
		/// <returns></returns>
		public override DataTable ExecuteQuery(string sqlCommand)
		{
			try
			{
				GC.Collect(2, GCCollectionMode.Forced);

				NpgsqlCommand SQLiteCommand=new NpgsqlCommand(sqlCommand, IntlPostgreSQLConnection);

				DataTable ret=new DataTable();
				NpgsqlDataReader tmpDataReader=SQLiteCommand.ExecuteReader();
				ret.Load(tmpDataReader);
				tmpDataReader.Close();
				return ret;
			}
			catch(NpgsqlException sqlex)
			{
				throw new Exception(sqlex.Message);
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		private DataTable ExecuteQuery(NpgsqlCommand sqlCommand)
		{
			try
			{
				GC.Collect(2, GCCollectionMode.Forced);

				sqlCommand.Connection=IntlPostgreSQLConnection;

				DataTable ret=new DataTable();
				NpgsqlDataReader tmpDataReader=sqlCommand.ExecuteReader();
				ret.Load(tmpDataReader);
				tmpDataReader.Close();
				return ret;
			}
			catch(NpgsqlException sqlex)
			{
				throw sqlex;
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		#endregion

		//Edits
		public override void InsertData(DataTable insertData)
		{
			InsertData(insertData, false);
		}

		/// <summary>
		/// Fügt Daten zur einer Tabelle hinzu
		/// </summary>
		/// <param name="addData"></param>
		private DataTable InsertData(DataTable insertData, bool returningIndexID)
		{
			string sqlCommand=String.Format("INSERT INTO \"{0}\" (", insertData.TableName);
			DataTable ret=null;

			//Columns
			for(int i=0; i<insertData.Columns.Count; i++)
			{
				if(insertData.Columns[i].ToString()=="IndexID")
				{
					continue;
				}
				if(insertData.Columns[i].ToString()=="idx")
				{
					continue;
				}

				if(i==insertData.Columns.Count-1) sqlCommand+=" \""+insertData.Columns[i]+"\"";
				else sqlCommand+=" \""+insertData.Columns[i]+"\",";
			}
			sqlCommand+=") VALUES";
			string tmpSqlCommand=sqlCommand;

			StartTransaction();

			//Rows (Daten)
			for(int i=0; i<insertData.Rows.Count; i++)
			{
				sqlCommand=tmpSqlCommand;
				sqlCommand+=" (";

				NpgsqlCommand cmd=new NpgsqlCommand();

				for(int j=0; j<insertData.Columns.Count; j++)
				{
					NpgsqlParameter parameter=new NpgsqlParameter();
					parameter.NpgsqlDbType=GetNpgsqlDbType(insertData.Columns[j].DataType.FullName);
					parameter.ParameterName=insertData.Columns[j].Caption;
					parameter.Value=insertData.Rows[i].ItemArray[j];

					if(parameter.ParameterName=="IndexID")
					{
						continue;
					}
					if(parameter.ParameterName=="idx")
					{
						continue;
					}

					cmd.Parameters.Add(parameter);

					//Parameters zusammenbauen
					if(j==insertData.Columns.Count-1) sqlCommand+=String.Format("@{0}", insertData.Columns[j].Caption);
					else sqlCommand+=String.Format("@{0},", insertData.Columns[j].Caption);
				}

				if(returningIndexID)
				{
					sqlCommand+=String.Format(") RETURNING \"{0}\".\"IndexID\"", insertData.TableName);
				}
				else
				{
					sqlCommand+=")";
				}

				//Kommando ausführen (pro Datensatz)
				sqlCommand=sqlCommand.Replace(",)", ")");
				cmd.CommandText=sqlCommand;
				ret=ExecuteQuery(cmd);
			}

			CommitTransaction();

			return ret;
		}

		/// <summary>
		/// Updatet bestimmte Daten
		/// </summary>
		/// <param name="insertData"></param>
		public override void UpdateData(DataTable updateData, string PrimaryKey)
		{
			string sqlCommand=String.Format("UPDATE \"{0}\" SET ", updateData.TableName);

			string tmpSqlCommand=sqlCommand;

			StartTransaction();

			//Rows (Daten)
			for(int i=0; i<updateData.Rows.Count; i++)
			{
				sqlCommand=tmpSqlCommand;

				NpgsqlCommand cmd=new NpgsqlCommand();

				for(int j=0; j<updateData.Columns.Count; j++)
				{
					NpgsqlParameter parameter=new NpgsqlParameter();
					parameter.NpgsqlDbType=GetNpgsqlDbType(updateData.Columns[j].DataType.FullName);
					parameter.ParameterName=updateData.Columns[j].Caption;
					parameter.Value=updateData.Rows[i].ItemArray[j];
					cmd.Parameters.Add(parameter);

					//Parameters zusammenbauen
					if(j==updateData.Columns.Count-1) sqlCommand+=String.Format("\"{0}\"=@{0}", updateData.Columns[j].Caption);
					else sqlCommand+=String.Format("\"{0}\"=@{0},", updateData.Columns[j].Caption);
				}

				sqlCommand+=" WHERE \""+updateData.TableName+"\".\""+PrimaryKey+"\" = "+updateData.Rows[i][PrimaryKey].ToString()+";";

				//Kommando ausführen (pro Datensatz)
				cmd.CommandText=sqlCommand;
				ExecuteNonQuery(cmd);
			}

			CommitTransaction();
		}

		public override void RemoveData(string table, string key, string value)
		{
			string sqlCommand=String.Format("DELETE FROM public.\"{0}\" WHERE \"{0}\".\"{1}\"={2}", table, key, value);
			ExecuteNonQuery(sqlCommand);
		}

		//Transaktionen
		/// <summary>
		/// Startet eine Transaction
		/// </summary>
		public override void StartTransaction()
		{
			IntllPostgreSQLTransaction=IntlPostgreSQLConnection.BeginTransaction();
		}

		/// <summary>
		/// Commitet eine Transaction
		/// </summary>
		public override void CommitTransaction()
		{
			IntllPostgreSQLTransaction.Commit();
			IntllPostgreSQLTransaction=null;
		}

		//Tabellen
		/// <summary>
		/// Erzeugt eine Tabelle aus einer Datatable
		/// in der Datenbank
		/// </summary>
		/// <param name="table"></param>
		/// <returns></returns>
		public override void CreateTable(DataTable table)
		{
			string tblName;
			List<string> Colums=new List<string>();

			tblName=table.TableName;
			if(tblName=="") throw new Exception("Can't create table without name!");

			for(int i=0; i<table.Columns.Count; i++)
			{
				string Column=table.Columns[i].Caption;

				//Typ bestimmen

				//PostgreSQL Datentyp ermitteln
				Column+=" "+GetDBSystemDatatype(table.Columns[i].DataType.FullName);

				//Primärschlüssel
				if(table.Columns[i].Unique=true&&table.Columns[i].AllowDBNull==false)
				{
					Column+=" PRIMARY KEY";
				}

				if(table.Columns[i].AutoIncrement==true)
				{
					Column+=" AUTOINCREMENT";
				}

				Colums.Add(Column);
			}

			//SQL Kommando zusammenbauen
			string sqlCommand="CREATE table \""+tblName+"\" (";
			for(int i=0; i<Colums.Count; i++)
			{
				if(i==Colums.Count-1) sqlCommand+=" "+Colums[i];
				else sqlCommand+=" "+Colums[i]+",";
			}
			sqlCommand+=")";

			ExecuteNonQuery(sqlCommand);
		}

		/// <summary>
		/// Entfernt eine Tabelle aus der Datenbank
		/// </summary>
		/// <param name="tblName"></param>
		public override void RemoveTable(string tblName)
		{
			string sqlCommand=String.Format("drop table \"{0}\";", tblName);
			ExecuteNonQuery(sqlCommand);
		}

		/// <summary>
		/// Gibt eine Liste aller Tabellen der Datenbank zurück
		/// </summary>
		/// <returns></returns>
		public override List<string> GetTables()
		{
			// executes query that select names of all tables and views in master table of every database
			string sqlCommand="SELECT table_schema, table_name FROM information_schema.tables WHERE table_schema = 'public'";

			DataTable dt=ExecuteQuery(sqlCommand);
			List<string> ret=new List<string>();

			foreach(DataRow i in dt.Rows)
			{
				ret.Add(i["table_name"].ToString());
			}

			return ret;
		}

		/// <summary>
		/// Gibt eine Leere Tabelle zurück
		/// z.B. für die Add Funktion
		/// </summary>
		/// <param name="tblName"></param>
		/// <returns></returns>
		public override DataTable GetTableStructure(string tblName)
		{
			string sqlCommand=String.Format("SELECT * FROM public.\"{0}\" LIMIT 0;", tblName);
			DataTable ret=ExecuteQuery(sqlCommand);
			ret.TableName=tblName;
			return ret;
		}

		//Sonstige Funktionen
		/// <summary>
		/// Gibt den Connection String zurück
		/// </summary>
		/// <param name="server"></param>
		/// <param name="database"></param>
		/// <param name="userid"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		private static string GetConnectionString(string server, string database, string userid, string password)
		{
			return GetConnectionString(server, 5432, database, userid, password);
		}

		/// <summary>
		/// Gibt den Connection String zurück
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="pooling"></param>
		/// <param name="UsePassword"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		private static string GetConnectionString(string server, int port, string database, string userid, string password)
		{
			return String.Format("Server={0};Port={1};Database={2};User Id={3};Password={4};", server, port, database, userid, password);
		}

		/// <summary>
		/// Gibt anhand des .NET Datentypes 
		/// den Postgre Datentyp zurück
		/// </summary>
		/// <param name="datatype"></param>
		/// <returns></returns>
		public override string GetDBSystemDatatype(string datatype)
		{
			switch(datatype)
			{
				case "System.Boolean":
					{
						return "Boolean";
					}
				case "System.Int16":
				case "System.Int32":
				case "System.Int64":
				case "System.UInt16":
				case "System.UInt32":
				case "System.UInt64":
					{
						return "Bigint";
					}
				case "System.Decimal":
				case "System.Double":
					{
						return "Numeric";
					}
				case "System.String":
					{
						return "Varchar";
					}
				case "System.Byte[]":
					{
						return "Bytea";
					}
				default:
					{
						throw new Exception("Type not supported!");
					}
			}
		}

		/// <summary>
		/// Gibt den DBType zurück
		/// </summary>
		/// <param name="datatype"></param>
		/// <returns></returns>
		public static NpgsqlDbType GetNpgsqlDbType(string datatype)
		{
			switch(datatype)
			{
				case "System.Boolean": return NpgsqlDbType.Boolean;
				case "System.UInt16":
				case "System.Int16": return NpgsqlDbType.Smallint;
				case "System.UInt32":
				case "System.Int32": return NpgsqlDbType.Integer;
				case "System.UInt64":
				case "System.Int64": return NpgsqlDbType.Bigint;
				case "System.Decimal": return NpgsqlDbType.Numeric;
				case "System.Double": return NpgsqlDbType.Double;
				case "System.String": return NpgsqlDbType.Text;
				case "System.DateTime": return NpgsqlDbType.Timestamp;
				case "System.Byte[]": return NpgsqlDbType.Bytea;
				case "NpgsqlTypes.NpgsqlPoint": return NpgsqlDbType.Point;
				case "NpgsqlTypes.NpgsqlBox": return NpgsqlDbType.Box;
				case "System.Int64[]": return NpgsqlDbType.Array|NpgsqlDbType.Bigint;
				case "System.String[]": return NpgsqlDbType.Array|NpgsqlDbType.Text;
				default:
					{
						throw new Exception("Type not supported!");
					}
			}
		}

		//Noch nicht sortierte Funktionen
		public DataTable GetTableSchema(string tblName)
		{
			string sqlCommand=String.Format("SELECT a.attnum, a.attname AS field, t.typname AS type, "
			+"a.attlen AS length, a.atttypmod AS lengthvar, a.attnotnull AS notnull "
			+"FROM pg_class c, pg_attribute a, pg_type t "
			+"WHERE c.relname = '{0}' AND a.attnum > 0 AND a.attrelid = c.oid AND a.atttypid = t.oid "
			+"ORDER BY a.attnum", tblName);

			DataTable ret=ExecuteQuery(sqlCommand);
			ret.TableName=tblName;
			return ret;
		}
		
		public List<string> GetTableColumns(string tblName)
		{
			string sqlCommand=String.Format("SELECT table_name, column_name FROM INFORMATION_SCHEMA.COLUMNS WHERE table_schema = 'public' AND table_name = '{0}'", tblName);

			DataTable dt=ExecuteQuery(sqlCommand);
			List<string> ret=new List<string>();

			foreach(DataRow i in dt.Rows)
			{
				ret.Add(i.ItemArray[1].ToString());
			}

			return ret;
		}

		public string GetSELECTStubWithoutFromAndWhere(params string[] tables)
		{
			//SELECT
			string ret="SELECT ";

			foreach(string i in tables)
			{
				List<string> columns=GetTableColumns(i);

				foreach(string j in columns)
				{
					ret+=String.Format("\"{0}\".\"{1}\" AS \"{0}.{1}\",", i, j);
				}
			}

			ret=ret.TrimEnd();
			ret=ret.TrimEnd(',');
			ret+=" ";
			return ret;
		}

		public string GetSELECTStubWithoutWhere(params string[] tables)
		{
			return GetSELECTStubWithoutWhere(false, tables);
		}

		public string GetSELECTStubWithoutWhere(bool DISTINCT, params string[] tables)
		{
			//SELECT
			string ret="SELECT ";

			if(DISTINCT)
			{
				ret+="DISTINCT ";
			}

			foreach(string i in tables)
			{
				List<string> columns=GetTableColumns(i);

				foreach(string j in columns)
				{
					ret+=String.Format("\"{0}\".\"{1}\" AS \"{0}.{1}\",", i, j);
				}
			}

			//FROM
			ret=ret.TrimEnd(',');
			ret+="FROM ";

			foreach(string i in tables)
			{
				ret+=String.Format("public.\"{0}\", ", i);
			}

			ret=ret.TrimEnd();
			ret=ret.TrimEnd(',');
			ret+=" ";
			return ret;
		}

		public string GetSELECTAllWithoutFollowColumns(string Table, params string[] ExcludesColumns)
		{
			List<string> excludesColumns=new List<string>(ExcludesColumns);

			//SELECT
			string ret="SELECT ";
			List<string> columns=GetTableColumns(Table);

			foreach(string j in columns)
			{
				if(excludesColumns.IndexOf(j)==-1)
				{
					ret+=String.Format("\"{0}\".\"{1}\" AS \"{0}.{1}\",", Table, j);
				}
			}

			//FROM
			ret=ret.TrimEnd(',');
			ret+=String.Format("FROM public.\"{0}\" ", Table);

			return ret;
		}

		public string GetSELECTAllWithoutAsAndFullquoteAndWithoutFollowColumns(string Table, params string[] ExcludesColumns)
		{
			List<string> excludesColumns=new List<string>(ExcludesColumns);

			//SELECT
			string ret="SELECT ";
			List<string> columns=GetTableColumns(Table);

			foreach(string j in columns)
			{
				if(excludesColumns.IndexOf(j)==-1)
				{
					ret+=String.Format("\"{0}\",", j);
				}
			}

			//FROM
			ret=ret.TrimEnd(',');
			ret+=String.Format("FROM public.\"{0}\" ", Table);

			return ret;
		}

		#region Sonstige Funktionen
		public bool ChangePassword(string password)
		{
			string sql=String.Format("ALTER USER {0} WITH PASSWORD '{1}';", Username, password);

			try
			{
				ExecuteNonQuery(sql);
			}
			catch
			{
				return false;
			}

			return true;
		}
		#endregion
	}
}
