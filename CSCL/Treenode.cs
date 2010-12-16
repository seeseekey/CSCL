using System;
using System.Collections.Generic;
using System.Text;

namespace CSCL
{
	public class Treenode<T>
	{
		public T Value
		{
			get;
			set;
		}

		//public Treenode<T> Parent
		//{
		//    get;
		//    private set;
		//}

		public List<Treenode<T>> Childs
		{
			get;
			set;
		}

		public Treenode()
		{
			Childs=new List<Treenode<T>>();
		}

		public Treenode(T value)
		{
			Childs=new List<Treenode<T>>();
			Value=value;
		}
	}
}
