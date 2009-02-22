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
	public class wdbFrame : WidgetTreeNode, IDisposable
	{
		protected wx.Frame hfrm = null;
		
		#region IDisposable implementation		
		// Track whether Dispose has been called.
        private bool disposed = false;

        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (this.disposed == false)
            {
                hfrm.Close();
                hfrm.Dispose();
                disposed = true;
            }
        }
        #endregion
				
		public wdbFrame(wx.Window _pc, wx.Sizer _ps) : base("Frame")
		{
			this.ImageIndex = 5;
			this.SelectedImageIndex = 5;			
			// Create a wxWindow on the top of canvas panel.
			wx.Frame hfrm = new wx.Frame(null, -1, "");		
			_elem = new wiwFrame(hfrm);
			_elem.InsertWidget();
			wx.Window win = (wx.Window)_elem;
			Win32Utils.SetParent(win.GetHandle(), Common.Instance().Canvas.Handle);
			wiwFrame wf = (wiwFrame)_elem;
			this.Text = wf.Name;
		}
	}
}
