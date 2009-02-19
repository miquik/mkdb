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
		public wdbApp(wx.Window _pc, wx.Sizer _ps) : base(_pc, _ps, "Project")
		{
			this.ImageIndex = 1;
			this.SelectedImageIndex = 1;			
			_elem = new wiwApp(_pc, _ps);
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
