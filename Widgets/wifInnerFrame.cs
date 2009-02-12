/*
 * Creato da SharpDevelop.
 * Utente: michele
 * Data: 11/02/2009
 * Ora: 12.20
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Resources;
using wx;

namespace mkdb.Widgets
{
	public class wifTitleBar : wx.Panel
	{
		protected wx.Bitmap m_minimize;
		protected wx.Bitmap m_minimize_dis;
		protected wx.Bitmap m_maximize;
		protected wx.Bitmap m_maximize_dis;
		protected wx.Bitmap m_close;
		protected wx.Bitmap m_close_dis;
		protected uint m_style;
		protected wxColor m_color1;
		protected wxColor m_color2;
		protected string m_titleText;
		
		public wifTitleBar(wx.Window _parent, int _id, System.Drawing.Point _pos, System.Drawing.Size _size, uint _style)
			: base(_parent, _id, _pos, _size, _style)
		{
			// get a reference to the current assembly
			
			string str = XpmIcons.minimize_xpm;
			byte[] dbyte = StrToByteArray(str);
			// wx.XPMHandler img = new wx.XPMHandler();
			wx.Image img = new wx.Image(dbyte, wx.BitmapType.wxBITMAP_TYPE_XPM_DATA);
			m_minimize = new wx.Bitmap(new wx.Image(StrToByteArray(XpmIcons.minimize_xpm), wx.BitmapType.wxBITMAP_TYPE_XPM_DATA));
			
			/*
            Assembly ass = Assembly.GetExecutingAssembly();            
            m_minimize = new wx.Bitmap("minimize_xpm", ass);
            m_minimize_dis = new wx.Bitmap("minimize_disabled_xpm", ass);
            m_maximize = new wx.Bitmap("maximize_xpm", ass);
            m_maximize_dis = new wx.Bitmap("maximize_disabled_xpm", ass);
            m_close = new wx.Bitmap("close_xpm", ass);
            m_close_dis = new wx.Bitmap("close_disabled_xpm", ass);
            */          
           	// m_minimize = new wx.Bitmap();
           	// bool res = m_minimize.LoadFile("../../icons/windows/minimize_xpm.xpm", wx.BitmapType.wxBITMAP_TYPE_XPM);
           	m_minimize_dis = new wx.Bitmap();
           	m_minimize_dis.LoadFile("../../icons/windows/minimize_disabled_xpm.xpm", wx.BitmapType.wxBITMAP_TYPE_XPM);
           	m_maximize = new wx.Bitmap();
           	m_maximize.LoadFile("../../icons/windows/maximize_xpm.xpm", wx.BitmapType.wxBITMAP_TYPE_XPM);
           	m_maximize_dis = new wx.Bitmap();
           	m_maximize_dis.LoadFile("../../icons/windows/maximize_disabled_xpm.xpm", wx.BitmapType.wxBITMAP_TYPE_XPM);
           	m_close = new wx.Bitmap();
           	m_close.LoadFile("../../icons/windows/close_xpm.xpm", wx.BitmapType.wxBITMAP_TYPE_XPM);
           	m_close_dis = new wx.Bitmap();
           	m_close_dis.LoadFile("../../icons/windows/close_disabled_xpm.xpm", wx.BitmapType.wxBITMAP_TYPE_XPM);
			
			m_style = _style;	
			
			m_color1 = new wxColor();
			m_color1.Set(wx.SystemSettings.GetColour(wx.SystemColour.wxSYS_COLOUR_ACTIVECAPTION));
			byte r, g, b;
			r = (byte)Math.Min( 255, m_color1.Red + 30 );
			g = (byte)Math.Min( 255, m_color1.Green + 30 );
			b = (byte)Math.Min( 255, m_color1.Blue + 30 );
			m_color2 = new wxColor(r, g, b);
			m_titleText = "mkdbTitleBar";
			this.MinSize = new System.Drawing.Size(100, 19);
			
			// Event connection
			this.EVT_LEFT_DOWN(new wx.EventListener(OnLeftClick));
			this.EVT_PAINT(new wx.EventListener(OnPaint));
			this.EVT_SIZE(new wx.EventListener(OnSize));			
		}		
		
		// C# to convert a string to a byte array.
		public static byte[] StrToByteArray(string str)
		{
    		System.Text.ASCIIEncoding  encoding=new System.Text.ASCIIEncoding();
    		return encoding.GetBytes(str);
		}
		
		
		protected System.Drawing.Size DoGetBestSize()
		{
			return new System.Drawing.Size(100, 19);
		}
		
		public string BarTitle
		{
			get	{	return m_titleText;	}
			set	{	m_titleText = value;	}
		}				
		
		public void SetStyle(uint style) 
		{ 
			m_style = style; 
		}
		
		// protected void OnMouseEvent(object sender, wx.Event evt)
		protected void OnLeftClick(object sender, wx.Event e)
		{
			// GetParent()->GetEventHandler()->ProcessEvent( event );
			Parent.EventHandler.ProcessEvent(e);
		}

		protected void OnSize(object sender, wx.Event e)
		{
			// wx.ClientDC dc = new wx.ClientDC(this);
			wx.ClientDC dc = new wx.ClientDC(this);
			wx.BufferedDC bdc = new wx.BufferedDC(dc, this.ClientSize);
			DrawTitleBar(bdc, bdc.GetPPI());
			// wx.BufferedDC bdc = new wx.BufferedDC(dc, this.ClientSize);
			// DrawTitleBar(bdc, bdc.GetPPI());
		}

		protected void OnPaint(object sender, wx.Event e)
		{
			// wx.Bitmap bmp = new wx.Bitmap(this.ClientSize.Width, this.ClientSize.Height, 24);
			wx.PaintDC dc = new wx.PaintDC(this);
			// wx.BufferedDC bdc = new BufferedDC(dc, bmp);
			DrawTitleBar(dc, dc.GetPPI());
			// wx.BufferedDC bdc = new BufferedDC(dc, this.ClientSize);
			// DrawTitleBar(bdc, bdc.GetPPI());
		}

		public void DrawTitleBar(wx.DC dc, System.Drawing.Size ppi)
		{
			int margin = 2;
			int tbPosX, tbPosY; // title bar
			int tbWidth, tbHeight;

			int wbPosX, wbPosY; // window buttons
			int wbWidth, wbHeight;

			int txtPosX, txtPosY; // title text position
			int txtWidth, txtHeight;

			// setup all variables
			
			// System.Drawing.Size clientSize( GetClientSize() );
			tbPosX = tbPosY = 0;
			tbHeight = m_close.Height + margin * 2;
			tbWidth = ClientSize.Width;

			wbHeight = m_close.Height;
			wbWidth = m_close.Width;
			wbPosX = tbPosX + tbWidth - wbWidth - 2 * margin;
			wbPosY = tbPosX + margin;

			txtPosY = tbPosY + margin;
			txtPosX = tbPosX + 15 + 2 * margin;
			txtHeight = tbHeight - 2 * margin + 1;
			txtWidth = wbPosX - 2 * margin - txtPosX;

			// Draw title background with vertical gradient.
			float incR = ( float )( m_color2.Red - m_color1.Red ) / tbWidth;
			float incG = ( float )( m_color2.Green - m_color1.Green ) / tbWidth;
			float incB = ( float )( m_color2.Blue - m_color1.Blue ) / tbWidth;

			float colourR = (float)m_color1.Red;
			float colourG = (float)m_color1.Green;
			float colourB = (float)m_color1.Blue;

			wxColor col = new wxColor();
			wx.Pen pen = new wx.Pen("pen");
			for (int i = 0; i < tbWidth; i++)
			{
				col.Set((byte)colourR, (byte)colourG, (byte)colourB);
				pen.Colour = col;
				dc.Pen = pen;
				dc.DrawLine(tbPosX + i, tbPosY, tbPosX + i, tbPosY + tbHeight);
				colourR += incR;
				colourG += incG;
				colourB += incB;
			}

			// Draw title text
			wx.Font font = dc.Font;
			// System.Drawing.Size ppi = dc.SizeMM;
			// System.Drawing.Size ppi = dc.DeviceOrigin;

			int fontSize = 72 * txtHeight / ppi.Height;
			font.Weight = wx.FontWeight.wxBOLD;
			dc.TextForeground = wx.Colour.wxWHITE;

			// text vertical adjustment
			int tw, th;
			do
			{
				font.PointSize = fontSize--;
				dc.Font = font;
				dc.GetTextExtent(m_titleText,  out tw, out th);
			} while ( th > txtHeight );

			dc.DrawLabel(m_titleText, new System.Drawing.Rectangle(txtPosX, txtPosY, tw, th));

			// Draw Buttons
			bool hasClose = ( m_style & wx.Panel.wxCLOSE_BOX ) != 0;
			bool hasMinimize = ( m_style & wx.Panel.wxMINIMIZE_BOX ) != 0;
			bool hasMaximize = ( m_style & wx.Panel.wxMAXIMIZE_BOX ) != 0;

			if (( m_style & wx.Panel.wxSYSTEM_MENU) == 0)
			{
				// On Windows, no buttons are drawn without System Menu
				return;
			}

			dc.DrawBitmap(hasClose ? m_close : m_close_dis, wbPosX, wbPosY, true);
			wbPosX -= m_close.Width;

			if (hasMaximize)
			{
				dc.DrawBitmap(m_maximize, wbPosX, wbPosY, true);
			}
			else if (hasMinimize)
			{
				dc.DrawBitmap(m_maximize_dis, wbPosX, wbPosY, true);
			}
			wbPosX -= m_maximize.Width;

			if (hasMinimize)
			{
				dc.DrawBitmap(m_minimize, wbPosX, wbPosY, true);
			}
			else if (hasMaximize)
			{
				dc.DrawBitmap(m_minimize_dis, wbPosX, wbPosY, true);
			}
		}
	}
	
	
	public class wifInnerFrame : wx.Panel
	{
    	private enum mSizing {
      		NONE,
      		RIGHTBOTTOM,
      		RIGHT,
      		BOTTOM
    	};
		public const int wxEVT_INNER_FRAME_RESIZED = 6000;
		private	mSizing m_sizing;
    	protected int m_curX, m_curY, m_difX, m_difY;
    	protected int m_resizeBorder;
    	protected System.Drawing.Size m_minSize;
    	protected System.Drawing.Size m_baseMinSize;
  		protected wifTitleBar	m_titleBar;
  		protected wx.Panel	m_frameContent;
  		protected wx.EventListener m_inner_frame_resized;
		
		public wifInnerFrame(wx.Window _parent, int _id, System.Drawing.Point _pos, System.Drawing.Size _size, uint _style)
					: base(_parent, _id, _pos, _size, wxRAISED_BORDER | wxFULL_REPAINT_ON_RESIZE )
		{
			m_sizing = mSizing.NONE;
			m_curX = m_curY = -1;
			m_resizeBorder = 10;

			m_titleBar = new wifTitleBar(this, -1, wx.Panel.wxDefaultPosition, wx.Panel.wxDefaultSize, _style);
			m_frameContent = new wx.Panel(this, -1, wx.Panel.wxDefaultPosition, wx.Panel.wxDefaultSize);

			// Use spacers to create a 1 pixel border on left and top of content panel - this is for drawing the selection box
			// Use borders to create a 2 pixel border on right and bottom - this is so the back panel can catch mouse events for resizing
			wx.BoxSizer sizer = new wx.BoxSizer(wx.Orientation.wxVERTICAL);
			sizer.Add(m_titleBar, 0, wx.Stretch.wxGROW | wx.Direction.wxRIGHT, 2);
			// sizer->AddSpacer( 1 );
			wx.BoxSizer horiSizer = new wx.BoxSizer(wx.Orientation.wxHORIZONTAL);
			// horiSizer->AddSpacer( 1 );
			horiSizer.Add(m_frameContent, 1, wx.Stretch.wxGROW);
			sizer.Add(horiSizer, 1, wx.Stretch.wxGROW | wx.Direction.wxBOTTOM | wx.Direction.wxRIGHT, 2);

			SetSizer(sizer);
			this.AutoLayout = true;
			Layout();

			m_minSize = m_titleBar.MinSize;
			m_minSize.Width += 8;
			m_minSize.Height += 10;
			m_baseMinSize = m_minSize;

			if (this.Size == wx.Panel.wxDefaultSize)
			{
				SetSize(this.BestSize);
			}			
			m_inner_frame_resized = null;
			this.EVT_MOTION(new wx.EventListener(OnMouseMotion));
			this.EVT_LEFT_DOWN(new wx.EventListener(OnLeftDown));
			this.EVT_LEFT_UP(new wx.EventListener(OnLeftUp));
			/*
			BEGIN_EVENT_TABLE( wxInnerFrame, wxPanel )
			EVT_MOTION( wxInnerFrame::OnMouseMotion )
			EVT_LEFT_DOWN( wxInnerFrame::OnLeftDown )
			EVT_LEFT_UP( wxInnerFrame::OnLeftUp )
			END_EVENT_TABLE()
			*/
		}
		
		/**/
		public System.Drawing.Size DoGetBestSize()
		{
			System.Drawing.Size best;
			best = m_titleBar.BestSize;
			System.Drawing.Size content = m_frameContent.BestSize;
			best.Height += content.Height;
			int border = wx.SystemSettings.GetMetric(wx.SystemMetric.wxSYS_BORDER_X);
			best.Width = (content.Width + 1 > best.Width ? content.Width + 1 : best.Width) + 2 + 2 * (border > 0 ? border : 2);
			// spacers and borders
			best.Height += 3;
			return best;
		}

		protected void OnMouseMotion(object sender, wx.Event evt)
		{
			wx.MouseEvent e = (wx.MouseEvent)evt;
			if (m_sizing != mSizing.NONE)
			{
				IntPtr hdc = Win32Utils.GetDC((IntPtr)0);
				Graphics dc = Graphics.FromHdc(hdc);
				System.Drawing.Brush brush = new HatchBrush(HatchStyle.DottedGrid, Color.Black, Color.Transparent);
				System.Drawing.Pen pen = new System.Drawing.Pen(brush, 1.0f);
				// System.Drawing.Pen pen = new System.Drawing.Pen(Brushes.Black, 1.0f);
				System.Drawing.Point pos = Parent.ClientToScreen(Position);
				/*
				wxPen pen( *wxBLACK, 1, wxDOT );
				dc.SetPen( pen );
				dc.SetBrush( *wxTRANSPARENT_BRUSH );
				dc.SetLogicalFunction( wxINVERT );
				*/
				
				if ( m_curX >= 0 && m_curY >= 0 )
					dc.DrawRectangle(pen, pos.X, pos.Y, m_curX, m_curY);

				if ( m_sizing == mSizing.RIGHT || m_sizing == mSizing.RIGHTBOTTOM )
					m_curX = e.Position.X + m_difX;
				else
					m_curX = this.Size.Width;

				if ( m_sizing == mSizing.BOTTOM || m_sizing == mSizing.RIGHTBOTTOM )
					m_curY = e.Position.Y + m_difY;
				else
					m_curY = this.Size.Height;

				// User min size
				System.Drawing.Size minSize = this.MinSize;
				if ( m_curX < minSize.Width )	m_curX = minSize.Width;
				if ( m_curY < minSize.Height ) 	m_curY = minSize.Height;

				// Internal min size
				if ( m_curX < m_minSize.Width )		m_curX = m_minSize.Width;
				if ( m_curY < m_minSize.Height ) 	m_curY = m_minSize.Height;

				System.Drawing.Size maxSize = this.MaxSize;
				if ( m_curX > maxSize.Width && maxSize.Width != wx.Panel.wxDefaultCoord ) m_curX = maxSize.Width;
				if ( m_curY > maxSize.Height && maxSize.Height != wx.Panel.wxDefaultCoord ) m_curY = maxSize.Height;

				dc.DrawRectangle(pen, pos.X, pos.Y, m_curX, m_curY);
				// dc.ReleaseHdc();
				/*				
				dc.SetLogicalFunction( wxCOPY );
				dc.SetPen( wxNullPen );
				dc.SetBrush( wxNullBrush );
				*/
			} else 
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
				m_titleBar.Cursor = new wx.Cursor(wx.StockCursor.wxCURSOR_ARROW);
				m_frameContent.Cursor = new wx.Cursor(wx.StockCursor.wxCURSOR_ARROW);
				// m_titleBar->SetCursor( *wxSTANDARD_CURSOR );
				// m_frameContent->SetCursor( *wxSTANDARD_CURSOR );
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

		protected void OnLeftUp(object sender, wx.Event evt)
		{
			if ( m_sizing != mSizing.NONE )
			{
				m_sizing = mSizing.NONE;
				ReleaseMouse();
				
				IntPtr hdc = Win32Utils.GetDC((IntPtr)0);
				Graphics dc = Graphics.FromHdc(hdc);
				System.Drawing.Brush brush = new HatchBrush(HatchStyle.DottedGrid, Color.Black, Color.Transparent);
				System.Drawing.Pen pen = new System.Drawing.Pen(brush, 1.0f);
				// System.Drawing.Pen pen = new System.Drawing.Pen(Brushes.Black, 1.0f);
				System.Drawing.Point pos = Parent.ClientToScreen(Position);
				
				/*
				wxScreenDC dc;
				wxPen pen( *wxBLACK, 1, wxDOT );
				dc.SetPen( pen );
				dc.SetBrush( *wxTRANSPARENT_BRUSH );
				dc.SetLogicalFunction( wxINVERT );
				*/
				
				dc.DrawRectangle(pen, pos.X, pos.Y, m_curX, m_curY);
				/*
				dc.SetLogicalFunction( wxCOPY );
				dc.SetPen( wxNullPen );
				dc.SetBrush( wxNullBrush );
				*/
				SetSize(m_curX, m_curY);
				if (m_inner_frame_resized != null)
				{
					wx.CommandEvent _event = new wx.CommandEvent(wxEVT_INNER_FRAME_RESIZED, this.ID);
					this.EventHandler.ProcessEvent(_event);
				}
				/*
				wxCommandEvent event( wxEVT_INNER_FRAME_RESIZED, GetId() );
				event.SetEventObject( this );
				GetEventHandler()->ProcessEvent( event );
				*/
				m_curX = m_curY = -1;
				// dc.ReleaseHdc();				
			}
		}


		public void ShowTitleBar(bool show)
		{
			m_titleBar.Show(show);
			m_minSize = (show ? m_baseMinSize : new System.Drawing.Size(10, 10));
			Layout();
		}

		public void SetToBaseSize()
		{
			if (m_titleBar.IsShown)
			{
				SetSize(m_baseMinSize);
			}
			else
			{
				SetSize(new System.Drawing.Size(10, 10));
			}
		}
		
		public string BarTitle
		{
			get	{	return m_titleBar.BarTitle;		}
			set	{	m_titleBar.BarTitle = value;	}
		}

		public uint TitleStyle
		{
			set	{	m_titleBar.SetStyle(value);	}
		}
		
		public bool IsTitleBarShown
		{
			get	{	return m_titleBar.IsShown;	}
		}
		
		public wx.EventListener EvtInnerFrameResized
		{
			get	{	return m_inner_frame_resized;	}
			set	
			{
				m_inner_frame_resized = value;
				this.EventHandler.AddCommandListener(wxEVT_INNER_FRAME_RESIZED, this.ID,
				                                     m_inner_frame_resized);
			}
		}
	}
}
