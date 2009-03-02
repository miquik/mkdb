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
using System.Collections.Specialized;
using System.Drawing.Design;



namespace mkdb.Widgets
{

	public class wdbComboBoxProps : wxWindowProps
	{
		// * wxButton props : name, style, label, default, wxWindow, Align
		protected int _select;
		protected StringCollection _items;
		
		public wdbComboBoxProps() : base()
		{
			_name = "ComboBox";
			_select = 0;
			_items = new StringCollection();
			_items.Add("ComboBox");
		}
		
		[CategoryAttribute("ComboBox"), DescriptionAttribute("ComboBox Props")]
		public int Select
		{
			get	{	return _select;	}
			set	{	_select = value;	NotifyPropertyChanged("Select");	}
		}
		
		[Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, " +
		        "Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[CategoryAttribute("ComboBox"), DescriptionAttribute("ComboBox Props")]
		public StringCollection Items
		{
			get	{	return _items;	}
			set	{	_items = value;	NotifyPropertyChanged("Items");	}
		}		
	}
}
