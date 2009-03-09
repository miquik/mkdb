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
		protected XmlNode _btext;
		protected XmlNode _atext;
		protected XmlNodeList _classes;
		protected int curclass;
		
		public PyCodeTree()
		{
			_doc = new XmlDocument();
			_name = "";
			curclass = -1;
		}
		
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
				res += s + '\n';
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
		
		public void ParseFile(string filename)
		{
    		// Assembly a = System.Reflection.Assembly.GetExecutingAssembly();
    		Stream str = Assembly.GetExecutingAssembly().GetManifestResourceStream("testxml.wxclass.xsd");
			_doc.Schemas.Add("wxclass", new XmlTextReader(str));
			_doc.Load(filename);
			XmlNode node = _doc.GetElementsByTagName("pythonfile")[0];
			foreach (XmlNode n in node.ChildNodes)
			{
				if (n.Name == "beforelines")
				{
					_btext = n;
				}
				if (n.Name == "afterlines")
				{
					_atext = n;
				}
			}
			_classes = _doc.GetElementsByTagName("class");			
		}
		
		public void RenderTree(RichTextBox box)
		{
			string[] sep = {"\\n"};
			string[] bstrs;
			bstrs = _btext.InnerText.Split(sep, StringSplitOptions.None);
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
			bstrs = _atext.InnerText.Split(sep, StringSplitOptions.None);
			foreach (string s in bstrs)
			{
				box.AppendText(s + '\n');
			}
		}
	}
	
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
			PyCodeTree tree = new PyCodeTree();
			tree.ParseFile("../../wxclass.xml");
			tree.RenderTree(richTextBox1);
		}
	}
}
