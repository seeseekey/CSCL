using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing.Design;
using System.Text;
using System.Windows.Forms;

namespace CSCL.Controls
{
	//TODO: Merken welcher der Aktiver View ist (im Designer)

    /// <summary>
    /// Interne Panel für das Multiview
    /// </summary>
    public class InternalMultiViewPanel: Panel
    {
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override AnchorStyles Anchor
        {
            get
            {
                return base.Anchor;
            }
            set
            {
                base.Anchor = value;
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override DockStyle Dock
        {
            get
            {
                return base.Dock;
            }
            set
            {
                base.Dock = value;
            }
        }
    }

    /// <summary>
    /// MultiView Control
    /// </summary>
	public class MultiView : Panel
	{
		#region MVControlCollection
		public class MVControlCollection : Control.ControlCollection
		{
			private MultiView owner;

			public MVControlCollection(MultiView owner)
				: base(owner)
			{
				if(owner==null) throw new ArgumentNullException("owner");
				this.owner=owner;
			}

			public override void Add(Control value)
			{
				if(!(value is InternalMultiViewPanel)) throw new ArgumentException("value");
				base.Add(value);
				owner.views.Add((InternalMultiViewPanel)value);
				owner.ActiveView=owner.views[owner.views.Count-1];
			}

			public override void Remove(Control value)
			{
				base.Remove(value);
				owner.views.Remove((InternalMultiViewPanel)value);
				if(owner.views.Count>0) owner.ActiveView=owner.views[0];
			}
		}
		#endregion

        #region InternalMultiViewPanelList
        public class InternalMultiViewPanelList : IList, ICollection, IEnumerable
		{
			private MultiView owner;

			public InternalMultiViewPanelList(MultiView owner)
			{
				if(owner==null) throw new ArgumentNullException("owner");
				this.owner=owner;
			}

			public void Add(InternalMultiViewPanel value)
			{
				if(value==null) throw new ArgumentNullException("value");
				owner.Controls.Add(value);
			}

			public void Remove(InternalMultiViewPanel value)
			{
				owner.Controls.Remove(value);
			}

			public void Clear()
			{
				owner.Controls.Clear();
				owner.views.Clear();
			}

			public bool Contains(InternalMultiViewPanel value)
			{
				if(value==null) throw new ArgumentNullException("value");
				return IndexOf(value)!=-1;
			}

			public InternalMultiViewPanel this[int index]
			{
				get { return owner.views[index]; }
				set { if(value==null) throw new ArgumentNullException("value"); owner.views[index]=value; }
			}

			public int IndexOf(InternalMultiViewPanel page)
			{
				if(page==null) throw new ArgumentNullException("value");
				for(int i=0; i<Count; i++) if(this[i]==page) return i;
				return -1;
			}

			public void Insert(int index, InternalMultiViewPanel value)
			{
				if(index<0||index>Count) throw new ArgumentOutOfRangeException("index");
				if(value==null) throw new ArgumentNullException("value");

				owner.views.Insert(index, value);
				owner.Controls.Add(value);
				owner.Controls.SetChildIndex(value, index);
			}

			#region IList Members
			int IList.Add(object value)
			{
				if(!(value is InternalMultiViewPanel)) throw new ArgumentException("value");
				Add((InternalMultiViewPanel)value);
				return IndexOf((InternalMultiViewPanel)value);
			}

			bool IList.Contains(object value)
			{
				if(!(value is InternalMultiViewPanel)) return false;
				return owner.views.Contains((InternalMultiViewPanel)value);
			}

			int IList.IndexOf(object value)
			{
				if(!(value is InternalMultiViewPanel)) return -1;
				return owner.views.IndexOf((InternalMultiViewPanel)value);
			}

			void IList.Insert(int index, object value)
			{
				if(!(value is InternalMultiViewPanel)) throw new ArgumentException("value");
				Insert(index, (InternalMultiViewPanel)value);
			}

			void IList.Remove(object value)
			{
				if(!(value is InternalMultiViewPanel)) return;
				this.Remove((InternalMultiViewPanel)value);
			}

			bool IList.IsFixedSize
			{
				get { return false; }
			}

			public bool IsReadOnly
			{
				get { return false; }
			}

			void IList.RemoveAt(int index)
			{
				owner.Controls.RemoveAt(index);
			}

			object IList.this[int index]
			{
				get { return this[index]; }
				set
				{
					if(!(value is InternalMultiViewPanel)) throw new ArgumentException("value");
					this[index]=(InternalMultiViewPanel)value;
				}
			}
			#endregion

			#region ICollection Members
			void ICollection.CopyTo(Array array, int index)
			{
				if(Count>0) Array.Copy(owner.views.ToArray(), 0, array, index, Count);
			}

			[Browsable(false)]
			public int Count
			{
				get { return owner.views.Count; }
			}

			bool ICollection.IsSynchronized
			{
				get { return false; }
			}

			object ICollection.SyncRoot
			{
				get { return this; }
			}
			#endregion

			#region IEnumerable Members
			public IEnumerator GetEnumerator()
			{
				return owner.views.GetEnumerator();
			}
			#endregion
		}
		#endregion

		InternalMultiViewPanelList _views;
		List<InternalMultiViewPanel> views;

		public MultiView()
		{
			_views=new InternalMultiViewPanelList(this);
			views=new List<InternalMultiViewPanel>();
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MergableProperty(false)]
		public InternalMultiViewPanelList Views { get { return _views; } }

		//[DefaultValue(false)]
		public InternalMultiViewPanel ActiveView
		{
			get
			{
				foreach(InternalMultiViewPanel view in views) if(view.Visible) return view;
				return null;
			}
			set
			{
				if(views.IndexOf(value)<0) return;
				foreach(InternalMultiViewPanel view in views)
				{
					view.Visible=view==value;
					view.Dock=DockStyle.Fill;
					if(view.Visible) view.BringToFront();
				}
			}
		}

		protected override Control.ControlCollection CreateControlsInstance()
		{
			return new MVControlCollection(this);
		}
	}
}
