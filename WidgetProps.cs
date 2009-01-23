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
	[Flags]
	public enum BorderFlag
	{
		wxLEFT 						= 0x0001,
		wxRIGHT 					= 0x0002,
    	wxTOP 						= 0x0004,
    	wxBOTTOM 					= 0x0008,
    	wxALL 						= 0x0010,
	}
	
	[Flags]
	public enum AlignFlag
	{
	    wxALIGN_LEFT				= 0x0020,
    	wxALIGN_TOP               	= 0x0040,
    	wxALIGN_RIGHT             	= 0x0080,
    	wxALIGN_BOTTOM            	= 0x0100,
    	wxALIGN_CENTER_HORIZONTAL 	= 0x0200,
    	wxALIGN_CENTER_VERTICAL   	= 0x0400,
    	wxALIGN_CENTER            	= 0x0800,
    	wxEXPAND                  	= 0x1000,
    	wxSHAPED                  	= 0x2000,
    	wxFIXED_MINSIZE           	= 0x4000
   	}
	
	[Flags]
	public enum WindowStyle
	{
		wxALWAYS_SHOW_SB 			= 0x0001,
		wxCLIP_CHILDREN				= 0x0002,
		wxFULL_REPAINT_ON_RESIZE	= 0x0004,
		wxNO_BORDER 				= 0x0008,
    	wxRAISED_BORDER 			= 0x0010,
    	wxSIMPLE_BORDER 			= 0x0020,
    	wxSTATIC_BORDER 			= 0x0040,
    	wxSUNKEN_BORDER 			= 0x0080,
    	wxDOUBLE_BORDER 			= 0x0100,
    	wxHSCROLL 					= 0x0200,
    	wxVSCROLL 					= 0x0400,
    	wxTAB_TRAVERSAL 			= 0x0800,
    	wxTRANSPARENT_WINDOW 		= 0x1000,
    	wxWANTS_CHARS 				= 0x2000
	}
	
	/// <summary>
	/// Abstract class for properties (WidgetProps).
	/// </summary>
	public abstract class WidgetProps
	{
		public WidgetProps()
		{
		}
	}
	
	/// <summary>
	/// Alignment and borders (Ref. to Parent).
	/// </summary>
	public class wxABProps : WidgetProps
	{
		private int _border;
		private AlignFlag _aflag;
		private BorderFlag _bflag;
		
		public wxABProps()
		{
			_border = 1;
			_bflag = BorderFlag.wxALL;
			_aflag = AlignFlag.wxALIGN_LEFT|AlignFlag.wxALIGN_TOP;
		}
		
		[CategoryAttribute("Alignment & Border"), DescriptionAttribute("Alignment and Border flags")]
		public int BorderWidth
		{
			get	{	return _border;		}
			set	{	_border = value;	}
		}
				
		[CategoryAttribute("Alignment & Border"), DescriptionAttribute("Alignment and Border flags")]
        [Editor(typeof(FlagsEditor), typeof(UITypeEditor))]
		public BorderFlag Border
		{
			get	{	return _bflag;		}
			set	{	_bflag = value;		}
		}		
		
		[CategoryAttribute("Alignment & Border"), DescriptionAttribute("Alignment and Border flags")]
        [Editor(typeof(FlagsEditor), typeof(UITypeEditor))]
		public AlignFlag Alignment
		{
			get	{	return _aflag;		}
			set	{	_aflag = value;		}
		}		
	}
	
	public class wxWindowProps : wxABProps
	{
		protected int _id;
		protected string _wname;
		protected Point _pos;
		protected Size _size;
		protected Font _font;
		protected Color _fc;
		protected Color _bc;
		protected bool _enabled;
		protected bool _hidden;
		protected WindowStyle _wstyle;
		
// * wxWindow props : id, pos, size, font, fc, bc, window_name, window_style, 
// 					tooltip, enabled, hidden.		
		public wxWindowProps()
		{					
		}
		
		[CategoryAttribute("wxWindows"), DescriptionAttribute("wxWindows properties")]
		public int ID		
		{
			get	{	return _id;	}
			set	{	_id = value; }
		}		
		[CategoryAttribute("wxWindows"), DescriptionAttribute("wxWindows properties")]
		public string WindowName
		{
			get	{	return _wname;	}
			set	{	_wname = value; }
		}
		[CategoryAttribute("wxWindows"), DescriptionAttribute("wxWindows properties")]
		public Color FC
		{
			get	{	return _fc;	}
			set	{	_fc = value;	}
		}
		[CategoryAttribute("wxWindows"), DescriptionAttribute("wxWindows properties")]
		public Color BC
		{
			get	{	return _bc;	}
			set	{	_bc = value;	}
		}		
		[CategoryAttribute("wxWindows"), DescriptionAttribute("wxWindows properties")]
		public Font Font
		{
			get	{	return _font;	}
			set	{	_font = value;	}
		}
		[CategoryAttribute("wxWindows"), DescriptionAttribute("wxWindows properties")]
		public bool Enabled
		{
			get	{	return _enabled;	}
			set	{	_enabled = value;	}
		}
		[CategoryAttribute("wxWindows"), DescriptionAttribute("wxWindows properties")]
		public Point Pos
		{
			get	{	return _pos;	}
			set	{	_pos = value;	}
		}
		[CategoryAttribute("wxWindows"), DescriptionAttribute("wxWindows properties")]
		public Size Size
		{
			get	{	return _size;	}
			set	{	_size = value;	}
		}
		[CategoryAttribute("wxWindows"), DescriptionAttribute("wxWindows properties")]
		public bool Hidden
		{
			get	{	return _hidden;	}
			set	{	_hidden = value;	}
		}
		
		[CategoryAttribute("wxWindows"), DescriptionAttribute("wxWindows properties")]
        [Editor(typeof(FlagsEditor), typeof(UITypeEditor))]
		public WindowStyle WindowStyle
		{
			get	{	return _wstyle;	}
			set	{	_wstyle = value;	}
		}
	}
	
	
	
	// Cast C# Enum to right wxWidget Flags
	public class CastWidgetFlags
	{
		public CastWidgetFlags()
		{}
		
		public static long ConvWxDirectionFlag(BorderFlag flag)
		{
			long res = 0;
			// if( (item.value & value)== item.value && item.value!=0)
			if ((flag & BorderFlag.wxALL) == BorderFlag.wxALL) res |= wx.Direction.wxALL;
			if ((flag & BorderFlag.wxBOTTOM) == BorderFlag.wxBOTTOM) res |= wx.Direction.wxBOTTOM;
			if ((flag & BorderFlag.wxTOP) == BorderFlag.wxTOP) res |= wx.Direction.wxTOP;
			if ((flag & BorderFlag.wxLEFT) == BorderFlag.wxLEFT) res |= wx.Direction.wxLEFT;
			if ((flag & BorderFlag.wxRIGHT) == BorderFlag.wxRIGHT) res |= wx.Direction.wxRIGHT;
			return res;
		}
		
		public static long ConvWxAlignmentFlag(AlignFlag flag)
		{
			long res = 0;
			if ((flag & AlignFlag.wxALIGN_CENTER) == AlignFlag.wxALIGN_CENTER) res |= wx.Alignment.wxALIGN_CENTER;
			if ((flag & AlignFlag.wxALIGN_CENTER_HORIZONTAL) == AlignFlag.wxALIGN_CENTER_HORIZONTAL) res |= wx.Alignment.wxALIGN_CENTER_HORIZONTAL;
			if ((flag & AlignFlag.wxALIGN_CENTER_VERTICAL) == AlignFlag.wxALIGN_CENTER_VERTICAL) res |= wx.Alignment.wxALIGN_CENTER_VERTICAL;
			if ((flag & AlignFlag.wxALIGN_BOTTOM) == AlignFlag.wxALIGN_BOTTOM) res |= wx.Alignment.wxALIGN_BOTTOM;
			if ((flag & AlignFlag.wxALIGN_LEFT) == AlignFlag.wxALIGN_LEFT) res |= wx.Alignment.wxALIGN_LEFT;
			if ((flag & AlignFlag.wxALIGN_RIGHT) == AlignFlag.wxALIGN_RIGHT) res |= wx.Alignment.wxALIGN_RIGHT;
			if ((flag & AlignFlag.wxALIGN_TOP) == AlignFlag.wxALIGN_TOP) res |= wx.Alignment.wxALIGN_TOP;
			return res;
		}
		
		public static long ConvWxStretchFlag(AlignFlag flag)
		{
			long res = 0;
			if ((flag & AlignFlag.wxEXPAND) == AlignFlag.wxEXPAND) res |= wx.Stretch.wxEXPAND;
			if ((flag & AlignFlag.wxSHAPED) == AlignFlag.wxSHAPED) res |= wx.Stretch.wxSHAPED;
			if ((flag & AlignFlag.wxFIXED_MINSIZE) == AlignFlag.wxFIXED_MINSIZE) res |= wx.Stretch.wxFIXED_MINSIZE;
			return res;
		}		
	}
	
	
	/*
	public class CastWidgetFlags
	{
		public CastWidgetFlags()
		{}
		
		public static long ConvWxDirectionFlag(BorderFlag flag)
		{
			switch (flag )
			{				
				case BorderFlag.wxALL : return wx.Direction.wxALL;
				case BorderFlag.wxBOTTOM : return wx.Direction.wxBOTTOM;
				case BorderFlag.wxTOP : return wx.Direction.wxTOP;
				case BorderFlag.wxLEFT : return wx.Direction.wxLEFT;
				case BorderFlag.wxRIGHT : return wx.Direction.wxRIGHT;
			}
			return wx.Direction.wxTOP;
		}
		
		public static long ConvWxAlignmentFlag(AlignFlag flag)
		{
			switch (flag)
			{				
				case AlignFlag.wxALIGN_CENTER : return wx.Alignment.wxALIGN_CENTER;
				case AlignFlag.wxALIGN_CENTER_HORIZONTAL : return wx.Alignment.wxALIGN_CENTER_HORIZONTAL;
				case AlignFlag.wxALIGN_CENTER_VERTICAL : return wx.Alignment.wxALIGN_CENTER_VERTICAL;	
				case AlignFlag.wxALIGN_BOTTOM : return wx.Alignment.wxALIGN_BOTTOM;
				case AlignFlag.wxALIGN_LEFT : return wx.Alignment.wxALIGN_LEFT;
				case AlignFlag.wxALIGN_RIGHT : return wx.Alignment.wxALIGN_RIGHT;
				case AlignFlag.wxALIGN_TOP : return wx.Alignment.wxALIGN_TOP;
			}
			return wx.Alignment.wxALIGN_MASK;
		}
		
		public static long ConvWxStretchFlag(AlignFlag flag)
		{
			switch (flag )
			{				
				case AlignFlag.wxEXPAND : return wx.Stretch.wxEXPAND;
				case AlignFlag.wxSHAPED : return wx.Stretch.wxSHAPED;
				case AlignFlag.wxFIXED_MINSIZE : return wx.Stretch.wxFIXED_MINSIZE;
			}
			return wx.Stretch.wxSHAPED;
		}		
	}
	*/
}
