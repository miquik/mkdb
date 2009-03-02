/*
 * Creato da SharpDevelop.
 * Utente: michele
 * Data: 02/03/2009
 * Ora: 8.44
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Drawing.Design;



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
			_name = "Frame";
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
}
