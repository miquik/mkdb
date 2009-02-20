/*
 * Creato da SharpDevelop.
 * Utente: michele
 * Data: 20/02/2009
 * Ora: 16.37
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Drawing;


namespace mkdb.Widgets
{
	
	/* wxGridSizer : name, rows, cols, vgap, hgap, min_size, Align */
	/* wxFlexGridSizer : name, rows, cols, vgap, hgap, Align */	
	public class wdbGridSizerProps : wxAlignProps
	{
		protected int _rows;
		protected int _cols;
		protected int _vgap;
		protected int _hgap;
		
		public wdbGridSizerProps() : base()
		{
			_rows = 2;
			_cols = 2;
			_vgap = 0;
			_hgap = 0;
		}				
		[CategoryAttribute("Grid Sizer"), DescriptionAttribute("Grid Sizer")]
		public int Cols
		{
			get	{	return _cols;	}
			set	{	_cols = value;	NotifyPropertyChanged("Cols");	}
		}				
		[CategoryAttribute("Grid Sizer"), DescriptionAttribute("Grid Sizer")]
		public int Rows
		{
			get	{	return _rows;	}
			set	{	_rows = value;	NotifyPropertyChanged("Rows");	}
		}		
		[CategoryAttribute("Grid Sizer"), DescriptionAttribute("Grid Sizer")]
		public int VGap
		{
			get	{	return _vgap;	}
			set	{	_vgap = value;	NotifyPropertyChanged("VGap");	}
		}		
		[CategoryAttribute("Grid Sizer"), DescriptionAttribute("Grid Sizer")]
		public int HGap
		{
			get	{	return _hgap;	}
			set	{	_hgap = value;	NotifyPropertyChanged("HGap");	}
		}						
	}
	
	public class wiwGridSizer : wx.GridSizer, IWDBBase, IWXSizer
	{
		protected static long _grid_cur_index=0;
		protected wdbGridSizerProps _props;
		protected bool _is_selected;
		protected wx.Window _p_container;
		protected wx.Sizer _p_sizer;
		protected ArrayList _list;		
		
		public wiwGridSizer(wx.Window _pc, wx.Sizer _ps, int _r, int _c, int _vg, int _hg) : 
			base(_r, _c, _vg, _hg)
		{
			_props = new wdbGridSizerProps();
			_grid_cur_index++;			
			string name = "GridSizer" + _grid_cur_index.ToString();
			SetDefaultProps(name);
			SetWidgetProps();
			_p_container = _pc;
			_p_sizer = _ps;
			_list = new ArrayList();
		}
		
		#region IWXSizer Interface implementation
		public void AddWDBBase(IWDBBase elem, int prop, int flag, int border)
		{
			if (elem.IsSizer == true)
				this.Add((wx.Sizer)elem, prop, flag, border);
			else
				this.Add((wx.Window)elem, prop, flag, border);
			this._list.Add(elem);
		}
		public void InsertWDBBase(int index, IWDBBase elem, int prop, int flag, int border)
		{
			if (elem.IsSizer == true)
				this.Insert(index, (wx.Sizer)elem, prop, flag, border, null);
			else
				this.Insert(index, (wx.Window)elem, prop, flag, border, null);
			this._list.Insert(index, elem);
		}		
		public void RemoveWDBBase(IWDBBase elem)
		{
			if (elem.IsSizer == true)
				this.Detach((wx.Sizer)elem);
			else
				this.Detach((wx.Window)elem);
			this._list.Remove(elem);
		}				
		public ArrayList List
		{
			get	{	return _list;	}
		}		
		#endregion
		
		#region IWidgetElem Interface implementation
		public wx.Window Me
		{
			get	{	return null;	}
		}		
		public WidgetProps Properties	
		{	
			get	{	return _props; }
		}
		public wx.Window ParentContainer	
		{	
			get	{	return _p_container;	}
		}
		public wx.Sizer	ParentSizer		
		{	
			get	{	return	_p_sizer;	}
		}
		public bool	IsSizer		
		{	
			get	{	return true;	}
		}
		public bool	IsSelected	
		{	
			get	{	return _is_selected;	}
			set	{	_is_selected = value;	}
		}		
		public int WidgetType
		{
			get	{	return (int)StandardWidgetType.WID_BOXSIZER; }
		}		
		
		private void SetDefaultProps(string name)
		{
			_props.EnableNotification = false;
			_props.Name = name;
			_props.Cols = 2;
			_props.Rows = 2;
			_props.VGap = 0;
			_props.HGap = 0;
			_props.EnableNotification = true;
		}
				
		public bool InsertWidget(IWDBBase parent)
		{			
			if (_p_sizer == null)			
			{				
				_p_container.SetSizer(this, true);
			} else
			{
				IWXSizer wxsizer = (IWXSizer)_p_sizer;
				wxsizer.AddWDBBase((IWDBBase)this, 0, (int)(_props.Alignment.ToLong|_props.Border.ToLong), _props.BorderWidth);				
				// _p_sizer.Add(this, 0, (int)(_props.Alignment.ToLong|_props.Border.ToLong), _props.BorderWidth);
			}
			_p_container.AutoLayout = true;
			_p_container.Layout();
			return true;
		}
		
		public bool DeleteWidget()
		{
			return false;
		}
		
		public long FindBlockInText()
		{
			return -1;
		}
		
		public bool CanAcceptChildren()
		{
			if (List.Count < this.Cols * this.Rows)
				return true;
			return false;
		}				
				
		public bool InsertWidgetInText()
		{
			return true;			
		}		
		public bool DeleteWidgetFromText()
		{
			return true;
		}
		
		public void PaintOnSelection()
		{
			Panel pan = Common.Instance().Canvas;
			// Graphics area = Graphics.FromHwnd(this.GetHandle());
			Graphics area = Graphics.FromHwnd(_p_container.GetHandle());
			Color cl = Color.FromArgb(255, _p_container.BackgroundColour.Red,
			                     _p_container.BackgroundColour.Green,
			                     _p_container.BackgroundColour.Blue);
			area.Clear(cl);
			if (IsSelected)
			{
				Pen _pen = new Pen(Color.Red, 2);
				Point _ps = this.Position;
				Size _sz = new Size(this.Size.Width, this.Size.Height);
				if (_sz.Width == 0)					
					_sz.Width = _p_container.Size.Width - _ps.X - 10;
				if (_sz.Height == 0)					
					_sz.Height = _p_container.Size.Height - _ps.Y - 10;
				area.DrawRectangle(_pen, _ps.X, _ps.Y, _sz.Width, _sz.Height);
				/*				
				int wpart = _sz.Width / this.Cols;
				int hpart = _sz.Height / this.Rows;
				for (int i=0; i<this.Cols; i++)
				{
					for (int j=0; j<this.Rows; j++)
					{
						area.DrawRectangle(_pen, j*wpart, i*hpart, wpart, hpart);						
					}
				}
				*/
				// area.DrawRectangle(_pen, _ps.X, _ps.Y, _sz.Width, _sz.Height);
			}						
		}		
		
		public void winProps_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
            	case "Name":
					Common.Instance().ObjTree.SelectedNode.Text = _props.Name;	
            		break;
            	case "Cols":
            		this.Cols = _props.Cols;
            		this.Layout();
            		break;
            	case "Rows":
            		this.Rows = _props.Rows;
            		this.Layout();
            		break;
            	case "VGap":
            		this.VGap = _props.VGap;
            		this.Layout();
            		break;
            	case "HGap":
            		this.HGap = _props.HGap;
            		this.Layout();
            		break;
            }
            this.PaintOnSelection();
        }
				
		public void SetWidgetProps()
		{
			_props.PropertyChanged += new PropertyChangedEventHandler(winProps_PropertyChanged);
			Common.Instance().ObjPropsPanel.SelectedObject = _props;
		}		
		#endregion
	}
}
