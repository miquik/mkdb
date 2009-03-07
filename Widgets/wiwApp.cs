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
using System.Text.RegularExpressions;


namespace mkdb.Widgets
{
	public class wiwApp : wx.Window, IWDBBase
	{
		protected wdbAppProps _props;
		protected bool _is_selected;
		protected Python.PySection _section;
			
		public wiwApp(wx.Window _pc, wx.Sizer _ps) : base(null)
		{
			_props = new wdbAppProps();
			SetDefaultProps("Project");
			SetWidgetProps();
		}
		
		public Python.PySection AppSection
		{
			get	{	return _section;	}
			set	{	_section = value;	}
		}
		
		#region IWidgetElem Interface implementation
		public wx.SizerItem SizerItem
		{
			get	{	return null;	}
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
		public int WidgetType
		{
			get	{	return (int)StandardWidgetType.WID_APP; }
		}
		#endregion
		
		
		private void SetDefaultProps(string name)
		{
			_props.EnableNotification = false;
			_props.Name = name;
			_props.EnableNotification = true;
		}
				
		public bool InsertWidget()
		{
			InsertWidgetInText();
			return true;
		}
		public bool DeleteWidget()
		{
			return false;
		}		
		
		public bool InsertWidgetInText()
		{
			Python.PyFileEditor ed = Common.Instance().PyEditor;
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
		
		public void HighlightSelection()
		{			
		}
		
		private void SetPythonText(string appname)
		{
			string regexp1;
			if (_section == null)
				return;
			// Looking for : class MyApp(wxApp):
			Python.PySection appbody = _section.FindChildByName("App");
			if (appbody == null)
				return;
			regexp1 = @"^(?<space>[\t\s]*)class\s*(?<appreg>\w*)\(";
			appbody.RegexFindAndReplace(regexp1, appname, "appreg");			
			// Looking for : app = MyApp(0)
			Python.PySection apprun = _section.FindChildByName("Run");
			if (apprun == null) 
				return;
			regexp1 = @"^(?<space>[\t\s]*)app\s*=\s*(?<appreg>\w*)\(";
			apprun.RegexFindAndReplace(regexp1, appname, "appreg");
		}
								
		public void winProps_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
            	case "Name":
            		this.Name = _props.Name;
            		_section.Name = _props.Name;
					Common.Instance().ObjTree.SelectedNode.Text = _props.Name;		
            		break;
            	case "AppName":
            		SetPythonText(_props.AppName);
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
	
/*
			// class MyApp(wxApp):
			string regexp1 = @"^(?<space>[\t\s]*)class\s*(?<appreg>\w*)\(";
			string regexp2 = @"${space}class " + appname + "(";
			Regex r = new Regex(regexp1);
			for (int i=0; i<appbody.Lines.Count; i++)
			{				
				Match m = r.Match(appbody.Lines[i]);
				if (m.Success)
				{
					// Change name
					string temp = appbody.Lines[i].Replace(m.Groups[1].Value, appname);
					appbody.Lines[i] = temp;
					break;
				}
			}
			Python.PySection apprun = _section.FindChildByName("Run");
			// app = MyApp(0)
			regexp1 = @"^(?<space>[\t\s]*)app\s*=\s*(?<appreg>\w*)\(";
			regexp2 = @"${space}app = " + appname + "(";
			r = new Regex(regexp1);
			for (int i=0; i<appbody.Lines.Count; i++)
			{				
				Match m = r.Match(apprun.Lines[i]);
				if (m.Success)
				{
					// Change name
					string temp = apprun.Lines[i].Replace(m.Groups[1].Value, appname);
					apprun.Lines[i] = temp;
					break;
				}
			}
*/	
}
