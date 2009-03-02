/*
 * Creato da SharpDevelop.
 * Utente: michele
 * Data: 02/03/2009
 * Ora: 8.41
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
 using System;
 using System.ComponentModel;

 
 namespace mkdb.Widgets
 {
 	
	public class wdbAdoProps : wxWindowProps
	{
		protected string _label;
		
		public wdbAdoProps() : base()
		{
			_name = "Ado";
			_label = "Ado";
		}
		
		[CategoryAttribute("Ado"), DescriptionAttribute("Ado Props")]
		public string Label
		{
			get	{	return _label;	}
			set	{	_label = value;	NotifyPropertyChanged("Label");	}
		}		
	}
	
 }