/*
 * Creato da SharpDevelop.
 * Utente: michele
 * Data: 17/02/2009
 * Ora: 16.24
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace mkdb.Widgets
{
	/// <summary>
	/// Description of wdbBoxSizer.
	/// </summary>
	public class wdbBoxSizer : WidgetTreeNode
	{
		public wdbBoxSizer(wx.Window _pc, wx.Sizer _ps) : base("Sizer")
		{
			this.ImageIndex = 10;
			this.SelectedImageIndex = 10;
			_elem = new wiwBoxSizer(_pc, _ps);
			_elem.InsertWidget();
			this.Text = _elem.Properties.Name;
		}
	}
		
}
