/*
 * Creato da SharpDevelop.
 * Utente: michele
 * Data: 02/03/2009
 * Ora: 8.42
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Drawing.Design;


namespace mkdb.Widgets
{
	
	/* wxBoxSizer : name, orient, min_size, Align */	
	public class wdbBoxSizerProps : wxAlignProps
	{
		protected wxFlags _orient;
		protected int _min_size;
		
		public wdbBoxSizerProps() : base()
		{
			_orient = new wxFlags();
			_orient.AddItem("wxVERTICAL", wx.Orientation.wxVERTICAL, true);
			_orient.AddItem("wxHORIZONTAL", wx.Orientation.wxHORIZONTAL, false);
			_min_size = 100;
			this.EnableNotification = false;
			this.BorderWidth = 0;
			this.EnableNotification = true;
		}
				
		[TypeConverter(typeof(wxFlagsTypeConverter))]
        [Editor(typeof(wxFlagsEditor), typeof(UITypeEditor))]
		[CategoryAttribute("Box Sizer"), DescriptionAttribute("Box Sizer")]
		public wxFlags Orientation
		{
			get	{	return _orient;	}
			set	{	_orient = value;	NotifyPropertyChanged("Orientation");	}
		}		
		
		[CategoryAttribute("Box Sizer"), DescriptionAttribute("Box Sizer")]
		public int MinSize
		{
			get	{	return _min_size;	}
			set	{	_min_size = value; NotifyPropertyChanged("MinSize");	}
		}
	}
}
