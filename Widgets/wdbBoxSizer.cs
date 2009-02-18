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
	public class wdbBoxSizer : WidgetElem
	{
		public wdbBoxSizer() : base("Sizer")
		{
			this.ImageIndex = 10;
			this.SelectedImageIndex = 10;
		}
		
		public override IWDBBase CreateWidget(IWDBBase parent)
		{
			_elem = new wiwBoxSizer();
			_elem.InsertWidget(parent);
			wdbBoxSizerProps _p = (wdbBoxSizerProps)_elem.Properties;
			this.Text = _p.Name;
			return _elem;
		}		
	}
		
}
