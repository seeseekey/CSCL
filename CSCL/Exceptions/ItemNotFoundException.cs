//
//  ItemNotFoundException.cs
//
//  Copyright (c) 2011, 2012 by seeseekey <seeseekey@gmail.com>
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

namespace CSCL.Exceptions
{
	public class ItemNotFoundException : System.Exception
	{
		/// <summary>Just overriding default constructor.</summary>
		/// <returns>Returns Exception object.</returns>
		public ItemNotFoundException() { }
		/// <summary>Just overriding constructor.</summary>
		/// <returns>Returns Exception object.</returns>
		public ItemNotFoundException(string message)
			: base(message) { /*Console.ForegroundColor = ConsoleColor.Red;*/ }
		/// <summary>Just overriding constructor.</summary>
		/// <returns>Returns Exception object.</returns>
		public ItemNotFoundException(string message, System.Exception inner)
			: base(message, inner) { }
		/// <summary>Just overriding constructor.</summary>
		/// <returns>Returns Exception object.</returns>
		protected ItemNotFoundException(System.Runtime.Serialization.SerializationInfo info,
			System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
		/// <summary>Destructor is invoked automatically when exception object becomes
		/// inaccessible.</summary>
		~ItemNotFoundException() { }
	}
}
