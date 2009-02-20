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
using System.Collections;

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
	
	public interface IWXSizer
	{
		ArrayList 	List	{	get;	}
		void 		AddWDBBase(IWDBBase elem, int prop, int flag, int border);
		void 		InsertWDBBase(int index, IWDBBase elem, int prop, int flag, int border);
		void 		RemoveWDBBase(IWDBBase elem);		
	}
	
	public interface IWDBBase
	{
		wx.Window   Me				{	get;	}
		wx.Window 	ParentContainer	{	get;	}
		wx.Sizer	ParentSizer		{	get;	}
		bool		IsSizer			{	get;	}
		bool		IsSelected		{	get; set;	}
		WidgetProps	Properties		{	get;	}		
		int			WidgetType		{	get;	}
		
		void 		PaintOnSelection();		
		bool 		InsertWidget(IWDBBase parent);
		bool 		DeleteWidget();		
		long 		FindBlockInText();		
		bool 		CanAcceptChildren();				
	}
	
	
	public abstract class WidgetElem : TreeNode
	{
		protected IWDBBase _elem;
		
		public WidgetElem(string name) : base(name)
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
