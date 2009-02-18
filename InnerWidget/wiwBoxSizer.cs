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
		protected string _name;
		protected int _orient;
		protected int _min_size;
		
		public wdbBoxSizerProps() : base()
		{
			_name = "";
			_orient = 0;
			_min_size = -1;
		}
		
		[CategoryAttribute("Box Sizer"), DescriptionAttribute("Box Sizer")]
		public string Name
		{
			get	{	return _name;	}
			set	{	_name = value;	NotifyPropertyChanged("Name");	}
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
		protected IWDBBase _client;
		protected wdbBoxSizerProps _props;
		protected bool _is_selected;
		protected IWDBBase _client_parent;
		
		public wiwBoxSizer() : base(wx.Orientation.wxHORIZONTAL)
		{
			_props = new wdbBoxSizerProps();
			_frame_cur_index++;			
			string name = "BoxSizer" + _frame_cur_index.ToString();
			SetDefaultProps(name);
			SetWidgetProps();
		}
		
		#region IWidgetElem Interface implementation
		public WidgetProps Properties	
		{	
			get	{	return _props; }
		}
		public wx.Window WxWindow	
		{	
			get	{	return null;	}
		}
		public wx.Sizer	WxSizer		
		{	
			get	{	return	this;	}
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
		public IWDBBase ClientParent
		{
			get	{	return _client_parent;	}
		}
		public Point AreaOrigin
		{	
			get {	return _client_parent.AreaOrigin;	}
		}
		public Size AreaSize
		{	
			get	{	return _client_parent.AreaSize;	}
		}		
		public int WidgetID
		{
			get	{	return (int)StandardWidgetID.WID_BOXSIZER; }
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
			// _elem = parent.WDBBase.WxWindow;
			if (parent.IsSizer)
			{
				// Add a sizer to a sizer
				parent.WxSizer.Add(this);
				_client_parent = parent.ClientParent;
			} else {
				parent.WxWindow.SetSizer(this, true);
				this.FitInside(parent.WxWindow);
				_client_parent = parent;
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
		}		
		
		public void winProps_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
            	case "Name":
					Common.Instance().ObjTree.SelectedNode.Text = _props.Name;	
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
