/*
 * Creato da SharpDevelop.
 * Utente: michele
 * Data: 17/02/2009
 * Ora: 15.02
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;

namespace mkdb.Widgets
{
	/// <summary>
	/// Description of wdbFrame.
	/// </summary>
	public class wdbFrame : WidgetElem
	{
		public wdbFrame(wx.Window _pc, wx.Sizer _ps) : base(_pc, _ps, "Frame")
		{
			this.ImageIndex = 5;
			this.SelectedImageIndex = 5;			
			// Create a wxWindow on the top of canvas panel.
			wx.Frame hfrm = new wx.Frame(null);		
			hfrm.Hide();
			_elem = new wiwFrame(hfrm, "", new Point(0, 0), new Size(300, 300), wx.Frame.wxDEFAULT_FRAME_STYLE);
			wx.Panel f = (wx.Panel)_elem;
			Win32Utils.SetParent(f.GetHandle(), Common.Instance().Canvas.Handle);
			// Win32Utils.SetParent(_elem.ParentContainer.GetHandle(), Common.Instance().Canvas.Handle);
			// _elem.InsertWidget(null);
			// Set Parent
			// Common.Instance().WidgetList.Add(_elem);
			// Common.Instance().ChangeCurrentWindow(_elem);
			wiwFrame wf = (wiwFrame)_elem;
			this.Text = wf.Name;
		}
		/*
		public override IWDBBase CreateWidget(IWDBBase parent)
		{
			// Create a wxWindow on the top of canvas panel.
			wx.Frame hfrm = new wx.Frame(null);			
			_elem = new wiwFrame(hfrm, "", new Point(0, 0), new Size(300, 300), wx.Frame.wxDEFAULT_FRAME_STYLE);
			Win32Utils.SetParent(_elem.WxWindow.GetHandle(), Common.Instance().Canvas.Handle);
			_elem.InsertWidget(null);
			// Set Parent
			Common.Instance().WidgetList.Add(_elem);
			// Common.Instance().ChangeCurrentWindow(_elem);
			wiwFrame wf = (wiwFrame)_elem;
			this.Text = wf.Name;
			return _elem;
		}
		*/
	}
}
