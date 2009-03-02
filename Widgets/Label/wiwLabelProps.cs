/*
 * Creato da SharpDevelop.
 * Utente: michele
 * Data: 02/03/2009
 * Ora: 8.46
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Drawing.Design;



namespace mkdb.Widgets
{

	public class wdbLabelProps : wxWindowProps
	{
		// * wxButton props : name, style, label, default, wxWindow, Align
		protected wxFlags _lstyle;
		protected string _text;
		
		public wdbLabelProps() : base()
		{
			_name = "Label";
			_text = "Static Text";
			_lstyle = new wxFlags();
			_lstyle.AddItem("wxALIGN_LEFT", wx.Alignment.wxALIGN_LEFT, false);
			_lstyle.AddItem("wxALIGN_RIGHT", wx.Alignment.wxALIGN_RIGHT, false);
			_lstyle.AddItem("wxALIGN_CENTER", wx.Alignment.wxALIGN_CENTRE, true);
			_lstyle.AddItem("wxST_NO_AUTORESIZE", wx.StaticText.wxST_NO_AUTORESIZE, false);
		}
		
		[CategoryAttribute("Label"), DescriptionAttribute("Label Props")]
		public string Text
		{
			get	{	return _text;	}
			set	{	_text = value;	NotifyPropertyChanged("Text");	}
		}
		
		[TypeConverter(typeof(wxFlagsTypeConverter))]
		[Editor(typeof(wxFlagsEditor), typeof(UITypeEditor))]
		[CategoryAttribute("Label"), DescriptionAttribute("Label Props")]
		public wxFlags TextStyle
		{
			get	{	return _lstyle;	}
			set	{	_lstyle = value;	NotifyPropertyChanged("TextStyle");	}
		}
		
	}

}
