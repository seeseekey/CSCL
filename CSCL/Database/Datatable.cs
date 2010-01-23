using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Security.Cryptography;

namespace CSCL.Database
{
    public static class Datatable
    {
        /// <summary>
        /// Removes duplicate rows in a DataTable.
        /// </summary>
        /// <param name="dataTable">The DataTable.</param> 
        /// <param name="columnName">Name of the column</param>
        public static void RemoveDuplicateRows(DataTable dataTable, string columnName)
        {
            Hashtable hashTable = new Hashtable();
            List<DataRow> duplicateList = new List<DataRow>();
            foreach(DataRow dataRow in dataTable.Rows)
                try
                {
                    hashTable.Add(dataRow[columnName], string.Empty);
                }
                catch
                {
                    duplicateList.Add(dataRow);
                }
            foreach(DataRow dataRow in duplicateList)
                dataTable.Rows.Remove(dataRow);
        }

        /// <summary>
        /// Schreibt den Inhalt einer DataTable in eine CSV Datei
        /// </summary>
        /// <param name="path">Pfad der CSV Datei</param>
        /// <param name="datatable">die zu schreibene DataTable</param>
        /// <param name="seperator">Zeichen mit dem die Spalten getrennt werden. Meist ';' oder ','</param>
        public static void WriteDataTableToCSV(string path, DataTable datatable, char seperator)
        {
            using(StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
            {
                int numberOfColumns = datatable.Columns.Count;

                for(int i = 0; i < numberOfColumns; i++)
                {
                    sw.Write(datatable.Columns[i]);
                    if(i < numberOfColumns - 1)
                        sw.Write(seperator);
                }
                sw.Write(sw.NewLine);

                foreach(DataRow dr in datatable.Rows)
                {
                    for(int i = 0; i < numberOfColumns; i++)
                    {
                        sw.Write(dr[i].ToString());

                        if(i < numberOfColumns - 1)
                            sw.Write(seperator);
                    }
                    sw.Write(sw.NewLine);
                }
            }
        }

        /// <summary>
        /// Gibt den Inhalt einer CSV Datei in einer DataTable zurück
        /// </summary>
        /// <param name="path">Pfad der CSV Datei</param>
        /// <param name="seperator">Zeichen mit dem die Spalten getrennt werden. Meist ';' oder ','</param>
        /// <returns></returns>
        public static DataTable GetDataTableFromCSV(string path, char seperator)
        {
            DataTable dt = new DataTable();
            FileStream aFile = new FileStream(path, FileMode.Open);
            using(StreamReader sr = new StreamReader(aFile, System.Text.Encoding.Default))
            {
                string strLine = sr.ReadLine();
                string[] strArray = strLine.Split(seperator);

                foreach(string value in strArray)
                    dt.Columns.Add(value.Trim());

                DataRow dr = dt.NewRow();

                while(sr.Peek() > -1)
                {
                    strLine = sr.ReadLine();
                    strArray = strLine.Split(seperator);
                    dt.Rows.Add(strArray);
                }
            }
            return dt;
        }
    }
}
