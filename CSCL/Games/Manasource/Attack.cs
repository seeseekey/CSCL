//
//  Attack.cs
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
using System.Xml;
using System.Globalization;
using System.Threading;

namespace CSCL.Games.Manasource
{
    public class Attack
    {
        public int ID { get; private set; }
        public int Priority { get; private set; }
        public string Type { get; private set; }
        public int PreDelay { get; private set; }
        public int AftDelay { get; private set; }
        public int DamageFactor { get; private set; }
        public int Range { get; private set; }
        public int WarmUpTime { get; private set; }
        public int CoolDownTime { get; private set; }
        public int BaseDamage { get; private set; }
        public int DeltaDamage { get; private set; }
        public int ReUseTime { get; private set; }
        public int ChanceToHit { get; private set; }
        public string Animation { get; private set; }
        public string ParticleEffect { get; private set; }
        public string Action { get; private set; }
        public Elements Element { get; private set; }

        public Attack(XmlNode node)
        {
            CultureInfo nc=new CultureInfo("");
            CultureInfo oldCult=Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture=nc;
			
            foreach(XmlAttribute i in node.Attributes)
            {
                switch(i.Name.ToLower())
                {
                    case "id":
                        {
                            ID=Convert.ToInt32(i.Value);
                            break;
                        }
                    case "priority":
                        {
                            Priority=Convert.ToInt32(i.Value);
                            break;
                        }
                    case "warmuptime":
                        {
                            WarmUpTime=Convert.ToInt32(i.Value);
                            break;
                        }
                    case "cooldowntime":
                        {
                            CoolDownTime=Convert.ToInt32(i.Value);
                            break;
                        }
                    case "reusetime":
                        {
                            ReUseTime=Convert.ToInt32(i.Value);
                            break;
                        }
                    case "basedamage":
                        {
                            BaseDamage=Convert.ToInt32(i.Value);
                            break;
                        }
                    case "deltadamage":
                        {
                            DeltaDamage=Convert.ToInt32(i.Value);
                            break;
                        }
                    case "chancetohit":
                        {
                            ChanceToHit=Convert.ToInt32(i.Value);
                            break;
                        }
                    case "type":
                        {
                            //TODO: Sollte in Objekt(bzw enum) umgebaut werden
                            Type=i.Value.ToString();
                            break;
                        }
                    case "pre-delay":
                        {
                            PreDelay=Convert.ToInt32(i.Value);
                            break;
                        }
                    case "aft-delay":
                        {
                            AftDelay=Convert.ToInt32(i.Value);
                            break;
                        }
                    case "damage-factor":
                        {
                            DamageFactor=Convert.ToInt32(i.Value);
                            break;
                        }
                    case "range":
                        {
                            Range=Convert.ToInt32(i.Value);
                            break;
                        }
                    case "animation":
                        {
                            //TODO: Sollte in Objekt(bzw enum) umgebaut werden
                            Animation=i.Value.ToString();
                            break;
                        }
                    case "particle-effect":
                        {
                            ParticleEffect=i.Value.ToString();
                            break;
                        }
                    case "action":
                        {
                            Action=i.Value.ToString();
                            break;
                        }
                    case "element":
                        {
                            Element=Enums.GetElementFromString(i.Value.ToString());
                            break;
                        }
                    default:
                        {
                            throw new NotImplementedException();
                        }
                }
            }
			
            Thread.CurrentThread.CurrentCulture=oldCult;
        }
    }
}
