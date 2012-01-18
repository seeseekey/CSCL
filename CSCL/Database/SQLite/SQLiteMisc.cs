//
//  SQLiteMisc.cs
//
//  Copyright (c) 2011, 2012 by seeseekey <seeseekey@googlemail.com>
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace CSCL.Database.SQLite
{
	public partial class SQLiteVdbe
	{
		public int BindObjectToType(int index, object bObject)
		{
			string TypeId=bObject.GetType().FullName;
			int ret=0;
			LastError="";

			switch(TypeId)
			{
				case "System.Int32":
					{
						Int32 tmpValue=(Int32)bObject;

						if((ret=Sqlite3.sqlite3_bind_int(vm, index, tmpValue))!=Sqlite3.SQLITE_OK)
						{
							LastError="Error "+LastError+"binding Integer ["+tmpValue+"]";
						}
						break;
					}
				case "System.Int64":
					{
						Int64 tmpValue=(Int64)bObject;

						if((ret=Sqlite3.sqlite3_bind_int64(vm, index, tmpValue))!=Sqlite3.SQLITE_OK)
						{
							LastError="Error "+LastError+"binding Integer64 ["+tmpValue+"]";
						}
						break;
					}
				case "System.String":
					{
						String tmpValue=(String)bObject;

						if((ret=Sqlite3.sqlite3_bind_text(vm, index, tmpValue, -1, null))!=Sqlite3.SQLITE_OK)
						{
							LastError="Error "+LastError+"binding Text ["+tmpValue+"]";
						}
						break;
					}
				case "System.Byte[]":
					{
						byte[] tmpValue=(byte[])bObject;
						//string tmpVxx=Encoding.UTF8.GetString(tmpValue);
						////string tmpVxx=Encoding.ASCII.GetString(tmpValue);
						//string tmpVxx=System.Text.Encoding.GetEncoding("iso-8859-1").GetString(tmpValue);

						if((ret=Sqlite3.sqlite3_bind_blob(vm, index, tmpValue, -1, null))!=Sqlite3.SQLITE_OK)
						{
							LastError="Error "+LastError+"binding Blob ["+tmpValue+"]";
						}
						break;
					}
				case "System.DBNull":
					{
						//wird ignoriert (Autoincrement)
						break;
					}
				default:
					{
						throw new NotImplementedException();
					}
			}

			if(ret!=0)
			{
				throw new Exception(LastError+"/ return code: "+ret.ToString());
			}
			else
			{
				return ret;
			}
		}
	}

	public partial class SQLiteDatabase
	{
		public List<string> GetTables()
		{
			List<string> ret=new List<string>();

			// executes query that select names of all tables in master table of the database
			String query="SELECT name FROM sqlite_master "+
										"WHERE type = 'table'"+
										"ORDER BY 1";
			DataTable table=ExecuteQuery(query);

			// Return all table names in the ArrayList
			foreach(DataRow row in table.Rows)
			{
				ret.Add(row.ItemArray[0].ToString());
			}
			return ret;
		}
	}
}
