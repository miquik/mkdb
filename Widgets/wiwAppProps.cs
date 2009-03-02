/*
 * Creato da SharpDevelop.
 * Utente: Family Rose
 * Data: 01/03/2009
 * Ora: 14.34
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;

namespace mkdb.Widgets
{
	public class wdbAppProps : wxAlignProps
	{
		// * wxFrame props : name, title, style, wxWindow, toParent
		protected string _appname;
		protected string _dirname;
		protected string _appfilename;
				
		public wdbAppProps() : base()
		{
			_appname = "Project";
			_dirname = ".";
			_appfilename = "project.py";
		}
		
		[CategoryAttribute("Application"), DescriptionAttribute("Application Props")]
		public string AppName
		{
			get	{	return _appname;	}
			set	{	_appname = value;	NotifyPropertyChanged("AppName");	}
		}
		
		[CategoryAttribute("Application"), DescriptionAttribute("Application Props")]
		public string Directory
		{
			get	{	return _dirname;	}
			set	{	_dirname = value;	NotifyPropertyChanged("Directory");	}
		}
		
		[CategoryAttribute("Application"), DescriptionAttribute("Application Props")]
		public string FileName
		{
			get	{	return _appfilename;	}
			set	{	_appfilename = value;	NotifyPropertyChanged("FileName");	}
		}		
	}
}
