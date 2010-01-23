using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.Common;
using CSCL;
using CSCL.Database.SQLite;

using System.IO; //TODO Wieder entfernen nachdem es läuft

namespace CSCL.Database.SQLite
{
	public partial class SQLite
	{
		//TODO
		//- Verschlüsselung

		#region Variablen
		public SQLiteDatabase InstSQLiteDatabase;
		#endregion

		#region Hilfsfunktionen
		/// <summary>
		/// Gibt anhand des .NET Datentypes 
		/// den SQLite Datentyp zurück
		/// 
		///SQLite 3 Typen
		///NULL. The value is a NULL value.
		///INTEGER. The value is a signed integer, stored in 1, 2, 3, 4, 6, or 8 bytes depending on the magnitude of the value.
		///REAL. The value is a floating point value, stored as an 8-byte IEEE floating point number.
		///TEXT. The value is a text string, stored using the database encoding (UTF-8, UTF-16BE or UTF-16-LE).
		///BLOB. The value is a blob of data, stored exactly as it was input.
		/// </summary>
		/// <param name="datatype"></param>
		/// <returns></returns>
		public static string GetSQLiteDatatype(string datatype)
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
						return "REAL";
					}
				case "System.String":
					{
						return "TEXT";
					}
				case "System.Byte[]":
					{
						return "BLOB";
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

		#region Erzeugung, Öffnen und Schließen der Datenbank
		public SQLite(string filename)
		{
			InstSQLiteDatabase=new SQLiteDatabase(filename);
		}

		public void CloseDatabase()
		{
			InstSQLiteDatabase.CloseDatabase();
		}
		#endregion

		#region Erzeugen & Löschen von Tabellen
		public void CreateTable(DataTable table)
		{
			string tblName;
			List<string> Colums=new List<string>();

			tblName=table.TableName;
			if(tblName=="") throw new Exception("Can't create table whitout name!");

			for(int i=0;i<table.Columns.Count;i++)
			{
				string Column=table.Columns[i].Caption;

				//SQLite Datentyp ermitteln
				Column+=" "+GetSQLiteDatatype(table.Columns[i].DataType.FullName);

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
			for(int i=0;i<Colums.Count;i++)
			{
				if(i==Colums.Count-1) sqlCommand+=" "+Colums[i];
				else sqlCommand+=" "+Colums[i]+",";
			}
			sqlCommand+=")";

			InstSQLiteDatabase.ExecuteNonQuery(sqlCommand);
		}

		/// <summary>
		/// Entfernt eine Tabelle aus der Datenbank
		/// </summary>
		/// <param name="tblName"></param>
		public void RemoveTable(string tblName)
		{
			string sqlCommand=String.Format("drop table \"{0}\";", tblName);
			InstSQLiteDatabase.ExecuteNonQuery(sqlCommand);
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
		#endregion

		#region Datensätze erzeugen, editieren & löschen
		/// <summary>
		/// Fügt Daten zur einer Tabelle hinzu
		/// </summary>
		/// <param name="addData"></param>
		public void InsertData(DataTable insertData)
		{
			string sqlCommand=String.Format("INSERT INTO \"{0}\" (", insertData.TableName);

			//Columns
			for(int i=0;i<insertData.Columns.Count;i++)
			{
				if(i==insertData.Columns.Count-1) sqlCommand+=" "+insertData.Columns[i];
				else sqlCommand+=" "+insertData.Columns[i]+",";
			}
			sqlCommand+=") VALUES";
			string tmpSqlCommand=sqlCommand;

			sqlCommand=tmpSqlCommand;
			sqlCommand+=" (";

			for(int j=0;j<insertData.Columns.Count;j++)
			{
				if(j==insertData.Columns.Count-1) sqlCommand+=String.Format("?");
				else sqlCommand+=String.Format("?,");
			}

			sqlCommand+=")";

			SQLiteVdbe tmpVdbe=new SQLiteVdbe(InstSQLiteDatabase, sqlCommand);

			//Rows (Daten)
			for(int i=0;i<insertData.Rows.Count;i++)
			{
				tmpVdbe.Reset();

				for(int j=0;j<insertData.Columns.Count;j++)
				{
					tmpVdbe.BindObjectToType(j+1, insertData.Rows[i].ItemArray[j]);
				}

				tmpVdbe.ExecuteStep();
			}

			tmpVdbe.Close();
		}

		/// <summary>
		/// Updatet bestimmte Daten
		/// </summary>
		/// <param name="insertData"></param>
		public void UpdateData(DataTable updateData, string PrimaryKey, string TableName)
		{
			//TODO: TableName entfernen?
			updateData.TableName=TableName;

			string sqlCommand=String.Format("UPDATE \"{0}\" SET ", updateData.TableName);

			string tmpSqlCommand=sqlCommand;

			//Rows (Daten)
			for(int i=0;i<updateData.Rows.Count;i++)
			{
				sqlCommand=tmpSqlCommand;

				for(int j=0;j<updateData.Columns.Count;j++)
				{
					if(updateData.Columns[j].Caption!=PrimaryKey)
					{
						//Parameters zusammenbauen
						if(j==updateData.Columns.Count-1) sqlCommand+=String.Format("{0}=?", updateData.Columns[j].Caption);
						else sqlCommand+=String.Format("{0}=?,", updateData.Columns[j].Caption);
					}
				}

				sqlCommand+=" WHERE "+PrimaryKey+" = "+updateData.Rows[i][PrimaryKey].ToString()+";";

				SQLiteVdbe tmpVdbe=new SQLiteVdbe(InstSQLiteDatabase, sqlCommand);

				tmpVdbe.Reset();

				int zTmp=1;

				for(int j=0;j<updateData.Columns.Count;j++)
				{
					if(updateData.Columns[j].Caption!=PrimaryKey)
					{
						//tmpVdbe.BindObjectToType(j+1, updateData.Rows[i].ItemArray[j]);
						tmpVdbe.BindObjectToType(zTmp, updateData.Rows[i].ItemArray[j]);
						zTmp++;
					}
				}

				tmpVdbe.ExecuteStep();

				tmpVdbe.Close();
			}

			////string update="UPDATE Root SET strIndex = ? WHERE intIndex = 2324";
			//SQLiteVdbe tmpVdbe=new SQLiteVdbe(InstSQLiteDatabase, sqlCommand);

			////Rows (Daten)

			//for(int i=0;i<updateData.Rows.Count;i++)
			//{
			//    tmpVdbe.Reset();

			//    int zTmp=1;

			//    for(int j=0;j<updateData.Columns.Count;j++)
			//    {
			//        if(updateData.Columns[j].Caption!=PrimaryKey)
			//        {
			//            //tmpVdbe.BindObjectToType(j+1, updateData.Rows[i].ItemArray[j]);
			//            tmpVdbe.BindObjectToType(zTmp, updateData.Rows[i].ItemArray[j]);
			//            zTmp++;
			//        }
			//    }

			//    tmpVdbe.ExecuteStep();
			//}

			//tmpVdbe.Close();
		}

		public void RemoveEntry(string table, string key, string value)
		{
			string sqlCommand=String.Format("DELETE FROM {0} WHERE {1}={2}", table, key, value);
			InstSQLiteDatabase.ExecuteNonQuery(sqlCommand);
		}
		#endregion

		#region Allgemeine Query Funktionen
		public void ExecuteNonQuery(string sqlCommand)
		{
			//TODO: Int Rückgabe für geänmderte Datensätze?
			InstSQLiteDatabase.ExecuteNonQuery(sqlCommand);
		}

		/// <summary>
		/// Führt eine Query aus und gibt sie zurück
		/// </summary>
		/// <param name="sqlCommand"></param>
		/// <returns></returns>
		public DataTable ExecuteQuery(string sqlCommand)
		{
			return InstSQLiteDatabase.ExecuteQuery(sqlCommand);
		}
		#endregion

		#region Informationsfunktionen
		/// <summary>
		/// Gibt eine Liste aller Tabellen der Datenbank zurück
		/// </summary>
		/// <returns></returns>
		public List<string> GetTables()
		{
			return InstSQLiteDatabase.GetTables();
			//// executes query that select names of all tables and views in master table of every database
			//string sqlCommand="SELECT name FROM sqlite_master "+
			//                            "WHERE type IN ('table','view') AND name NOT LIKE 'sqlite_%'"+
			//                            "UNION ALL "+
			//                            "SELECT name FROM sqlite_temp_master "+
			//                            "WHERE type IN ('table','view') "+
			//                            "ORDER BY 1";

			//DataTable dt=ExecuteQuery(sqlCommand);
			//List<string> ret=new List<string>();

			//foreach(DataRow i in dt.Rows)
			//{
			//    ret.Add(i.ItemArray[0].ToString());
			//}

			//return ret;
		}

		/// <summary>
		/// Gibt eine Leere Tabelle zurück
		/// z.B. für die Add Funktion
		/// </summary>
		/// <param name="tblName"></param>
		/// <returns></returns>
		public DataTable GetTableStructure(string tblName)
		{
			string sqlCommand="SELECT * FROM \""+tblName+"\" LIMIT 0;";
			DataTable ret=InstSQLiteDatabase.ExecuteQuery(sqlCommand);
			ret.TableName=tblName;
			return ret;
		}

		/// <summary>
		/// Überprüft die xReflection Tabelle
		/// </summary>
		/// <param name="FormatDescription"></param>
		/// <returns></returns>
		public bool CheckXReflection(string FormatDescription)
		{
			string sqlCommand=String.Format("SELECT * from xReflection WHERE Key=\"FormatDescription\";");
			DataTable dt;
			string dbFormatDescription;

			try
			{
				dt=InstSQLiteDatabase.ExecuteQuery(sqlCommand);
				dbFormatDescription=dt.Rows[0]["Value"].ToString();
			}
			catch
			{
				return false;
			}

			if(dbFormatDescription==FormatDescription) return true;
			else return false;
		}
		#endregion
	}
}