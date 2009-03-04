/*
 * Creato da SharpDevelop.
 * Utente: michele
 * Data: 02/03/2009
 * Ora: 11.17
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace textops
{		
	public class PyParser
	{
		private StreamReader _t;
		
		public PyParser()
		{			
		}
		
		public void ParseFile(string filename)
		{
			_t = File.OpenText(filename);
			StringCollection coll = new StringCollection();
			PySection sec = new PySection();
			while (!_t.EndOfStream)
			{
				coll.Add(_t.ReadLine());
			}
			ParseFileSection(coll, sec, 0);
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
						section.Children.Add(new PySection());
						PySection child = (PySection)section.Children[parent.Children.Count - 1];
						child.Begin = count;						
						int cnt = ParseFileSection(tempc, child, count);	
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
	
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
			PyParser parser = new PyParser();
			parser.ParseFile("../../app_struct.py");
			/*
			Match myMatch = Regex.Match(item, "class");
			if (!myMatch.Success)
			{
			}
			*/
		}
	}
	
	/*
	public class PyParser
	{
		StringCollection _keywords;
		string _compiled_keywords;
		StreamReader _t;
		
		public PyParser()
		{
			_t = null;
			_compiled_keywords = "";
			_keywords = new StringCollection();
		}
		
		public StringCollection Keywords
		{
			get	{	return _keywords;	}
		}
		
		public void CompileKeywords()
		{
			_compiled_keywords = "";
			foreach (string it in _keywords)
			{
				if (i == Settings.Keywords.Count-1)
					_compiled_keywords += "\\b" + it + "\\b";
				else
					_compiled_keywords += "\\b" + it + "\\b|";
			}
		}
		
		private int ParseResursive(StreamReader read, PySection parent, int row)
		{
			int count = row;
			bool insection = false;
			Regex regKeywords = new Regex(_compiled_keywords, RegexOptions.IgnoreCase | RegexOptions.Compiled);
			Match regMatch,
			
			while (insection)
			{
				string item = read.ReadLine();
				regMatch = regKeywords.Match(item);	
				if (regMatch.Success)
				{
					// begin or end??
					if (item.Contains("begin"))
					{				
						// parse sub
					}
					if (item.Contains("end"))
					{
						insection = false;
					}
					count++;
				}
			}
			return count;
			// count++;			
		}
		
		public bool ParseFile(string filename)
		{
			_t = File.OpenText("../../complex_app.py");
			
			if (_t == null) return false;
			
			int count = 0;
			string item;
			while (!t.EndOfStream)
			{
				item = t.ReadLine();
				if (item.Contains("# --- Class begin ---"))
				{
					_cur_sec.Children.Add(new PySection());
					PySection temp = (PySection)_cur_sec.Children[_cur_sec.Children.Count - 1];
					_cur_sec = temp;
					_cur_sec.Begin = count;
				}
				if (item.Contains("# --- Class end ---"))
				{
				}
				count++;
			}
			return true;
		}
	}
	*/
	
}
