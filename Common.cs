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

namespace mkdb
{
  	public class Common
  	{
    	private static Common _instance;
		private	wx.Window	_cur_window;	
		private string _cur_action;
		private PropertyGrid _obj_props_panel;
    	// Constructor is 'protected'

    	protected Common()
    	{
			_cur_window = null;    		
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
    	
    	public void ChangeCurrentWindow(wx.Window neww)
    	{
    		if (_cur_window != null) _cur_window.Hide();
    		_cur_window = neww;
    		_cur_window.Show();
    	}
    	
		public wx.Window CurrentWindow
		{
			get	{	return _cur_window;		}
			set	{	_cur_window = value;	}
		}    	
		
		public string CurAction
		{
			get	{	return _cur_action;		}
			set	{	_cur_action = value;	}
		}
		
		public PropertyGrid ObjPropsPanel
		{
			get	{	return	_obj_props_panel; 	}
			set	{	_obj_props_panel = value;	}
		}
  	}	
}
