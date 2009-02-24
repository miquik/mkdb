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
			this.EnableNotification = false;
			this.BorderWidth = 0;
			this.EnableNotification = true;
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
	
	public class wiwGridSizer : wx.GridSizer, IWDBBase
	{
		protected static long _grid_cur_index=0;
		protected wdbGridSizerProps _props;
		protected bool _is_selected;
		protected wx.Window _p_container;
		protected wx.Sizer _p_sizer;
		protected wx.SizerItem _sizer_item;
		
		public wiwGridSizer(wx.Window _pc, wx.Sizer _ps) : 
			base(2, 2, 0, 0)
		{
			_props = new wdbGridSizerProps();
			_grid_cur_index++;			
			string name = "GridSizer" + _grid_cur_index.ToString();
			SetDefaultProps(name);
			SetWidgetProps();
			_p_container = _pc;
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
			_props.EnableNotification = true;
		}
				
		public bool InsertWidget()
		{			
			if (_p_sizer == null)			
			{				
				_p_container.SetSizer(this, true);
			} else
			{
				// IWXSizer wxsizer = (IWXSizer)_p_sizer;
				// wxsizer.AddWDBBase((IWDBBase)this, 0, (int)(_props.Alignment.ToLong|_props.Border.ToLong), _props.BorderWidth);				
				_p_sizer.Add(this, 0, (int)(_props.Alignment.ToLong|_props.Border.ToLong), _props.BorderWidth);
				_sizer_item = (wx.SizerItem)_p_sizer.GetItem(_p_sizer.GetItemCount() - 1);
			}
			_p_container.AutoLayout = true;
			_p_container.Layout();
			return true;
		}
		
		public bool DeleteWidget()
		{
			this.Clear(true);
			_p_sizer.Remove(this);				
			return false;
		}
		
		public long FindBlockInText()
		{
			return -1;
		}
		
		public bool CanAcceptChildren()
		{
			if (this.GetItemCount() < this.Cols * this.Rows)
				return true;
			return false;
		}				
				
		public void HighlightSelection()
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
				Pen _pen = new Pen(Color.Red, 1);
				Point _ps = Point.Add(this.Position, new Size(_props.BorderWidth, _props.BorderWidth));
				Size _sz = this.Size;
				if ((_sz.Width == 0) || (_sz.Height == 0))
				{
					return;
				}
				_sz.Width -= (_props.BorderWidth*2 + 2);
				_sz.Height -= (_props.BorderWidth*2 + 2);
				area.DrawRectangle(_pen, _ps.X, _ps.Y, _sz.Width, _sz.Height);
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
            this.HighlightSelection();
        }
				
		public void SetWidgetProps()
		{
			_props.PropertyChanged += new PropertyChangedEventHandler(winProps_PropertyChanged);
			Common.Instance().ObjPropsPanel.SelectedObject = _props;
		}		
		#endregion
	}
}
