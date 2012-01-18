//
//  SQLiteDatabase.cs
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
using System.Collections;
using System.Data;

namespace CSCL.Database.SQLite
{

  using sqlite = Sqlite3.sqlite3;
  using Vdbe = Sqlite3.Vdbe;
  /// <summary>
  /// C#-SQLite wrapper with functions for opening, closing and executing queries.
  /// </summary>
  public partial class SQLiteDatabase
  {
    // pointer to database
    private sqlite db;

	public sqlite DB
	{
		get
		{
			return db;
		}
	}

    /// <summary>
    /// Creates new instance of SQLiteBase class with no database attached.
    /// </summary>
    public SQLiteDatabase()
    {
      db = null;
    }
    /// <summary>
    /// Creates new instance of SQLiteDatabase class and opens database with given name.
    /// </summary>
    /// <param name="DatabaseName">Name (and path) to SQLite database file</param>
    public SQLiteDatabase( String DatabaseName )
    {
      OpenDatabase( DatabaseName );
    }

    /// <summary>
    /// Opens database. 
    /// </summary>
    /// <param name="DatabaseName">Name of database file</param>
    public void OpenDatabase( String DatabaseName )
    {
      // opens database 
      if (
#if NET_35
 Sqlite3.Open
#else
Sqlite3.sqlite3_open
#endif
( DatabaseName, ref db ) != Sqlite3.SQLITE_OK )
      {
        // if there is some error, database pointer is set to 0 and exception is throws
        db = null;
        throw new Exception( "Error with opening database " + DatabaseName + "!" );
      }
    }

    /// <summary>
    /// Closes opened database.
    /// </summary>
    public void CloseDatabase()
    {
      // closes the database if there is one opened
      if ( db != null )
      {
#if NET_35
        Sqlite3.Close
#else
Sqlite3.sqlite3_close
#endif
( db );
      }
    }

    /// <summary>
    /// Returns connection
    /// </summary>
    public sqlite Connection()
    {
      return db;
    }

    /// <summary>
    /// Executes query that does not return anything (e.g. UPDATE, INSERT, DELETE).
    /// </summary>
    /// <param name="query"></param>
    public void ExecuteNonQuery( String query )
    {
      // calles SQLite function that executes non-query
      Sqlite3.exec( db, query, 0, 0, 0 );
      // if there is error, excetion is thrown
      if ( db.errCode != Sqlite3.SQLITE_OK )
        throw new Exception( "Error with executing non-query: \"" + query + "\"!\n" +
#if NET_35
 Sqlite3.Errmsg
#else
Sqlite3.sqlite3_errmsg
#endif
( db ) );
    }

    /// <summary>
    /// Executes query that does return something (e.g. SELECT).
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public DataTable ExecuteQuery( String query )
    {
      // compiled query
      SQLiteVdbe statement = new SQLiteVdbe( this, query );

      // table for result of query
      DataTable table = new DataTable();

      // create new instance of DataTable with name "resultTable"
      table = new DataTable( "resultTable" );

      // reads rows
      do { } while ( ReadNextRow( statement.VirtualMachine(), table ) == Sqlite3.SQLITE_ROW );
      // finalize executing this query
      statement.Close();

      // returns table
      return table;
    }

    // private function for reading rows and creating table and columns
    private int ReadNextRow( Vdbe vm, DataTable table )
    {
      int columnCount = table.Columns.Count;
      if ( columnCount == 0 )
      {
        if ( ( columnCount = ReadColumnNames( vm, table ) ) == 0 ) return Sqlite3.SQLITE_ERROR;
      }

      int resultType;
      if ( ( resultType =
#if NET_35
 Sqlite3.Step
#else
Sqlite3.sqlite3_step
#endif
( vm ) ) == Sqlite3.SQLITE_ROW )
      {
        object[] columnValues = new object[columnCount];

        for ( int i = 0; i < columnCount; i++ )
        {
          int columnType =
#if NET_35
 Sqlite3.ColumnType
#else
Sqlite3.sqlite3_column_type
#endif
( vm, i );
          switch ( columnType )
          {
            case Sqlite3.SQLITE_INTEGER:
              {
                table.Columns[i].DataType = typeof( Int64 );
                columnValues[i] =
#if NET_35
 Sqlite3.ColumnInt
#else
Sqlite3.sqlite3_column_int
#endif
( vm, i );
                break;
              }
            case Sqlite3.SQLITE_FLOAT:
              {
                table.Columns[i].DataType = typeof( Double );
                columnValues[i] =
#if NET_35
 Sqlite3.ColumnDouble
#else
Sqlite3.sqlite3_column_double
#endif
( vm, i );
                break;
              }
            case Sqlite3.SQLITE_TEXT:
              {
                table.Columns[i].DataType = typeof( String );
                columnValues[i] =
#if NET_35
 Sqlite3.ColumnText
#else
Sqlite3.sqlite3_column_text
#endif
( vm, i );
                break;
              }
            case Sqlite3.SQLITE_BLOB:
              {
                table.Columns[i].DataType = typeof( Byte[] );
                columnValues[i] =
#if NET_35
 Sqlite3.ColumnBlob
#else
Sqlite3.sqlite3_column_blob
#endif
( vm, i );
                break;
              }
            default:
              {
                table.Columns[i].DataType = null;
                columnValues[i] = "";
                break;
              }
          }
        }
        table.Rows.Add( columnValues );
      }
      return resultType;
    }
    // private function for creating Column Names
    // Return number of colums read
    private int ReadColumnNames( Vdbe vm, DataTable table )
    {

      String columnName = "";
      int columnType = 0;
      // returns number of columns returned by statement
      int columnCount =
#if NET_35
 Sqlite3.ColumnCount
#else
Sqlite3.sqlite3_column_count
#endif
( vm );
      object[] columnValues = new object[columnCount];

      try
      {
        // reads columns one by one
        for ( int i = 0; i < columnCount; i++ )
        {
          columnName =
#if NET_35
 Sqlite3.ColumnName
#else
Sqlite3.sqlite3_column_name
#endif
( vm, i );
          columnType =
#if NET_35
 Sqlite3.ColumnType
#else
Sqlite3.sqlite3_column_type
#endif
( vm, i );

          switch ( columnType )
          {
            case Sqlite3.SQLITE_INTEGER:
              {
                // adds new integer column to table
                table.Columns.Add( columnName, Type.GetType( "System.Int64" ) );
                break;
              }
            case Sqlite3.SQLITE_FLOAT:
              {
                table.Columns.Add( columnName, Type.GetType( "System.Double" ) );
                break;
              }
            case Sqlite3.SQLITE_TEXT:
              {
                table.Columns.Add( columnName, Type.GetType( "System.String" ) );
                break;
              }
            case Sqlite3.SQLITE_BLOB:
              {
                table.Columns.Add( columnName, Type.GetType( "System.byte[]" ) );
                break;
              }
            default:
              {
                table.Columns.Add( columnName, Type.GetType( "System.String" ) );
                break;
              }
          }
        }
      }
      catch
      {
        return 0;
      }
      return table.Columns.Count;
    }

  }
}
