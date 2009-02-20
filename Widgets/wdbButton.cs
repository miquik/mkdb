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
	public class wdbButton : WidgetElem
	{
		public wdbButton(wx.Window _pc, wx.Sizer _ps) : base("Button")
		{
			this.ImageIndex = 1;
			this.SelectedImageIndex = 1;			
			_elem = new wiwButton(_pc, _ps, "Button", wx.Button.wxDefaultPosition, wx.Button.wxDefaultSize, 0);
			_elem.InsertWidget(null);
			this.Text = _elem.Properties.Name;			
		}
		/*
		public override IWDBBase CreateWidget(IWDBBase parent)
		{
			wx.Window _win = Common.Instance().CurrentWindow;
			_elem = new wiwButton(_win, "Button", wx.Button.wxDefaultPosition, wx.Button.wxDefaultSize, 0);
			_elem.InsertWidget(parent);			
			return _elem;
		}*/
	}
}
