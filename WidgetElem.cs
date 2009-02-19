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
using System.Drawing;

namespace mkdb
{
	public enum StandardWidgetType
	{
		WID_UNKNOWN = 0,
		WID_FRAME = 1,
		WID_BOXSIZER = 2,
		WID_APP = 3,
		WID_BUTTON = 4,
	}
	
	public interface IWDBBase
	{
		wx.Window 	ParentContainer	{	get;	}
		wx.Sizer	ParentSizer		{	get;	}
		bool		IsSizer			{	get;	}
		bool		IsSelected		{	get; set;	}
		WidgetProps	Properties		{	get;	}		
		int			WidgetType		{	get;	}
		
		Point		AreaOrigin		{	get;	}
		Size		AreaSize		{	get;	}
		
		void 		PaintOnSelection();		
		bool 		InsertWidget(IWDBBase parent);
		bool 		DeleteWidget();		
		long 		FindBlockInText();		
		bool 		CanAcceptChildren();				
	}
	
	/*
	public interface IWDBBase
	{
		wx.Window	WxWindow	{	get; 	}
		wx.Sizer	WxSizer		{	get;	}
		bool		IsSizer		{	get;	}
		bool		IsSelected	{	get; set;	}
		WidgetProps	Properties	{	get;	}		
		int			WidgetID	{	get;	}
		
		// Drawing props
		IWDBBase	ClientParent	{	get;	}
		Point		AreaOrigin	{	get;	}
		Size		AreaSize		{	get;	}
		
		void 		PaintOnSelection();		
		bool 		InsertWidget(IWDBBase parent);
		bool 		DeleteWidget();		
		long 		FindBlockInText();		
		bool 		CanAcceptChildren();		
	}
	*/
	
	public abstract class WidgetElem : TreeNode
	{
		protected IWDBBase _elem;
		
		public WidgetElem(wx.Window _pc, wx.Sizer _ps, string name) : base(name)
		{
		}
		
		public IWDBBase WDBBase
		{
			get	{	return _elem;	}
			set	{	_elem = value;	}
		}
		
		// public abstract IWDBBase CreateWidget(IWDBBase parent);
	}
}
