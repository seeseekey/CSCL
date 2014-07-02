//
//  TreeViewHelpers.cs
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
