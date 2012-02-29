//
//  SpecialTables.cs
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

namespace CSCL.Database
{
	public class SpecialTables
	{
		/// <summary>
		/// Gibt eine ausgefüllte xReflection Tabelle zurück
		/// </summary>
		/// <returns></returns>
		public static DataTable GetReflection(string FormatDescription, string FormatVersion, string FormatVersionCompatibleDown, string Creator)
		{
			DataTable dt=new DataTable();
			dt.TableName="Reflection";

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
