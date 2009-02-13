/*
 * Creato da SharpDevelop.
 * Utente: michele
 * Data: 13/02/2009
 * Ora: 9.57
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;

namespace provablit
{
	public class InnerTitleBar : wx.Panel
	{
		protected uint style;
		
		public InnerTitleBar(wx.Window _parent, int _id, string _title, Point _pos, Size _size, uint _style) :
				base(_parent, _id, _pos, _size, 0)
		{
			style = _style;		
			SetSize(this.Width, 12);
			this.EVT_PAINT(new wx.EventListener(OnPaint));
		}
		
		protected void DrawTitleBar(wx.DC dc)
		{
			int margin = 2;
			int height = this.Font.PointSize + 2*margin;
			// title_bar.SetSize(0, 0, this.Width, height);
			// dc.Pen = new wx.Pen(new wx.Colour(0, 0, 255));
			dc.Pen = new wx.Pen(wx.Colour.wxCYAN, 1);
			dc.Brush = new wx.Brush(wx.Colour.wxCYAN, wx.FillStyle.wxNORMAL);
			dc.DrawRectangle(margin, margin, this.Width-margin, height);
			// dc.DrawLine(margin, height/2, this.Width-margin, height/2);
		}
		
		protected void OnPaint(object sender, wx.Event e)
		{
			wx.PaintDC pdc = new wx.PaintDC(this);
			DrawTitleBar(pdc);			
		}
		
	}
	
	/// <summary>
	/// Description of InnerFrame.
	/// </summary>
	public class InnerFrame : wx.Panel
	{
		protected wx.Panel title_bar;
		protected wx.Panel frame_content;
		protected string wintitle;
		protected uint style;
		
		public InnerFrame(wx.Window _parent, int _id, string _title, Point _pos, Size _size, uint _style) :
				base(_parent, _id, _pos, _size, wx.Panel.wxRAISED_BORDER | wx.Panel.wxFULL_REPAINT_ON_RESIZE)
		{
			wintitle = _title;
			style = _style;
			//
			title_bar = new InnerTitleBar(this, -1, wx.Panel.wxDefaultPosition, wx.Panel.wxDefaultSize, _style);
			frame_content = new wx.Panel(this, -1, wx.Panel.wxDefaultPosition, wx.Panel.wxDefaultSize, 0);
			//
			wx.BoxSizer sizer = new wx.BoxSizer(wx.Orientation.wxVERTICAL);
			sizer.Add(title_bar, 0, wx.Stretch.wxGROW | wx.Direction.wxRIGHT, 2);
			wx.BoxSizer horiSizer = new wx.BoxSizer(wx.Orientation.wxHORIZONTAL);
			horiSizer.Add(frame_content, 1, wx.Stretch.wxGROW);
			sizer.Add(horiSizer, 1, wx.Stretch.wxGROW | wx.Direction.wxBOTTOM | wx.Direction.wxRIGHT, 2);
			SetSizer(sizer);
			this.AutoLayout = true;
			Layout();
		}		
	}
}
