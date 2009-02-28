/*
 * Creato da SharpDevelop.
 * Utente: Family Rose
 * Data: 26/12/2008
 * Ora: 12.01
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Xml;


namespace mkdb.Properties
{	
	public class WidgetProps
	{
		protected bool _enable_not;
		protected string _name;
		protected PropertyBag _bag;
		
		public WidgetProps()
		{
			_enable_not = true;
			_name = "";
			_bag = new PropertyBag();
		}	
		
		public bool EnableNotification
		{
			get	{	return	_enable_not;	}
			set	{	_enable_not = value;	}
		}			
		
		public string Name
		{
			get	{	return	_name;	}
			set	{	_name = value;	}
		}		
		
		public PropertyBag Properties
		{
			get	{	return _bag;	}
		}
	}
	
	public class WidgetPropsManager
	{
		protected XmlReaderSettings xsd_settings;
		protected XmlReader r;
		protected ArrayList _wplist;
		
		public WidgetPropsManager()
		{
			_wplist = new ArrayList();
			xsd_settings = null;
			r = null;			
			ParseXml("../../Properties/wxalignment.xml");
			ParseXml("../../Properties/wxwindows.xml");
		}
		
		public WidgetProps GetCatalogByName(string name)
		{
			foreach (WidgetProps item in _wplist)
			{
				if (item.Name == name)
					return item;
			}
			return null;
		}
		
		public int AddCatalog(WidgetProps item)
		{
			return _wplist.Add(item);
		}
		
		public void RemoveCatalog(WidgetProps item)
		{
			_wplist.Remove(item);
		}
		
		public void RemoveCatalogByName(string name)
		{
			foreach (WidgetProps item in _wplist)
			{
				if (item.Name == name)
				{
					_wplist.Remove(item);
					return;
				}
			}
		}
				
		public WidgetProps ParseXml(string xmlfilename)
		{
			WidgetProps props = null;
			string elemType = "";
			string elemName = "";
    		object elemValue = null;
			string elemCategory = "";
    		string elemDesc = "";
			bool elemBrowsable = true;
			bool elemReadOnly = false;
			string elemEditor = "";
    		string elemTypeConv = "";
			
			// Parse XML file
    		System.Reflection.Assembly a = System.Reflection.Assembly.GetExecutingAssembly();
    		System.IO.Stream str = a.GetManifestResourceStream("mkdb.Properties.wxprops.xsd");
			xsd_settings = new XmlReaderSettings();
			xsd_settings.ValidationType = ValidationType.Schema;
			xsd_settings.Schemas.Add("wxprops", new XmlTextReader(str));
			// xsd_settings.Schemas.Add("props", xsdfilename);
			
			using (r = XmlReader.Create(xmlfilename, xsd_settings))
  			while (r.Read())
  			{
    			switch (r.NodeType)
    			{
      				case XmlNodeType.Element:
    					if (r.Name == "Catalog")
    					{
    						// Set Name
    						if (props == null)
    						{
    							props = new WidgetProps();
    						}
    						props.Name = r["Name"];
    						// this.name = r["Name"];
    						// this.basecontainername = r["Base"];
    					}
    					if (r.Name == "Category") 
    					{
    						elemCategory = GetElemString(r);
    					}
    					if (r.Name == "Property")
    					{
    						// Parse the data...
							elemType = "";
    						elemName = r["Name"];
    						elemValue = null;
    						elemDesc = "";
    						elemBrowsable = true;
    						elemReadOnly = false;
    						elemEditor = "";
    						elemTypeConv = "";
    					}
    					if (r.Name == "Type") elemType = GetElemString(r);
    					if (r.Name == "Description") elemDesc = GetElemString(r);
    					if (r.Name == "Editor") elemEditor = GetElemString(r);
    					if (r.Name == "TypeConverter") elemTypeConv = GetElemString(r);
    					if (r.Name == "Value") GetElemValue(r, elemType, out elemValue);
    					break;
      				case XmlNodeType.EndElement:
    					if (r.Name == "Property")
    					{
    						// We should have all the info...
    						// build this GenericProperty
    						int attCount = 2;
    						if (elemEditor != "") attCount++;
    						if (elemTypeConv != "") attCount++;
    						Attribute[] attr = new Attribute[attCount];
    						attr[0] = new BrowsableAttribute(elemBrowsable);
    						attr[1] = new ReadOnlyAttribute(elemReadOnly);
    						attCount = 2;
    						if (elemEditor != "")
    						{
    							attr[attCount++] = new EditorAttribute(elemEditor, "UITypeEditor");
    						}
    						if (elemTypeConv != "")
    						{
    							attr[attCount++] = new TypeConverterAttribute(elemTypeConv);
    						}
    						if (elemValue == null)
    						{
    							GetElemDefault(r, elemType, elemValue);
    						}    							
    						props.Properties.AddProperty(new GenericProperty(elemName, elemValue, elemCategory, 
    						                                                 elemDesc, attr));    					
    					}
    					if (r.Name == "Category")
    					{
    						elemCategory = "";
    					}
        				break;

      				case XmlNodeType.Text:
      				case XmlNodeType.CDATA:
      				case XmlNodeType.Comment:
      				case XmlNodeType.XmlDeclaration:
        				break;

      				case XmlNodeType.DocumentType:
        				break;

      				default: break;
    			}    			
  			}			
			_wplist.Add(props);
			return props;
		}
		
		void GetElemDefault(XmlReader r, string type, out object elem)
		{
			switch (type)
			{
				case "int" :	elem =  (int)0;
				case "uint" :	elem =  (uint)0;
				case "float" :	elem =  0.0f;
				case "double" :	elem =  0.0;
				case "Size" : elem = new Size(-1, -1);
				case "Point" : elem = new Point(-1, -1);
				case "wxColor" : elem = new wxColor(0, 0 ,0);
				case "wxFont" : elem = new wxFont("Arial", 8);
				case "wxFlags" : elem = new wxFlags();
				case "bool": elem = true;
			}
		}
		
		void  GetElemValue(XmlReader r, string type, out object elem)
		{
			char[] div = {','};			
			string val = r.ReadElementContentAsString();			
			switch (type)
			{
				case "int" :	
					int vint = Convert.ToInt32(val);
					elem = new System.Int32(vint);
					break;
				case "uint" :	
					int vuint = Convert.ToUInt32(val);
					elem = new System.UInt32(vint);
					break;
				case "float" :
					float v = Convert.ToSingle(val);
					elem = new System.Single(v);
					break;
				case "double" :
					double v = Convert.ToDouble(val);
					elem = new System.Double(v);
					break;
				case "Size" :
					string[] comp = val.Split(div);
					elem = new Size(Convert.ToInt32(comp[0].Trim()), Convert.ToInt32(comp[1].Trim()));
					break;
				case "Point" :
					string[] comp = val.Split(div);
					elem = new Point(Convert.ToInt32(comp[0].Trim()), Convert.ToInt32(comp[1].Trim()));
					break;
				case "wxColor" :
					string[] comp = val.Split(div);
					elem = new wxColor(Convert.ToByte(comp[0].Trim()), Convert.ToByte(comp[1].Trim()), Convert.ToByte(comp[2].Trim()));
					break;
				case "wxFont" :
					string[] comp = val.Split(div);
					elem = new wxFont(comp[0].Trim(), Convert.ToInt32(comp[1].Trim()), Convert.ToByte(comp[2].Trim()));
					break;
				case "wxFlags" :
					if (elem == null) elem = new wxFlags();
					wxFlags fl = (wxFlags)elem;
					string[] comp = val.Split(div);
					fl.AddItem(comp[0].Trim(), Convert.ToInt32(comp[1].Trim()), Convert.ToBoolean(comp[2].Trim()));
					break;
				case "bool":
					bool v = Convert.ToBoolean(val);
					elem = new System.Boolean(v);
					break;					
			}
			return elem;
		}		
		
		string GetElemString(XmlReader r)
		{
			return r.ReadElementContentAsString();
		}		
		
		bool GetElemFlag(XmlReader r)
		{
			return r.ReadElementContentAsBoolean();
		}				
	}

	/*
	/// <summary>
	/// Alignment and borders (Ref. to Parent).
	/// </summary>
	public class wxAlignProps : WidgetProps
	{
		private int _proportion;
		private int _border;
		private wxFlags _aflag;
		private wxFlags _bflag;
		
		public wxAlignProps()
		{
			_border = 10;
			_bflag = new wxFlags();
			_bflag.AddItem("wxLEFT", wx.Direction.wxLEFT, false);
			_bflag.AddItem("wxRIGHT", wx.Direction.wxRIGHT, false);
			_bflag.AddItem("wxTOP", wx.Direction.wxTOP, false);
			_bflag.AddItem("wxBOTTOM", wx.Direction.wxBOTTOM, false);
			_bflag.AddItem("wxALL", wx.Direction.wxALL, true);			
			_aflag = new wxFlags();
			_aflag.AddItem("wxALIGN_LEFT", wx.Alignment.wxALIGN_LEFT, false);
			_aflag.AddItem("wxALIGN_RIGHT", wx.Alignment.wxALIGN_RIGHT, false);
			_aflag.AddItem("wxALIGN_TOP", wx.Alignment.wxALIGN_TOP, false);
			_aflag.AddItem("wxALIGN_BOTTOM", wx.Alignment.wxALIGN_BOTTOM, false);
			_aflag.AddItem("wxALIGN_CENTER", wx.Alignment.wxALIGN_CENTER, false);
			_aflag.AddItem("wxALIGN_CENTER_HORIZONTAL", wx.Alignment.wxALIGN_CENTER_HORIZONTAL, false);
			_aflag.AddItem("wxALIGN_CENTER_VERTICAL", wx.Alignment.wxALIGN_CENTER_VERTICAL, false);
			_aflag.AddItem("wxEXPAND", wx.Stretch.wxGROW, true);
			_aflag.AddItem("wxSHAPED", wx.Stretch.wxSHAPED, false);
			_aflag.AddItem("wxFIXED_MINSIZE", wx.Stretch.wxFIXED_MINSIZE, false);
		}
		
		[CategoryAttribute("Alignment & Border"), DescriptionAttribute("Alignment and Border flags")]
		public int Proportion
		{
			get	{	return _proportion;		}
			set	{	_proportion = value; NotifyPropertyChanged("Proportion");}
		}		
		
		[CategoryAttribute("Alignment & Border"), DescriptionAttribute("Alignment and Border flags")]
		public int BorderWidth
		{
			get	{	return _border;		}
			set	{	_border = value; NotifyPropertyChanged("BorderWidth");}
		}
				
		[CategoryAttribute("Alignment & Border"), DescriptionAttribute("Alignment and Border flags")]
		[TypeConverter(typeof(wxFlagsTypeConverter))]
        [Editor(typeof(wxFlagsEditor), typeof(UITypeEditor))]
		public wxFlags Border
		{
			get	{	return _bflag;		}
			set	{	_bflag = value;	NotifyPropertyChanged("Border");	}
		}		
		
		[CategoryAttribute("Alignment & Border"), DescriptionAttribute("Alignment and Border flags")]
		[TypeConverter(typeof(wxFlagsTypeConverter))]
        [Editor(typeof(wxFlagsEditor), typeof(UITypeEditor))]
		public wxFlags Alignment
		{
			get	{	return _aflag;		}
			set	{	_aflag = value;	NotifyPropertyChanged("Alignment");	}
		}		
	}
	
	
	public class wxWindowProps : wxAlignProps
	{
		protected int _id;
		protected Point _pos;
		protected Size _size;
		protected wxFont _font;
		protected wxColor _fc;
		protected wxColor _bc;
		protected bool _enabled;
		protected bool _hidden;
		protected wxFlags _wstyle;
		
		public wxWindowProps()
		{
			_id = -1;
			_name = "";
			_fc = new wxColor(0, 0, 0);	
			_bc = new wxColor(100, 100, 100);	
			_font = new wxFont("Arial", 8);
			_wstyle = new wxFlags();
			_wstyle.AddItem("wxCLIP_CHILDREN", wx.Window.wxCLIP_CHILDREN, false);
			_wstyle.AddItem("wxNO_BORDER", wx.Window.wxNO_BORDER, false);			
			_wstyle.AddItem("wxRAISED_BORDER", wx.Window.wxRAISED_BORDER, false);			
			_wstyle.AddItem("wxSIMPLE_BORDER", wx.Window.wxSIMPLE_BORDER, false);			
			_wstyle.AddItem("wxSTATIC_BORDER", wx.Window.wxSTATIC_BORDER, false);			
			_wstyle.AddItem("wxSUNKEN_BORDER", wx.Window.wxSUNKEN_BORDER, false);			
			_wstyle.AddItem("wxDOUBLE_BORDER", wx.Window.wxDOUBLE_BORDER, false);						
			_wstyle.AddItem("wxHSCROLL", wx.Window.wxHSCROLL, false);						
			_wstyle.AddItem("wxVSCROLL", wx.Window.wxVSCROLL, false);						
			_wstyle.AddItem("wxTAB_TRAVERSAL", wx.Window.wxTAB_TRAVERSAL, true);						
			_wstyle.AddItem("wxWANTS_CHARS", wx.Window.wxWANTS_CHARS, false);						
		}
		
		[CategoryAttribute("wxWindows"), DescriptionAttribute("wxWindows properties")]
		public int ID		
		{
			get	{	return _id;	}
			set	{	_id = value; NotifyPropertyChanged("ID"); }
		}		
		[CategoryAttribute("wxWindows"), DescriptionAttribute("wxWindows properties")]
		[Editor(typeof(wxColorEditors), typeof(UITypeEditor))]
		public wxColor FC
		{
			get	{	return _fc;	}
			set	{	_fc = value; NotifyPropertyChanged("FC");	}
		}
		[CategoryAttribute("wxWindows"), DescriptionAttribute("wxWindows properties")]
		[Editor(typeof(wxColorEditors), typeof(UITypeEditor))]
		public wxColor BC
		{
			get	{	return _bc;	}
			set	{	_bc = value; NotifyPropertyChanged("BC");	}
		}		
		[CategoryAttribute("wxWindows"), DescriptionAttribute("wxWindows properties")]
		[Editor(typeof(wxFontEditors), typeof(UITypeEditor))]
		public wxFont Font
		{
			get	{	return _font;	}
			set	{	_font = value; NotifyPropertyChanged("Font");	}
		}
		[CategoryAttribute("wxWindows"), DescriptionAttribute("wxWindows properties")]
		public Point Pos
		{
			get	{	return _pos;	}
			set	{	_pos = value; NotifyPropertyChanged("Pos");	}
		}
		[CategoryAttribute("wxWindows"), DescriptionAttribute("wxWindows properties")]
		public Size Size
		{
			get	{	return _size;	}
			set	{	_size = value; NotifyPropertyChanged("Size");	}
		}
		[CategoryAttribute("wxWindows"), DescriptionAttribute("wxWindows properties")]
		public bool Enabled
		{
			get	{	return _enabled;	}
			set	{	_enabled = value; NotifyPropertyChanged("Enabled");	}
		}		
		[CategoryAttribute("wxWindows"), DescriptionAttribute("wxWindows properties")]
		public bool Hidden
		{
			get	{	return _hidden;	}
			set	{	_hidden = value; NotifyPropertyChanged("Hidden");	}
		}
		[CategoryAttribute("wxWindows"), DescriptionAttribute("wxWindows properties")]
		[TypeConverter(typeof(wxFlagsTypeConverter))]
		[Editor(typeof(wxFlagsEditor), typeof(UITypeEditor))]
		public wxFlags WindowStyle
		{
			get	{	return _wstyle;	}
			set	{	_wstyle = value; NotifyPropertyChanged("WindowStyle");	}
		}		
	}
	*/	
}
