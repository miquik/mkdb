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
using System.Drawing;


namespace mkdb.Widgets
{
	public class wdbAppProps : wxAlignProps
	{
		// * wxFrame props : name, title, style, wxWindow, toParent
		protected string _appname;
				
		public wdbAppProps() : base()
		{
			_appname = "";
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
	public class wiwApp : wx.Window, IWDBBase
	{
		protected wdbAppProps _props;
		protected bool _is_selected;
			
		public wiwApp(wx.Window _pc, wx.Sizer _ps) : base(_pc)
		{
			_props = new wdbAppProps();
			SetDefaultProps("Project");
			SetWidgetProps();
		}
		
		#region IWidgetElem Interface implementation
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
			get	{	return	null;	}
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
		public Point AreaOrigin
		{	
			get {	return new Point(-1, -1);	}
		}
		public Size AreaSize
		{	
			get	{	return new Size(-1, -1);	}
		}
		public int WidgetType
		{
			get	{	return (int)StandardWidgetType.WID_APP; }
		}
		#endregion
		
		
		private void SetDefaultProps(string name)
		{
			_props.EnableNotification = false;
			_props.AppName = name;
			// Common.Instance().ObjTree.SelectedNode.Text = name;
			_props.EnableNotification = true;
		}
				
		public bool InsertWidget(IWDBBase parent)
		{
			// _client_parent = parent;
			return true;
		}
		public bool DeleteWidget()
		{
			return false;
		}		
		
		public bool InsertWidgetInText()
		{
			return true;			
		}
		
		public bool DeleteWidgetFromText()
		{
			return true;
		}
		
		public long FindBlockInText()
		{
			return -1;
		}
		
		public bool CanAcceptChildren()
		{
			return false;
		}
		
		public void PaintOnSelection()
		{			
		}
								
		public void winProps_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
            	case "AppName":
            		this.Name = _props.AppName;
					Common.Instance().ObjTree.SelectedNode.Text = _props.AppName;		
            		break;
            }
            this.UpdateWindowUI();            
        }
				
		public void SetWidgetProps()
		{
			_props.PropertyChanged += new PropertyChangedEventHandler(winProps_PropertyChanged);
			Common.Instance().ObjPropsPanel.SelectedObject = _props;
		}		
				
	}
}
