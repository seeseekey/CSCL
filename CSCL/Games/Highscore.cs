//
//  Highscore.cs
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
using System.Xml;

namespace CSCL.Games
{
    /// <summary>
    /// Ein Element der Highscore
    /// </summary>
    public struct HighscoreItem: IComparable<HighscoreItem>
    {
        private string name;
        /// <summary>
        /// Der Name des Elements, z.B. der Name des Spielers
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        private int points;
        /// <summary>
        /// Dir Punkte, die der Spieler erziehlt hat
        /// </summary>
        public int Points
        {
            get
            {
                return points;
            }
            set
            {
                points = value;
            }
        }

        public int CompareTo(HighscoreItem item)
        {
            return this.Points - item.Points;
        }
    }

    public class Highscore
    {
        //Die Items der Highscore
        HighscoreItem[] _score;

        public Highscore(int itemnbr)
        {
            _score = new HighscoreItem[itemnbr];
        }

        public HighscoreItem[] Score
        {
            get
            {
                return this._score;
            }
        }

        /// <summary>
        /// L�d Werte f�r die Highscore aus einer XML-Datei
        /// </summary>
        /// <param name="filename">Der Name der XML-Datei</param>
        public void LoadFromXml(string filename)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            int count = 0;
            this._score = new HighscoreItem[doc.ChildNodes[0].ChildNodes.Count];
            foreach(XmlNode platz in doc.ChildNodes[0].ChildNodes)
            {
                if(!string.IsNullOrEmpty(platz.ChildNodes[0].InnerText))
                {
                    //ein neues Item erzeugen, wenn der Spielername nicht null oder leer ist
                    HighscoreItem i = new HighscoreItem();
                    i.Name = platz.ChildNodes[0].InnerText;
                    i.Points = int.Parse(platz.ChildNodes[1].InnerText);
                    this._score[count] = i;
                    count++;
                }
            }
        }

        /// <summary>
        /// Speichert die Werte der Highscore in eine XML-Datei
        /// </summary>
        /// <param name="filename">Der Name der XML-Datei</param>
        public void Save(string filename)
        {
            XmlDocument doc = new XmlDocument();
            //root-node
            doc.AppendChild(doc.CreateElement("Highscore"));
            int placenr = 1;
            foreach(HighscoreItem i in this._score)
            {
                XmlElement item = doc.CreateElement("Platz" + placenr.ToString());
                //Das Element, das den Namen des Spielers enth�lt
                XmlElement name = doc.CreateElement("Name");
                name.InnerText = i.Name;
                //Das Element, das die Punkte des Spielers enth�lt
                XmlElement points = doc.CreateElement("Punkte");
                points.InnerText = i.Points.ToString();
                item.AppendChild(name);
                item.AppendChild(points);
                placenr++;
                doc.ChildNodes[0].AppendChild(item);
            }
            doc.Save(filename);
        }

        /// <summary>
        /// F�gt der Highscore einen neuen Wert hinzu
        /// </summary>
        /// <param name="Name">Der Name des Spielers mit dem neuen Wert</param>
        /// <param name="Points">Die Punkte des Spielers</param>
        public void AddValue(string Name, int Points)
        {
            if(Name != string.Empty)
            {
                HighscoreItem item = new HighscoreItem();
                item.Name = Name;
                item.Points = Points;

                //neuer Wert bekommt letzten Platz, wenn er besser als der aktuelle letzte Wert ist
                if(_score[_score.Length - 1].Points < item.Points)
                    _score[_score.Length - 1] = item;

                //jeder Wert, der kleiner ist als der nachfolgende wird mit diesem getauscht
                for(int x = _score.Length - 2; x >= 0; x--)
                {
                    if(_score[x].Points < _score[x + 1].Points)
                    {
                        HighscoreItem tmp = _score[x + 1];
                        _score[x + 1] = _score[x];
                        _score[x] = tmp;
                    }
                    //alle weiteren Werte sind in der richtigen Reihenfolge, also kann abgebrochen werden
                    else
                        break;
                }
            }
        }
    }
}
