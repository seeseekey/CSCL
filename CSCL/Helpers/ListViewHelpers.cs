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
