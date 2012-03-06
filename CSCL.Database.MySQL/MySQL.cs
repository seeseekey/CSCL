using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace CSCL.Database.MySQL
{
	public class MySQL : Database
	{
		//Eigenschaften
		public string Host { get; set; }
		public int Port { get; set; }
		public string Database { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }

		//Variablen
		MySqlConnection IntlMySQlConnection;
		MySqlTransaction IntlMySqlTransaction;

		//Konstruktor
		public MySQL()
		{
		}

		public MySQL(string host, int port, string database, string username, string passwort)
		{
			Host=host;
			Port=port;
			Database=database;
			Username=username;
			Password=passwort;
		}

		//Verbindung
		public override bool Connect()
		{
			try
			{
				if(Connected) Disconnect(); //Datenbank schließen falls noch eine geöffnet ist

				string cs=GetConnectionString(Host, Username, Password, Database);

				IntlMySQlConnection=new MySqlConnection();
				IntlMySQlConnection.ConnectionString=cs;
				IntlMySQlConnection.Open();

				Connected=true;
			}
			catch
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Schließt dies MySQL Datenbank
		/// </summary>
		public override void Disconnect()
		{
			IntlMySQlConnection.Close();
			IntlMySQlConnection.Dispose();
			IntlMySQlConnection=null;
			Connected=false;
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
		private int ExecuteNonQuery(MySqlCommand sqlCommand)
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
				MySqlCommand command=new MySqlCommand(sqlCommand, IntlMySQlConnection);

				DataTable ret=new DataTable();
				MySqlDataReader tmpDataReader=command.ExecuteReader();
				ret.Load(tmpDataReader);
				tmpDataReader.Close();
				return ret;
			}
			catch(MySqlException ex)
			{
				throw new Exception(ex.Message);
			}
		}
		#endregion

		//Edits
		#region Edits
		/// <summary>
		/// Fügt Daten zur einer Tabelle hinzu
		/// </summary>
		/// <param name="addData"></param>
		/// <summary>
		/// Fügt Daten zur einer Tabelle hinzu
		/// </summary>
		/// <param name="addData"></param>
		public override void InsertData(DataTable insertData)
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

			//StartTransaction();

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

			//CommitTransaction();
		}

		public override void UpdateData(DataTable updateData, string PrimaryKey)
		{
			string TableToUpdate=updateData.TableName;
			string sqlCommand=String.Format("UPDATE {0} SET ", TableToUpdate);

			string tmpSqlCommand=sqlCommand;

			//StartTransaction();

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

			//CommitTransaction();
		}

		public override void RemoveData(string table, string key, string value)
		{
			string sqlCommand=String.Format("DELETE FROM {0} WHERE {1}={2}", table, key, value);
			ExecuteNonQuery(sqlCommand);
		}
		#endregion

		//Transaktionen
		public override void StartTransaction()
		{
			IntlMySqlTransaction=IntlMySQlConnection.BeginTransaction();
		}

		public override void CommitTransaction()
		{
			IntlMySqlTransaction.Commit();
			IntlMySqlTransaction=null;
		}

		//Tabellen
		#region Tabellen
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
			if(tblName=="") throw new Exception("Can't create table whitout name!");

			for(int i=0; i<table.Columns.Count; i++)
			{
				string Column=table.Columns[i].Caption;

				//neuer text:
				string type=GetDBSystemDatatype(table.Columns[i].DataType.FullName);
				if(type=="INTEGER"&&table.Columns[i].AutoIncrement) Column+=" AUTOINCREMENT";
				else Column+=" "+type;

				if(table.Columns[i].AllowDBNull==false) Column+=" NOT NULL";

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
			DataTable dt=IntlMySQlConnection.GetSchema("Tables");

			List<string> ret=new List<string>();

			for(int i=0; i<dt.Rows.Count; i++)
			{
				if(dt.Rows[i]["TABLE_TYPE"].ToString()=="BASE TABLE")
				{
					ret.Add(dt.Rows[i]["TABLE_NAME"].ToString());
				}
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
			string sqlCommand="SELECT * FROM \""+tblName+"\";";
			DataTable ret=ExecuteQuery(sqlCommand);
			ret.Rows.Clear();
			ret.TableName=tblName;
			return ret;
		}
		#endregion

		//Sonstige Funktionen
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

		/// <summary>
		/// Gibt den DBType zurück
		/// </summary>
		/// <param name="datatype"></param>
		/// <returns></returns>
		public static MySqlDbType GetMySQLDbType(string datatype)
		{
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
	}
}
