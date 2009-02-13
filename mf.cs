/*
 * Creato da SharpDevelop.
 * Utente: michele
 * Data: 04/02/2009
 * Ora: 15.36
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel.Design;

namespace provablit
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();			
			// wx.Frame frame = new wx.Frame(null, -1, "Prova");
			wx.Frame frame = new wx.Frame(null, -1, "");
			InnerFrame panel = new InnerFrame(frame, -1, "Prova", wx.Panel.wxDefaultPosition, new Size(frame.Width, frame.Height), 0);
			frame.StyleFlags = 0;
			frame.Show();
			// Win32Utils.SetParent(panel.GetHandle(), this.Handle);
			// frame.Show();
			// panel.EVT_MOUSE_EVENTS(new wx.EventListener(OnMouseEvent));
			// wx.MiniFrame frame = new wx.MDIClientWindow(null, -1, "Prova");
			// frame.StyleFlags = 0;
			// frame.Show();
			// panel = new wx.Panel(frame, -1, new System.Drawing.Point(0,0), new System.Drawing.Size(100,100));
			// bmp = new wx.Bitmap();
			// bmp.LoadFile("../../minimize_xpm.xpm", wx.BitmapType.wxBITMAP_TYPE_XPM);
		}
		
		protected void OnMouseEvent(object sender, wx.Event e)
		{
			wx.MouseEvent evt = (wx.MouseEvent)e;
			if (evt.Entering)
			{
				InnerFrame pan = (InnerFrame)sender;
				pan.Cursor = new wx.Cursor(wx.StockCursor.wxCURSOR_CROSS);
			}
		}
		
		void MainFormPaint(object sender, PaintEventArgs e)
		{
			/*
			wx.WindowDC wdc = new wx.WindowDC(panel);
			wdc.DrawBitmap(bmp, 0, 0);
			*/
			/*
		'/ <summary>
        '/ Creates an Image object containing a screen shot of a specific window
        '/ </summary>
        '/ <param name="handle">The handle to the window. (In windows forms, this is obtained by the Handle property)</param>
        '/ <returns></returns>
        Public Function CaptureWindow(ByVal handle As IntPtr) As Image
            ' get te hDC of the target window
            Dim hdcSrc As IntPtr = User32.GetWindowDC(handle)
            ' get the size
            Dim windowRect As New User32.RECT()
            User32.GetWindowRect(handle, windowRect)
            Dim width As Integer = windowRect.right - windowRect.left
            Dim height As Integer = windowRect.bottom - windowRect.top
            ' create a device context we can copy to
            Dim hdcDest As IntPtr = GDI32.CreateCompatibleDC(hdcSrc)
            ' create a bitmap we can copy it to,
            ' using GetDeviceCaps to get the width/height
            Dim hBitmap As IntPtr = GDI32.CreateCompatibleBitmap(hdcSrc, width, height)
            ' select the bitmap object
            Dim hOld As IntPtr = GDI32.SelectObject(hdcDest, hBitmap)
            ' bitblt over
            GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, GDI32.SRCCOPY)
            ' restore selection
            GDI32.SelectObject(hdcDest, hOld)
            ' clean up 
            GDI32.DeleteDC(hdcDest)
            User32.ReleaseDC(handle, hdcSrc)

            ' get a .NET image object for it
            Dim img As Image = Image.FromHbitmap(hBitmap)
            ' free up the Bitmap object
            GDI32.DeleteObject(hBitmap)

            Return img
        End Function 'CaptureWindow*/		
			/*
			Graphics gContainer = this.panel1.CreateGraphics();	
			Graphics frameContainer = Graphics.FromHwnd(frame.GetHandle());
			
			IntPtr hdc = gContainer.GetHdc();
			IntPtr hMemdc = frameContainer.GetHdc();
			if (Win32Utils.BitBlt(hdc, 0, 0, frame.Width, frame.Height,
			                      hMemdc, 0, 0, (int)Win32Utils.TernaryRasterOperations.SRCCOPY) == true)
			{
				int k=0;
			}
			gContainer.ReleaseHdc(hdc);
			frameContainer.ReleaseHdc(hMemdc);			
			*/
			/*
			Graphics gContainer = this.CreateGraphics();			
			IntPtr hdc = gContainer.GetHdc();
			if (Win32Utils.BitBlt(hdc, 0, 0, 100, 100,
			                      hdc, 0, 0, Win32Utils.TernaryRasterOperations.NOTSRCCOPY) == true)
			{
				int k=0;
			}
			gContainer.ReleaseHdc(hdc);
			*/
		}
	}
}
