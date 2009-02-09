/*
 * Creato da SharpDevelop.
 * Utente: Family Rose
 * Data: 26/12/2008
 * Ora: 12.12
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using wx;
using System.Windows.Forms;

namespace mkdb
{
	public enum StandardWidgetID
	{
		WID_UNKNOWN = 0,
		WID_FRAME = 1,
		WID_BOXSIZER = 2,
	}
	
	/// <summary>
	/// Description of WidgetElem.
	/// </summary>
	public abstract class WidgetElem : TreeNode
	{
		protected wx.Window _elem;
		protected wx.Sizer _sizer;
		protected WidgetProps _props;
		protected bool _is_sizer;
		protected int _widget_id;
		
		public WidgetElem(int _id)
		{
			_widget_id = _id;
			_elem = null;
			_sizer = null;
			_props = null;
			_is_sizer = false;
		}
		
		public abstract void PaintOnSelection();
		
		public abstract bool InsertWidget(WidgetElem parent);
		public abstract bool DeleteWidget();		
		public abstract long FindBlockInText();		
		public abstract bool CanAcceptChildren();
		
		protected abstract bool InsertWidgetInText();	
		protected abstract bool DeleteWidgetFromText();		
		
		public WidgetProps Properties
		{
			get	{	return _props;	}
		}		
		public wx.Window Element
		{
			get	{	return _elem;	}
		}
		public wx.Sizer Sizer
		{
			get	{	return _sizer;	}
		}
		public bool IsSizer
		{
			get	{	return _is_sizer;	}
			set	{	_is_sizer = value;	}
		}
		public int WidgetID
		{
			get	{	return _widget_id;	}
		}
	}
}
