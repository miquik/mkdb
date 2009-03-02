/*
 * Creato da SharpDevelop.
 * Utente: Family Rose
 * Data: 26/12/2008
 * Ora: 12.01
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;


namespace mkdb
{
	
	/// <summary>
	/// Abstract class for properties (WidgetProps).
	/// </summary>
	public abstract class WidgetProps : INotifyPropertyChanged
	{
		protected bool _enable_not;
		protected string _name;
		
		[Browsable(false)]
		public bool EnableNotification
		{
			get	{	return	_enable_not;	}
			set	{	_enable_not = value;	}
		}				
		
		[CategoryAttribute("Element Name"), DescriptionAttribute("The Name of this element")]
		public string Name
		{
			get	{	return	_name;	}
			set	{	_name = value;	NotifyPropertyChanged("Name"); }
		}						
			
		public WidgetProps()
		{
			_enable_not = true;
		}
		
		public void NotifyPropertyChanged(String propertyName)
        {
			if (_enable_not == false)
				return;
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion		
	}
	 	
	/// <summary>
	/// Alignment and borders (Ref. to Parent).
	/// </summary>
	public class wxAlignProps : WidgetProps
	{
		private int _proportion;
		private int _border;
		private wxFlags _aflag;
		private wxFlags _bflag;
		
		public wxAlignProps()
		{
			_proportion = 0;
			_border = 5;
			_bflag = new wxFlags();
			_bflag.AddItem("wxLEFT", wx.Direction.wxLEFT, false);
			_bflag.AddItem("wxRIGHT", wx.Direction.wxRIGHT, false);
			_bflag.AddItem("wxTOP", wx.Direction.wxTOP, false);
			_bflag.AddItem("wxBOTTOM", wx.Direction.wxBOTTOM, false);
			_bflag.AddItem("wxALL", wx.Direction.wxALL, true);			
			_aflag = new wxFlags();
			_aflag.AddItem("wxALIGN_LEFT", wx.Alignment.wxALIGN_LEFT, false);
			_aflag.AddItem("wxALIGN_RIGHT", wx.Alignment.wxALIGN_RIGHT, false);
			_aflag.AddItem("wxALIGN_TOP", wx.Alignment.wxALIGN_TOP, false);
			_aflag.AddItem("wxALIGN_BOTTOM", wx.Alignment.wxALIGN_BOTTOM, false);
			_aflag.AddItem("wxALIGN_CENTER", wx.Alignment.wxALIGN_CENTER, false);
			_aflag.AddItem("wxALIGN_CENTER_HORIZONTAL", wx.Alignment.wxALIGN_CENTER_HORIZONTAL, false);
			_aflag.AddItem("wxALIGN_CENTER_VERTICAL", wx.Alignment.wxALIGN_CENTER_VERTICAL, false);
			_aflag.AddItem("wxEXPAND", wx.Stretch.wxGROW, true);
			_aflag.AddItem("wxSHAPED", wx.Stretch.wxSHAPED, false);
			_aflag.AddItem("wxFIXED_MINSIZE", wx.Stretch.wxFIXED_MINSIZE, false);
		}
		
		[CategoryAttribute("Alignment & Border"), DescriptionAttribute("Alignment and Border flags")]
		public int Proportion
		{
			get	{	return _proportion;		}
			set	{	_proportion = value; NotifyPropertyChanged("Proportion");}
		}		
		
		[CategoryAttribute("Alignment & Border"), DescriptionAttribute("Alignment and Border flags")]
		public int BorderWidth
		{
			get	{	return _border;		}
			set	{	_border = value; NotifyPropertyChanged("BorderWidth");}
		}
				
		[CategoryAttribute("Alignment & Border"), DescriptionAttribute("Alignment and Border flags")]
		[TypeConverter(typeof(wxFlagsTypeConverter))]
        [Editor(typeof(wxFlagsEditor), typeof(UITypeEditor))]
		public wxFlags Border
		{
			get	{	return _bflag;		}
			set	{	_bflag = value;	NotifyPropertyChanged("Border");	}
		}		
		
		[CategoryAttribute("Alignment & Border"), DescriptionAttribute("Alignment and Border flags")]
		[TypeConverter(typeof(wxFlagsTypeConverter))]
        [Editor(typeof(wxFlagsEditor), typeof(UITypeEditor))]
		public wxFlags Alignment
		{
			get	{	return _aflag;		}
			set	{	_aflag = value;	NotifyPropertyChanged("Alignment");	}
		}		
	}
	
	
	public class wxWindowProps : wxAlignProps
	{
		protected int _id;
		protected Point _pos;
		protected Size _size;
		protected wxFont _font;
		protected wxColor _fc;
		protected wxColor _bc;
		protected bool _enabled;
		protected bool _hidden;
		protected wxFlags _wstyle;
		
		public wxWindowProps()
		{
			_id = -1;
			_name = "";
			_fc = new wxColor(0, 0, 0);	
			_bc = new wxColor(100, 100, 100);	
			_font = new wxFont("Arial", 8);
			_wstyle = new wxFlags();
			_wstyle.AddItem("wxCLIP_CHILDREN", wx.Window.wxCLIP_CHILDREN, false);
			_wstyle.AddItem("wxNO_BORDER", wx.Window.wxNO_BORDER, false);			
			_wstyle.AddItem("wxRAISED_BORDER", wx.Window.wxRAISED_BORDER, false);			
			_wstyle.AddItem("wxSIMPLE_BORDER", wx.Window.wxSIMPLE_BORDER, false);			
			_wstyle.AddItem("wxSTATIC_BORDER", wx.Window.wxSTATIC_BORDER, false);			
			_wstyle.AddItem("wxSUNKEN_BORDER", wx.Window.wxSUNKEN_BORDER, false);			
			_wstyle.AddItem("wxDOUBLE_BORDER", wx.Window.wxDOUBLE_BORDER, false);						
			_wstyle.AddItem("wxHSCROLL", wx.Window.wxHSCROLL, false);						
			_wstyle.AddItem("wxVSCROLL", wx.Window.wxVSCROLL, false);						
			_wstyle.AddItem("wxTAB_TRAVERSAL", wx.Window.wxTAB_TRAVERSAL, true);						
			_wstyle.AddItem("wxWANTS_CHARS", wx.Window.wxWANTS_CHARS, false);						
		}
		
		[CategoryAttribute("wxWindows"), DescriptionAttribute("wxWindows properties")]
		public int ID		
		{
			get	{	return _id;	}
			set	{	_id = value; NotifyPropertyChanged("ID"); }
		}		
		[CategoryAttribute("wxWindows"), DescriptionAttribute("wxWindows properties")]
		[Editor(typeof(wxColorEditor), typeof(UITypeEditor))]
		public wxColor FC
		{
			get	{	return _fc;	}
			set	{	_fc = value; NotifyPropertyChanged("FC");	}
		}
		[CategoryAttribute("wxWindows"), DescriptionAttribute("wxWindows properties")]
		[Editor(typeof(wxColorEditor), typeof(UITypeEditor))]
		public wxColor BC
		{
			get	{	return _bc;	}
			set	{	_bc = value; NotifyPropertyChanged("BC");	}
		}		
		[CategoryAttribute("wxWindows"), DescriptionAttribute("wxWindows properties")]
		[Editor(typeof(wxFontEditor), typeof(UITypeEditor))]
		public wxFont Font
		{
			get	{	return _font;	}
			set	{	_font = value; NotifyPropertyChanged("Font");	}
		}
		[CategoryAttribute("wxWindows"), DescriptionAttribute("wxWindows properties")]
		public Point Pos
		{
			get	{	return _pos;	}
			set	{	_pos = value; NotifyPropertyChanged("Pos");	}
		}
		[CategoryAttribute("wxWindows"), DescriptionAttribute("wxWindows properties")]
		public Size Size
		{
			get	{	return _size;	}
			set	{	_size = value; NotifyPropertyChanged("Size");	}
		}
		[CategoryAttribute("wxWindows"), DescriptionAttribute("wxWindows properties")]
		public bool Enabled
		{
			get	{	return _enabled;	}
			set	{	_enabled = value; NotifyPropertyChanged("Enabled");	}
		}		
		[CategoryAttribute("wxWindows"), DescriptionAttribute("wxWindows properties")]
		public bool Hidden
		{
			get	{	return _hidden;	}
			set	{	_hidden = value; NotifyPropertyChanged("Hidden");	}
		}
		[CategoryAttribute("wxWindows"), DescriptionAttribute("wxWindows properties")]
		[TypeConverter(typeof(wxFlagsTypeConverter))]
		[Editor(typeof(wxFlagsEditor), typeof(UITypeEditor))]
		public wxFlags WindowStyle
		{
			get	{	return _wstyle;	}
			set	{	_wstyle = value; NotifyPropertyChanged("WindowStyle");	}
		}		
	}
	
}
