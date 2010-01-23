using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace CSCL.Database
{
	public class SpecialTables
	{
		/// <summary>
		/// Gibt eine ausgefüllte xReflection Tabelle zurück
		/// </summary>
		/// <returns></returns>
		public static DataTable GetXReflection(string FormatDescription, string FormatVersion, string FormatVersionCompatibleDown, string Creator)
		{
			DataTable dt=new DataTable();
			dt.TableName="xReflection";

			//Columns anlegen
			//Primärschlüssel
			dt.Columns.Add("IndexID", Type.GetType("System.UInt32"));
			dt.Columns["IndexID"].AllowDBNull=false;
			dt.Columns["IndexID"].Unique=true;
			dt.Columns["IndexID"].AutoIncrement=true;

			dt.Columns.Add("Key", Type.GetType("System.String"));
			dt.Columns.Add("Value", Type.GetType("System.String"));

			//Daten in Tabelle Schreiben
			DataRow dr=dt.NewRow();
			dr["Key"]="ReflectionVersion";
			dr["Value"]="1.00";
			dt.Rows.Add(dr);

			dr=dt.NewRow();
			dr["Key"]="FormatDescription";
			dr["Value"]=FormatDescription;
			dt.Rows.Add(dr);

			dr=dt.NewRow();
			dr["Key"]="FormatVersion";
			dr["Value"]=FormatVersion;
			dt.Rows.Add(dr);

			dr=dt.NewRow();
			dr["Key"]="FormatVersionCompatibleDown";
			dr["Value"]=FormatVersionCompatibleDown;
			dt.Rows.Add(dr);

			dr=dt.NewRow();
			dr["Key"]="Creator";
			dr["Value"]=Creator;
			dt.Rows.Add(dr);

			//Datatable zurückgeben
			return dt;
		}
	}
}
