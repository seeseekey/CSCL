//  $Header: Benchmark/Classes/SQLiteVdbe.cs,v 7491e0c5add9 2010/01/12 20:47:08 Noah $
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using CSCL.Database.SQLite;
using System.IO;

namespace CSCL.Database.SQLite
{
	using sqlite=csSQLite.sqlite3;
	using Vdbe=csSQLite.Vdbe;

	/// <summary>
	/// C#-SQLite wrapper with functions for opening, closing and executing queries.
	/// </summary>
	public class SQLiteVdbe
	{
		private Vdbe vm=null;
		private string LastError="";
		private int LastResult=0;

		/// <summary>
		/// Creates new instance of SQLiteVdbe class by compiling a statement
		/// </summary>
		/// <param name="query"></param>
		/// <returns>Vdbe</returns>
		public SQLiteVdbe(SQLiteDatabase db, String query)
		{
			vm=null;

			// prepare and compile 
			csSQLite.sqlite3_prepare_v2(db.Connection(), query, query.Length, ref vm, 0);
		}

		/// <summary>
		/// Return Virtual Machine Pointer
		/// </summary>
		/// <param name="query"></param>
		/// <returns>Vdbe</returns>
		public Vdbe VirtualMachine()
		{
			return vm;
		}

		/// <summary>
		/// Execute statement
		/// </summary>
		/// </param>
		/// <returns>LastResult</returns>
		public int ExecuteStep()
		{
			// Execute the statement
			int LastResult=csSQLite.sqlite3_step(vm);

			return LastResult;
		}

		/// <summary>
		/// Returns Result column as Long
		/// </summary>
		/// </param>
		/// <returns>Result column</returns>
		public long Result_Long(int index)
		{
			return csSQLite.sqlite3_column_int64(vm, index);
		}

		/// <summary>
		/// Returns Result column as Text
		/// </summary>
		/// </param>
		/// <returns>Result column</returns>
		public string Result_Text(int index)
		{
			return csSQLite.sqlite3_column_text(vm, index);
		}


		/// <summary>
		/// Returns Count of Result Rows
		/// </summary>
		/// </param>
		/// <returns>Count of Results</returns>
		public int ResultColumnCount()
		{
			return vm.pResultSet==null?0:vm.pResultSet.Length;
		}

		/// <summary>
		/// Reset statement
		/// </summary>
		/// </param>
		/// </returns>
		public void Reset()
		{
			// Reset the statment so it's ready to use again
			csSQLite.sqlite3_reset(vm);
		}

		/// <summary>
		/// Closes statement
		/// </summary>
		/// </param>
		/// <returns>LastResult</returns>
		public void Close()
		{
			csSQLite.sqlite3_finalize(ref vm);
		}

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

						if((ret=csSQLite.sqlite3_bind_int(vm, index, tmpValue))!=csSQLite.SQLITE_OK)
						{
							LastError="Error "+LastError+"binding Integer ["+tmpValue+"]";
						}
						break;
					}
				case "System.Int64":
					{
						Int64 tmpValue=(Int64)bObject;

						if((ret=csSQLite.sqlite3_bind_int64(vm, index, tmpValue))!=csSQLite.SQLITE_OK)
						{
							LastError="Error "+LastError+"binding Integer64 ["+tmpValue+"]";
						}
						break;
					}
				case "System.String":
					{
						String tmpValue=(String)bObject;

						if((ret=csSQLite.sqlite3_bind_text(vm, index, tmpValue, -1, null))!=csSQLite.SQLITE_OK)
						{
							LastError="Error "+LastError+"binding Text ["+tmpValue+"]";
						}
						break;
					}
				//case "System.Byte[]":
				//    {
				//        byte[] tmpValue=(byte[])bObject;
				//        byte[] tmpValue=File.ReadAllBytes(filename);


				//        if((ret=csSQLite.sqlite3_bind_blob(vm, index, tmpValue, -1, null))!=csSQLite.SQLITE_OK)
				//        {
				//            LastError="Error "+LastError+"binding Blob ["+tmpValue+"]";
				//        }
				//        break;
				//    }
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
}
