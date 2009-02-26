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
using System.Reflection;

namespace mkdb.Widgets
{

	public class wdbAdoProps : wxWindowProps
	{
		protected string _label;
		
		public wdbAdoProps() : base()
		{
			_name = "Ado";
			_label = "Ado";
		}
		
		[CategoryAttribute("Ado"), DescriptionAttribute("Ado Props")]
		public string Label
		{
			get	{	return _label;	}
			set	{	_label = value;	NotifyPropertyChanged("Label");	}
		}		
	}

	public class wiwAdo : wx.BoxSizer, IWDBBase
	{
		protected static long _ado_cur_index=0;
		protected wdbAdoProps _props;
		protected bool _is_selected;
		protected wx.Window _p_container;
		protected wx.Sizer _p_sizer;
		protected wx.SizerItem _sizer_item;		
		// sub widgets
		protected wx.Button _btn_first;
		protected wx.Button _btn_prev;
		protected wx.TextCtrl _ado_text;
		protected wx.Button _btn_next;
		protected wx.Button _btn_last;
		protected wx.Button _btn_new;
		protected wx.Button _btn_canc;
		
		public wiwAdo(wx.Window _pc, wx.Sizer _ps) : base(wx.Orientation.wxHORIZONTAL)
		{
			int fl = wx.Alignment.wxALIGN_CENTRE_VERTICAL|wx.Direction.wxRIGHT;
			_props = new wdbAdoProps();
			_btn_first = new wx.Button(_pc, -1, "|<", wx.Button.wxDefaultPosition, new Size(20, -1));
			_btn_prev = new wx.Button(_pc, -1, "<", wx.Button.wxDefaultPosition, new Size(20, -1));
			_ado_text = new wx.TextCtrl(_pc, -1, "Ado", wx.Button.wxDefaultPosition, new Size(100, -1));
			_btn_next = new wx.Button(_pc, -1, ">", wx.Button.wxDefaultPosition, new Size(20, -1));
			_btn_last = new wx.Button(_pc, -1, ">|", wx.Button.wxDefaultPosition, new Size(20, -1));
			_btn_new = new wx.Button(_pc, -1, "+", wx.Button.wxDefaultPosition, new Size(20, -1));
			_btn_canc = new wx.Button(_pc, -1, "-", wx.Button.wxDefaultPosition, new Size(20, -1));			
			this.Add(_btn_first, 0, fl, 2);
			this.Add(_btn_prev, 0, fl, 2);			
			this.Add(_ado_text, 0, fl, 2);
			this.Add(_btn_next, 0, fl, 2);		
			this.Add(_btn_last, 0, fl, 2);		
			this.Add(_btn_new, 0, fl, 2);		
			this.Add(_btn_canc, 0, fl, 2);
			_p_container = _pc;
			_p_sizer = _ps;
			_sizer_item = null;
			_ado_cur_index++;			
			string name = "Ado" + _ado_cur_index.ToString();			
			SetDefaultProps(name);
			SetWidgetProps();
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
			get	{	return _p_container;	}
		}
		public wx.Sizer	ParentSizer		
		{	
			get	{	return	_p_sizer;	}
		}
		public bool	IsSizer		
		{	
			get	{	return true;	}
		}
		public bool	IsSelected	
		{	
			get	{	return _is_selected;	}
			set	{	_is_selected = value;	}
		}		
		public int WidgetType
		{
			get	{	return (int)StandardWidgetType.WID_BOXSIZER; }
		}		
		
		private void SetDefaultProps(string name)
		{
			_props.EnableNotification = false;	
			_props.Label = name;
			_props.Name = name;
			_props.EnableNotification = true;
			this._ado_text.Label = name;
			_p_container.AutoLayout = true;
			_p_container.Layout();			
		}
				
		public bool InsertWidget()
		{			
			_p_sizer.Add(this, 0, wx.Stretch.wxEXPAND|wx.Direction.wxALL, _props.BorderWidth);
			_sizer_item = (wx.SizerItem)_p_sizer.GetItem(_p_sizer.GetItemCount() - 1);			
			// _p_container.AutoLayout = true;
			_p_container.Layout();
			return true;
		}
		
		public bool DeleteWidget()
		{
			this.Clear(true);
			if (_p_sizer != null)
			{
				_p_sizer.Remove(this);				
			}
			return false;
		}
		
		public long FindBlockInText()
		{
			return -1;
		}
		
		public bool CanAcceptChildren()
		{
			return true;
		}				
				
		public void HighlightSelection()
		{
			Panel pan = Common.Instance().Canvas;
			// Graphics area = Graphics.FromHwnd(this.GetHandle());
			Graphics area = Graphics.FromHwnd(_p_container.GetHandle());
			Color cl = Color.FromArgb(255, _p_container.BackgroundColour.Red,
			                     _p_container.BackgroundColour.Green,
			                     _p_container.BackgroundColour.Blue);
			area.Clear(cl);
			if (IsSelected)
			{
				Pen _pen = new Pen(Color.Red, 1);						
				Point _ps = Point.Subtract(this.Position, new Size(_props.BorderWidth, _props.BorderWidth));
				Size _sz = this.Size;
				if ((_sz.Width == 0) || (_sz.Height == 0))
				{
					return;
				}
				_sz.Width += (_props.BorderWidth*2);
				_sz.Height += (_props.BorderWidth*2 + 2);
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
            		this._ado_text.Label = _props.Label;
            		break;
            	case "Proportion":
            		baa = true;
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
            	_sizer_item.Proportion = _props.Proportion;
           		_sizer_item.Border = _props.BorderWidth;
           		_sizer_item.Flag = (int)(_props.Alignment.ToLong|_props.Border.ToLong);
           		// Recalc minsize
				_p_container.AutoLayout = true;
				_p_container.Layout();  
				this.HighlightSelection();
            }
        }
				
		public void SetWidgetProps()
		{
			_props.PropertyChanged += new PropertyChangedEventHandler(winProps_PropertyChanged);
			Common.Instance().ObjPropsPanel.SelectedObject = _props;
		}		
		#endregion
	}
}
