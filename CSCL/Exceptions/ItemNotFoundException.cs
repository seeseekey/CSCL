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
