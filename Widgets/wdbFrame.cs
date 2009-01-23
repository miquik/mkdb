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
				
		public override bool InsertWidget(Panel _canvas)
		{
			InsertWidgetInEditor(_canvas);
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
		
		protected override bool InsertWidgetInEditor(Panel _canvas)
		{
			IntPtr wxh;
			Common cm = Common.Instance();
			
			wdbFrameProps winProps = (wdbFrameProps)_props;
			_frame_cur_index++;
			winProps.Name = "Frame" + _frame_cur_index.ToString();
			winProps.WindowName = "wxFrameClass";
			winProps.Title = "Frame" + _frame_cur_index.ToString();
			winProps.Pos = new System.Drawing.Point(0, 0);
			winProps.Size = new System.Drawing.Size(300, 300);
			winProps.ID = -1;
			_label = winProps.Title;
			cm.ObjPropsPanel.SelectedObject = winProps;
			
			// TODO : Style
			_elem = new wx.Frame(null, winProps.ID, winProps.Title, winProps.Pos, winProps.Size, wx.Frame.wxDEFAULT_FRAME_STYLE);
			wxh = Win32Utils.FindWindow("wxWindowClassNR", winProps.Title);
			Win32Utils.SetParent(wxh, _canvas.Handle);
			_elem.EVT_MOUSE_EVENTS(new wx.EventListener(OnMouseEvent));
			_elem.EVT_MOUSE_EVENTS(new wx.EventListener(OnMouseEvent));
			cm.ChangeCurrentWindow(_elem);
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
			if (evt.EventType == wx.Event.wxEVT_LEAVE_WINDOW)
			{
				// _elem.AcceleratorTable
			}
        }
	}
}
