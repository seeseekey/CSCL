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
            //TODO: Überprüfen ob das auch mit Unicode Strings funktioniert
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
        /// Mit Hilfe dieser Methode wird ein übergebener String in einen "Barcodestring" 
        /// (inkl. Start-, End- und berechnetem Prüfzeichen) umgewandelt. Dieser kann dann mit 
        /// Hilfe einer Code128 TTF (einfach google'n) ausgegeben werden.
        /// </summary>
        /// <param name="textToConvert"></param>
        /// <returns></returns>
        public static string GetCode128BString(string textToConvert)
        {
            /* Diese Methode wandelt den übergebenen String in einen String um, welcher mit Hilfer einer Code128-Schrift 
             * ausgegeben werden kann. Es wird ein Start- und Endzeichen eingefügt und die erforderliche Prüfsumme 
             * berechnet. */

            char[] convertCharArray = textToConvert.ToCharArray();
            char startSign = (char)204, endSign = (char)206;
            string barcodeString = startSign.ToString();
            int index = 1; // Multiplikator zum Berechnen der Prüfsumme
            int checkDigitValue = 104; // Code 128B erfordert einen Startwert von 104

            /* Inhalt des zu codierenden Strings zeichenweise wieder zum Ergebnisstring hinzufügen und dabei die 
             * Prüfsumme mitberechnen. Enthält der zu codierende String ein Zeichen, welches sich nicht im gültigen
             * ACSII-Bereich befindet, wird der Vorgang abgebrochen und ein leerer String zurückgegeben. */

            foreach(char c in convertCharArray)
            {
                if(((int)c < 32) || ((int)c > 126))
                    return string.Empty;

                barcodeString += c.ToString();
                checkDigitValue += ((int)c - 32) * index;
                ++index;
            }

            // Das Prüfzeichen bei Code128B setzt sich aus dem Rest der Division durch 103 zusammen.
            checkDigitValue = checkDigitValue % 103;

            if(checkDigitValue > 94)
                checkDigitValue += 100;
            else
                checkDigitValue += 32;

            // Prüfzeichen und Endzeichen zum Ergebnisstring hinzufügen
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

                    longString += " …";
                }
            }
            return longString;
        }

        /// <summary>
        /// Zählt wie oft ein String in einem String vorkommt.
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
        /// Diese Funktion prüft, ob im übergebenen String nur Zeichen von A-Z a-z und von 0-9 vorhanden sind.
        /// </summary>
        /// <param name="strAlphanum"></param>
        /// <returns></returns>
        public static bool IsValidAlphaNumericString(string strAlphanum)
        {
            System.Text.RegularExpressions.Regex pattern = new System.Text.RegularExpressions.Regex(@"^[A-Za-z0-9]+$");
            return pattern.IsMatch(strAlphanum.Trim());
        }

        /// <summary>
        /// Diese Funktion entfernt mehrfach vorkommende Worte aus dem übergebenen String.
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
        /// für die Übertragung im HttpHeader zur weiteren Verwendung
        /// in einen Hexcodierten String, damit Sonder- und Leerzeichen so übertragen werden
        /// das es beim Client "richtig" ankommt.
        /// 
        /// Beispiel: FileDownload mit Response.WriteFile
        /// 
        /// Dabei ist es möglich Dateien vom Filesystem per
        /// Http an den Client zu übertragen und der Datei
        /// einen Namen zugeben.
        /// Dateiname auf dem System:
        /// {7fcd1a85-d5c8-4419-a5e0-ee12b1ab2057}.zip
        /// Gewünschter Name:
        /// Überführungrichtlinie EU Neufahrzeuge.zip
        /// 
        /// Dazu fügt man dem Response.Header folgendes hinzu:
        /// Response.AddHeader("Content-Disposition", "attachment; filename=Überführungrichtlinie EU Neufahrzeuge.zip");
        /// 
        /// Da der Dateiname jedoch Sonderzeichen / Leerzeichen enthält gibts Probleme beim Client,
        /// im Dialog speichern erscheint: ÃœberfÃ¼hrungsrichtlinie_EU_Neufahrzeuge.zip
        /// 
        /// Auch UrlEncode hilft da nicht: Überführungsrichtlinie+EU+Neufahrzeuge.zip
        /// 
        /// Daher diese Methode ToHexEncodedString(string s)
        /// "attachment; filename=" + ToHexEncodedString("Überführungrichtlinie EU Neufahrzeuge.zip");
        /// wird zu %c3%9cberf%c3%bchrungrichtlinie%20EU%20Neufahrzeuge.zip
        /// im speichern Dialog erscheint nun
        /// Überführungrichtlinie EU Neufahrzeuge.zip
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
		/// Gibt einen String in Anführungszeichen zurück
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string QuotedString(string input)
		{
			return QuotedString(input, '\"');
		}

		/// <summary>
		/// Gibt einen String in Anführungszeichen zurück
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string QuotedString(string input, char quotes)
		{
			return String.Format("{0}{1}{2}", quotes, input, quotes);
		}
    }
}
