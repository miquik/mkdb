/*
 * Creato da SharpDevelop.
 * Utente: michele
 * Data: 02/03/2009
 * Ora: 8.47
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Drawing.Design;



namespace mkdb.Widgets
{
	public class wdbListBoxProps : wxWindowProps
	{
		protected StringCollection _items;
		protected wxFlags _lbstyle;
		
		public wdbListBoxProps() : base()
		{
			_name = "ComboBox";
			_items = new StringCollection();
			_lbstyle = new wxFlags();
			_lbstyle.AddItem("wxLB_ALWAYS_SB", wx.ListBox.wxLB_ALWAYS_SB, false);
			_lbstyle.AddItem("wxLB_EXTENDED", wx.ListBox.wxLB_EXTENDED, false);
			_lbstyle.AddItem("wxLB_HSCROLL", wx.ListBox.wxLB_HSCROLL, false);
			_lbstyle.AddItem("wxLB_MULTIPLE", wx.ListBox.wxLB_MULTIPLE, false);
			_lbstyle.AddItem("wxLB_NEED_SB", wx.ListBox.wxLB_NEED_SB, false);
			_lbstyle.AddItem("wxLB_SINGLE", wx.ListBox.wxLB_SINGLE, false);
			_lbstyle.AddItem("wxLB_SORT", wx.ListBox.wxLB_SORT, false);
		}
		
		[Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, " +
		        "Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]		
		[CategoryAttribute("Listbox"), DescriptionAttribute("Listbox Props")]
		public StringCollection Items
		{
			get	{	return _items;	}
			set	{	_items = value;	NotifyPropertyChanged("Items");	}
		}		
		
		[TypeConverter(typeof(wxFlagsTypeConverter))]
		[Editor(typeof(wxFlagsEditor), typeof(UITypeEditor))]
		[CategoryAttribute("Listbox"), DescriptionAttribute("Listbox Props")]
		public wxFlags ListboxStyle
		{
			get	{	return _lbstyle;	}
			set	{	_lbstyle = value;	NotifyPropertyChanged("ListboxStyle");	}
		}		
	}
}
