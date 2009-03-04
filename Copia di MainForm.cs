/*
 * Creato da SharpDevelop.
 * Utente: Family Rose
 * Data: 02/03/2009
 * Ora: 20.56
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Collections.Specialized;

namespace testtext
{
	public class PyParser
	{
		private StreamReader _t;
		private PySection _main;
		
		public PyParser()
		{			
		}
		
		public void Render(StringCollection box)
		{
			RenderSection(box, _main);
		}
		
		private void InsertToPos(StringCollection coll, int pos, string str)
		{
			if (pos >= coll.Count)
			{
				coll.Add(str);
			} else {
				coll.Insert(pos, str);
			}
		}
		
		public void RenderSection(StringCollection box, PySection section)
		{
			if (section.Name != "")
			{
				InsertToPos(box, section.Begin, "# --- " + section.Name + " : begin ---\n");
			}
			if (section.Children.Count == 0)
			{
				int cnt = 0;
				foreach (string it in section.Lines)
				{
					InsertToPos(box, section.Begin + cnt, it + "\n");					
					cnt++;
				}
			} else {
				int cnt = 0;
				foreach (string it in section.Lines)
				{
					InsertToPos(box, section.Begin + cnt, it + "\n");					
					cnt++;
				}
				foreach (PySection sec in section.Children)
				{
					RenderSection(box, sec);
				}
			}
			if (section.Name != "")
			{
				InsertToPos(box, section.Begin, "# --- " + section.Name + " : end ---\n");
			}
		}
		
		public void ParseFile(string filename)
		{
			_t = File.OpenText(filename);
			StringCollection coll = new StringCollection();
			_main = new PySection("");
			while (!_t.EndOfStream)
			{
				coll.Add(_t.ReadLine());
			}
			ParseFileSection(coll, _main, 0);
		}
		
		public int ParseFileSection(StringCollection sec, PySection section, int row)
		{
			// sec collection, doesn't contain # --- begin / # --- end
			int count = row;
			bool insection = false;
			StringCollection tempc = new StringCollection();
			foreach (string item in sec)
			{
				if (insection == true)
				{
					if (item.Contains("# ---") && item.Contains("end"))
					{
						insection = false;
						string nn = item.Substring(6, item.IndexOf(':') - 6);
						section.Children.Add(new PySection(nn.Trim()));
						PySection child = (PySection)section.Children[section.Children.Count - 1];
						child.Begin = count;						
						int cnt = ParseFileSection(tempc, child, count);
						child.Size = cnt - child.Begin;
					} else {
						string debugstr = sec[count];
						tempc.Add(debugstr);						
					}
				}				
				else if (item.Contains("# ---") && item.Contains("begin"))
				{
					// begin a new section
					insection = true;
					tempc.Clear();
				}
				else if (item.Contains("# ---") && item.Contains("end"))
				{
				    // something to do here?
				    insection = false;
				}
				else
				{
					section.Lines.Add(item);
				}
				count++;
			}
			return row;
		}
	}	
	
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
			StringCollection c = new StringCollection();
			PyParser parser = new PyParser();
			parser.ParseFile("../../app_struct.py");
			parser.Render(c);
			foreach (string item in c)
			{
				richTextBox1.AppendText(item);
				// i++;
			}
			/*
			PyFileEditor ed = new PyFileEditor();
			ed.InsertSingleLineToSection(-1, PyFileSection.PY_EVENTDECL_SECTION, "Ciao\n");
			ed.InsertAfterSection(PyFileSection.PY_EVENTIMPL_SECTION, "Ecco dopo\n");
			ed.InsertAfterSection(PyFileSection.PY_EVENTIMPL_SECTION, "Ecco dopo\n");
			ed.InsertBeforeSection(PyFileSection.PY_EVENTIMPL_SECTION, "Ecco prima!\n");
			ed.InsertBeforeSection(PyFileSection.PY_EVENTIMPL_SECTION, "Ecco prima!\n");
			string[] s = new string[ed.PyStream.Count];
			int i=0;
			foreach (string item in ed.PyStream)
			{
				s[i++] = item;
			}
			for (i=0; i<8; i++)
			{
				PySection sec = ed._py_sections[i];
				string str = s[sec.Begin];
				str = str.Substring(0, str.Length - 1);
				s[sec.Begin] = str + " : " + sec.Begin.ToString() + "\n";
			}
			i=0;
			foreach (string item in s)
			{
				richTextBox1.AppendText(i.ToString() + " :: " + item);
				i++;
			}
			*/
		}
	}
}
