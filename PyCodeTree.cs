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
using System.Text.RegularExpressions;

namespace pyprova
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
    		Stream str = Assembly.GetExecutingAssembly().GetManifestResourceStream("pyprova.wxclass.xsd");
			_doc.Schemas.Add("wxclass", new XmlTextReader(str));			
			_name = "";
			curclass = -1;
		}
		
		public static StringCollection ToStringCollection(string text)
		{
			string[] sep = {"\\n"};
			string[] strs = text.Split(sep, StringSplitOptions.None);
			StringCollection coll = new StringCollection();
			coll.AddRange(strs);
			return coll;
		}
		
		public static string ToText(StringCollection coll)
		{
			string res = "";
			foreach (string s in coll)
			{
				res += s + "\\n";
			}
			return res;
		}
		
		public StringCollection GetFunctionBody(string classname, string funcname)
		{			
			XmlElement root = _doc.DocumentElement;
			XmlNodeList nodes = root.SelectNodes("/pythonfile/class[@Name='" + classname + "']" +
			                                     "/function[@Name='" + funcname + "']/body");
			StringCollection c = null;
			if (nodes.Count > 0)
			{
				c = new StringCollection();
				foreach (XmlNode n in nodes)
				{
					c.Add(n.InnerText);
				}
			}
			return c;
		}
		
		public bool CreateNewFunction(string classname, string funcname, string parameters)
		{
			XmlElement root = _doc.DocumentElement;
			XmlNode node = root.SelectSingleNode("/pythonfile/class[@Name='" + classname + "']");
			if (node == null)
				return false;
			
			XmlElement func = _doc.CreateElement("function");
			XmlAttribute funcname = _doc.CreateAttribute("Name");
			XmlAttribute funcparams = _doc.CreateAttribute("Parameters");					
			funcname.InnerText = funcname;
			if (parameters != "")
				funcparams.InnerText = parameters;
			else
				funcparams.InnerText = "";
			func.Attributes.Append(funcname);
			func.Attributes.Append(funcparams);
			func.InnerText = "";
			node.AppendChild(func);
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
					while (funcend <= end)
					{
						if (coll[funcend].Contains("# end " + funcname.InnerText))
						{
							break;							
						}
						// str += coll[funcend] + "\\n";
						XmlElement body = _doc.CreateElement("body");	
						if (coll[funcend] == "")
							body.InnerText = "\\n";
						else
							body.InnerText = coll[funcend].Trim();
						func.AppendChild(body);						
						funcend++;						
					}
					classnode.AppendChild(func);
					i = funcend;
				} else {
					// Create base element <pythonfile>
					XmlElement dt = _doc.CreateElement("classtext");
					if (item == "")
						dt.InnerText = "\\n";
					else
						dt.InnerText = item.Trim();
					classnode.AppendChild(dt);
				}
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
			
			string classexp = @"^[\t\s]*class\s*(?<classname>\w*)\(";
			
			for (int i=0; i<filelines.Count; i++)
			{
				string item = filelines[i];
				Match m = Regex.Match(item, classexp);
				if (m.Success)
				{
					// this is a class...
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
					XmlElement dt = _doc.CreateElement("doctext");
					if (item == "")
						dt.InnerText = "\\n";
					else
						dt.InnerText = item.Trim();
					pfile.AppendChild(dt);
				}
			}
			_doc.AppendChild(pfile);
			XmlTextWriter xmlWriter = new XmlTextWriter("../../at.xml", System.Text.Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;
			_doc.WriteContentTo(xmlWriter);
			xmlWriter.Flush();
			xmlWriter.Close();
		}		
	}
}
