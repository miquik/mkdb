/*
 * Creato da SharpDevelop.
 * Utente: michele
 * Data: 17/02/2009
 * Ora: 14.52
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace mkdb.Widgets
{
	/// <summary>
	/// Description of wdbApp.
	/// </summary>
	/// 
	public class wdbApp : WidgetElem
	{
		public wdbApp(wx.Window _pc, wx.Sizer _ps) : base("Project")
		{
			this.ImageIndex = 14;
			this.SelectedImageIndex = 14;			
			_elem = new wiwApp(_pc, _ps);
			_elem.InsertWidget(null);
		}
		/*
		public override IWDBBase CreateWidget(IWDBBase parent)
		{
			_elem = new wiwApp();
			return _elem;
		}
		*/
	}
}
