/*
 * Creato da SharpDevelop.
 * Utente: michele
 * Data: 13/01/2009
 * Ora: 12.23
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;

namespace mkdb
{
  	public class Common
  	{
    	private static Common _instance;
		private	IWDBBase	_cur_element;	
		private	wx.Window	_cur_window;	
		private CommandFlags _cur_action;
		private TreeView	_obj_tree;
		private PropertyGrid _obj_props_panel;
		private List<IWDBBase> _widget_list;
		private Panel _canvas;

		// Constructor is 'protected'
    	protected Common()
    	{
			_cur_element = null;
			_cur_window = null;
			_widget_list = new List<IWDBBase>();
			_obj_props_panel = null;
			_cur_action = CommandFlags.TB_NONE;
    	}

    	public static Common Instance()
    	{
      		// Uses lazy initialization.
      		// Note: this is not thread safe.
      		if (_instance == null)
      		{
        		_instance = new Common();
      		}
      		return _instance;
    	}
    	/*
    	public void ChangeCurrentWindow(IWDBBase neww)
    	{
   			if (neww.IsSizer) return;
 			if (neww.WidgetID == (int)StandardWidgetID.WID_FRAME)
 			{
 	  			if (_cur_window != null)
   					_cur_window.Hide();
   				_cur_window = neww.WxWindow;
   				_cur_window.Show();
   			}
    	}
    	*/
		public wx.Window CurrentWindow
		{
			get	{	return _cur_window;	}
			set	{	_cur_window = value;	}
		}    	    	
    	
		public IWDBBase CurrentElement
		{
			get	{	return _cur_element;	}
			set	{	_cur_element = value;	}
		}    	
		
		public CommandFlags CurrentAction
		{
			get	{	return _cur_action;		}
			set	{	_cur_action = value;	}
		}
		
		public PropertyGrid ObjPropsPanel
		{
			get	{	return	_obj_props_panel; 	}
			set	{	_obj_props_panel = value;	}
		}
		
		public TreeView ObjTree
		{
			get	{	return _obj_tree; 	}
			set	{	_obj_tree = value;	}
		}		
		
		public List<IWDBBase> WidgetList
		{
			get	{	return _widget_list;	}
		}
		
		public Panel Canvas
		{
			get	{	return _canvas; }
			set	{	_canvas = value; }
		}
  	}	
}
