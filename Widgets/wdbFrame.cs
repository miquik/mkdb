/*
 * Creato da SharpDevelop.
 * Utente: michele
 * Data: 12/01/2009
 * Ora: 17.17
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
	
	public class wdbFrameProps : wxWindowProps
	{
		// * wxFrame props : name, title, style, wxWindow, toParent
		protected string _name;
		protected string _title;
		protected wxFlags _fstyle;
		
		/*
		 *	Frame style
		 *  - DEFAULT
		 *  - TOOL_WINDOW
		 * 	- wxCAPTION + SYSTEM MENU
		 *  - MINIM_BOX
		 *  - MIN_
		 * 	- MAX_BOX
		 *  - MAX
		 *  - RESIZE_BORDER
		 * */		
		
		public wdbFrameProps() : base()
		{
			_name = "";
			_title = "Frame";
			_fstyle = new wxFlags();
			_fstyle.AddItem("wxFRAME_DEFAULT", wx.Frame.wxDEFAULT_FRAME_STYLE, true);
			_fstyle.AddItem("wxFRAME_TOOL_WINDOW", wx.Frame.wxCAPTION|wx.Frame.wxCLOSE_BOX|
			                	wx.Frame.wxFRAME_TOOL_WINDOW|wx.Frame.wxSYSTEM_MENU, false);
			_fstyle.AddItem("wxFRAME_BASE", wx.Frame.wxCAPTION|wx.Frame.wxCLOSE_BOX|wx.Frame.wxSYSTEM_MENU, false);			
			_fstyle.AddItem("wxMAXIMIZE", wx.Frame.wxMAXIMIZE, false);						
			_fstyle.AddItem("wxMAXIMIZE_BOX", wx.Frame.wxMAXIMIZE_BOX, false);						
			_fstyle.AddItem("wxMINIMIZE", wx.Frame.wxMINIMIZE, false);						
			_fstyle.AddItem("wxMINIMIZE_BOX", wx.Frame.wxMINIMIZE_BOX, false);						
			_fstyle.AddItem("wxRESIZE_BORDER", wx.Frame.wxRESIZE_BORDER, false);						
		}
		
		[CategoryAttribute("Frame"), DescriptionAttribute("Frame props")]
		public string Name
		{
			get	{	return _name;	}
			set	{	_name = value;	NotifyPropertyChanged("Name");	}
		}
		
		[CategoryAttribute("Frame"), DescriptionAttribute("Frame props")]
		public string Title
		{
			get	{	return _title;	}
			set	{	_title = value;	NotifyPropertyChanged("Title");	}
		}
		
		[CategoryAttribute("Frame"), DescriptionAttribute("Frame props")]
		[TypeConverter(typeof(wxFlagsTypeConverter))]
		[Editor(typeof(wxFlagsEditor), typeof(UITypeEditor))]
		public wxFlags Style
		{
			get	{	return _fstyle;	}
			set	{	_fstyle = value; NotifyPropertyChanged("Style");	}
		}
	}
	
	/// <summary>
	/// Description of wdbFrame.
	/// </summary>
	public class wdbFrame : WidgetElem, IDisposable
	{
		protected static long _frame_cur_index=0;
		private bool disposed = false;
		
		public wdbFrame()
		{
			_elem = null;
			_props = new wdbFrameProps();
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
			wdbFrameProps winProps = (wdbFrameProps)_props;
			winProps.WindowName = "wxFrameClass";			
			winProps.Name = name;
			winProps.Title = name;
			winProps.Pos = new System.Drawing.Point(0, 0);
			winProps.Size = new System.Drawing.Size(300, 300);
			winProps.ID = -1;
			// winProps.FC = (wxColor)wxColor.wxWHITE;
			// winProps.BC = wxColor.wxLIGHT_GREY;
		}
				
		public override bool InsertWidget()
		{
			InsertWidgetInEditor();
			return false;
		}
		
		public override bool DeleteWidget()
		{
			return false;
		}
		
		public override long FindBlockInText()
		{
			return -1;
		}
				
		protected override bool InsertWidgetInEditor()
		{
			uint _cstyle;
			wdbFrameProps winProps = (wdbFrameProps)_props;
			_frame_cur_index++;
			SetDefaultProps("Frame" + _frame_cur_index.ToString());			
			_cstyle = wx.Frame.wxDEFAULT_FRAME_STYLE;
			_elem = new wx.Frame(null, winProps.ID, winProps.Title, winProps.Pos, winProps.Size, _cstyle);
			_elem.EVT_MOUSE_EVENTS(new wx.EventListener(OnMouseEvent));
			_elem.EVT_CLOSE(new wx.EventListener(OnClose));
			SetWidgetProps();
			this.Text = winProps.Title;
			return true;
		}
		protected override bool InsertWidgetInText()
		{
			return true;			
		}
		
		protected override bool DeleteWidgetFromEditor()
		{
			return true;
		}
		protected override bool DeleteWidgetFromText()
		{
			return true;
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
            // System.Diagnostics.Debug.WriteLine(e.PropertyName + " has been changed.");
 
			wdbFrameProps wp = (wdbFrameProps)_props;
            switch (e.PropertyName)
            {
            	case "Title":
            		_elem.Title = wp.Title; this.Text = wp.Title;
            		break;
            	case "ID":
            		_elem.ID = wp.ID;
            		break;
            	case "Pos":
            		// Only change in Python text
            		// _elem.Position = wp.Pos;
            		break;
            	case "Size":
            		_elem.Size = wp.Size;
            		break;
            	case "Font":
            		_elem.Font = wp.Font;
            		break;
            	case "FC":
            		_elem.ForegroundColour = wp.FC;
            		break;
            	case "BC":
            		_elem.BackgroundColour = wp.BC;
            		break;
            	case "Enabled":
            		// Only change in Python text
            		break;            		
            	case "Hidden":
            		// Only change in Python text
            		break;            		
            	case "Style":
            		// Only change in Python text
            		_elem.StyleFlags = wp.Style.ToLong;
            		break;            		
            	case "WindowStyle":
            		// Only change in Python text
            		_elem.WindowStyle = wp.WindowStyle.ToLong;
            		break;            		            		
            	case "Border":
            		break;            		            		
            	case "Alignment":
            		break;            		            		
            }
       		_elem.Raise();
       		_elem.Update();
       		// Common.Instance().Canvas.Invalidate();
			// System.Drawing.Rectangle invalidateRect = new System.Drawing.Rectangle(0, 0, _elem.Width, _elem.Height);         		
       		// Win32Utils.InvalidateRect(_elem.GetHandle(), ref invalidateRect, true);
        }
				
		public void SetWidgetProps()
		{
			wdbFrameProps winProps = (wdbFrameProps)_props;
			_props.PropertyChanged += new PropertyChangedEventHandler(winProps_PropertyChanged);
			Common.Instance().ObjPropsPanel.SelectedObject = winProps;
		}		
				
	}
}
