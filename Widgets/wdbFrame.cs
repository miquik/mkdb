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



namespace mkdb.Widgets
{
	[Flags]
	public enum FrameStyle
	{
		wxCAPTION 				= 0x0001,
		wxCLOSE_BOX				= 0x0002,
		wxDEFAULT_FRAME_STYLE	= 0x0004,
		wxFRAME_FLOAT_ON_PARENT = 0x0008,
    	wxFRAME_NO_TASKBAR 		= 0x0010,
    	wxFRAME_SHAPED 			= 0x0020,
    	wxFRAME_TOOL_WINDOW 	= 0x0040,
    	wxICONIZE 				= 0x0080,
    	wxMAXIMIZE 				= 0x0100,
    	wxMAXIMIZE_BOX 			= 0x0200,
    	wxMINIMIZE 				= 0x0400,
    	wxMINIMIZE_BOX 			= 0x0800,
    	wxRESIZE_BORDER 		= 0x1000,
    	wxSTAY_ON_TOP 			= 0x2000,
		wxSYSTEM_MENU    		= 0x4000
	}
	
	
	public class wdbFrameProps : wxWindowProps
	{
		// * wxFrame props : name, title, style, wxWindow, toParent
		protected string _name;
		protected string _title;
		protected FrameStyle _fstyle;
		
		public wdbFrameProps() : base()
		{
			_name = "";
			_title = "Frame";
			_fstyle = FrameStyle.wxDEFAULT_FRAME_STYLE;
		}
		
		[CategoryAttribute("Frame"), DescriptionAttribute("Frame props")]
		public string Name
		{
			get	{	return _name;	}
			set	{	_name = value;	}
		}
		
		[CategoryAttribute("Frame"), DescriptionAttribute("Frame props")]
		public string Title
		{
			get	{	return _title;	}
			set	{	_title = value;	}
		}
		
		[CategoryAttribute("Frame"), DescriptionAttribute("Frame props")]
        [Editor(typeof(FlagsEditor), typeof(UITypeEditor))]
		public FrameStyle Style
		{
			get	{	return _fstyle;	}
			set	{	_fstyle = value;	}
		}
	}
	
	/// <summary>
	/// Description of wdbFrame.
	/// </summary>
	public class wdbFrame : WidgetElem
	{
		protected static long _frame_cur_index=0;
		
		public wdbFrame()
		{
			_elem = null;
			_props = new wdbFrameProps();
		}
				
		public override bool InsertWidget()
		{
			InsertWidgetInEditor();
			return false;
		}
		
		public override bool DeleteWidget()
		{
			return false;
		}
		
		public override long FindBlockInText()
		{
			return -1;
		}
		
		protected override bool InsertWidgetInEditor()
		{
			long _cstyle;
			wdbFrameProps winProps = (wdbFrameProps)_props;
			_frame_cur_index++;
			winProps.Name = "Frame" + _frame_cur_index.ToString();
			winProps.WindowName = "wxFrameClass";
			winProps.Title = "Frame" + _frame_cur_index.ToString();
			winProps.Pos = new System.Drawing.Point(0, 0);
			winProps.Size = new System.Drawing.Size(300, 300);
			winProps.ID = -1;
			_label = winProps.Title;			
			// _cstyle = ParseFrameStyle(winProps.Style);
			_cstyle = wx.Frame.wxDEFAULT_FRAME_STYLE;			
			_elem = new wx.Frame(null, winProps.ID, winProps.Title, winProps.Pos, winProps.Size, _cstyle);
			_elem.EVT_MOUSE_EVENTS(new wx.EventListener(OnMouseEvent));
			SetWidgetProps();
			this.Text = winProps.Title;
			return true;
		}
		protected override bool InsertWidgetInText()
		{
			return true;			
		}
		
		protected override bool DeleteWidgetFromEditor()
		{
			return true;
		}
		protected override bool DeleteWidgetFromText()
		{
			return true;
		}
		
		protected void OnMouseEvent(object sender, wx.Event evt)
        {
			// Manage mouse events when inside this widget
			// Mouse Left : show properties associates to this widget
			// Mouse Right : Popup menu
			// if (evt.EventType == wx.Event.wxEVT_LEAVE_WINDOW)
			// {
			// }
        }
				
		public void winProps_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.PropertyName + " has been changed.");            
        }
		
		private long ParseFrameStyle(FrameStyle curstyle)
		{
			long _cstyle = 0;
			if ((curstyle & FrameStyle.wxCAPTION) == FrameStyle.wxCAPTION) _cstyle |= wx.Frame.wxCAPTION;
			if ((curstyle & FrameStyle.wxCLOSE_BOX) == FrameStyle.wxCLOSE_BOX) _cstyle |= wx.Frame.wxCLOSE_BOX;
			if ((curstyle & FrameStyle.wxDEFAULT_FRAME_STYLE) == FrameStyle.wxDEFAULT_FRAME_STYLE) _cstyle |= wx.Frame.wxDEFAULT_FRAME_STYLE;
			if ((curstyle & FrameStyle.wxFRAME_FLOAT_ON_PARENT) == FrameStyle.wxFRAME_FLOAT_ON_PARENT) _cstyle |= wx.Frame.wxFRAME_FLOAT_ON_PARENT;
			if ((curstyle & FrameStyle.wxFRAME_NO_TASKBAR) == FrameStyle.wxFRAME_NO_TASKBAR) _cstyle |= wx.Frame.wxFRAME_NO_TASKBAR;
			if ((curstyle & FrameStyle.wxFRAME_SHAPED) == FrameStyle.wxFRAME_SHAPED) _cstyle |= wx.Frame.wxFRAME_SHAPED;
			if ((curstyle & FrameStyle.wxFRAME_TOOL_WINDOW) == FrameStyle.wxFRAME_TOOL_WINDOW) _cstyle |= wx.Frame.wxFRAME_TOOL_WINDOW;
			if ((curstyle & FrameStyle.wxICONIZE) == FrameStyle.wxICONIZE) _cstyle |= wx.Frame.wxICONIZE;
			if ((curstyle & FrameStyle.wxMAXIMIZE) == FrameStyle.wxMAXIMIZE) _cstyle |= wx.Frame.wxMAXIMIZE;
			if ((curstyle & FrameStyle.wxMAXIMIZE_BOX) == FrameStyle.wxMAXIMIZE_BOX) _cstyle |= wx.Frame.wxMAXIMIZE_BOX;
			if ((curstyle & FrameStyle.wxMINIMIZE) == FrameStyle.wxMINIMIZE) _cstyle |= wx.Frame.wxMINIMIZE;
			if ((curstyle & FrameStyle.wxMINIMIZE_BOX) == FrameStyle.wxMINIMIZE_BOX) _cstyle |= wx.Frame.wxMINIMIZE_BOX;
			if ((curstyle & FrameStyle.wxRESIZE_BORDER) == FrameStyle.wxRESIZE_BORDER) _cstyle |= wx.Frame.wxRESIZE_BORDER;			
			if ((curstyle & FrameStyle.wxSTAY_ON_TOP) == FrameStyle.wxSTAY_ON_TOP) _cstyle |= wx.Frame.wxSTAY_ON_TOP;			
			if ((curstyle & FrameStyle.wxSYSTEM_MENU) == FrameStyle.wxSYSTEM_MENU) _cstyle |= wx.Frame.wxSYSTEM_MENU;						
			return _cstyle;			
		}
		
		public void SetWidgetProps()
		{
			wdbFrameProps winProps = (wdbFrameProps)_props;
			winProps.PropertyChanged += new PropertyChangedEventHandler(winProps_PropertyChanged);
			Common.Instance().ObjPropsPanel.SelectedObject = winProps;
		}		
		
		
	}
}
