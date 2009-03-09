/*
 * Creato da SharpDevelop.
 * Utente: michele
 * Data: 09/03/2009
 * Ora: 15.00
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;

namespace pyprova
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
    		System.Reflection.Assembly a = System.Reflection.Assembly.GetExecutingAssembly();
    		System.IO.Stream str = a.GetManifestResourceStream("pyprova.wxclass.xsd");
			// xsd_settings = new XmlReaderSettings();
			// xsd_settings.ValidationType = ValidationType.Schema;
			// xsd_settings.Schemas.Add("wxprops", new XmlTextReader(str));
			XmlDocument doc = new XmlDocument();
			doc.Schemas.Add("wxprops", new XmlTextReader(str));
			doc.Load("../../template_app.xml");
/*			
<pythonfile xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <beforelines>#!/usr/bin/env python</beforelines>
  <class Name="MyApp">
    <function Name="OnInit(self)">
      <line>return true</line>
    </function>
    <classafterlines></classafterlines>
  </class>
  <afterlines>app = MyApp(0)</afterlines>
</pythonfile>			
*/			
			XmlNode file = null;
			foreach (XmlNode node in doc.ChildNodes)
			{				
				if (node.Name == "pythonfile")
				{
					file = node;
					break;
				}
			}
			
			foreach (XmlNode node in file.ChildNodes)
			{				
				if (node.Name == "beforelines")
				{
					string[] sep = {"\\n"};
					string[] strs = node.InnerText.Split(sep, StringSplitOptions.None);
					foreach (string s in strs)
					{
						richTextBox1.AppendText(s + '\n');
					}
				}
				if (node.Name == "afterlines")
				{
					string[] sep = {"\\n"};
					string[] strs = node.InnerText.Split(sep,StringSplitOptions.None);
					foreach (string s in strs)
					{
						richTextBox1.AppendText(s + '\n');
					}
				}
			}
			
		}
	}
}
