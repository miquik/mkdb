/*
 * Creato da SharpDevelop.
 * Utente: Family Rose
 * Data: 24/12/2008
 * Ora: 14.05
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using wx;

namespace mkdb
{
	/// <summary>
	/// Description of wxApp.
	/// </summary>
	public class wxApp : App
	{
		public wxApp()
		{
		}
		
		public override bool OnInit()
		{
			MainForm mForm = new MainForm();
			mForm.Show();
			return true;
		}
	}
}
