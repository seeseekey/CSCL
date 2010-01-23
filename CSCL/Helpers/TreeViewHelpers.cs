using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace CSCL.Helpers
{
	public class TreeViewHelpers
	{
		public static TreeNode FindNode(TreeNodeCollection tncoll, String strText)
		{
			TreeNode tnFound;
			foreach (TreeNode tnCurr in tncoll)
			{
				if (tnCurr.Text.StartsWith(strText))
				{
					return tnCurr;
				}
				tnFound=FindNode(tnCurr.Nodes, strText);
				if (tnFound!=null)
				{
					return tnFound;
				}
			}
			return null;
		}
	}
}
