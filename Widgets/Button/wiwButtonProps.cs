/*
 * Creato da SharpDevelop.
 * Utente: michele
 * Data: 02/03/2009
 * Ora: 8.43
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Drawing.Design;



namespace mkdb.Widgets
{
	public class wdbButtonProps : wxWindowProps
	{
		// * wxButton props : name, style, label, default, wxWindow, Align
		protected wxFlags _bstyle;
		protected string _label;
		
		public wdbButtonProps() : base()
		{
			_name = "Button";
			_label = "Button";
			_bstyle = new wxFlags();
			_bstyle.AddItem("wxBU_LEFT", wx.Button.wxBU_LEFT, false);
			_bstyle.AddItem("wxBU_RIGHT", wx.Button.wxBU_RIGHT, false);
			_bstyle.AddItem("wxBU_TOP", wx.Button.wxBU_TOP, false);
			_bstyle.AddItem("wxBU_BOTTOM", wx.Button.wxBU_BOTTOM, false);
			_bstyle.AddItem("wxBU_EXACTFIT", wx.Button.wxBU_EXACTFIT, true);
			_bstyle.AddItem("wxNO_BORDER", wx.Button.wxNO_BORDER, false);						
		}
		
		[CategoryAttribute("Button"), DescriptionAttribute("Button Props")]
		public string Label
		{
			get	{	return _label;	}
			set	{	_label = value;	NotifyPropertyChanged("Label");	}
		}
		
		[TypeConverter(typeof(wxFlagsTypeConverter))]
		[Editor(typeof(wxFlagsEditor), typeof(UITypeEditor))]
		[CategoryAttribute("Button"), DescriptionAttribute("Button Props")]
		public wxFlags ButtonStyle
		{
			get	{	return _bstyle;	}
			set	{	_bstyle = value;	NotifyPropertyChanged("ButtonStyle");	}
		}
		
	}
}
