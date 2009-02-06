/*
 * Creato da SharpDevelop.
 * Utente: michele
 * Data: 06/02/2009
 * Ora: 16.07
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
	
	/* wxBoxSizer : name, orient, min_size, Align */	
	public class wdbBoxSizerProps : wxAlignProps
	{
		protected string _name;
		protected int _orient;
		protected int _min_size;
		
		public wdbBoxSizerProps() : base()
		{
			_name = "";
			_orient = 0;
			_min_size = -1;
		}
		
		[CategoryAttribute("Box Sizer"), DescriptionAttribute("Box Sizer")]
		public string Name
		{
			get	{	return _name;	}
			set	{	_name = value;	NotifyPropertyChanged("Name");	}
		}
		
		[CategoryAttribute("Box Sizer"), DescriptionAttribute("Box Sizer")]
		public int Orient
		{
			get	{	return _orient;	}
			set	{	_orient = value;	NotifyPropertyChanged("Orient");	}
		}
		
		[CategoryAttribute("Box Sizer"), DescriptionAttribute("Box Sizer")]
		public int MinSize
		{
			get	{	return _min_size;	}
			set	{	_min_size = value; NotifyPropertyChanged("MinSize");	}
		}
	}

	
	public class wdbBoxSizer : WidgetElem, IDisposable
	{
		protected static long _frame_cur_index=0;
		private bool disposed = false;
		
		public wdbBoxSizer()
		{
			_elem = null;
			_props = new wdbBoxSizerProps();
		}
		
		#region IDisposable methods implementation
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
			wdbBoxSizerProps bsProps = (wdbBoxSizerProps)_props;
			bsProps.Name = name;
			bsProps.Orient = 1;
			bsProps.MinSize = -1;
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
			wdbBoxSizerProps bsProps = (wdbBoxSizerProps)_props;
			_frame_cur_index++;
			SetDefaultProps("BoxSizer" + _frame_cur_index.ToString());
			wx.Frame cframe = (wx.Frame)Common.Instance().CurrentWindow;
			_elem = new wx.BoxSizer((int)bsProps.Orient);
			_elem.EVT_MOUSE_EVENTS(new wx.EventListener(OnMouseEvent));
			cframe.SetSizerAndFit(_elem, true);
			SetWidgetProps();
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
 			/*
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
       		*/
       		// Common.Instance().Canvas.Invalidate();
			// System.Drawing.Rectangle invalidateRect = new System.Drawing.Rectangle(0, 0, _elem.Width, _elem.Height);         		
       		// Win32Utils.InvalidateRect(_elem.GetHandle(), ref invalidateRect, true);
        }
				
		public void SetWidgetProps()
		{
			wdbBoxSizerProps winProps = (wdbBoxSizerProps)_props;
			_props.PropertyChanged += new PropertyChangedEventHandler(winProps_PropertyChanged);
			Common.Instance().ObjPropsPanel.SelectedObject = winProps;
		}		
				
	}
}
