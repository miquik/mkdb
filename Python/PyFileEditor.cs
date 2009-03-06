/*
 * Creato da SharpDevelop.
 * Utente: michele
 * Data: 02/03/2009
 * Ora: 10.21
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;

namespace mkdb.Python
{
	public class PyClassFile
	{
		private string _name;
		private PySection _base_class_section;
		private PySection _class_section;
		
		public PyClassFile(string name)
		{
			_name = name;
			_base_class_section = new PySection(name + "_base_class");
			_class_section = new PySection(name + "_class");
		}
		
		public string ClassFileName
		{
			get	{	return _name;	}
			set	
			{	
				_name = value;
				_base_class_section.Name = (value + "_base_class");
				_class_section.Name = (value + "_class");
			}
		}		
		public PySection BaseClassSection
		{
			get	{	return _base_class_section;	}
		}		
		public PySection ClassSection
		{
			get	{	return _class_section;	}
		}		
	}
	
	
	
	public class PyFileEditor
	{
		protected ArrayList _class_files;
		protected PySection _app_file;
		protected PyParser _parser;
		
		public PyFileEditor()
		{
			_parser = new PyParser();
			CreateEmptyApp();
		}
		
		protected void CreateEmptyApp()
		{
			_app_file = new PySection("application");
   			Assembly a = Assembly.GetExecutingAssembly();
   			StreamReader str = new StreamReader(a.GetManifestResourceStream("mkdb.Python.app_template.py"));
        	_parser.ParseFile(str, _app_file);
     	}
		
		public PySection ApplicationSection
		{
			get	{	return _app_file;	}
		}
		
		public PyParser Parser
		{
			get	{	return _parser;	}
		}
		
		public PyClassFile CreateNewClassFile(string name)
		{
			PyClassFile pf = new PyClassFile(name);
			_class_files.Add(pf);
			//			
 	  		Assembly a = Assembly.GetExecutingAssembly();
   			StreamReader str1 = new StreamReader(a.GetManifestResourceStream("mkdb.Python.base_class_template.py"));
        	_parser.ParseFile(str1, pf.BaseClassSection);
			//
   			StreamReader str2 = new StreamReader(a.GetManifestResourceStream("mkdb.Python.class_template.py"));
        	_parser.ParseFile(str2, pf.ClassSection);
			return pf;
		}
	}
}
