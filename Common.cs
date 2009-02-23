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
		private ImageList	_obj_tree_il;
		private Panel _canvas;

		// Constructor is 'protected'
    	protected Common()
    	{
			_cur_element = null;
			_cur_window = null;
			_obj_props_panel = null;
			_cur_action = CommandFlags.TB_NONE;
			_obj_tree_il = null;
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
		
		public ImageList ObjTreeImageList
		{
			get	{	return _obj_tree_il;	}
			set	{	_obj_tree_il = value;	}
		}    	    	
    			
		
		public void ChangeCurrentWindow(WidgetTreeNode neww)
    	{
			if (neww.Widget.WidgetType == (int)StandardWidgetType.WID_APP)
			{
				// Hide, when we select the application.
				if (_cur_window != null)
   					_cur_window.Hide();				
			}
 	  		IWDBBase _cur = FindTopMostFrame(neww);
  			if (_cur != null)
  			{
 	  			if (_cur_window != null)
   					_cur_window.Hide();
 	  			_cur_window = (wx.Window)_cur;
				_cur_window.Show();
  			} 	  		
    	}

		public IWDBBase FindTopMostFrame(WidgetTreeNode node)
		{
			if (node.Level == 1)
			{
				if (node.Widget.WidgetType == (int)StandardWidgetType.WID_FRAME)
				{
					return node.Widget;
				}
				else
				{
					return null;
				}
			}
			if (node.Parent != null)
			{
				return FindTopMostFrame((WidgetTreeNode)node.Parent);
			}
			return null;
		}
		
		public WidgetTreeNode FindBestParentContainer(WidgetTreeNode curNode, bool sizerIsAsking)
		{
			if (sizerIsAsking == false)
			{
				return null;
			}
			if (curNode.Widget.IsSizer == false)
			{
				if (curNode.Widget.CanAcceptChildren())
				{
					return curNode;
				} else 
				{
					return null;
				}
			} else
			{
				if (curNode.Parent == null)
				{
					return null;
				} else
				{
					return FindBestParentContainer((WidgetTreeNode)curNode.Parent, true);
				}
			}
		}
		
		public WidgetTreeNode FindBestParentSizer(WidgetTreeNode curNode, bool sizerIsAsking)
		{
			if (curNode.Widget.IsSizer == false)
			{
				return null;
			}
			if (curNode.Widget.CanAcceptChildren())
			{
				return curNode;
			} else
			{
				if (curNode.Parent == null)
				{
					return null;
				} else
				{
					return FindBestParentSizer((WidgetTreeNode)curNode.Parent, false);
				}
			}
		}						
		
		public void CheckParentForSizer(WidgetTreeNode _c, WidgetTreeNode _s, out wx.Window _rc, out wx.Sizer _rs)
		{
			_rc = null;
			_rs = null;
			if (_c != null) _rc = (wx.Window)_c.Widget;
			if (_s != null) 
			{
				_rs = (wx.Sizer)_s.Widget;
				_rc = _s.Widget.ParentContainer;
			}			
		}
		
		public void CheckParentForWidget(WidgetTreeNode _c, WidgetTreeNode _s, out wx.Window _rc)
		{
			_rc = null;
			if (_c == null)
			{
				_rc = (wx.Window)_s.Widget.ParentContainer;
			} else
			{
				_rc = (wx.Window)_c.Widget;
			}
		}
		
		public Panel Canvas
		{
			get	{	return _canvas; }
			set	{	_canvas = value; }
		}
  	}	
}
