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
		
		public wdbBoxSizer() : base((int)StandardWidgetID.WID_BOXSIZER)
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
			bsProps.MinSize = 500;
		}
				
		public override bool InsertWidget(WidgetElem parent)
		{
			wdbBoxSizerProps bsProps = (wdbBoxSizerProps)_props;
			_frame_cur_index++;
			SetDefaultProps("BoxSizer" + _frame_cur_index.ToString());			
			_sizer = new wx.BoxSizer((int)bsProps.Orient);
			/** NB. In this case "_elem" contains the parent of this sizer **/
			_elem = parent.Element;
			/** *** *** **/
			if (parent.IsSizer)
			{
				// Add a sizer to a sizer
				parent.Sizer.Add(_sizer);
			} else {
				parent.Element.SetSizer(_sizer, true);
				_sizer.FitInside(parent.Element);
			}
			// _sizer.EVT_MOUSE_EVENTS(new wx.EventListener(OnMouseEvent));
			// parent.Sizer.SetSizerAndFit(_sizer, true);
			SetWidgetProps();
			this.Text = "BoxSizer" + _frame_cur_index.ToString();			
			IsSizer = true;
			return true;
		}
		
		public override bool DeleteWidget()
		{
			return false;
		}
		
		public override long FindBlockInText()
		{
			return -1;
		}
		
		public override bool CanAcceptChildren()
		{
			return true;
		}
				
				
		protected override bool InsertWidgetInText()
		{
			return true;			
		}		
		protected override bool DeleteWidgetFromText()
		{
			return true;
		}
		
		public override void PaintOnSelection()
		{
			int w, h;
			wx.ClientDC wdc = new wx.ClientDC(_elem);
			if (wdc != null)
			{
				wdc.Pen = new wx.Pen(new wx.Colour(255, 0, 0), 3);
				wdc.Pen.Width = -1;
				wdc.GetSize(out w, out h);
				// wdc.DrawCircle(_elem.Width/2, _elem.Height/2, 100);
				wdc.DrawLine(0, 0, w - 1, 0);
				wdc.DrawLine(w - 1, 0, w - 1, h - 1);
				wdc.DrawLine(0, h - 1, w - 1, h - 1);
				wdc.DrawLine(0, 0, 0, h - 1);
				// wdc.DrawRectangle(0, 0, _elem.Width, _elem.Height);
			}
			wdc.Dispose();
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
       		_elem.SetSize(0, 0, _elem.Width+1, _elem.Height+1);
       		_elem.SetSize(0, 0, _elem.Width-1, _elem.Height-1);
       		_elem.Refresh();
   	   		wx.App.SafeYield(_elem);
       		*/
        }
				
		public void SetWidgetProps()
		{
			wdbBoxSizerProps winProps = (wdbBoxSizerProps)_props;
			// _props.PropertyChanged += new PropertyChangedEventHandler(winProps_PropertyChanged);
			Common.Instance().ObjPropsPanel.SelectedObject = winProps;
		}		
				
	}
}
