/*
 * Creato da SharpDevelop.
 * Utente: Family Rose
 * Data: 18/02/2009
 * Ora: 18.34
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Drawing;
using System.Collections;

namespace mkdb.Widgets
{

	public class wdbLabelProps : wxWindowProps
	{
		// * wxButton props : name, style, label, default, wxWindow, Align
		protected wxFlags _lstyle;
		protected string _text;
		
		public wdbLabelProps() : base()
		{
			_name = "Label";
			_text = "Static Text";
			_lstyle = new wxFlags();
			_lstyle.AddItem("wxALIGN_LEFT", wx.Alignment.wxALIGN_LEFT, false);
			_lstyle.AddItem("wxALIGN_RIGHT", wx.Alignment.wxALIGN_RIGHT, false);
			_lstyle.AddItem("wxALIGN_CENTER", wx.Alignment.wxALIGN_CENTRE, true);
			_lstyle.AddItem("wxST_NO_AUTORESIZE", wx.StaticText.wxST_NO_AUTORESIZE, false);
		}
		
		[CategoryAttribute("Label"), DescriptionAttribute("Label Props")]
		public string Text
		{
			get	{	return _text;	}
			set	{	_text = value;	NotifyPropertyChanged("Text");	}
		}
		
		[TypeConverter(typeof(wxFlagsTypeConverter))]
		[Editor(typeof(wxFlagsEditor), typeof(UITypeEditor))]
		[CategoryAttribute("Label"), DescriptionAttribute("Label Props")]
		public wxFlags TextStyle
		{
			get	{	return _lstyle;	}
			set	{	_lstyle = value;	NotifyPropertyChanged("TextStyle");	}
		}
		
	}

	/// <summary>
	/// Description of wdbApp.
	/// </summary>
	public class wiwLabel : wx.StaticText, IWDBBase
	{
		protected static long _label_cur_index=0;		
		protected wdbLabelProps _props;
		protected bool _is_selected;
		protected wx.Sizer _p_sizer;
		protected wx.SizerItem _sizer_item;
			
		// public wiwButton(wx.Window _pc, wx.Sizer _ps, string _label, Point _pos, Size _sz, uint _style) 
		// 	: base(_pc, _label, _pos, _sz, _style)
		public wiwLabel(wx.Window _pc, wx.Sizer _ps)
		 	: base(_pc, "Label", wx.Button.wxDefaultPosition, wx.Button.wxDefaultSize, wx.Button.wxBU_EXACTFIT)
		{
			_props = new wdbLabelProps();
			_label_cur_index++;			
			string name = "Label" + _label_cur_index.ToString();			
			SetDefaultProps(name);
			SetWidgetProps();
			_p_sizer = _ps;
			_sizer_item = null;
		}
		
		#region IWidgetElem Interface implementation
		public wx.SizerItem SizerItem
		{
			get	{	return _sizer_item;	}
		}		
		public WidgetProps Properties	
		{	
			get	{	return _props; }
		}
		public wx.Window ParentContainer	
		{	
			get	{	return this.Parent;	}
		}
		public wx.Sizer	ParentSizer		
		{	
			get	{	return	_p_sizer;	}
		}
		public bool	IsSizer		
		{	
			get	{	return false;	}
		}
		public bool	IsSelected	
		{	
			get	{	return _is_selected;	}
			set	{	_is_selected = value;	}
		}		
		public int WidgetType
		{
			get	{	return (int)StandardWidgetType.WID_BUTTON; }
		}
		#endregion
		
		private void SetDefaultProps(string name)
		{
			_props.EnableNotification = false;	
			_props.Text = name;
			_props.Name = name;
			_props.EnableNotification = true;
			this.Label = name;
		}
				
		public bool InsertWidget()
		{
			_p_sizer.Add(this, 0, (int)(_props.Alignment.ToLong|_props.Border.ToLong), _props.BorderWidth);
			_sizer_item = (wx.SizerItem)_p_sizer.GetItem(_p_sizer.GetItemCount() - 1);
			this.Parent.AutoLayout = true;
			this.Parent.Layout();	
			return true;
		}
		public bool DeleteWidget()
		{
			// _p_sizer.Detach(this);
			_p_sizer.Remove(this);
			return false;
		}		
		
		public long FindBlockInText()
		{
			return -1;
		}
		
		public bool CanAcceptChildren()
		{
			return false;
		}
		
		public void HighlightSelection()
		{			
			Panel pan = Common.Instance().Canvas;
			// Graphics area = Graphics.FromHwnd(this.GetHandle());
			Graphics area = Graphics.FromHwnd(this.Parent.GetHandle());
			// Graphics area = Graphics.FromHwnd(pan.Handle);
			Color cl = Color.FromArgb(255, this.Parent.BackgroundColour.Red,
			                     this.Parent.BackgroundColour.Green,
			                     this.Parent.BackgroundColour.Blue);
			area.Clear(cl);
			if (IsSelected)
			{
				Pen _pen = new Pen(Color.Red, 1);
				Point _ps = Point.Subtract(this.Position, new Size(_props.BorderWidth, _props.BorderWidth));
				Size _sz = Size.Add(this.Size, new Size(_props.BorderWidth * 2 - 1, _props.BorderWidth * 2 - 1));				
				area.DrawRectangle(_pen, _ps.X, _ps.Y, _sz.Width, _sz.Height);
			}			
		}
								
		public void winProps_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
			bool baa = false;
            switch (e.PropertyName)
            {
            	case "Name":
					Common.Instance().ObjTree.SelectedNode.Text = _props.Name;	
            		break;            		
            	case "Label":
            		this.Label = _props.Text;
            		this.Refresh();
            		break;
            	case "LabelStyle":
            		this.StyleFlags = _props.TextStyle.ToLong;
            		this.Refresh();
            		break;
            	case "Border":
            		baa = true;
            		break;
            	case "BorderWidth":
            		baa = true;
            		break;
            	case "Alignment":
            		baa = true;
            		break;
            }
            if (baa)
            {
           		// Is this the only way???           
           		_sizer_item.Border = _props.BorderWidth;
           		_sizer_item.Flag = (int)(_props.Alignment.ToLong|_props.Border.ToLong);
				this.Parent.AutoLayout = true;
				this.Parent.Layout();  
				this.HighlightSelection();
            }
            this.UpdateWindowUI();            
        }
		/*
		int FindPositionInSizerList(IWDBBase node, IWXSizer sizer)
		{
			int i=0;
			foreach (IWDBBase e in sizer.List)
			{
				if (node.Equals(e))
					return i;
				i++;
			}
			return -1;
		}
		*/		
		public void SetWidgetProps()
		{
			_props.PropertyChanged += new PropertyChangedEventHandler(winProps_PropertyChanged);
			Common.Instance().ObjPropsPanel.SelectedObject = _props;
		}		
				
	}
}
