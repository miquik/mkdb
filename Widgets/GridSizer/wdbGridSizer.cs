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
	public class wdbGridSizer : WidgetTreeNode
	{
		public wdbGridSizer(wx.Window _pc, wx.Sizer _ps) : base("Sizer")
		{
			this.ImageIndex = 7;
			this.SelectedImageIndex = 7;
			_elem = new wiwGridSizer(_pc, _ps);
			_elem.InsertWidget();
			this.Text = _elem.Properties.Name;
		}
		
		public override void OnCut()		{}
		public override void OnCopy()		{}
		public override void OnPaste()		{}
		public override void OnDelete()		{}
		public override void OnMoveUp()		{}
		public override void OnMoveDown()	{}		
	}
}
