/*
 * Creato da SharpDevelop.
 * Utente: michele
 * Data: 05/03/2009
 * Ora: 9.59
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;

namespace mkdb.Python
{
	public class PyParser
	{
		private StreamReader _t;
		
		public PyParser()
		{			
		}
		
		public void Render(StringCollection box, PySection _main)
		{
			RenderSection(box, _main);
		}
		
		private void InsertToPos(StringCollection coll, int pos, string str)
		{
			if (pos >= coll.Count)
			{
				// coll.Add(pos.ToString() + " :: " + str);
				coll.Add(str);
			} else {
				// coll.Insert(pos, pos.ToString() + " :: " + str);
				coll.Insert(pos, str);
			}
		}
		
		public void RenderSection(StringCollection box, PySection section)
		{
			if (section.Name != "")
			{
				string str = "# begin " + section.Name + "\n";
				InsertToPos(box, section.Begin, str);
			}
			if (section.Children.Count == 0)
			{
				int cnt = 1;
				foreach (string it in section.Lines)
				{
					InsertToPos(box, section.Begin + cnt, it + "\n");					
					cnt++;
				}
			} else {
				int cnt = 1;
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
				InsertToPos(box, section.End, "# end " + section.Name + "\n");
			}
		}

		public void ParseFile(StreamReader stream, PySection _main)
		{
			// _t = File.OpenText(filename);
			StringCollection coll = new StringCollection();
			while (!stream.EndOfStream)
			{
				coll.Add(stream.ReadLine());
			}
			ParseFileSection(coll, _main, 0, coll.Count);
		}		
		
		public void ParseFile(string filename, PySection _main)
		{
			_t = File.OpenText(filename);
			ParseFile(_t, _main);
		}
		
		public int ParseFileSection(StringCollection sec, PySection section, int begin, int end)
		{
			// sec collection, doesn't contain # --- begin / # --- end
			int count = begin;
			
			for (int j=begin; j<end; j++)
			{
				string item = sec[j];
				if (item.Contains("# begin"))
				{
					// here's we have a new section
					string trimstr = item.Trim();
					string nn = trimstr.Substring(7);
					section.Children.Add(new PySection(nn.Trim()));
					PySection child = (PySection)section.Children[section.Children.Count - 1];
					child.Begin = count;
					// find the end...
					string ends = item.Replace("begin", "end");
					int end_idx = sec.IndexOf(ends);
					if ((end_idx >= begin) || (end_idx <= end))
					{					
						child.Size = end_idx - child.Begin - 1;
						ParseFileSection(sec, child, count + 1, end_idx);
						j = end_idx;
						count = end_idx;
					} else {
						// Not found error...
					}
				} else if (item.Contains("# end"))
				{
					// DO NOTHING
				} else
				{
					section.Lines.Add(item);					
				}
				count++;
			} 
			return count;
		}			
	}
	
}
