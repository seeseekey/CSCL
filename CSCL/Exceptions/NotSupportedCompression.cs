using System;
using System.Collections.Generic;
using System.Text;

namespace CSCL.Exceptions
{
	public class NotSupportedCompressionException : System.Exception
	{
		/// <summary>Just overriding default constructor.</summary>
		/// <returns>Returns Exception object.</returns>
		public NotSupportedCompressionException() { }
		/// <summary>Just overriding constructor.</summary>
		/// <returns>Returns Exception object.</returns>
		public NotSupportedCompressionException(string message)
			: base(message) { /*Console.ForegroundColor = ConsoleColor.Red;*/ }
		/// <summary>Just overriding constructor.</summary>
		/// <returns>Returns Exception object.</returns>
		public NotSupportedCompressionException(string message, System.Exception inner)
			: base(message, inner) { }
		/// <summary>Just overriding constructor.</summary>
		/// <returns>Returns Exception object.</returns>
		protected NotSupportedCompressionException(System.Runtime.Serialization.SerializationInfo info,
			System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
		/// <summary>Destructor is invoked automatically when exception object becomes
		/// inaccessible.</summary>
		~NotSupportedCompressionException() { }
	}
}
