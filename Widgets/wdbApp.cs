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
	public class wdbApp : WidgetTreeNode
	{
		protected int _app_begin;
		protected int _app_end;
		// protected int _decl_begin;
		// protected int _decl_end;
		protected int _creation_begin;
		protected int _creation_end;
		protected int _layout_begin;
		protected int _layout_end;
		
		public wdbApp(wx.Window _pc, wx.Sizer _ps) : base("Project")
		{
			this.ImageIndex = 0;
			this.SelectedImageIndex = 0;			
			_elem = new wiwApp(_pc, _ps);
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
