/*
 * Creato da SharpDevelop.
 * Utente: michele
 * Data: 13/01/2009
 * Ora: 12.23
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;

namespace mkdb
{
  	public class Common
  	{
    	private static Common _instance;
		private	wx.Window	_cur_window;		
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
    	
		public wx.Window CurrentWindow
		{
			get	{	return _cur_window;		}
			set	{	_cur_window = value;	}
		}    	
  	}	
}
