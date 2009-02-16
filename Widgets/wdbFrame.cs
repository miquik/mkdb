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
using System.Drawing;



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
		
		public wdbFrame() : base((int)StandardWidgetID.WID_FRAME)
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
		}
				
		public override bool InsertWidget(WidgetElem parent)
		{
			uint _cstyle = 0;
			Panel pan = Common.Instance().Canvas;			
			wdbFrameProps winProps = (wdbFrameProps)_props;
			_frame_cur_index++;
			SetDefaultProps("Frame" + _frame_cur_index.ToString());			
			// _elem = new wx.MDIChildFrame((wx.MDIParentFrame)parent.Element, winProps.ID, winProps.Title, winProps.Pos, winProps.Size, _cstyle);
			_cstyle = wx.Frame.wxDEFAULT_FRAME_STYLE;
			wx.Frame fr = new wx.Frame(null, -1, "");
			// _elem = new wifInnerFrame(fr, -1, winProps.Title, winProps.Pos, winProps.Size, _cstyle);
			_elem = new wifSimpleFrame(fr, -1, winProps.Title, winProps.Pos, winProps.Size, _cstyle);
			Win32Utils.SetParent(_elem.GetHandle(), pan.Handle);
			wifSimpleFrame wiff = (wifSimpleFrame)_elem;
			// wifInnerFrame wiff = (wifInnerFrame)_elem;
			wiff.ParentColor = pan.BackColor;
			wiff.ParentHandle = pan.Handle;
			// _elem = new wifInnerFrame(parent.Element, -1, winProps.Title, winProps.Pos, winProps.Size, _cstyle);
			// _elem.EVT_MOUSE_EVENTS(new wx.EventListener(OnMouseEvent));
			_elem.EVT_CLOSE(new wx.EventListener(OnClose));
			SetWidgetProps();
			this.Text = winProps.Title;
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
			if (_elem.Sizer == null)
				return true;
			return false;
		}
		
		public override void PaintOnSelection()
		{		
			if (this.IsSelected) 
			{
				Graphics dc = Graphics.FromHwnd(_elem.GetHandle());
				Pen _pen = new Pen(Color.Red, 2);
				dc.DrawRectangle(_pen, 0, 0, _elem.ClientSize.Width, _elem.ClientSize.Height);
				/*
				wx.WindowDC wdc = new wx.WindowDC(_elem);
				// wdc.SetLogicalFunction((int)wx.Logic.wxNOR);
				if (wdc != null)
				{
					wdc.Pen = new wx.Pen(new wx.Colour(255, 0, 0), 3);
					wdc.DrawLine(0, 0, _elem.Width - 1, 0);
					wdc.DrawLine(_elem.Width - 1, 1, _elem.Width - 1, _elem.Height - 1);
					wdc.DrawLine(0, _elem.Height - 1, _elem.Width - 1, _elem.Height - 1);
					wdc.DrawLine(0, 0, 0, _elem.Height - 1);
				}
				wdc.Dispose();
				*/
			}
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
            _elem.UpdateWindowUI();
        }
				
		public void SetWidgetProps()
		{
			wdbFrameProps winProps = (wdbFrameProps)_props;
			_props.PropertyChanged += new PropertyChangedEventHandler(winProps_PropertyChanged);
			Common.Instance().ObjPropsPanel.SelectedObject = winProps;
		}		
				
	}
}
