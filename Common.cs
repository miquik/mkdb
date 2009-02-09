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
		private	WidgetElem	_cur_element;	
		private CommandFlags _cur_action;
		private PropertyGrid _obj_props_panel;
		private Hashtable _widget_list;
		private Panel _canvas;

		// Constructor is 'protected'
    	protected Common()
    	{
			_cur_element = null;
			_widget_list = new Hashtable();
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
    	
    	public void ChangeCurrentWindow(WidgetElem neww)
    	{
   			if (neww.IsSizer) return;
   			if (_cur_element != null) _cur_element.Element.Hide();
   			_cur_element = neww;
   			_cur_element.Element.Show();
    	}
    	
		public WidgetElem CurrentElement
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
		
		public Hashtable WidgetList
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
