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
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Resources;
using System.IO;

namespace mkdb
{	
/*	
	public class wifInnerTitleBar : wx.Panel
	{		
		protected uint m_style;
		protected string m_title_text;
		protected Color m_col1;
		protected Color m_col2;
		protected Image m_close;
		protected Image m_close_dis;
		protected Image m_minimize;
		protected Image m_minimize_dis;
		protected Image m_maximize;
		protected Image m_maximize_dis;
		protected byte[] m_file_data;
		private Bitmap offScreenBmp;
		private Graphics offScreenDC;
						
		
		public wifInnerTitleBar(wx.Window _parent, int _id, string _title, Point _pos, Size _size, uint _style) :
				base(_parent, _id, _pos, _size, 0)
		{
			m_style = _style;		
			m_title_text =_title;
			//
			m_col1 = SystemColors.ActiveCaption;
			byte r, g, b;
			r = (byte)Math.Min( 255, m_col1.R + 30 );
			g = (byte)Math.Min( 255, m_col1.G + 30 );
			b = (byte)Math.Min( 255, m_col1.B + 30 );
			m_col2 = Color.FromArgb(255, r, g, b);
			this.MinSize = new Size(100, 20);	
			// 		
			// Bitmap
			ResourceManager rs = new ResourceManager("mkdb.MainForm", Assembly.GetExecutingAssembly());
			m_close = (Image)rs.GetObject("close");
			m_close_dis = (Image)rs.GetObject("close_disabled");
			m_minimize = (Image)rs.GetObject("minimize");
			m_minimize_dis = (Image)rs.GetObject("minimize_disabled");
			m_maximize = (Image)rs.GetObject("maximize");
			m_maximize_dis = (Image)rs.GetObject("maximize_disabled");
			this.EVT_PAINT(new wx.EventListener(OnPaint));
			this.EVT_SIZE(new wx.EventListener(OnSize));
			// !!!
			SetBufferSize(this.Width, this.Height);
		}
		
		protected void SetBufferSize(int w, int h)
		{
			// Double buffering
			offScreenBmp = new Bitmap(w, h);
			offScreenDC = Graphics.FromImage(offScreenBmp);
		}*//*
				
		protected void OnPaint(object sender, wx.Event e)
		{
			Graphics clientDC = Graphics.FromHwnd(this.GetHandle());
			offScreenDC.FillRectangle(Brushes.Black, 0, 0, this.Width, this.Height);
			DrawTitleBar(offScreenDC);									
			clientDC.DrawImage(offScreenBmp, this.Left, this.Top);
		}
		
		protected void OnSize(object sender, wx.Event e)
		{
			SetBufferSize(this.Width, this.Height);
			Graphics clientDC = Graphics.FromHwnd(this.GetHandle());
			offScreenDC.FillRectangle(Brushes.Black, 0, 0, this.Width, this.Height);
			DrawTitleBar(offScreenDC);									
			clientDC.DrawImage(offScreenBmp, this.Left, this.Top);
		}				
		
		protected void DrawTitleBar(Graphics dc)
		{
			int margin = 2;
			int tbPosX, tbPosY; // title bar
			int tbWidth, tbHeight;

			int wbPosX, wbPosY; // window buttons
			int wbWidth, wbHeight;

			int txtPosX, txtPosY; // title text position
			int txtWidth, txtHeight;

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

			Brush _brush = new LinearGradientBrush(new Rectangle(tbPosX,  tbPosY, tbPosX + tbWidth, tbPosY + tbHeight),
			                                          m_col1, m_col2, LinearGradientMode.Horizontal);
			Pen _pen = new Pen(_brush);
			Font _font = new Font(this.Font.FaceName, this.Font.PointSize, FontStyle.Bold);
			Brush _font_brush = new SolidBrush(Color.White);
			
			// Pen pen = new Pen(Brushes.Black, 10);
			dc.DrawRectangle(_pen, tbPosX,  tbPosY, tbWidth, tbHeight);
			dc.FillRectangle(_brush, tbPosX,  tbPosY, tbWidth, tbHeight);
			dc.DrawString(m_title_text, _font, _font_brush, txtPosX, txtPosY);
			// Draw Buttons
			bool hasClose = ( m_style & wx.Panel.wxCLOSE_BOX ) != 0;
			bool hasMinimize = ( m_style & wx.Panel.wxMINIMIZE_BOX ) != 0;
			bool hasMaximize = ( m_style & wx.Panel.wxMAXIMIZE_BOX ) != 0;
			
			if (( m_style & wx.Panel.wxSYSTEM_MENU) == 0)
			{
				// On Windows, no buttons are drawn without System Menu
				return;
			}		
			dc.DrawImage(hasClose ? m_close : m_close_dis, wbPosX, wbPosY);
			wbPosX -= m_close.Width;

			if (hasMaximize)
			{
				dc.DrawImage(m_maximize, wbPosX, wbPosY);
			}
			else if (hasMinimize)
			{
				dc.DrawImage(m_maximize_dis, wbPosX, wbPosY);
			}
			wbPosX -= m_maximize.Width;

			if (hasMinimize)
			{
				dc.DrawImage(m_minimize, wbPosX, wbPosY);
			}
			else if (hasMaximize)
			{
				dc.DrawImage(m_minimize_dis, wbPosX, wbPosY);
			}
		}		
	}
	*/
	
	/// <summary>
	/// Description of InnerFrame.
	/// </summary>
	public class wifSimpleFrame : wx.Panel
	{
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
		
		
		public wifSimpleFrame(wx.Window _parent, int _id, string _title, Point _pos, Size _size, uint _style) :
				base(_parent, _id, _pos, _size, wx.Panel.wxRAISED_BORDER | wx.Panel.wxFULL_REPAINT_ON_RESIZE)
		{
			m_wintitle = _title;
			m_style = _style;
			m_sizing = mSizing.NONE;
			m_resizeBorder = 10;
			this.AutoLayout = true;
			Layout();			
			m_minSize = new Size(100, 30);			
			// !!!
			// SetBufferSize(_parent.Width, _parent.Height);
			// !!!			
			this.EVT_MOTION(new wx.EventListener(OnMouseMotion));
			this.EVT_LEFT_DOWN(new wx.EventListener(OnLeftDown));
			this.EVT_LEFT_UP(new wx.EventListener(OnLeftUp));			
			this.EVT_PAINT(new wx.EventListener(OnPaint));		
			this.EVT_SIZE(new wx.EventListener(OnSize));
		}		
		
		public IntPtr ParentHandle
		{
			get	{	return m_parent_hwnd;	}
			set	{	m_parent_hwnd = value;	}
		}		
		
		public Color ParentColor
		{
			get	{	return m_parent_back;	}
			set	{	m_parent_back = value;	}
		}				
		
		public void SetBufferSize(int w, int h)
		{
			// Double buffering
			offScreenBmp = new Bitmap(w, h);
			offScreenDC = Graphics.FromImage(offScreenBmp);
		}
		
		protected void OnMouseMotion(object sender, wx.Event evt)
		{		
			wx.MouseEvent e = (wx.MouseEvent)evt;
			if (m_sizing != mSizing.NONE)
			{
				Graphics dc = Graphics.FromHwnd((IntPtr)m_parent_hwnd);
				Pen _pen = new Pen(Color.Black, 2.0f);
				_pen.DashStyle = DashStyle.Dash;
				Point _pos = Parent.ClientAreaOrigin;
				dc.Clear(m_parent_back);
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
				Graphics dc = Graphics.FromHwnd((IntPtr)m_parent_hwnd);
				dc.Clear(m_parent_back);
				SetSize(0, 0, m_curX, m_curY);
				// title_bar.SetSize(0, 0, m_curX - 8, 20);
				
				/*
				if (m_inner_frame_resized != null)
				{
					wx.CommandEvent _event = new wx.CommandEvent(wxEVT_INNER_FRAME_RESIZED, this.ID);
					this.EventHandler.ProcessEvent(_event);
				}
				*/
				/*
				wxCommandEvent event( wxEVT_INNER_FRAME_RESIZED, GetId() );
				event.SetEventObject( this );
				GetEventHandler()->ProcessEvent( event );
				*/
				m_curX = m_curY = -1;
			}
		}				
		
		protected void OnPaint(object sender, wx.Event e)
		{
			Graphics clientDC = Graphics.FromHwnd(m_parent_hwnd);
			clientDC.Clear(m_parent_back);
			e.Skip();
		}
		
		protected void OnSize(object sender, wx.Event e)
		{
			Graphics clientDC = Graphics.FromHwnd(m_parent_hwnd);
			clientDC.Clear(m_parent_back);
			e.Skip();
		}				
	}
	
/*	
			// close_xpm
			m_file_data = (byte[])rs.GetObject("close_xpm");
			m_close = new wx.Bitmap(new wx.Image(m_file_data, wx.BitmapType.wxBITMAP_TYPE_XPM));			
			// close_dis_xpm
			m_file_data = (byte[])rs.GetObject("close_disabled_xpm");
			m_close_dis = new wx.Bitmap(new wx.Image(m_file_data, wx.BitmapType.wxBITMAP_TYPE_XPM));			
			
			// minimize_xpm
			m_file_data = (byte[])rs.GetObject("minimize_xpm");
			m_minimize = new wx.Bitmap(new wx.Image(m_file_data, wx.BitmapType.wxBITMAP_TYPE_XPM));			
			// minimize_disabled_xpm
			m_file_data = (byte[])rs.GetObject("minimize_disabled_xpm");
			m_minimize_dis = new wx.Bitmap(new wx.Image(m_file_data, wx.BitmapType.wxBITMAP_TYPE_XPM));			
			
			// maximize_xpm
			m_file_data = (byte[])rs.GetObject("maximize_xpm");
			m_maximize = new wx.Bitmap(new wx.Image(m_file_data, wx.BitmapType.wxBITMAP_TYPE_XPM));			
			// maximize_disabled_xpm
			m_file_data = (byte[])rs.GetObject("maximize_disabled_xpm");
			m_maximize_dis = new wx.Bitmap(new wx.Image(m_file_data, wx.BitmapType.wxBITMAP_TYPE_XPM));			
*/	
	
}
