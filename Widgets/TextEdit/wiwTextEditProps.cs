/*
 * Creato da SharpDevelop.
 * Utente: michele
 * Data: 02/03/2009
 * Ora: 8.48
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Drawing.Design;



namespace mkdb.Widgets
{
	public class wdbTextEditProps : wxWindowProps
	{
		// * wxButton props : name, style, label, default, wxWindow, Align
		protected wxFlags _tstyle;
		protected wxFlags _lstyle;
		protected string _label;
		protected int _height;
		protected int _label_width;
		
		public wdbTextEditProps() : base()
		{
			_name = "TextEdit";
			_label = "TextEdit";
			_height = 20;
			_label_width = 100;
			_lstyle = new wxFlags();
			_lstyle.AddItem("wxALIGN_LEFT", wx.Alignment.wxALIGN_LEFT, true);
			_lstyle.AddItem("wxALIGN_RIGHT", wx.Alignment.wxALIGN_RIGHT, false);
			_lstyle.AddItem("wxALIGN_CENTER", wx.Alignment.wxALIGN_CENTRE, false);
			_lstyle.AddItem("wxST_NO_AUTORESIZE", wx.StaticText.wxST_NO_AUTORESIZE, false);
			_tstyle = new wxFlags();
			_tstyle.AddItem("wxTE_CENTER", wx.TextCtrl.wxTE_CENTER, false);
			_tstyle.AddItem("wxTE_LEFT", wx.TextCtrl.wxTE_LEFT, true);
			_tstyle.AddItem("wxTE_MULTILINE", wx.TextCtrl.wxTE_MULTILINE, false);
			_tstyle.AddItem("wxTE_NO_VSCROLL", wx.TextCtrl.wxTE_NO_VSCROLL, false);
			_tstyle.AddItem("wxTE_PROCESS_ENTER", wx.TextCtrl.wxTE_PROCESS_ENTER, false);
			_tstyle.AddItem("wxTE_PROCESS_TAB", wx.TextCtrl.wxTE_PROCESS_TAB, false);
			_tstyle.AddItem("wxTE_READONLY", wx.TextCtrl.wxTE_READONLY, false);
			_tstyle.AddItem("wxTE_RIGHT", wx.TextCtrl.wxTE_RIGHT, false);
		}
		
		[CategoryAttribute("TextEdit"), DescriptionAttribute("TextEdit Props")]
		public string Label
		{
			get	{	return _label;	}
			set	{	_label = value;	NotifyPropertyChanged("Label");	}
		}
		
		[TypeConverter(typeof(wxFlagsTypeConverter))]
		[Editor(typeof(wxFlagsEditor), typeof(UITypeEditor))]
		[CategoryAttribute("TextEdit"), DescriptionAttribute("TextEdit Props")]
		public wxFlags LabelStyle
		{
			get	{	return _lstyle;	}
			set	{	_lstyle = value;	NotifyPropertyChanged("LabelStyle");	}
		}
		
		[TypeConverter(typeof(wxFlagsTypeConverter))]
		[Editor(typeof(wxFlagsEditor), typeof(UITypeEditor))]
		[CategoryAttribute("TextEdit"), DescriptionAttribute("TextEdit Props")]
		public wxFlags TextEditStyle
		{
			get	{	return _tstyle;	}
			set	{	_tstyle = value;	NotifyPropertyChanged("TextEditStyle");	}
		}
		
		[CategoryAttribute("TextEdit"), DescriptionAttribute("TextEdit Props")]
		public int Height
		{
			get	{	return _height;	}
			set	{	_height = value;	NotifyPropertyChanged("Height");	}
		}
				
		[CategoryAttribute("TextEdit"), DescriptionAttribute("TextEdit Props")]
		public int LabelWidth
		{
			get	{	return _label_width;	}
			set	{	_label_width = value;	NotifyPropertyChanged("LabelWidth");	}
		}		
	}
}
