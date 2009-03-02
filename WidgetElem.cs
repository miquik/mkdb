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
	
	public interface IWDBBase
	{
		wx.SizerItem	SizerItem		{	get;	}
		wx.Window 		ParentContainer	{	get;	}
		wx.Sizer		ParentSizer		{	get;	}
		bool			IsSizer			{	get;	}
		bool			IsSelected		{	get; set;	}
		WidgetProps		Properties		{	get;	}		
		int				WidgetType		{	get;	}
		
		void 			HighlightSelection();		
		bool 			InsertWidget();
		bool 			DeleteWidget();		
		long 			FindBlockInText();		
		bool 			CanAcceptChildren();				
	}
	
	
	public class WidgetTreeNode : TreeNode
	{
		protected IWDBBase _elem;
		
		public WidgetTreeNode(string treenodename) : base(treenodename)
		{
		}
		
		public IWDBBase Widget
		{
			get	{	return _elem;	}
			set	{	_elem = value;	}
		}
		
		public void OnCut()		{}
		public void OnCopy()	{}
		public void OnPaste()	{}
		public void OnDelete()
		{
			// Delete this widget and
			// all his children, if any.
			// Remove as Node.
			Remove();
			_elem.DeleteWidget();
			if (_elem.IsSizer == true)
			{
				wx.Sizer sizer = (wx.Sizer)_elem;
				sizer.Dispose();
			} else {
				wx.Window win = (wx.Window)_elem;
				win.Close();
				win.Dispose();				
			}
		}
		public void OnMoveUp()		{}
		public void OnMoveDown()	{}
	}
}
