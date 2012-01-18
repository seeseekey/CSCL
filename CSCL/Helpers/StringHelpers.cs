//
//  StringHelpers.cs
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
using System.Text.RegularExpressions;
using System.IO;

namespace CSCL.Helpers
{
    public static partial class StringHelpers
    {
        /// <summary>
        /// Wandelt ein String in ein Byte Array um
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] StringToByteArray(string str)
        {
            //TODO: �berpr�fen ob das auch mit Unicode Strings funktioniert
            System.Text.ASCIIEncoding enc=new System.Text.ASCIIEncoding();
            return enc.GetBytes(str);
        }

        /// <summary>
        /// Wandelt ein ByteArray in einen String um
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static string ByteArrayToString(byte[] arr)
        {
            System.Text.ASCIIEncoding enc=new System.Text.ASCIIEncoding();
            return enc.GetString(arr);
        }

		public static string ByteArrayToStringUTF8(byte[] arr)
        {
			System.Text.UTF8Encoding enc=new System.Text.UTF8Encoding();
            return enc.GetString(arr);
        }

        /// <summary>
        /// Helper-Method
        /// Converted an ASCII-Text to Hex-Value
        /// </summary>
        /// <param name="i_asciiText">ASCII-Text</param>
        /// <returns>String with Hex-Representation</returns>
        public static string ConvertASCIITextToHex(string i_asciiText)
        {
            StringBuilder sBuffer=new StringBuilder();
            for(int i=0;i<i_asciiText.Length;i++)
            {
                sBuffer.Append(System.Convert.ToInt32(i_asciiText[i]).ToString("x"));
            }
            return sBuffer.ToString().ToUpper();
        }

        /// <summary>
        /// Get byte by string 
        /// </summary>
        /// <param name="strHexvalue">a hex string e.g. "0xff"</param>
        /// <returns>byte</returns>
        public static byte GetByte(string strHexvalue)
        {
            int intHex=int.Parse(strHexvalue.Substring(2), System.Globalization.NumberStyles.AllowHexSpecifier);
            return System.Convert.ToByte(intHex.ToString(), 10);
        }

		public static string GetRandomString(Int64 Length)
		{
			System.Random rnd=new System.Random();
			StringBuilder Temp=new StringBuilder();
			for (Int64 i=0; i<Length; i++)
			{
				Temp.Append(Convert.ToChar(((byte)rnd.Next(254))).ToString());
			}
			return Temp.ToString();
		}

		public static string GetRandomASCIIString(Int64 Length)
		{
			System.Random rnd=new System.Random((int)System.DateTime.Now.Ticks);
			StringBuilder Temp=new StringBuilder();
			for (Int64 i=0; i<Length; i++)
			{
				Temp.Append(Convert.ToChar(((byte)rnd.Next(97, 121))).ToString());
			}
			return Temp.ToString();
		}

        /// <summary>
        /// Gets the last word of the given string.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>the last word of the given string</returns>
        public static string GetLastWordOfString(string token)
        {
            string[] words = Regex.Split(token.TrimEnd(), @"[\s]");
            return words[words.Length - 1];
        }

        /// <summary>
        /// Gets the first word of the given string.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>the first word of the given string</returns>
        public static string GetFirstWordOfString(string token)
        {
            return Regex.Split(token.TrimStart(), @"[\s]")[0];
        }

        /// <summary>
        /// Determines whether the specified text is an palindrome.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="CaseSensitive">if set to <c>true</c> [case sensitive].</param>
        /// <returns>
        /// 	<c>true</c> if the specified text is an palindrome; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPalindrome(string text, bool CaseSensitive)
        {
            if (String.IsNullOrEmpty(text))
                return false;

            int textLength = text.Length - 1;
            int halfTextLength = textLength / 2;

            if (!CaseSensitive)
                text = text.ToLower();

            for (int i = 0; i <= halfTextLength; i++)
            {
                if (text[i] != text[textLength])
                    return false;
                textLength--;
            }
            return true;
        }

        /// <summary>
        /// Reverse the string. 
        /// </summary>
        /// <param name="text">The String.</param>
        /// <returns></returns>
        public static string ReverseString(string text)
        {
            if (text.Length == 1 || String.IsNullOrEmpty(text))
                return text;
            else
                return ReverseString(text.Substring(1)) + text.Substring(0, 1);
        }

        /// <summary>
        /// Adds the line numbers to text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string AddLineNumbersToText(string text)
        {
            StringBuilder sb = new StringBuilder();
            string[] splitStrings = { "\r", "\r\n", "\n" };
            string[] lines = text.Split(splitStrings, StringSplitOptions.None);
            int lineCounter = 0;

            foreach (string s in lines)
            {
                sb.Append(lineCounter.ToString());
                sb.Append(":\t");
                sb.Append(s);
                sb.Append(Environment.NewLine);
                lineCounter++;
            }
            return sb.ToString();
        }

        /// <summary>
        /// Mit Hilfe dieser Methode wird ein �bergebener String in einen "Barcodestring" 
        /// (inkl. Start-, End- und berechnetem Pr�fzeichen) umgewandelt. Dieser kann dann mit 
        /// Hilfe einer Code128 TTF (einfach google'n) ausgegeben werden.
        /// </summary>
        /// <param name="textToConvert"></param>
        /// <returns></returns>
        public static string GetCode128BString(string textToConvert)
        {
            /* Diese Methode wandelt den �bergebenen String in einen String um, welcher mit Hilfer einer Code128-Schrift 
             * ausgegeben werden kann. Es wird ein Start- und Endzeichen eingef�gt und die erforderliche Pr�fsumme 
             * berechnet. */

            char[] convertCharArray = textToConvert.ToCharArray();
            char startSign = (char)204, endSign = (char)206;
            string barcodeString = startSign.ToString();
            int index = 1; // Multiplikator zum Berechnen der Pr�fsumme
            int checkDigitValue = 104; // Code 128B erfordert einen Startwert von 104

            /* Inhalt des zu codierenden Strings zeichenweise wieder zum Ergebnisstring hinzuf�gen und dabei die 
             * Pr�fsumme mitberechnen. Enth�lt der zu codierende String ein Zeichen, welches sich nicht im g�ltigen
             * ACSII-Bereich befindet, wird der Vorgang abgebrochen und ein leerer String zur�ckgegeben. */

            foreach(char c in convertCharArray)
            {
                if(((int)c < 32) || ((int)c > 126))
                    return string.Empty;

                barcodeString += c.ToString();
                checkDigitValue += ((int)c - 32) * index;
                ++index;
            }

            // Das Pr�fzeichen bei Code128B setzt sich aus dem Rest der Division durch 103 zusammen.
            checkDigitValue = checkDigitValue % 103;

            if(checkDigitValue > 94)
                checkDigitValue += 100;
            else
                checkDigitValue += 32;

            // Pr�fzeichen und Endzeichen zum Ergebnisstring hinzuf�gen
            barcodeString += ((char)checkDigitValue).ToString() + endSign.ToString();
            return barcodeString;
        }

        /// <summary>
        /// Removes double space characters.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string RemoveDoubleSpaceCharacters(string text)
        {
            return System.Text.RegularExpressions.Regex.Replace(text, "[ ]+", " ");
        }

        /// <summary>
        /// Cuts the long string.
        /// </summary>
        /// <param name="longString">The long string.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string CutLongString(string longString, int length)
        {
            if(longString.Trim() != string.Empty)
            {
                if(longString.Length > length)
                {
                    longString = longString.Substring(0, length);
                    int positionLastSpace = longString.LastIndexOf(" ");
                    if(positionLastSpace > -1 && positionLastSpace < length)
                        longString = longString.Substring(0, positionLastSpace);

                    longString += " �";
                }
            }
            return longString;
        }

        /// <summary>
        /// Z�hlt wie oft ein String in einem String vorkommt.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="regexStr"></param>
        /// <returns></returns>
        public static int CountStrings(string str, string regexStr)
        {
            Regex regex = new Regex(regexStr);
            return regex.Matches(str).Count;
        }

        /// <summary>
        /// Determines whether the specified input is numeric.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        /// 	<c>true</c> if the specified input is numeric; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNumeric(string input)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(input, "^\\d+$");
        }

        /// <summary>
        /// Counts the lines of a File.
        /// </summary>
        /// <param name="fileToCount">The file to count.</param>
        /// <returns>lines of a File</returns>
        public static int CountLines(string fileToCount)
        {
            int counter = 0;
            using(StreamReader countReader = new StreamReader(fileToCount))
            {
                while(countReader.ReadLine() != null)
                    counter++;
            }
            return counter;
        }

        /// <summary>
        /// Diese Funktion pr�ft, ob im �bergebenen String nur Zeichen von A-Z a-z und von 0-9 vorhanden sind.
        /// </summary>
        /// <param name="strAlphanum"></param>
        /// <returns></returns>
        public static bool IsValidAlphaNumericString(string strAlphanum)
        {
            System.Text.RegularExpressions.Regex pattern = new System.Text.RegularExpressions.Regex(@"^[A-Za-z0-9]+$");
            return pattern.IsMatch(strAlphanum.Trim());
        }

        /// <summary>
        /// Diese Funktion entfernt mehrfach vorkommende Worte aus dem �bergebenen String.
        /// </summary>
        /// <param name="Orginaltext">der Orginaltext</param>
        /// <returns>der Orginaltext ohne mehrfach vorkommende Worte</returns>
        public static string RemoveDoubleStrings(string orginaltext)
        {
            string[] arrOriginaltext = orginaltext.Split(' ');

            Dictionary<string, object> dicOriginaltext = new Dictionary<string, object>();

            foreach(string wort in arrOriginaltext)
                dicOriginaltext[wort] = true;

            StringBuilder ret = new StringBuilder();

            foreach(string wort in dicOriginaltext.Keys)
                ret.Append(wort + " ");

            return ret.ToString();
        }

        /// <summary>
        /// Encodiert Sonderzeichen
        /// Diese Methode encodiert Zeichenfolgen: Urls,Dateinamen etc.
        /// f�r die �bertragung im HttpHeader zur weiteren Verwendung
        /// in einen Hexcodierten String, damit Sonder- und Leerzeichen so �bertragen werden
        /// das es beim Client "richtig" ankommt.
        /// 
        /// Beispiel: FileDownload mit Response.WriteFile
        /// 
        /// Dabei ist es m�glich Dateien vom Filesystem per
        /// Http an den Client zu �bertragen und der Datei
        /// einen Namen zugeben.
        /// Dateiname auf dem System:
        /// {7fcd1a85-d5c8-4419-a5e0-ee12b1ab2057}.zip
        /// Gew�nschter Name:
        /// �berf�hrungrichtlinie EU Neufahrzeuge.zip
        /// 
        /// Dazu f�gt man dem Response.Header folgendes hinzu:
        /// Response.AddHeader("Content-Disposition", "attachment; filename=�berf�hrungrichtlinie EU Neufahrzeuge.zip");
        /// 
        /// Da der Dateiname jedoch Sonderzeichen / Leerzeichen enth�lt gibts Probleme beim Client,
        /// im Dialog speichern erscheint: Überführungsrichtlinie_EU_Neufahrzeuge.zip
        /// 
        /// Auch UrlEncode hilft da nicht: �berf�hrungsrichtlinie+EU+Neufahrzeuge.zip
        /// 
        /// Daher diese Methode ToHexEncodedString(string s)
        /// "attachment; filename=" + ToHexEncodedString("�berf�hrungrichtlinie EU Neufahrzeuge.zip");
        /// wird zu %c3%9cberf%c3%bchrungrichtlinie%20EU%20Neufahrzeuge.zip
        /// im speichern Dialog erscheint nun
        /// �berf�hrungrichtlinie EU Neufahrzeuge.zip
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToHexEncodedString(string s)
        {
            System.Text.StringBuilder returnValue = new System.Text.StringBuilder();
            System.Text.UTF8Encoding utf8 = new System.Text.UTF8Encoding();
            string reservedChars = "_+-=.,!'()@&$";
            char[] chr = s.ToCharArray();
            for(int i = 0; i != chr.Length; i++)
            {
                if(chr[i] <= 127 && (reservedChars.IndexOf(chr[i]) != -1 || char.IsLetterOrDigit(chr[i])))
                    returnValue.Append(chr[i]);
                else
                {
                    System.Text.StringBuilder encodedValue = new System.Text.StringBuilder();
                    byte[] encBytes = utf8.GetBytes(chr[i].ToString());
                    for(int j = 0; j != encBytes.Length; j++)
                    {
                        encodedValue.AppendFormat("%{0}", Convert.ToString(encBytes[j], 16));
                    }
                    returnValue.Append(encodedValue.ToString());
                }
            }
            return returnValue.ToString();
        }

		/// <summary>
		/// Gibt einen String in Anf�hrungszeichen zur�ck
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string QuotedString(string input)
		{
			return QuotedString(input, '\"');
		}

		/// <summary>
		/// Gibt einen String in Anf�hrungszeichen zur�ck
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string QuotedString(string input, char quotes)
		{
			return String.Format("{0}{1}{2}", quotes, input, quotes);
		}
    }
}
