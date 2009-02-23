/*
 * Creato da SharpDevelop.
 * Utente: michele
 * Data: 23/02/2009
 * Ora: 9.00
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;

namespace mkdb
{
	/// <summary>
	/// Description of MenuLayout.
	/// </summary>
	public abstract class wxToolStripButton : ToolStripButton
	{
		public wxToolStripButton() : base()
		{
		}
				
		public abstract void AddToolStripButton();
	}
}
