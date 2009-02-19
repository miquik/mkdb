/*
 * Creato da SharpDevelop.
 * Utente: michele
 * Data: 06/02/2009
 * Ora: 16.07
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Drawing;


namespace mkdb.Widgets
{
	
	/* wxBoxSizer : name, orient, min_size, Align */	
	public class wdbBoxSizerProps : wxAlignProps
	{
		protected int _orient;
		protected int _min_size;
		
		public wdbBoxSizerProps() : base()
		{
			_orient = 0;
			_min_size = -1;
		}
				
		[CategoryAttribute("Box Sizer"), DescriptionAttribute("Box Sizer")]
		public int Orient
		{
			get	{	return _orient;	}
			set	{	_orient = value;	NotifyPropertyChanged("Orient");	}
		}
		
		[CategoryAttribute("Box Sizer"), DescriptionAttribute("Box Sizer")]
		public int MinSize
		{
			get	{	return _min_size;	}
			set	{	_min_size = value; NotifyPropertyChanged("MinSize");	}
		}
	}

	
	public class wiwBoxSizer : wx.BoxSizer, IWDBBase
	{
		protected static long _frame_cur_index=0;
		protected wdbBoxSizerProps _props;
		protected bool _is_selected;
		protected wx.Window _p_container;
		protected wx.Sizer _p_sizer;
		
		public wiwBoxSizer(wx.Window _pc, wx.Sizer _ps) : base(wx.Orientation.wxHORIZONTAL)
		{
			_props = new wdbBoxSizerProps();
			_frame_cur_index++;			
			string name = "BoxSizer" + _frame_cur_index.ToString();
			SetDefaultProps(name);
			SetWidgetProps();
			_p_container = _pc;
			_p_sizer = _ps;
		}
		
		#region IWidgetElem Interface implementation
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
		public Point AreaOrigin
		{	
			get {	return _p_container.ClientAreaOrigin;	}
		}
		public Size AreaSize
		{	
			get	{	return _p_container.ClientSize;	}
		}		
		public int WidgetType
		{
			get	{	return (int)StandardWidgetType.WID_BOXSIZER; }
		}		
		
		private void SetDefaultProps(string name)
		{
			_props.EnableNotification = false;
			_props.Name = name;
			_props.Orient = 1;
			_props.MinSize = 500;
			_props.EnableNotification = true;
		}
				
		public bool InsertWidget(IWDBBase parent)
		{			
			if (_p_sizer == null)			
			{
				_p_container.SetSizer(this, true);
			} else
			{
				_p_sizer.Add(this, 0, (int)_props.Alignment.ToLong, (int)_props.Border.ToLong);
			}
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
			return true;
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
			/*
			Panel pan = Common.Instance().Canvas;
			// Graphics area = pan.CreateGraphics();
			Graphics area = Graphics.FromHwnd(ClientParent.WxWindow.GetHandle());
			Color cl = Color.FromArgb(255, ClientParent.WxWindow.BackgroundColour.Red,
			                     ClientParent.WxWindow.BackgroundColour.Green,
			                     ClientParent.WxWindow.BackgroundColour.Blue);
			area.Clear(cl);
			if (IsSelected)
			{
				Pen _pen = new Pen(Color.Red, 2);
				Point _ps = _client_parent.AreaOrigin;
				Size _sz = _client_parent.AreaSize;
				area.DrawRectangle(_pen, _ps.X, _ps.Y, _sz.Width, _sz.Height);
			}
			*/
		}		
		
		public void winProps_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
            	case "Name":
					// Common.Instance().ObjTree.SelectedNode.Text = _props.Name;	
            		break;
            }
        }
				
		public void SetWidgetProps()
		{
			_props.PropertyChanged += new PropertyChangedEventHandler(winProps_PropertyChanged);
			Common.Instance().ObjPropsPanel.SelectedObject = _props;
		}		
		#endregion
	}
}
