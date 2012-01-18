//
//  StringHelpers.Tokenizer.cs
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
using System.Collections;
using System.Text;

namespace CSCL.Helpers
{
	public static partial class StringHelpers
	{
		public class Tokenizer : IEnumerable, IEnumerator
		{
			//Beispiel 1
			//static void Main(string[] args)
			//{
			//    FileStream inputFile=null;
			//    ArrayList seperators=new ArrayList();
			//    bool includeSeperators=false;

			//    if (args.Length<3)
			//    {
			//        Console.Out.WriteLine("usage: tokenizer file showsep(true/false) seperator [seperator .... n]");
			//        System.Environment.Exit(0);
			//    }
			//    else
			//    {
			//        inputFile=new FileStream(args[0], FileMode.Open);
			//        bool.TryParse(args[1], out includeSeperators);
			//        for (int run=2; run<args.Length; run++)
			//        {
			//            seperators.Add(args[run]);
			//        }
			//    }

			//    byte[] inputData=new byte[inputFile.Length];
			//    inputFile.Read(inputData, 0, (int)inputFile.Length);
			//    inputFile.Close();

			//    StringTokenizer tokenizer=new StringTokenizer();
			//    tokenizer.IncludeSeperators=includeSeperators;
			//    tokenizer.TokenString=System.Text.Encoding.ASCII.GetString(inputData);
			//    tokenizer.Seperators=seperators;

			//    string token;
			//    tokenizer.ResetTokenizer();
			//    // Testing NumTokens
			//    Console.Out.WriteLine("Number of Tokens: {0}", tokenizer.NumTokens);
			//    Console.Out.WriteLine("--");

			//    foreach (string tok1 in tokenizer)
			//    {
			//        Console.Out.WriteLine("[foreach] got token: [{0}]", tok1);
			//    }
			//    Console.Out.WriteLine("--");

			//    // Testing Indexer
			//    int index=0;
			//    string tok2=String.Empty;
			//    while ((tok2=tokenizer[index])!=null)
			//    {
			//        Console.Out.WriteLine("[indexer] got token: [{0}]", tok2);
			//        index++;
			//    }
			//    Console.Out.WriteLine("--");

			//    // Testing GetNextToken()
			//    tokenizer.ResetTokenizer();
			//    while ((token=tokenizer.GetNextToken())!=null)
			//    {
			//        Console.Out.WriteLine("[forward] got token: [{0}], [current=[{0}]]", token, tokenizer.GetCurrentToken);
			//    }
			//    Console.Out.WriteLine("--");

			//    // Testing GetPreviousToken()
			//    while ((token=tokenizer.GetPreviousToken())!=null)
			//    {
			//        Console.Out.WriteLine("[backward] got token: [{0}], [current=[{0}]]", token, tokenizer.GetCurrentToken);
			//    }
			//}

			//Beispiel 2
			//static void Main(string[] args)
			//{
			//    string line=@"Test |Hallo|blblblb |dddddd|ddd";
			//    StringTokenizer tmpST=new StringTokenizer();

			//    tmpST.IncludeSeperators=false;
			//    tmpST.TokenString=line;
			//    tmpST.Seperators.Add("|");

			//    tmpST.ResetTokenizer();
			//    // Testing NumTokens
			//    Console.Out.WriteLine("Number of Tokens: {0}", tmpST.NumTokens);
			//    Console.Out.WriteLine("--");

			//    foreach (string tok1 in tmpST)
			//    {
			//        Console.Out.WriteLine("[foreach] got token: [{0}]", tok1);
			//    }
			//    Console.Out.WriteLine("--");

			//    Console.WriteLine("Ready.");
			//    Console.ReadKey();
			//}

			//Beispiel 3
			//static void Main(string[] args)
			//{
			//    string line="abc.mtl \"hallo tuututu.mtl\" test\" \"haaa.mtl";
			//    StringTokenizer tmpST=new StringTokenizer();

			//    tmpST.IncludeSeperators=false;
			//    tmpST.TokenString=line;
			//    tmpST.Seperators.Add(".mtl");

			//    tmpST.ResetTokenizer();
			//    // Testing NumTokens
			//    Console.Out.WriteLine("Number of Tokens: {0}", tmpST.NumTokens);
			//    Console.Out.WriteLine("--");

			//    foreach (string tok1 in tmpST)
			//    {
			//        Console.Out.WriteLine("[foreach] got token: [{0}]", tok1.Replace("\"", "").Trim()+".mtl");
			//    }
			//    Console.Out.WriteLine("--");

			//    Console.WriteLine("Ready.");
			//    Console.ReadKey();
			//}

			#region Instance Variables
			private bool m_IncludeSeperators;       // Include Seperators as Tokens?
			private bool m_LastToken;               // Are we on the last Token?
			private string m_ToTokenize;            // String to Tokenize
			private List<string> m_Seperators;         // List of Seperators
			private List<string> m_TokenList;          // List of all scanned Tokens
			private int m_TokenListIndex;           // Index of Token in List
			private int m_CurPos;                   // Current position in String
			private int m_NumTokens;                // Number of Tokens in the String
			#endregion

			#region Instance Methodes

			/// <summary>
			/// Resets the Tokenizer to its initialised state
			/// </summary>
			public void ResetTokenizer()
			{
				m_CurPos=0;
				m_LastToken=false;
				m_TokenList.Clear();
				m_TokenListIndex=0;
				m_NumTokens=0;
			}

			/// <summary>
			/// Get the Number of Tokens in the String
			/// </summary>
			public int NumTokens
			{
				get
				{
					if (m_NumTokens>=0&&m_LastToken==true)
					{
						// The String already has been fully tokenized, so simply return
						// the number of Tokens
						return m_NumTokens;
					}
					else if (m_LastToken==false)
					{
						// The String has not been fully tokenized, so do it now!
						while (GetNextToken()!=null) ;
						m_TokenListIndex=0;
					}

					return m_NumTokens;
				}
			}

			/// <summary>
			/// Returns the current Token
			/// </summary>
			public string GetCurrentToken
			{
				get
				{
					if (m_TokenListIndex<m_NumTokens)
					{
						return (string)m_TokenList[m_TokenListIndex];
					}
					else
					{
						return null;
					}
				}
			}

			/// <summary>
			/// Returns the previous retrieved Token
			/// </summary>
			/// <returns></returns>
			public string GetPreviousToken()
			{
				m_TokenListIndex--;
				if (m_TokenListIndex>=0&&m_TokenListIndex<m_TokenList.Count)
				{
					if (m_TokenListIndex==m_NumTokens&&m_LastToken) m_TokenListIndex--;
					return (string)m_TokenList[m_TokenListIndex];
				}
				else
				{
					m_TokenListIndex=0;
					return null;
				}
			}

			/// <summary>
			/// Returns the next token of the string
			/// </summary>
			/// <returns></returns>
			public string GetNextToken()
			{
				if (m_TokenListIndex>=0&&m_TokenListIndex<m_TokenList.Count)
				{
					string curToken=(string)m_TokenList[m_TokenListIndex];
					m_TokenListIndex++;
					return curToken;
				}

				string seperator=String.Empty;
				int nextPos=-1;

				if (m_CurPos==-1) return null;
				if (Seperators==null) return null;

				// Determine the next Token Seperator to use
				foreach (string sep in Seperators)
				{
					int indexPos=TokenString.IndexOf(sep, m_CurPos);
					if (nextPos==-1) nextPos=indexPos;
					if (indexPos!=-1&&indexPos<=nextPos)
					{
						seperator=sep;
						nextPos=indexPos;
					}
				}

				// When no Seperator matches we are probably at the end
				// of the string and need special treatment for this
				if (nextPos==-1)
				{
					if (m_LastToken==false)
					{
						m_LastToken=true;
					}
					else
					{
						return null;
					}
				}

				if (m_CurPos==nextPos&&m_LastToken==false)
				{
					nextPos+=seperator.Length;
				}

				// Retrieve the Token from the String
				string token;
				if (m_LastToken==false)
				{
					token=TokenString.Substring(m_CurPos, (nextPos-m_CurPos));
				}
				else
				{
					token=TokenString.Substring(m_CurPos, (TokenString.Length-m_CurPos));
				}

				if (token==String.Empty) token=null;
				m_CurPos=nextPos;

				// If we do not want seperators, recursivley call ourself to retrieve the
				// next Non-Seperator             
				if (!IncludeSeperators)
				{
					if (token==seperator)
					{
						token=GetNextToken();
					}
					else
					{
						m_TokenList.Add(token);
						m_TokenListIndex++;
						m_NumTokens++;
					}
				}
				else
				{
					m_TokenList.Add(token);
					m_TokenListIndex++;
					m_NumTokens++;
				}

				return token;
			}
			#endregion

			#region Constructors
			public Tokenizer(string toTokenize)
			{
				TokenString=toTokenize;
				Seperators=null;
				m_CurPos=0;
				m_LastToken=false;
				m_TokenList=new List<string>();
				m_Seperators=new List<string>();
				m_TokenListIndex=0;
			}

			public Tokenizer()
				: this("Empty")
			{
			}
			#endregion

			#region Public Properties

			public List<string> Seperators
			{
				get
				{
					return m_Seperators;
				}

				set
				{
					m_Seperators=value;
				}
			}

			/// <summary>
			/// Indicates wheter the Tokens also contains the seperator or not.
			/// Possible values are boolean true or false
			/// </summary>
			public bool IncludeSeperators
			{
				get
				{
					return m_IncludeSeperators;
				}

				set
				{
					m_IncludeSeperators=value;
				}
			}

			public string TokenString
			{
				get
				{
					return m_ToTokenize;
				}

				set
				{
					m_ToTokenize=value;
				}
			}
			#endregion

			#region Implementation of IEnumerable and IEnumerator Interface

			/// <summary>
			/// To support the C# Keyword 'foreach' we need to implement the two Interfaces
			/// IEnumerator and IEnumerable
			/// </summary>

			private string m_IEnumeratorCurrentToken;

			/// <summary>
			/// IEnumerator.GetEnumerator() Implementation
			/// </summary>
			/// <returns></returns>
			public IEnumerator GetEnumerator()
			{
				return (IEnumerator)this;
			}

			/// <summary>
			/// IEnumerable.MoveNext() Implementation
			/// </summary>
			/// <returns>true if token is available, false if no more tokens available</returns>
			public bool MoveNext()
			{
				if ((m_IEnumeratorCurrentToken=GetNextToken())==null)
				{
					return false;
				}
				else
				{
					return true;
				}
			}

			/// <summary>
			/// IEnumerable.Current
			/// </summary>
			public object Current
			{
				get
				{
					return (object)m_IEnumeratorCurrentToken;
				}
			}

			/// <summary>
			/// IEnumerable.Reset()
			/// </summary>
			public void Reset()
			{
				m_IEnumeratorCurrentToken=null;
				ResetTokenizer();
			}

			#endregion

			#region Implementation of the Indexer
			public string this[int index]
			{
				get
				{
					// If the given index is larger than the content of the Token list, probably the
					// string has not been fully tokenized yet. Do it now!
					if (m_LastToken==false&&index>=m_TokenList.Count)
					{
						while (GetNextToken()!=null) ;
					}

					if (index>=0&&index<m_TokenList.Count)
					{
						return (string)m_TokenList[index];
					}
					else
					{
						return null;
					}
				}
			}

			#endregion
		}
	}
}
