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
	public class wdbListbox : WidgetTreeNode
	{
		public wdbListbox(wx.Window _pc, wx.Sizer _ps) : base("ListBox")
		{
			this.ImageIndex = 1;
			this.SelectedImageIndex = 1;			
			_elem = new wiwListbox(_pc, _ps);
			_elem.InsertWidget();
			this.Text = _elem.Properties.Name;			
		}
		
		public override void OnCut()		{}
		public override void OnCopy()		{}
		public override void OnPaste()		{}
		public override void OnMoveUp()		{}
		public override void OnMoveDown()	{}		
	}
}
