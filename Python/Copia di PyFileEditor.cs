/*
 * Creato da SharpDevelop.
 * Utente: michele
 * Data: 02/03/2009
 * Ora: 10.21
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Specialized;

namespace textops
{
		
	
	public class PyFileEditor
	{
		private string _pyname;
		private string _pybasename;
		private StringCollection _py_mem;
		// pyfile.py
		private const int _sec_count = 8;
		public PySection[] _py_sections;
		
		public PyFileEditor()
		{
			_py_mem = new StringCollection();
			_py_sections = new PySection[_sec_count];
			for (int i=0; i<_sec_count; i++)
			{
				_py_sections[i] = new PySection();
				_py_sections[i].Section = (PyFileSection)i;
			}
			// SetDefaultIndexes();
			CreateEmpty();
		}
		
		public string PyFileName
		{
			get	{	return _pyname;		}
			set	{	_pyname = value;	}
		}
		public string PyBaseFileName
		{
			get	{	return _pybasename;		}
			set	{	_pybasename = value;	}
		}
		public StringCollection Lines
		{
			get	{	return _py_mem;	}
		}
		
		
		private int SeekToSection(PyFileSection section, int pos)
		{
			/*
	 		* B = 0, S = 0, E = B + S + 1
	 		* Insert PS = B + 1 + Pos, If (Pos = -1) PS = E;
	 		*/
	 		int B = _py_sections[(int)section].Begin;
	 		int E = _py_sections[(int)section].End;
			
			int ps = B + pos + 1;
			if (pos == -1) 	ps = E;
			if (ps < 0) 	ps = B + 1;
			if (ps > E) 	ps = E;
			
			return ps;
		}		
		
		public void InsertSingleLine(int pos, PyFileSection section, string str)
		{
			// pos : relative to section, -1 mean end of section
			int row = SeekToSection(section, pos);
			if (row >= _py_mem.Count)
			{	
				_py_mem.Add(str);
			} else {
				_py_mem.Insert(row, str);
			}
			MoveSectionSize(section, 1);
		}
				
		public void InsertLines(int pos, PyFileSection section, string str)
		{
			// pos : relative to section, -1 mean end of section
			string[] arr = Regex.Split(str, "\r\n");
			int count = arr.Length;
			int row = SeekToSection(section, pos);
			for (int i=0; i<count; i++)
			{
				if (row + i >= _py_mem.Count)
				{	
					_py_mem.Add(arr[i]);
				} else {
					_py_mem.Insert(row + i, arr[i]);
				}
			}
			MoveSectionSize(section, count);
		}
						
		public void DeleteLine(PyFileSection section, int pos, int rowcount)
		{
			// MemoryStream temp = new MemoryStream();
			int row = SeekToSection(section, pos);
			for (int i=0; i<rowcount; i++)
			{
				_py_mem.RemoveAt(row + i);
			}
			MoveSectionSize(section, -rowcount);
		}
		
		public void ReplaceEntireLine(int pos, PyFileSection section, string str)
		{
			// pos : relative to section, -1 mean end of section
			int row = SeekToSection(section, pos);
			if (row < _py_mem.Count)
			{	
				_py_mem[row] = str;
			}
		}				
		
		
		public void InsertBeforeSection(PyFileSection section, string str)
		{
			// pos : relative to section, -1 mean end of section
			int row = SeekToSection(section, 0);
			if (row - 1 < 0)  row = 0; else row--;
			_py_mem.Insert(row, str);
			MoveIndexes(section, 1);
		}
		
		public void InsertAfterSection(PyFileSection section, string str)
		{
			// pos : relative to section, -1 mean end of section
			int row = SeekToSection(section, -1);
			row++;
			if (row >= _py_mem.Count)
			{
				_py_mem.Add(str);
			} else {
				_py_mem.Insert(row, str);
			}
			MoveIndexes((PyFileSection)(section+1), 1);
		}				
				
		public void MoveIndexes(PyFileSection startFrom, int count)
		{
			for (int i = (int)startFrom; i<_sec_count; i++)
			{
				_py_sections[i].MoveBeginIndex(count);
			}
		}
		
		public void MoveSectionSize(PyFileSection startFrom, int count)
		{
			_py_sections[(int)startFrom].MoveSizeIndex(count);
			for (int i = (int)startFrom + 1; i<_sec_count; i++)
			{
				_py_sections[i].MoveBeginIndex(count);
			}
		}
				
		public void ParsePyFile(string pyfilename, string pybasefilename)
		{
			_py_mem.Clear();
			StreamReader read = new StreamReader(pyfilename);
			while (!read.EndOfStream)
			{
				_py_mem.Add(read.ReadLine());
			}
			read.Close();
			read = new StreamReader(pybasefilename);
			while (!read.EndOfStream)
			{
				_py_mem.Add(read.ReadLine());
			}
			read.Close();
			// Create Indexes
			CreateIndexes();
		}
		
		private void CreateIndexes()
		{
			int count = 0;
			int cursize = 0;
			foreach (string item in _py_mem)
			{
				switch (item) {
					case "# --- begin Header section ---\n" :
						_py_sections[0].Begin = count;
						cursize = 0;
						break;
					case "# --- end Header section ---\n" :
						_py_sections[0].Size = cursize;
						break;
					case "# --- begin Init section ---\n" :
						_py_sections[1].Begin = count;
						cursize = 0;
						break;
					case "# --- end Init section ---\n" :
						_py_sections[1].Size = cursize;
						break;
					case "# --- begin EventDecl section ---\n" :
						_py_sections[2].Begin = count;
						cursize = 0;
						break;
					case "# --- end EventDecl section ---\n" :
						_py_sections[2].Size = cursize;
						break;
					case "# --- begin EventImpl section ---\n" :
						_py_sections[3].Begin = count;
						cursize = 0;
						break;
					case "# --- end EventImpl section ---\n" :
						_py_sections[3].Size = cursize;
						break;
					case "# --- begin App section ---\n" :
						_py_sections[4].Begin = count;
						cursize = 0;
						break;
					case "# --- end App section ---\n" :
						_py_sections[4].Size = cursize;
						break;
				}
				count++;
				cursize++;
			}
		}
				
		public void CreateEmpty()
		{
			// Header
			_py_sections[0].Begin = 1;
			_py_mem.Add("#!/usr/bin/env python\n");
			_py_mem.Add("# --- begin Header section ---\n");
			_py_mem.Add("# --- end Header section ---\n");
			_py_mem.Add("\n");
			_py_sections[1].Begin = _py_mem.Count;
			_py_mem.Add("# --- begin Init section ---\n");
			_py_mem.Add("# --- end Init section ---\n");
			_py_mem.Add("\n");
			_py_sections[2].Begin = _py_mem.Count;
			_py_mem.Add("# --- begin EventDecl section ---\n");
			_py_mem.Add("# --- end EventDecl section ---\n");
			_py_mem.Add("\n");
			_py_sections[3].Begin = _py_mem.Count;
			_py_mem.Add("# --- begin EventImpl section ---\n");
			_py_mem.Add("# --- end EventImpl section ---\n");
			_py_mem.Add("\n");
			_py_sections[4].Begin = _py_mem.Count;
			_py_mem.Add("# --- begin App section ---\n");
			_py_mem.Add("# --- end App section ---\n");
			_py_sections[5].Begin = _py_mem.Count;
			_py_mem.Add("# --- begin InitBase section ---\n");
			_py_mem.Add("# --- end InitBase section ---\n");
			_py_mem.Add("\n");
			_py_sections[6].Begin = _py_mem.Count;
			_py_mem.Add("# --- begin Props section ---\n");
			_py_mem.Add("# --- end Props section ---\n");
			_py_mem.Add("\n");
			_py_sections[7].Begin = _py_mem.Count;
			_py_mem.Add("# --- begin Layout section ---\n");
			_py_mem.Add("# --- end Layout section ---\n");
			InsertSingleLine(-1, PyFileSection.PY_HEADER_SECTION, "# generated by mkdb\n");
			InsertSingleLine(-1, PyFileSection.PY_HEADER_SECTION, "\n");
			InsertSingleLine(-1, PyFileSection.PY_HEADER_SECTION, "import wx\n");
			InsertSingleLine(-1, PyFileSection.PY_HEADER_SECTION, "\n");
		}
	}
}
