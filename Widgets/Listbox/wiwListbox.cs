/*
 * Creato da SharpDevelop.
 * Utente: Family Rose
 * Data: 18/02/2009
 * Ora: 18.34
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;


namespace mkdb.Widgets
{

	public class wdbListboxProps : wxWindowProps
	{
		protected StringCollection _items;
		protected wxFlags _lbstyle;
		
		public wdbListboxProps() : base()
		{
			_name = "ComboBox";
			_items = new StringCollection();
			_lbstyle = new wxFlags();
			_lbstyle.AddItem("wxLB_ALWAYS_SB", wx.ListBox.wxLB_ALWAYS_SB, false);
			_lbstyle.AddItem("wxLB_EXTENDED", wx.ListBox.wxLB_EXTENDED, false);
			_lbstyle.AddItem("wxLB_HSCROLL", wx.ListBox.wxLB_HSCROLL, false);
			_lbstyle.AddItem("wxLB_MULTIPLE", wx.ListBox.wxLB_MULTIPLE, false);
			_lbstyle.AddItem("wxLB_NEED_SB", wx.ListBox.wxLB_NEED_SB, false);
			_lbstyle.AddItem("wxLB_SINGLE", wx.ListBox.wxLB_SINGLE, false);
			_lbstyle.AddItem("wxLB_SORT", wx.ListBox.wxLB_SORT, false);
		}
		
		[Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, " +
		        "Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]		
		[CategoryAttribute("Listbox"), DescriptionAttribute("Listbox Props")]
		public StringCollection Items
		{
			get	{	return _items;	}
			set	{	_items = value;	NotifyPropertyChanged("Items");	}
		}		
		
		[TypeConverter(typeof(wxFlagsTypeConverter))]
		[Editor(typeof(wxFlagsEditor), typeof(UITypeEditor))]
		[CategoryAttribute("Listbox"), DescriptionAttribute("Listbox Props")]
		public wxFlags ListboxStyle
		{
			get	{	return _lbstyle;	}
			set	{	_lbstyle = value;	NotifyPropertyChanged("ListboxStyle");	}
		}		
	}

	/// <summary>
	/// Description of wdbApp.
	/// </summary>
	public class wiwListbox : wx.ListBox, IWDBBase
	{
		protected static long _lbox_cur_index=0;		
		protected wdbListboxProps _props;
		protected bool _is_selected;
		protected wx.Sizer _p_sizer;
		protected wx.SizerItem _sizer_item;
			
		public wiwListbox(wx.Window _pc, wx.Sizer _ps) 
		 	: base(_pc, wx.ListBox.wxDefaultPosition, wx.ListBox.wxDefaultSize, null)
		{
			_props = new wdbListboxProps();
			_lbox_cur_index++;			
			string name = "ListBox" + _lbox_cur_index.ToString();			
			SetDefaultProps(name);
			SetWidgetProps();
			_p_sizer = _ps;
			_sizer_item = null;
		}
		
		#region IWidgetElem Interface implementation
		public wx.SizerItem SizerItem
		{
			get	{	return _sizer_item;	}
		}		
		public WidgetProps Properties	
		{	
			get	{	return _props; }
		}
		public wx.Window ParentContainer	
		{	
			get	{	return this.Parent;	}
		}
		public wx.Sizer	ParentSizer		
		{	
			get	{	return	_p_sizer;	}
		}
		public bool	IsSizer		
		{	
			get	{	return false;	}
		}
		public bool	IsSelected	
		{	
			get	{	return _is_selected;	}
			set	{	_is_selected = value;	}
		}		
		public int WidgetType
		{
			get	{	return (int)StandardWidgetType.WID_BUTTON; }
		}
		#endregion
		
		private void SetDefaultProps(string name)
		{
			_props.EnableNotification = false;	
			_props.Name = name;
			_props.EnableNotification = true;
			this.Label = name;
			string[] tmp = new string[_props.Items.Count];
			_props.Items.CopyTo(tmp, 0);
			this.Append(tmp);			
		}
				
		public bool InsertWidget()
		{
			_p_sizer.Add(this, 0, (int)(_props.Alignment.ToLong|_props.Border.ToLong), _props.BorderWidth);
			_sizer_item = (wx.SizerItem)_p_sizer.GetItem(_p_sizer.GetItemCount() - 1);
			this.Parent.AutoLayout = true;
			this.Parent.Layout();	
			return true;
		}
		public bool DeleteWidget()
		{
			// _p_sizer.Detach(this);
			_p_sizer.Remove(this);			
			return false;
		}		
		
		public long FindBlockInText()
		{
			return -1;
		}
		
		public bool CanAcceptChildren()
		{
			return false;
		}
		
		public void HighlightSelection()
		{			
			Panel pan = Common.Instance().Canvas;
			// Graphics area = Graphics.FromHwnd(this.GetHandle());
			Graphics area = Graphics.FromHwnd(this.Parent.GetHandle());
			// Graphics area = Graphics.FromHwnd(pan.Handle);
			Color cl = Color.FromArgb(255, this.Parent.BackgroundColour.Red,
			                     this.Parent.BackgroundColour.Green,
			                     this.Parent.BackgroundColour.Blue);
			area.Clear(cl);
			if (IsSelected)
			{
				Pen _pen = new Pen(Color.Red, 1);
				Point _ps = Point.Subtract(this.Position, new Size(_props.BorderWidth, _props.BorderWidth));
				Size _sz = Size.Add(this.Size, new Size(_props.BorderWidth * 2 - 1, _props.BorderWidth * 2 - 1));				
				area.DrawRectangle(_pen, _ps.X, _ps.Y, _sz.Width, _sz.Height);
			}			
		}
								
		public void winProps_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
			bool baa = false;
            switch (e.PropertyName)
            {
            	case "Name":
					Common.Instance().ObjTree.SelectedNode.Text = _props.Name;	
            		break;            		
            	case "Items":
					string[] tmp = new string[_props.Items.Count];
					_props.Items.CopyTo(tmp, 0);
					this.Clear();
					// this.Append(tmp);            		
					this.InsertItems(tmp, 0);
					this.Refresh();
            		break;
            	case "ListboxStyle":
            		this.StyleFlags = _props.ListboxStyle.ToLong;
            		this.Refresh();
            		break;
            	case "Proportion":
            		baa = true;
            		break;            		            		
            	case "Border":
            		baa = true;
            		break;
            	case "BorderWidth":
            		baa = true;
            		break;
            	case "Alignment":
            		baa = true;
            		break;
            }
            if (baa)
            {
            	_sizer_item.Proportion = _props.Proportion;
           		_sizer_item.Border = _props.BorderWidth;
           		_sizer_item.Flag = (int)(_props.Alignment.ToLong|_props.Border.ToLong);
				this.Parent.AutoLayout = true;
				this.Parent.Layout();  
				this.HighlightSelection();
            }
            this.UpdateWindowUI();            
        }
		/*
		int FindPositionInSizerList(IWDBBase node, IWXSizer sizer)
		{
			int i=0;
			foreach (IWDBBase e in sizer.List)
			{
				if (node.Equals(e))
					return i;
				i++;
			}
			return -1;
		}
		*/		
		public void SetWidgetProps()
		{
			_props.PropertyChanged += new PropertyChangedEventHandler(winProps_PropertyChanged);
			Common.Instance().ObjPropsPanel.SelectedObject = _props;
		}		
				
	}
}
