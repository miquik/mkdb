/*
 * Creato da SharpDevelop.
 * Utente: michele
 * Data: 12/01/2009
 * Ora: 17.17
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
	
	public class wdbFrameProps : wxWindowProps
	{
		// * wxFrame props : name, title, style, wxWindow, toParent
		protected string _title;
		protected wxFlags _fstyle;
		
		/*
		 *	Frame style
		 *  - DEFAULT
		 *  - TOOL_WINDOW
		 * 	- wxCAPTION + SYSTEM MENU
		 *  - MINIM_BOX
		 *  - MIN_
		 * 	- MAX_BOX
		 *  - MAX
		 *  - RESIZE_BORDER
		 * */		
		
		public wdbFrameProps() : base()
		{
			_name = "";
			_title = "Frame";
			_fstyle = new wxFlags();
			_fstyle.AddItem("wxFRAME_DEFAULT", wx.Frame.wxDEFAULT_FRAME_STYLE, true);
			_fstyle.AddItem("wxFRAME_TOOL_WINDOW", wx.Frame.wxCAPTION|wx.Frame.wxCLOSE_BOX|
			                	wx.Frame.wxFRAME_TOOL_WINDOW|wx.Frame.wxSYSTEM_MENU, false);
			_fstyle.AddItem("wxFRAME_BASE", wx.Frame.wxCAPTION|wx.Frame.wxCLOSE_BOX|wx.Frame.wxSYSTEM_MENU, false);			
			_fstyle.AddItem("wxMAXIMIZE", wx.Frame.wxMAXIMIZE, false);						
			_fstyle.AddItem("wxMAXIMIZE_BOX", wx.Frame.wxMAXIMIZE_BOX, false);						
			_fstyle.AddItem("wxMINIMIZE", wx.Frame.wxMINIMIZE, false);						
			_fstyle.AddItem("wxMINIMIZE_BOX", wx.Frame.wxMINIMIZE_BOX, false);						
			_fstyle.AddItem("wxRESIZE_BORDER", wx.Frame.wxRESIZE_BORDER, false);						
		}
		
		[CategoryAttribute("Frame"), DescriptionAttribute("Frame props")]
		public string Title
		{
			get	{	return _title;	}
			set	{	_title = value;	NotifyPropertyChanged("Title");	}
		}
		
		[CategoryAttribute("Frame"), DescriptionAttribute("Frame props")]
		[TypeConverter(typeof(wxFlagsTypeConverter))]
		[Editor(typeof(wxFlagsEditor), typeof(UITypeEditor))]
		public wxFlags Style
		{
			get	{	return _fstyle;	}
			set	{	_fstyle = value; NotifyPropertyChanged("Style");	}
		}
	}

	/// <summary>
	/// Description of wdbFrame.
	/// </summary>
	public class wiwFrame : wx.Panel, IWDBBase
	{
		protected static long _frame_cur_index=0;
		protected wdbFrameProps _props;
		protected bool _is_selected;		
		protected Point _real_pos;
		protected Size _real_size;
		
		#region SimpleFrame Defs
    	private enum mSizing {
      		NONE,
      		RIGHTBOTTOM,
      		RIGHT,
      		BOTTOM
    	};
		private	mSizing m_sizing;
    	protected int m_resizeBorder;	
    	protected int m_curX, m_curY, m_difX, m_difY;    	
		protected string m_wintitle;
		protected uint m_style;
		protected Size m_minSize;
		protected Size m_baseMinSize;
		protected IntPtr m_parent_hwnd;
		protected Color m_parent_back;
		private Bitmap offScreenBmp;
		private Graphics offScreenDC;
		#endregion
		
					
		public wiwFrame(wx.Window _parent, string _title, Point _pos, Size _size, uint _style) :
				base(_parent, wx.Panel.wxID_ANY, new Point(5, 5), _size, wx.Panel.wxRAISED_BORDER | wx.Panel.wxFULL_REPAINT_ON_RESIZE)
		{
			m_sizing = mSizing.NONE;
			m_resizeBorder = 8;
			_real_pos = _pos;
			_real_size = _size;
			// this.AutoLayout = true;
			// Layout();
			this.EVT_MOTION(new wx.EventListener(OnMouseMotion));
			this.EVT_LEFT_DOWN(new wx.EventListener(OnLeftDown));
			this.EVT_LEFT_UP(new wx.EventListener(OnLeftUp));			
			this.EVT_PAINT(new wx.EventListener(OnPaint));		
			this.EVT_SIZE(new wx.EventListener(OnSize));
			
			_props = new wdbFrameProps();
			_frame_cur_index++;	
			string temp_name = "Frame" + _frame_cur_index.ToString();
			SetDefaultProps(temp_name);
			SetWidgetProps();
		}
		
		#region IWidgetElem Interface implementation
		public wx.Window Me
		{
			get	{	return this;	}
		}		
		public WidgetProps Properties	
		{	
			get	{	return _props; }
		}
		public wx.Window ParentContainer	
		{	
			get	{	return null;	}
		}
		public wx.Sizer	ParentSizer		
		{	
			get	{	return	null;	}
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
			get	{	return (int)StandardWidgetType.WID_FRAME; }
		}
		
				
		private void SetDefaultProps(string name)
		{
			_props.EnableNotification = false;
			_props.Name = name;
			_props.Title = name;
			_props.Pos = new Point(_real_pos.X, _real_pos.Y);
			_props.Size = new Size(_real_size.Width, _real_size.Height);
			_props.EnableNotification = true;
			this.Name = name;
			this.Title = name;
			// this.Position = new Point(_props.Pos.X+1, _props.Pos.Y+1);
			this.SetSize(5, 5, _real_size.Width, _real_size.Height);
			this.BackgroundColour = (wx.Colour)_props.BC;
			this.ForegroundColour = (wx.Colour)_props.FC;			
		}
				
		public bool InsertWidget(IWDBBase parent)
		{
			return true;
		}
		public bool DeleteWidget()
		{
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
		
		public long FindBlockInText()
		{
			return -1;
		}
		
		public bool CanAcceptChildren()
		{
			if (this.Sizer == null)
				return true;
			return false;
		}
		
		public void PaintOnSelection()
		{
			Panel pan = Common.Instance().Canvas;
			Graphics area = pan.CreateGraphics();
			area.Clear(pan.BackColor);
			// Graphics clientDC = Graphics.FromHwnd(m_parent_hwnd);
			// clientDC.Clear(m_parent_back);
			if (IsSelected)
			{
				Pen _pen = new Pen(Color.Red, 1);
				Point ps = new Point(4, 4);
				area.DrawRectangle(_pen, ps.X, ps.Y, Size.Width + 1, Size.Height + 1);
			}			
		}
				
		public void winProps_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
			wdbFrameProps wp = (wdbFrameProps)_props;
            switch (e.PropertyName)
            {
            	case "Title":
            		this.Title = wp.Title; 
            		break;
            	case "Name":
            		this.Name = wp.Name;
					Common.Instance().ObjTree.SelectedNode.Text = wp.Name;            		
            		break;
            	case "ID":
            		this.ID = wp.ID;
            		break;
            	case "Pos":
            		// Only change in Python text
            		// _elem.Position = wp.Pos;
            		break;
            	case "Size":
            		this.SetSize(5, 5, wp.Size.Width, wp.Size.Height);
            		_real_size.Width = wp.Size.Width;
            		_real_size.Height = wp.Size.Height;
            		break;
            	case "Font":
            		this.Font = wp.Font;
            		break;
            	case "FC":
            		this.ForegroundColour = (wx.Colour)wp.FC;
            		this.Refresh();
            		break;
            	case "BC":
            		this.BackgroundColour = (wx.Colour)wp.BC;
            		this.Refresh();
            		break;
            	case "Enabled":
            		// Only change in Python text
            		break;            		
            	case "Hidden":
            		// Only change in Python text
            		break;            		
            	case "Style":
            		// Only change in Python text
            		this.StyleFlags = wp.Style.ToLong;
            		this.Refresh();
            		break;            		
            	case "WindowStyle":
            		// Only change in Python text
            		this.WindowStyle = wp.WindowStyle.ToLong;
            		this.Refresh();
            		break;            		            		
            }            
            this.UpdateWindowUI();
        }
				
		public void SetWidgetProps()
		{
			_props.PropertyChanged += new PropertyChangedEventHandler(winProps_PropertyChanged);
			Common.Instance().ObjPropsPanel.SelectedObject = _props;
		}		
		#endregion
		
		#region SimpleFrame class implementation
		public void SetBufferSize(int w, int h)
		{
			// Double buffering
			offScreenBmp = new Bitmap(w, h);
			offScreenDC = Graphics.FromImage(offScreenBmp);		
		}
		
		// Check Check Check
		protected void OnMouseMotion(object sender, wx.Event evt)
		{		
			wx.MouseEvent e = (wx.MouseEvent)evt;
			if (m_sizing != mSizing.NONE)
			{
				Panel pan = Common.Instance().Canvas;
				Graphics dc = pan.CreateGraphics();
				dc.Clear(pan.BackColor);
				// Graphics dc = Graphics.FromHwnd((IntPtr)m_parent_hwnd);
				Pen _pen = new Pen(Color.Black, 2.0f);
				_pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
				Point _pos = new Point(5, 5);
				// Point _pos = Parent.ClientAreaOrigin;
				//dc.Clear(m_parent_back);
				if ( m_curX >= 0 && m_curY >= 0 )
					dc.DrawRectangle(_pen, _pos.X, _pos.Y, m_curX, m_curY);

				if ( m_sizing == mSizing.RIGHT || m_sizing == mSizing.RIGHTBOTTOM )
					m_curX = e.Position.X + m_difX;
				else
					m_curX = this.Size.Width;

				if ( m_sizing == mSizing.BOTTOM || m_sizing == mSizing.RIGHTBOTTOM )
					m_curY = e.Position.Y + m_difY;
				else
					m_curY = this.Size.Height;

				// User min size
				Size minSize = this.MinSize;
				if ( m_curX < minSize.Width )	m_curX = minSize.Width;
				if ( m_curY < minSize.Height ) 	m_curY = minSize.Height;

				// Internal min size
				if ( m_curX < m_minSize.Width )		m_curX = m_minSize.Width;
				if ( m_curY < m_minSize.Height ) 	m_curY = m_minSize.Height;

				System.Drawing.Size maxSize = this.MaxSize;
				if ( m_curX > maxSize.Width && maxSize.Width != wx.Panel.wxDefaultCoord ) m_curX = maxSize.Width;
				if ( m_curY > maxSize.Height && maxSize.Height != wx.Panel.wxDefaultCoord ) m_curY = maxSize.Height;

				dc.DrawRectangle(_pen, _pos.X, _pos.Y, m_curX, m_curY);
				// dc.ReleaseHdc();
			} 
			else
			{
				int x, y;
				x = this.ClientSize.Width;
				y = this.ClientSize.Height;

				if ( ( e.Position.X >= x - m_resizeBorder && e.Position.Y >= y - m_resizeBorder ) ||
				    ( e.Position.X < m_resizeBorder && e.Position.Y < m_resizeBorder ) )
				{
					this.Cursor = new wx.Cursor(wx.StockCursor.wxCURSOR_SIZENWSE);
				}
				else if ( ( e.Position.X < m_resizeBorder && e.Position.Y >= y - m_resizeBorder ) ||
				         ( e.Position.X > x - m_resizeBorder && e.Position.Y < m_resizeBorder ) )
				{
					this.Cursor = new wx.Cursor(wx.StockCursor.wxCURSOR_SIZENESW);
				}
				else if ( e.Position.X >= x - m_resizeBorder || e.Position.X < m_resizeBorder )
				{
					this.Cursor = new wx.Cursor(wx.StockCursor.wxCURSOR_SIZEWE);
				}
				else if ( e.Position.Y >= y - m_resizeBorder || e.Position.Y < m_resizeBorder )
				{
					this.Cursor = new wx.Cursor(wx.StockCursor.wxCURSOR_SIZENS);
				}
				else
				{
					this.Cursor = new wx.Cursor(wx.StockCursor.wxCURSOR_ARROW);
				}
				// title_bar.Cursor = new wx.Cursor(wx.StockCursor.wxCURSOR_ARROW);				
				// frame_content.Cursor = new wx.Cursor(wx.StockCursor.wxCURSOR_ARROW);
			}			
		}
		
		protected void OnLeftDown(object sender, wx.Event evt)
		{		
			wx.MouseEvent e = (wx.MouseEvent)evt;
			if ( m_sizing == mSizing.NONE )
			{
				if (e.Position.X >= this.Size.Width - m_resizeBorder && e.Position.Y >= this.Size.Height - m_resizeBorder)
				{
					m_sizing = mSizing.RIGHTBOTTOM;
				}
				else if (e.Position.X >= this.Size.Width - m_resizeBorder)
				{
					m_sizing = mSizing.RIGHT;
				}
				else if (e.Position.Y >= this.Size.Height - m_resizeBorder)
				{
					m_sizing = mSizing.BOTTOM;
				}
				if ( m_sizing != mSizing.NONE )
				{
					m_difX = this.Size.Width - e.Position.X;
					m_difY = this.Size.Height - e.Position.Y;
					CaptureMouse();
					OnMouseMotion(this, evt);
				}
			}			
		}

		protected void OnLeftUp(object sender, wx.Event e)
		{		
			if ( m_sizing != mSizing.NONE )
			{
				m_sizing = mSizing.NONE;
				ReleaseMouse();
				Panel pan = Common.Instance().Canvas;
				Graphics dc = pan.CreateGraphics();				
				dc.Clear(pan.BackColor);
				SetSize(5, 5, m_curX, m_curY);				
				// Check
				_props.EnableNotification = false;
				Size diff = new Size(m_curX, m_curY);
				_props.Size += diff;
				_props.EnableNotification = true;
				// Check
				m_curX = m_curY = -1;
			}
		}				
		
		protected void OnPaint(object sender, wx.Event e)
		{
			e.Skip();
		}
		
		protected void OnSize(object sender, wx.Event e)
		{
			e.Skip();
		}						
		#endregion		
		/*
		if (m_inner_frame_resized != null)
		{
			wx.CommandEvent _event = new wx.CommandEvent(wxEVT_INNER_FRAME_RESIZED, this.ID);
			this.EventHandler.ProcessEvent(_event);
		}
		*/		
	}
}
