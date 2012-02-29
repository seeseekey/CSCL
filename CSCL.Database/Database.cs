using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CSCL.Database
{
	public abstract class Database
	{
		public abstract List<string> GetTables();

		public abstract DataTable GetTableStructure(string tblName);

		public abstract string GetDBSystemDatatype(string datatype);
		//public abstract string GetNETDatatype(string datatype);

		public abstract int ExecuteNonQuery(string sqlCommand);
		public abstract DataTable ExecuteQuery(string sqlCommand);

		/// <summary>
		/// Überprüft ob eine bestimmte Tabelle existiert
		/// </summary>
		/// <param name="tblName"></param>
		/// <returns></returns>
		public bool ExistsTable(string tblName)
		{
			List<string> tables=GetTables();
			if(tables.IndexOf(tblName)==-1) return false;
			return true;
		}
	}
}
