/*
 * Creato da SharpDevelop.
 * Utente: Miki
 * Data: 09/03/2009
 * Ora: 21.17
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;
using System.IO;

namespace testxml
{
	public class PyCodeTree
	{
		protected XmlDocument _doc;
		protected string _name;
		protected XmlNodeList _classes;
		protected int curclass;
		
		public PyCodeTree()
		{
			_doc = new XmlDocument();
    		Stream str = Assembly.GetExecutingAssembly().GetManifestResourceStream("testxml.wxclass.xsd");
			_doc.Schemas.Add("wxclass", new XmlTextReader(str));			
			_name = "";
			curclass = -1;
		}
		/*		
		public StringCollection BeforeTextArray
		{
			get	{	return ToCollection(_btext.InnerText);	}
			set	{	_btext.InnerText = ToText(value);	}
		}
		
		public StringCollection AfterTextArray
		{
			get	{	return ToCollection(_atext.InnerText);	}
			set	{	_atext.InnerText = ToText(value);	}
		}		
		
		public string BeforeText
		{
			get	{	return _btext.InnerText;	}
			set	{	_btext.InnerText = value;	}
		}
		
		public string AfterText
		{
			get	{	return _atext.InnerText;	}
			set	{	_atext.InnerText = value;	}
		}
		*/
		private StringCollection ToCollection(string text)
		{
			string[] sep = {"\\n"};
			string[] strs = text.Split(sep, StringSplitOptions.None);
			StringCollection coll = new StringCollection();
			coll.AddRange(strs);
			return coll;
		}
		
		private string ToText(StringCollection coll)
		{
			string res = "";
			foreach (string s in coll)
			{
				res += s + "\\n";
			}
			return res;
		}
		
		public bool InsertBeginInFunction(string cname, string fname, string val)
		{			
			// check if exist
			if (curclass == -1) return false;
			XmlNodeList nodes = _doc.GetElementsByTagName("fname");
			XmlNode nd = null;
			foreach (XmlNode n in nodes)
			{
				if (n.ParentNode.Attributes[0].Value == cname)
				{
					nd = n;
					break;
				}
			}
			if (nd == null) return false;
			if (!nd.HasChildNodes)
			{
			} else {
				
			}
			return true;
		}
		
		public void ParseXmlFile(string filename)
		{
    		// Assembly a = System.Reflection.Assembly.GetExecutingAssembly();
    		_doc.RemoveAll();
			_doc.Load(filename);
			XmlNode node = _doc.GetElementsByTagName("pythonfile")[0];
			foreach (XmlNode n in node.ChildNodes)
			{
			}
			_classes = _doc.GetElementsByTagName("class");			
		}
		
		// ParseClass(cl, clbegin, clend, filelines);
		private int ParseClass(XmlNode classnode, int begin, int end, StringCollection coll)
		{
			StringCollection c = new StringCollection();
			string defexp = @"^[\t\s]*def\s*(?<defname>\w*)\((?<params>.*)\)";
			for (int i=begin; i<end; i++)
			{
				string item = coll[i];
				Match m = Regex.Match(item, defexp);
				if (m.Success)
				{
					// this is a function...
					// terminate <classtext> element
					if (c.Count > 0)
					{
						// Create base element <pythonfile>
						XmlElement dt = _doc.CreateElement("classtext");
						dt.InnerText = ToText(c);
						classnode.AppendChild(dt);
						c.Clear();
					}
					// Create <function> element
					XmlElement func = _doc.CreateElement("function");
					XmlAttribute funcname = _doc.CreateAttribute("Name");
					XmlAttribute funcparams = _doc.CreateAttribute("Parameters");					
					funcname.InnerText = m.Groups["defname"].Value;
					if (m.Groups["params"] != null)
						funcparams.InnerText = m.Groups["params"].Value;
					else
						funcparams.InnerText = "";
					func.Attributes.Append(funcname);
					func.Attributes.Append(funcparams);
					// find the end of this class.
					int funcbegin = i+1;
					int funcend = funcbegin;
					string str = "";
					while (funcend <= end)
					{
						if (coll[funcend].Contains("# end " + funcname.InnerText))
						{
							break;							
						}
						str += coll[funcend] + "\\n";
						funcend++;						
					}
					if (funcend - funcbegin > 0)
					{
						// text inside this function...
						// create body element...
						XmlElement body = _doc.CreateElement("body");
						body.InnerText = str;
						func.AppendChild(body);
					}
					classnode.AppendChild(func);
					i = funcend;
				} else {
					c.Add(item);
				}
			}
			if (c.Count > 0)
			{
				// Create base element <pythonfile>						
				XmlElement dt = _doc.CreateElement("classtext");
				dt.InnerText = ToText(c);
				classnode.AppendChild(dt);
				c.Clear();
			}			
			return end;
		}
		
		public void ParsePythonFile(string pyfilename)
		{
			// Try to parse a .py file and create the XML struct
			_doc.RemoveAll();
			// Declaration
			XmlDeclaration decl = _doc.CreateXmlDeclaration("1.0", "utf-8", "yes");
			_doc.AppendChild(decl);
			// Create base element <pythonfile>
			XmlElement pfile = _doc.CreateElement("pythonfile");
			
			// Read the file...
			StreamReader read = File.OpenText(pyfilename);
			if (read == null)
			{
				// exit
				return;
			}
			StringCollection filelines = new StringCollection();			
			while (!read.EndOfStream)
			{
				string item = read.ReadLine();
				filelines.Add(item);
			}
			read.Close();
			
			StringCollection c = new StringCollection();			
			string classexp = @"^[\t\s]*class\s*(?<classname>\w*)\(";
			
			for (int i=0; i<filelines.Count; i++)
			{
				string item = filelines[i];
				Match m = Regex.Match(item, classexp);
				if (m.Success)
				{
					// this is a class...
					// terminate <doctext> element
					if (c.Count > 0)
					{
						// Create base element <pythonfile>
						XmlElement dt = _doc.CreateElement("doctext");
						string str = ToText(c);
						dt.InnerText = str;
						pfile.AppendChild(dt);
						c.Clear();
					}
					// Create <class> element
					XmlElement cl = _doc.CreateElement("class");
					XmlAttribute clname = _doc.CreateAttribute("Name");
					clname.InnerText = m.Groups["classname"].Value;
					cl.Attributes.Append(clname);
					// find the end of this class.
					int clbegin = i+1;
					int clend = -1;
					for (int j=clbegin; j<filelines.Count; j++)
					{
						if (filelines[j].Contains("# end " + m.Groups["classname"].Value))
						{
							clend = j;		
							break;
						}
					}
					i = ParseClass(cl, clbegin, clend, filelines);
					// Parse sub-nodes
					pfile.AppendChild(cl);
				} else {
					c.Add(item);
				}
			}
			if (c.Count > 0)
			{
				XmlElement dt = _doc.CreateElement("doctext");
				dt.InnerText = ToText(c);
				pfile.AppendChild(dt);
				c.Clear();
			}			
			_doc.AppendChild(pfile);
			XmlTextWriter xmlWriter = new XmlTextWriter("../../at.xml", System.Text.Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;
			_doc.WriteContentTo(xmlWriter);
			xmlWriter.Flush();
			xmlWriter.Close();
		}		
		
		public void RenderTree(RichTextBox box)
		{
			string[] sep = {"\\n"};
			string[] bstrs = null;
			// bstrs = _btext.InnerText.Split(sep, StringSplitOptions.None);
			foreach (string s in bstrs)
			{
				box.AppendText(s + '\n');
			}
			// Render class
			foreach (XmlNode n in _classes)
			{
				box.AppendText("class " + n.Attributes[0].Value + "(wxFrame):\n");
				foreach (XmlNode func in n.ChildNodes)
				{
					if (func.Name == "classafterlines")
					{
						string[] ca = func.InnerText.Split(sep, StringSplitOptions.None);
						foreach (string s in ca)
						{
							box.AppendText(s + '\n');
						}
					} else if (func.Name == "function")
					{
						box.AppendText("\tdef " + func.Attributes[0].Value + ":\n");
						XmlNode text = func.ChildNodes[0];
						string[] ca = text.InnerText.Split(sep, StringSplitOptions.None);
						foreach (string s in ca)
						{
							box.AppendText("\t\t" + s + '\n');
						}						
						box.AppendText("\t# end " + func.Attributes[0].Value + "\n");
					}
				}
			}
			// bstrs = _atext.InnerText.Split(sep, StringSplitOptions.None);
			foreach (string s in bstrs)
			{
				box.AppendText(s + '\n');
			}
		}
	}
}
