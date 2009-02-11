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
			m_minimize = new wx.Bitmap("../../icons/windows/minimize_xpm.xpm", wx.BitmapType.wxBITMAP_TYPE_XPM);
			m_minimize_dis = new wx.Bitmap("../../icons/windows/minimize_disabled_xpm.xpm", wx.BitmapType.wxBITMAP_TYPE_XPM);
			m_maximize = new wx.Bitmap("../../icons/windows/maximize_xpm.xpm", wx.BitmapType.wxBITMAP_TYPE_XPM);
			m_maximize_dis = new wx.Bitmap("../../icons/windows/maximize_disabled_xpm.xpm", wx.BitmapType.wxBITMAP_TYPE_XPM);
			m_close = new wx.Bitmap("../../icons/windows/close_xpm.xpm", wx.BitmapType.wxBITMAP_TYPE_XPM);
			m_close_dis = new wx.Bitmap("../../icons/windows/close_disabled_xpm.xpm", wx.BitmapType.wxBITMAP_TYPE_XPM);
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
			wx.ClientDC dc = new wx.ClientDC(this);
			wx.BufferedDC bdc = new wx.BufferedDC(dc, this.ClientSize);
			DrawTitleBar(bdc);
		}

		protected void OnPaint(object sender, wx.Event e)
		{
			wx.PaintDC dc = new wx.PaintDC(this);
			wx.BufferedDC bdc = new BufferedDC(dc, this.ClientSize);
			DrawTitleBar(bdc);
		}

		public void DrawTitleBar(wx.DC dc)
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
				pen.Colour = col ;
				dc.Pen = pen;
				dc.DrawLine(tbPosX + i, tbPosY, tbPosX + i, tbPosY + tbHeight);
				colourR += incR;
				colourG += incG;
				colourB += incB;
			}

			// Draw title text
			Font font = dc.Font;
			System.Drawing.Size ppi = dc.SizeMM;

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
			bool hasClose = ( m_style & wxCLOSE_BOX ) != 0;
			bool hasMinimize = ( m_style & wxMINIMIZE_BOX ) != 0;
			bool hasMaximize = ( m_style & wxMAXIMIZE_BOX ) != 0;

			if (( m_style & wxSYSTEM_MENU) == 0)
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
		private	mSizing m_sizing;
    	protected int m_curX, m_curY, m_difX, m_difY;
    	protected int m_resizeBorder;
    	protected System.Drawing.Size m_minSize;
    	protected System.Drawing.Size m_baseMinSize;
  		protected wifTitleBar	m_titleBar;
  		protected wx.Panel	m_frameContent;
		
		public wifInnerFrame(wx.Window _parent, int _id, System.Drawing.Point _pos, System.Drawing.Size _size, uint _style)
					: base(_parent, _id, _pos, _size, wxRAISED_BORDER | wxFULL_REPAINT_ON_RESIZE )
		{
			m_sizing = mSizing.NONE;
			m_curX = m_curY = -1;
			m_resizeBorder = 10;

			m_titleBar = new wifTitleBar(this, -1, wx.Panel.wxDefaultPosition, wx.Panel.wxDefaultSize, style);
			m_frameContent = new wx.Panel(this, -1, wx.Panel.wxDefaultPosition, wx.Panel.wxDefaultSize);

			// Use spacers to create a 1 pixel border on left and top of content panel - this is for drawing the selection box
			// Use borders to create a 2 pixel border on right and bottom - this is so the back panel can catch mouse events for resizing
			wx.BoxSizer sizer = new wx.BoxSizer(0);
			sizer.Add(m_titleBar, 0, wx.Stretch.wxGROW | wx.Direction.wxRIGHT, 2);
			// sizer->AddSpacer( 1 );
			wx.BoxSizer horiSizer = new wx.BoxSizer(1);
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

		protected void OnMouseMotion(object sender, wx.Event e)
		{
			if (m_sizing != mSizing.NONE)
			{
				IntPtr dc = Win32Utils.GetDC(null);
				// Pen pen = new Pen(
				/*
				// HDC PaintDC = (HDC) PaintWxDC.GetHDC();
				// wx.ScreenDC dc = new wx.DC;
				wxPen pen( *wxBLACK, 1, wxDOT );

				dc.SetPen( pen );
				dc.SetBrush( *wxTRANSPARENT_BRUSH );
				dc.SetLogicalFunction( wxINVERT );

				//wxPoint pos = ClientToScreen(wxPoint(0, 0));
				wxPoint pos = GetParent()->ClientToScreen( GetPosition() );

				if ( m_curX >= 0 && m_curY >= 0 )
					dc.DrawRectangle( pos.x, pos.y, m_curX, m_curY );

				if ( m_sizing == RIGHT || m_sizing == RIGHTBOTTOM )
					m_curX = e.GetX() + m_difX;
				else
					m_curX = GetSize().x;

				if ( m_sizing == BOTTOM || m_sizing == RIGHTBOTTOM )
					m_curY = e.GetY() + m_difY;
				else
					m_curY = GetSize().y;

				// User min size
				wxSize minSize = GetMinSize();
				if ( m_curX < minSize.x ) m_curX = minSize.x;
				if ( m_curY < minSize.y ) m_curY = minSize.y;

				// Internal min size
				if ( m_curX < m_minSize.x ) m_curX = m_minSize.x;
				if ( m_curY < m_minSize.y ) m_curY = m_minSize.y;

				wxSize maxSize = GetMaxSize();
				if ( m_curX > maxSize.x && maxSize.x != wxDefaultCoord ) m_curX = maxSize.x;
				if ( m_curY > maxSize.y && maxSize.y != wxDefaultCoord ) m_curY = maxSize.y;

				dc.DrawRectangle( pos.x, pos.y, m_curX, m_curY );

				dc.SetLogicalFunction( wxCOPY );
				dc.SetPen( wxNullPen );
				dc.SetBrush( wxNullBrush );
				*/
			}

			else
			{
				int x, y;
				GetClientSize( &x, &y );

				if ( ( e.GetX() >= x - m_resizeBorder && e.GetY() >= y - m_resizeBorder ) ||
				    ( e.GetX() < m_resizeBorder && e.GetY() < m_resizeBorder ) )
				{
					SetCursor( wxCursor( wxCURSOR_SIZENWSE ) );
				}
				else if ( ( e.GetX() < m_resizeBorder && e.GetY() >= y - m_resizeBorder ) ||
				         ( e.GetX() > x - m_resizeBorder && e.GetY() < m_resizeBorder ) )
				{
					SetCursor( wxCursor( wxCURSOR_SIZENESW ) );
				}
				else if ( e.GetX() >= x - m_resizeBorder || e.GetX() < m_resizeBorder )
				{
					SetCursor( wxCursor( wxCURSOR_SIZEWE ) );
				}
				else if ( e.GetY() >= y - m_resizeBorder || e.GetY() < m_resizeBorder )
				{
					SetCursor( wxCursor( wxCURSOR_SIZENS ) );
				}
				else
				{
					SetCursor( *wxSTANDARD_CURSOR );
				}

				m_titleBar->SetCursor( *wxSTANDARD_CURSOR );
				m_frameContent->SetCursor( *wxSTANDARD_CURSOR );
			}
		}

		void wxInnerFrame::OnLeftDown( wxMouseEvent& e )
		{
			if ( m_sizing == NONE )
			{
				if ( e.GetX() >= GetSize().x - m_resizeBorder && e.GetY() >= GetSize().y - m_resizeBorder )
					m_sizing = RIGHTBOTTOM;
				else if ( e.GetX() >= GetSize().x - m_resizeBorder )
					m_sizing = RIGHT;
				else if ( e.GetY() >= GetSize().y - m_resizeBorder )
					m_sizing = BOTTOM;

				if ( m_sizing != NONE )
				{
					m_difX = GetSize().x - e.GetX();
					m_difY = GetSize().y - e.GetY();
					CaptureMouse();
					OnMouseMotion( e );
				}
			}
		}

		void wxInnerFrame::OnLeftUp( wxMouseEvent& )
		{
			if ( m_sizing != NONE )
			{
				m_sizing = NONE;
				ReleaseMouse();

				wxScreenDC dc;
				wxPen pen( *wxBLACK, 1, wxDOT );

				dc.SetPen( pen );
				dc.SetBrush( *wxTRANSPARENT_BRUSH );
				dc.SetLogicalFunction( wxINVERT );

				//wxPoint pos = ClientToScreen(wxPoint(0, 0));
				wxPoint pos = GetParent()->ClientToScreen( GetPosition() );

				dc.DrawRectangle( pos.x, pos.y, m_curX, m_curY );

				dc.SetLogicalFunction( wxCOPY );
				dc.SetPen( wxNullPen );
				dc.SetBrush( wxNullBrush );

				SetSize( m_curX, m_curY );

				wxCommandEvent event( wxEVT_INNER_FRAME_RESIZED, GetId() );
				event.SetEventObject( this );
				GetEventHandler()->ProcessEvent( event );

				m_curX = m_curY = -1;
			}
		}


		void wxInnerFrame::ShowTitleBar( bool show )
		{
			m_titleBar->Show( show );
			m_minSize = ( show ? m_baseMinSize : wxSize( 10, 10 ) );
			Layout();
		}

		void wxInnerFrame::SetToBaseSize()
		{
			if ( m_titleBar->IsShown() )
			{
				SetSize( m_baseMinSize );
			}
			else
			{
				SetSize( wxSize( 10, 10 ) );
			}
		}

		bool wxInnerFrame::IsTitleBarShown()
		{
			return m_titleBar->IsShown();
		}

		void wxInnerFrame::SetTitle( const wxString &title )
		{
			m_titleBar->SetTitle( title );
		}

		wxString wxInnerFrame::GetTitle()
		{
			return m_titleBar->GetTitle();
		}

		void wxInnerFrame::SetTitleStyle( long style )
		{
			m_titleBar->SetStyle( style );
		}
	}
}
