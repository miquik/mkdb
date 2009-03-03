/*
 * Creato da SharpDevelop.
 * Utente: michele
 * Data: 03/03/2009
 * Ora: 14.39
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;


namespace textops
{
	public enum PyFileSection
	{
		PY_CLASS_SECTION 	= 0,
		PY_APP_SECTION 		= 1
	}
		
	public class PyFile
	{
		protected ArrayList _class_list;
		protected PySection _app_class;
			
		public PyFile()
		{
			_class_list = new ArrayList();
			_app_class = new PySection();
		}
		
		public ArrayList ClassList
		{
			get	{	return _class_list;	}
		}
	}
}
