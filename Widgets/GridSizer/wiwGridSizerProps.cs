/*
 * Creato da SharpDevelop.
 * Utente: michele
 * Data: 02/03/2009
 * Ora: 8.45
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;


namespace mkdb.Widgets
{
	/* wxGridSizer : name, rows, cols, vgap, hgap, min_size, Align */
	/* wxFlexGridSizer : name, rows, cols, vgap, hgap, Align */	
	public class wdbGridSizerProps : wxAlignProps
	{
		protected int _rows;
		protected int _cols;
		protected int _vgap;
		protected int _hgap;
		
		public wdbGridSizerProps() : base()
		{
			_rows = 2;
			_cols = 2;
			_vgap = 0;
			_hgap = 0;
			this.EnableNotification = false;
			this.BorderWidth = 0;
			this.EnableNotification = true;
		}				
		[CategoryAttribute("Grid Sizer"), DescriptionAttribute("Grid Sizer")]
		public int Cols
		{
			get	{	return _cols;	}
			set	{	_cols = value;	NotifyPropertyChanged("Cols");	}
		}				
		[CategoryAttribute("Grid Sizer"), DescriptionAttribute("Grid Sizer")]
		public int Rows
		{
			get	{	return _rows;	}
			set	{	_rows = value;	NotifyPropertyChanged("Rows");	}
		}		
		[CategoryAttribute("Grid Sizer"), DescriptionAttribute("Grid Sizer")]
		public int VGap
		{
			get	{	return _vgap;	}
			set	{	_vgap = value;	NotifyPropertyChanged("VGap");	}
		}		
		[CategoryAttribute("Grid Sizer"), DescriptionAttribute("Grid Sizer")]
		public int HGap
		{
			get	{	return _hgap;	}
			set	{	_hgap = value;	NotifyPropertyChanged("HGap");	}
		}						
	}
}
