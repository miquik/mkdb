/*
 * Creato da SharpDevelop.
 * Utente: michele
 * Data: 20/02/2009
 * Ora: 16.54
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace mkdb.Widgets
{
	/// <summary>
	/// Description of wdbBoxSizer.
	/// </summary>
	public class wdbGridSizer : WidgetElem
	{
		public wdbGridSizer(wx.Window _pc, wx.Sizer _ps) : base("Sizer")
		{
			this.ImageIndex = 7;
			this.SelectedImageIndex = 7;
			_elem = new wiwGridSizer(_pc, _ps, 2, 2, 0, 0);
			_elem.InsertWidget(null);
			this.Text = _elem.Properties.Name;
		}
	}
}
