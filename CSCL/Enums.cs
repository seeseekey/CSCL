//
//  Enums.cs
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

namespace CSCL
{
	/// <summary>
	/// Enum welcher angibt in welchem Modus der Dialog ge�ffnet wird
	/// </summary>
	public enum DialogCreateMode
	{
		New,
		Edit
	};

	/// <summary>
	/// Ein Enum welcher ja und Nein definiert
	/// </summary>
	public enum YesNo
	{
		Yes,
		No
	}

	/// <summary>
	/// Enum welcher XYZ definiert
	/// </summary>
	public enum XYZ
	{
		X,
		Y,
		Z
	}

}
