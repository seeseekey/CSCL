using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace CSCL.Math
{
    public static class Collision
    {
        /// <summary>
        /// Prüft die Kollision von zwei rechteckigen Objekten anhand der Koordinaten
        /// </summary>
        /// <param name="rect1, der linke obere Punkt eines Rechteckes muss übergeben werden"></param>
        /// <param name="rect2 DITO"></param>
        /// <returns>Kollision true/false</returns>
        public static bool Intersect(Rectangle rect1, Rectangle rect2)
        {
            if(((rect1.X<(rect2.X+rect2.Width))
              &&(rect2.X<(rect1.X+rect1.Width)))
              &&(rect1.Y<(rect2.Y+rect2.Height)))
            {
                return (rect2.Y<(rect1.Y+rect1.Height));
            }
            return false;
        }
    }
}
