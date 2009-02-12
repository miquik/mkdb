/*
 * Creato da SharpDevelop.
 * Utente: michele
 * Data: 10/02/2009
 * Ora: 9.09
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.Drawing.Design;


namespace mkdb.Widgets
{
	public class wdbAppProps : wxWindowProps
	{
		// * wxFrame props : name, title, style, wxWindow, toParent
		protected string _appname;
				
		public wdbAppProps() : base()
		{
			_appname = "Noname";
		}
		
		[CategoryAttribute("Application"), DescriptionAttribute("Application Props")]
		public string AppName
		{
			get	{	return _appname;	}
			set	{	_appname = value;	NotifyPropertyChanged("AppName");	}
		}
		
	}

	/// <summary>
	/// Description of wdbApp.
	/// </summary>
	public class wdbApp : WidgetElem, IDisposable
	{
		private bool disposed = false;
		
		public wdbApp() : base(3)
		{
			_elem = null;
			_props = new wdbAppProps();
		}
		
		#region IDisposable methods implementation
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
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if(disposing)
                {
                    // Dispose managed resources.
                    _elem.Close();
                    _elem.Dispose();
                }
                // Note disposing has been done.
                disposed = true;
            }
        }
		#endregion			
		
		private void SetDefaultProps(string name)
		{
			wdbAppProps appProps = (wdbAppProps)_props;
			appProps.EnableNotification = false;
			appProps.AppName = "Noname";
			appProps.EnableNotification = true;
		}
				
		public override bool InsertWidget(WidgetElem parent)
		{
			Panel panel = Common.Instance().Canvas;
			wdbAppProps winProps = (wdbAppProps)_props;		
			uint _cstyle = wx.Frame.wxFRAME_TOOL_WINDOW|wx.Stretch.wxEXPAND;					
			/*
			_elem = new wx.MDIParentFrame(null, -1, "", new System.Drawing.Point(0, 0),
			                              new System.Drawing.Size(panel.Width, panel.Height), _cstyle);
			                              */
			_elem = new wx.Frame(null, -1, "", new System.Drawing.Point(0, 0),
			                     new System.Drawing.Size(panel.Width, panel.Height), _cstyle);
			Win32Utils.SetParent(_elem.GetHandle(), panel.Handle);
			_elem.Show();
			SetWidgetProps();
			this.Text = winProps.AppName;
			return true;
		}
		public override bool DeleteWidget()
		{
			return false;
		}		
		
		protected override bool InsertWidgetInText()
		{
			return true;			
		}
		
		protected override bool DeleteWidgetFromText()
		{
			return true;
		}
		
		public override long FindBlockInText()
		{
			return -1;
		}
		
		public override bool CanAcceptChildren()
		{
			return false;
		}
		
		public override void PaintOnSelection()
		{			
		}
		
		protected void OnMouseEvent(object sender, wx.Event evt)
        {
			if (evt.EventType == wx.Event.wxEVT_LEFT_DOWN)
			{
				/*
				switch (Common.Instance().CmdFlags) {
					case CommandFlags.TB_CMD_SIZER:
						// Add new sizer here...
						wx.BoxSizer sz = new wx.BoxSizer((int)1);
						_elem.SetSizerAndFit(sz, true);
						break;
				}
				*/
			}
			// Manage mouse events when inside this widget
			// Mouse Left : show properties associates to this widget
			// Mouse Right : Popup menu
			// if (evt.EventType == wx.Event.wxEVT_LEAVE_WINDOW)
			// {
			// }
        }
		
		protected void OnClose(object sender, wx.Event evt)
        {
			// DO NOTHING
        }		
						
		public void winProps_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }
				
		public void SetWidgetProps()
		{
			wdbAppProps winProps = (wdbAppProps)_props;
			_props.PropertyChanged += new PropertyChangedEventHandler(winProps_PropertyChanged);
			Common.Instance().ObjPropsPanel.SelectedObject = winProps;
		}		
				
	}
}
