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

namespace testtext
{
	public enum PyFileSection
	{
		PY_HEADER_SECTION = 0,
		PY_INIT_SECTION = 1,
		PY_EVENTDECL_SECTION = 2,
		PY_EVENTIMPL_SECTION = 3,
		PY_APP_SECTION = 4,
		//
		PYBASE_INIT_SECTION = 5,
		PYBASE_PROPS_SECTION = 6,
		PYBASE_LAYOUT_SECTION = 7
	}
	
	/*
	 * 0 : # --- begin [Sec1] section ---	B
	 * 1 : # --- end [Sec1] section ---		E
	 * 2 : # --- begin [Sec2] section ---	B
	 * (3) : data 1
	 * 3 : # --- begin [Sec2] section ---	B
	 * 
	 * B = 0, S = 0, E = B + S + 1
	 * Insert PS = B + 1 + Pos, If (Pos = -1) PS = E;
	 */
	
	public class PySection
	{
		private int _begin;
		private int _size;
		private PyFileSection _sec;
		
		public PySection()
		{
			_begin = 0;
			_size = 0;
		}
		
		public int Begin
		{
			get	{	return _begin;	}
			set	{	_begin = value;	}
		}		
		public int Size
		{
			get	{	return _size;	}
			set	{	_size = value;	}
		}				
		public int End
		{
			get	{	return _begin + _size + 1;	}
		}
		public PyFileSection Section
		{
			get	{	return _sec;	}
			set	{	_sec = value;	}
		}
		
		public void MoveBeginIndex(int count)
		{
			_begin += count;
		}
		
		public void MoveSizeIndex(int count)
		{
			_size += count;
			if (_size < 0) _size = 0;
		}
	}
		
	
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
		
		public void InsertSingleLineToSection(int pos, PyFileSection section, string str)
		{
			// pos : relative to section, -1 mean end of section
			int row = SeekToSection(section, pos);
			if (row > _py_mem.Count)
			{	
				_py_mem.Add(str);
			} else {
				_py_mem.Insert(row, str);
			}
			MoveSectionSize(section, 1);
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
		
		
		
		public void InsertLinesToSection(int pos, PyFileSection section, string str)
		{
			// pos : relative to section, -1 mean end of section
			string[] arr = Regex.Split(str, "\r\n");
			int count = arr.Length;
			int row = SeekToSection(section, pos);
			for (int i=0; i<count; i++)
			{
				if (row + i > _py_mem.Count)
				{	
					_py_mem.Add(arr[i]);
				} else {
					_py_mem.Insert(row + i, arr[i]);
				}
			}
			MoveSectionSize(section, count);
		}
		
		
		public void DeleteLineToSection(PyFileSection section, int pos, int rowcount)
		{
			// MemoryStream temp = new MemoryStream();
			int row = SeekToSection(section, pos);
			for (int i=0; i<rowcount; i++)
			{
				_py_mem.RemoveAt(row + i);
			}
			MoveSectionSize(section, -rowcount);
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
		}
				
		public StringCollection PyStream
		{
			get	{	return _py_mem;	}
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
			InsertSingleLineToSection(-1, PyFileSection.PY_HEADER_SECTION, "# generated by mkdb\n");
			InsertSingleLineToSection(-1, PyFileSection.PY_HEADER_SECTION, "\n");
			InsertSingleLineToSection(-1, PyFileSection.PY_HEADER_SECTION, "import wx\n");
			InsertSingleLineToSection(-1, PyFileSection.PY_HEADER_SECTION, "\n");
		}
		
		/*
		public Stream GetPyStream()
		{
			MemoryStream stream = new MemoryStream();
			foreach (string item in _py_mem)
			{
				byte[] byt = Encoding.ASCII.GetBytes(item);
				stream.Write(byt, 0, (int)byt.Length);
			}
			return stream;
		}
		*/
	}
}
