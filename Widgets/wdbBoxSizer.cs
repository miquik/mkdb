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
		public wdbBoxSizer(wx.Window _pc, wx.Sizer _ps) : base(_pc, _ps, "Sizer")
		{
			this.ImageIndex = 10;
			this.SelectedImageIndex = 10;
			_elem = new wiwBoxSizer(_pc, _ps);
			// _elem.InsertWidget(parent);
			wdbBoxSizerProps _p = (wdbBoxSizerProps)_elem.Properties;
			this.Text = _p.Name;
		}
		/*
		public override IWDBBase CreateWidget(IWDBBase parent)
		{
			_elem = new wiwBoxSizer();
			_elem.InsertWidget(parent);
			wdbBoxSizerProps _p = (wdbBoxSizerProps)_elem.Properties;
			this.Text = _p.Name;
			return _elem;
		}		
		*/
	}
		
}
