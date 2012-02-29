using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using CSCL.Database;

namespace CSCL.Database.MySQL
{
	public class MySQL : Database
	{
		#region Private Statische Funktionen
		#region GetConnectionString
		/// <summary>
		/// Gibt den Connection String zurück
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="pooling"></param>
		/// <param name="UsePassword"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		private static string GetConnectionString(string server, string username, string password, string databasename)
		{
			return String.Format("server={0};user id={1}; password={2}; database={3}; pooling=false", server, username, password, databasename);
		}
		#endregion
		#endregion

		#region Statische Funktionen
		/// <summary>
		/// Gibt anhand des .NET Datentypes 
		/// den SQLite Datentyp zurück
		/// </summary>
		/// <param name="datatype"></param>
		/// <returns></returns>
		public override string GetDBSystemDatatype(string datatype)
		{
			switch(datatype)
			{
				case "System.Int16":
				case "System.Int32":
				case "System.Int64":
				case "System.UInt16":
				case "System.UInt32":
				case "System.UInt64":
					{
						return "INTEGER";
					}
				case "System.Decimal":
				case "System.Double":
					{
						return "DOUBLE";
					}
				case "System.String":
					{
						return "TEXT";
					}
				case "System.Byte[]":
					{
						return "IMAGE";
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
		public static MySqlDbType GetMySQLDbType(string datatype)
		{
			//http://npgsql.projects.postgresql.org/docs/manual/UserManual.html

			switch(datatype)
			{
				case "System.Boolean": return MySqlDbType.Byte;
				case "System.UInt16": return MySqlDbType.UInt16;
				case "System.Int16": return MySqlDbType.Int16;
				case "System.UInt32": return MySqlDbType.UInt32;
				case "System.Int32": return MySqlDbType.Int32;
				case "System.UInt64": return MySqlDbType.UInt64;
				case "System.Int64": return MySqlDbType.Int64;
				case "System.Decimal": return MySqlDbType.Decimal;
				case "System.Double": return MySqlDbType.Double;
				case "System.String": return MySqlDbType.Text;
				case "System.DateTime": return MySqlDbType.Timestamp;
				case "System.Byte[]": return MySqlDbType.Blob;
				default:
					{
						throw new NotImplementedException("Type not supported!");
					}
			}
		}

		/// <summary>
		/// Gibt den DBType zurück
		/// </summary>
		/// <param name="datatype"></param>
		/// <returns></returns>
		public static DbType GetDBType(string datatype)
		{
			switch(datatype)
			{
				case "System.Boolean": return DbType.Boolean;
				case "System.Int16": return DbType.Int16;
				case "System.Int32": return DbType.Int32;
				case "System.Int64": return DbType.Int64;
				case "System.UInt16": return DbType.UInt16;
				case "System.UInt32": return DbType.UInt32;
				case "System.UInt64": return DbType.UInt64;
				case "System.Decimal": return DbType.Decimal;
				case "System.Double": return DbType.Double;
				case "System.String": return DbType.String;
				case "System.Byte[]": return DbType.Binary;
				default:
					{
						throw new Exception("Type not supported!");
					}
			}
		}
		#endregion

		#region Variablen
		MySqlConnection IntlMySQlConnection;
		//OdbcConnection IntlOdbcConnection=null;
		//OdbcTransaction IntlSQLiteTransaction=null;
		bool connected=false;
		#endregion

		#region Eigenschaften
		public bool Connected
		{
			get
			{
				return connected;
			}
		}
		#endregion

		#region Allgemeine Dinge
		/// <summary>
		/// Öffnet eine MySQL Datenbank
		/// </summary>
		/// <param name="filename"></param>
		public bool Connect(string server, string dbName, string username, string password)
		{
			try
			{
				if(connected) CloseDatabase(); //Datenbank schließen falls nóch eine geöffnet ist

				string cs=GetConnectionString(server, username, password, dbName);

				IntlMySQlConnection=new MySqlConnection();
				IntlMySQlConnection.ConnectionString=cs;
				IntlMySQlConnection.Open();

				connected=true;
			}
			catch
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Schließt dies SQLite Datenbank
		/// </summary>
		public void CloseDatabase()
		{
			IntlMySQlConnection.Close();
			IntlMySQlConnection.Dispose();
			IntlMySQlConnection=null;
			connected=false;
		}

		/// <summary>
		/// Startet eine Transaction
		/// </summary>
		public void StartTransaction()
		{
			//IntlSQLiteTransaction = IntlOdbcConnection.BeginTransaction();
		}

		/// <summary>
		/// Commitet eine Transaction
		/// </summary>
		public void CommitTransaction()
		{
			//IntlSQLiteTransaction.Commit();
		}
		#endregion

		#region Daten Edits
		/// <summary>
		/// Erzeugt eine Tabelle aus einer Datatable
		/// in der Datenbank
		/// </summary>
		/// <param name="table"></param>
		/// <returns></returns>
		public void CreateTable(DataTable table)
		{
			string tblName;
			List<string> Colums=new List<string>();

			tblName=table.TableName;
			if(tblName=="") throw new Exception("Can't create table whitout name!");

			for(int i=0; i<table.Columns.Count; i++)
			{
				string Column=table.Columns[i].Caption;

				//neuer text:
				string type=GetDBSystemDatatype(table.Columns[i].DataType.FullName);
				if(type=="INTEGER"&&table.Columns[i].AutoIncrement)
					Column+=" AUTOINCREMENT";
				else
					Column+=" "+type;

				if(table.Columns[i].AllowDBNull==false)
					Column+=" NOT NULL";


				#region alter text
				////Typ bestimmen
				////SQLite 3 Typen
				////NULL. The value is a NULL value.
				////INTEGER. The value is a signed integer, stored in 1, 2, 3, 4, 6, or 8 bytes depending on the magnitude of the value.
				////REAL. The value is a floating point value, stored as an 8-byte IEEE floating point number.
				////TEXT. The value is a text string, stored using the database encoding (UTF-8, UTF-16BE or UTF-16-LE).
				////BLOB. The value is a blob of data, stored exactly as it was input.

				////SQLite Datentyp ermitteln
				//Column += " " + GetAccessDatatype(table.Columns[i].DataType.FullName);

				////Primärschlüssel
				//if (table.Columns[i].Unique = true && table.Columns[i].AllowDBNull == false)
				//{
				//    Column += " PRIMARY KEY";
				//}

				//if (table.Columns[i].AutoIncrement == true)
				//{
				//    Column += " AUTOINCREMENT";
				//}
				#endregion

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
		public void RemoveTable(string tblName)
		{
			string sqlCommand=String.Format("drop table \"{0}\";", tblName);
			ExecuteNonQuery(sqlCommand);
		}

		/// <summary>
		/// Löscht mehrere Tabellen
		/// </summary>
		/// <param name="Tables"></param>
		public void RemoveTables(List<string> Tables)
		{
			foreach(string i in Tables)
			{
				RemoveTable(i);
			}
		}

		public void RemoveEntry(string table, string key, string value)
		{
			string sqlCommand=String.Format("DELETE FROM {0} WHERE {1}={2}", table, key, value);
			ExecuteNonQuery(sqlCommand);
		}

		/// <summary>
		/// Fügt Daten zur einer Tabelle hinzu
		/// </summary>
		/// <param name="addData"></param>
		/// <summary>
		/// Fügt Daten zur einer Tabelle hinzu
		/// </summary>
		/// <param name="addData"></param>
		public void InsertData(DataTable insertData)
		{
			string sqlCommand=String.Format("INSERT INTO \"{0}\" (", insertData.TableName);

			//Columns
			for(int i=0; i<insertData.Columns.Count; i++)
			{
				if(insertData.Columns[i].ToString()=="IndexID")
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

				MySqlCommand cmd=new MySqlCommand();

				for(int j=0; j<insertData.Columns.Count; j++)
				{
					MySqlParameter parameter=new MySqlParameter();
					//parameter.NpgsqlDbType=NpgsqlTypes.NpgsqlDbType.Point;
					parameter.MySqlDbType=GetMySQLDbType(insertData.Columns[j].DataType.FullName);
					parameter.ParameterName=insertData.Columns[j].Caption;
					parameter.Value=insertData.Rows[i].ItemArray[j];

					if(parameter.ParameterName=="IndexID")
					{
						continue;
					}

					cmd.Parameters.Add(parameter);

					//Parameters zusammenbauen
					if(j==insertData.Columns.Count-1) sqlCommand+=String.Format("@{0}", insertData.Columns[j].Caption);
					else sqlCommand+=String.Format("@{0},", insertData.Columns[j].Caption);
				}

				sqlCommand+=")";

				//Kommando ausführen (pro Datensatz)
				cmd.CommandText=sqlCommand;
				ExecuteNonQuery(cmd);
			}

			CommitTransaction();
		}


		public void UpdateData(DataTable updateData, string PrimaryKey, string TableToUpdate)
		{
			string sqlCommand=String.Format("UPDATE {0} SET ", TableToUpdate);

			string tmpSqlCommand=sqlCommand;

			StartTransaction();

			//Rows (Daten)
			for(int i=0; i<updateData.Rows.Count; i++)
			{
				sqlCommand=tmpSqlCommand;

				MySqlCommand cmd=new MySqlCommand();

				//Columns
				for(int j=0; j<updateData.Columns.Count; j++)
				{
					MySqlParameter parameter=new MySqlParameter();
					parameter.DbType=GetDBType(updateData.Columns[j].DataType.FullName);
					parameter.ParameterName=updateData.Columns[j].Caption;
					parameter.Value=updateData.Rows[i].ItemArray[j];
					cmd.Parameters.Add(parameter);

					//Parameters zusammenbauen
					if(j==updateData.Columns.Count-1) sqlCommand+=String.Format("{0}=?", updateData.Columns[j].Caption);
					else sqlCommand+=String.Format("{0}=?,", updateData.Columns[j].Caption);
				}

				//sqlCommand += " WHERE " + PrimaryKey + " = " +  + ";";
				sqlCommand+=" WHERE "+PrimaryKey+" = '"+updateData.Rows[i][PrimaryKey].ToString()+"';";

				//Kommando ausführen (pro Datensatz)
				cmd.CommandText=sqlCommand;
				ExecuteNonQuery(cmd);
			}

			CommitTransaction();
		}
		#endregion

		#region Abfragen
		/// <summary>
		/// Führt ein Kommando aus
		/// </summary>
		/// <param name="sqlCommand"></param>
		public override int ExecuteNonQuery(string sqlCommand)
		{
			try
			{
				MySqlCommand SQLiteCommand=new MySqlCommand(sqlCommand, IntlMySQlConnection);
				return SQLiteCommand.ExecuteNonQuery();
			}
			catch(MySqlException sqlex)
			{
				throw new Exception(sqlex.Message);
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Führt ein OdbcCommand aus
		/// </summary>
		/// <param name="sqlCommand"></param>
		/// <returns></returns>
		public int ExecuteNonQuery(MySqlCommand sqlCommand)
		{
			try
			{
				sqlCommand.Connection=IntlMySQlConnection;
				return sqlCommand.ExecuteNonQuery();
			}
			catch(MySqlException sqlex)
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
				MySqlCommand SQLiteCommand=new MySqlCommand(sqlCommand, IntlMySQlConnection);

				DataTable ret=new DataTable();
				MySqlDataReader tmpDataReader=SQLiteCommand.ExecuteReader();
				ret.Load(tmpDataReader);
				tmpDataReader.Close();
				return ret;
			}
			catch(MySqlException ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Gibt eine Liste aller Tabellen der Datenbank zurück
		/// </summary>
		/// <returns></returns>
		public override List<string> GetTables()
		{
			DataTable dt=IntlMySQlConnection.GetSchema("Tables");

			//executes query that select names of all tables and views in master table of every database
			//string sqlCommand = "SELECT * FROM MSysObjects WHERE Type=1 ORDER BY Name";

			//DataTable dt = ExecuteQuery(sqlCommand);

			List<string> ret=new List<string>();

			for(int i=0; i<dt.Rows.Count; i++)
			{
				if(dt.Rows[i]["TABLE_TYPE"].ToString()=="BASE TABLE")
				{
					ret.Add(dt.Rows[i]["TABLE_NAME"].ToString());
				}
			}

			//foreach (DataRow i in dt.Rows)
			//{
			//    ret.Add(i.ItemArray[0].ToString());
			//}

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
			string sqlCommand="SELECT * FROM \""+tblName+"\";";
			DataTable ret=ExecuteQuery(sqlCommand);
			ret.Rows.Clear();
			ret.TableName=tblName;
			return ret;
		}

		/// <summary>
		/// Gibt eine Tabelle zurück
		/// </summary>
		/// <param name="tblName"></param>
		/// <returns></returns>
		public DataTable GetTable(string tblName)
		{
			string sqlCommand="SELECT * FROM \""+tblName+"\";";
			DataTable ret=ExecuteQuery(sqlCommand);
			ret.TableName=tblName;
			return ret;
		}
		#endregion
	}
}
