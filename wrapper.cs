/*
 * Creato da SharpDevelop.
 * Utente: Miki
 * Data: 26/02/2009
 * Ora: 20.44
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PropertyUserInterface;
using System.Xml;
using System.Xml.Schema;

namespace testproperties
{
	public class PropsContainer
	{
		protected XmlReaderSettings xsd_settings;
		protected XmlReader r;
		protected string name;
		protected string basecontainername;
		protected GenericPropertyCollection_CustomTypeDescriptor properties;
		
		public PropsContainer()
		{
			xsd_settings = null;
			r = null;
			name = "";
			basecontainername = "";
			properties = null; // new GenericPropertyCollection_CustomTypeDescriptor();			
		}
		
		public GenericPropertyCollection_CustomTypeDescriptor Properties
		{
			get	{	return properties;	}
		}
		
		public string Name
		{
			get	{	return name;	}
		}
		
		public bool ParseXml(string xmlfilename, string xsdfilename)
		{
    		object elemValue;
			string elemCategory;
    		string elemDesc;
			bool elemBrowsable;
			bool elemReadOnly;
			string elemEditor;
    		string elemTypeConv;
/*    						
    <Type>string</Type>
    <Category>string</Category>
    <Description>string</Description>
    <InitialValue>string</InitialValue>
    <Browsable>0</Browsable>
    <ReadOnly>1</ReadOnly>
    <Editor>string</Editor>
    <TypeConverter>string</TypeConverter>
*/    						
			
			// Parse XML file
			xsd_settings = new XmlReaderSettings();
			xsd_settings.ValidationType = ValidationType.Schema;
			xsd_settings.Schemas.Add("props", xsdfilename);
			
			using (r = XmlReader.Create(xmlfilename, xsd_settings))
  			while (r.Read())
  			{
    			switch (r.NodeType)
    			{
      				case XmlNodeType.Element:
    					if (r.Name == "Container")
    					{
    						// Set Name
    						this.name = r["Name"];
    						this.basecontainername = r["BaseContainer"];
    					}
    					if (r.Name == "Properties")
    					{
    						// Parse the data...
    						elemValue = null;
    						elemCategory = "";
    						elemDesc = "";
    						elemBrowsable = true;
    						elemReadOnly = false;
    						elemEditor = "";
    						elemTypeConv = "";
    					}
    					if (r.Name == "Type") elemValue = GetElemValue(r);
    					break;
      				case XmlNodeType.EndElement:
    					if (r.Name == "Properties")
    					{
    						// We should have all the info...
    						// build this GenericProperty
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
			return true;
		}
		
		object GetElemValue(XmlReader r)
		{
			object val = null;
			string typestr = r.ReadElementContentAsString();
			Type type = Type.GetType("System.Drawing." + typestr);
			if (type != null)
			{
				// System.String mk = new String('s', 1);
				val = Activator.CreateInstance(type);
			}
			return val;
		}
	}
	
	public partial class MainForm : Form
	{
		protected PropsContainer props;
		
		public MainForm()
		{
			InitializeComponent();
			props = new PropsContainer();
			props.ParseXml("../../wxprops.xml", "../../wxprops.xsd");
		}
	}
	
	/*
			// creating various type properties: 
			properties = new GenericPropertyCollection_CustomTypeDescriptor();
			properties.AddProperty( new GenericProperty( "Integer", (int) 1, "Custom", "Int32" ) );
			properties.AddProperty( new GenericProperty( "Float", 2.3f, "Custom", "Single" ) );
			properties.AddProperty( new GenericProperty( "Float disabled", 4.5f, "Custom", "Single", new ReadOnlyAttributeEditor() ) );
			properties.AddProperty( new GenericProperty( "String", "text", "Custom", "String" ) );

			// adding a .net class to show that reflection kicks in, but we can't change the property: 
			properties.AddProperty( new GenericProperty( "Class", new Class(), ".Net", "Class" ) );

			// adding a property that acts like a expandable .net class: 
			GenericPropertyCollection_CustomTypeDescriptor structure = new GenericPropertyCollection_CustomTypeDescriptor();
				structure.AddProperty( new GenericProperty( "String", "text inside", "Custom", "String" ) );
			properties.AddProperty( new GenericProperty( "struct", structure, "Custom", "expandable" ) );

			// PropertyGrid, please display my properties: 
			propertyGrid1.SelectedObject = properties;
*/	
}
