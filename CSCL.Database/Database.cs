using System.Collections.Generic;
using System.Data;

namespace CSCL.Database
{
    public abstract class Database
    {
        //Eigenschaften
        public bool Connected { get; protected set; }

        //Verbindung
        public abstract bool Connect();
        public abstract void Disconnect();

        //Abfragen
        public abstract int ExecuteNonQuery(string sqlCommand);
        public abstract DataTable ExecuteQuery(string sqlCommand);

        //Edits
        public abstract void InsertData(DataTable insertData);
        public abstract void UpdateData(DataTable updateData, string PrimaryKey);
        public abstract void RemoveData(string table, string key, string value);

        //Transaktionen
        public abstract void StartTransaction();
        public abstract void CommitTransaction();

        //Tabellen
        public abstract void CreateTable(DataTable table);

        /// <summary>
        /// Erzeugt mehrere Tabellen
        /// </summary>
        /// <param name="tables"></param>
        public void CreateTables(List<DataTable> tables)
        {
            foreach(DataTable i in tables)
            {
                CreateTable(i);
            }
        }
        public abstract void RemoveTable(string tblName);

        /// <summary>
        /// Löscht mehrere Tabellen
        /// </summary>
        /// <param name="Tables"></param>
        public void RemoveTables(List<string> tables)
        {
            foreach(string i in tables)
            {
                RemoveTable(i);
            }
        }

        public abstract List<string> GetTables();

        /// <summary>
        /// Gibt eine Tabelle zurück
        /// </summary>
        /// <param name="tblName"></param>
        /// <returns></returns>
        public DataTable GetTable(string tblName)
        {
            string sqlCommand="SELECT * FROM \""+tblName+"\";";
            DataTable ret=ExecuteQuery(sqlCommand);
            ret.TableName=tblName;
            return ret;
        }

        public abstract DataTable GetTableStructure(string tblName);

        /// <summary>
        /// Überprüft ob eine bestimmte Tabelle existiert
        /// </summary>
        /// <param name="tblName"></param>
        /// <returns></returns>
        public bool ExistsTable(string tblName)
        {
            List<string> tables=GetTables();
            if(tables.IndexOf(tblName)==-1) return false;
            return true;
        }

        //Sonstige Funktionen
        public abstract string GetDBSystemDatatype(string datatype);
    }
}
