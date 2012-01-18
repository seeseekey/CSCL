//
//  ListViewHelpers.cs
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
using System.Collections;
using System.Windows.Forms;

namespace CSCL.Helpers
{
	public class ListViewHelpers
	{
		public class ListViewComparer : IComparer
		{
			private int col;
			private SortOrder order;
			public ListViewComparer(int col, SortOrder order)
			{
				this.col=col;
				this.order=order;
			}
			public int Compare(object x, object y)
			{
				ListViewItem item1, item2;
				item1=(ListViewItem)x;
				item2=(ListViewItem)y;
				if (this.order==SortOrder.Ascending)
					return item1.SubItems[col].Text.CompareTo(item2.SubItems[col].Text);
				else
					return
				  item2.SubItems[col].Text.CompareTo(item1.SubItems[col].Text);
			}
		}

		/// <summary>
		/// Setzt die Columns der Liste auf gleichgroße Werte
		/// </summary>
		/// <param name="lw"></param>
		/// <param name="toleranz"></param>
		public static void SetUniformColumnWidth(ListView lw, int toleranz)
		{
			foreach (ColumnHeader column in lw.Columns)
			{
				column.Width=(lw.Width/lw.Columns.Count)-toleranz;
			}
		}

		/// <summary>
		/// Setzt die Länge anhand des Inahltes
		/// </summary>
		/// <param name="lw"></param>
		public static void SetColumnWidthFromContent(ListView lw)
		{
			foreach (ColumnHeader column in lw.Columns)
			{
				column.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
			}
		}
	}
}
