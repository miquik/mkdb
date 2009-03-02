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

namespace mkdb.Python
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
	
	public class PySection
	{
		private int _begin;
		private int _size;
		private PyFileSection _sec;
		
		public PySection()
		{
			_begin = -1;
			_size = 1;
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
			get	{	return _begin + _size;	}
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
			if (_size < 1)
				_size = 1;
		}
	}
		
	
	public class PyFileEditor
	{
		private string _pyname;
		private string _pybasename;
		private StringCollection _py_mem;
		private StringCollection _pybase_mem;
		
		#region InFile indexes
		// Begin : begin include 'Comment'
		// Size : size include 'Comment' --> size == 1 => no code		
		// pyfile.py
		private const int _sec_count = 8;
		private PySection[] _py_sections;
		#endregion
		
		public PyFileEditor()
		{
			// _mem = new MemoryStream();
			_py_mem = new StringCollection();
			_pybase_mem = new StringCollection();
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
		
		public void InsertSingleLineToSection(int pos, PyFileSection section, string str)
		{
			// pos : relative to section, -1 mean end of section
			int row = SeekToSection(section, pos);
			if ((int)section < 5)
			{
				if (row > _py_mem.Count)
					_py_mem.Add(str);
				else
					_py_mem.Insert(row, str);
			} else {
				if (row > _pybase_mem.Count)
					_pybase_mem.Add(str);
				else
					_pybase_mem.Insert(row, str);
			}
			MoveSectionSize(section, 1);
		}
		
		public void InsertBlankLine(PyFileSection after)
		{
			// pos : relative to section, -1 mean end of section
			int row = SeekToSection(after, -1);
			
			if ((int)after < 5)
			{
				if (row >= _py_mem.Count)
					_py_mem.Add("\n");
				else
					_py_mem.Insert(row + 1, "\n");
			} else {
				if (row >= _pybase_mem.Count)
					_pybase_mem.Add("\n");
				else
					_pybase_mem.Insert(row + 1, "\n");
			}
			MoveIndexes((PyFileSection)(after+1), 1);
		}
		
		
		public void InsertLinesToSection(int pos, PyFileSection section, string str)
		{
			// pos : relative to section, -1 mean end of section
			string[] arr = Regex.Split(str, "\r\n");
			int count = arr.Length;
			int row = SeekToSection(section, pos);
			for (int i=0; i<count; i++)
			{
				if ((int)section < 5)
					_py_mem.Insert(row+i, arr[i]);
				else
					_pybase_mem.Insert(row+i, arr[i]);				
			}
			MoveSectionSize(section, 1);
		}
		
		
		public void DeleteLineToSection(PyFileSection section, int pos, int rowcount)
		{
			// MemoryStream temp = new MemoryStream();
			int row = SeekToSection(section, pos);
			for (int i=0; i<rowcount; i++)
			{
				if ((int)section < 5)
					_py_mem.RemoveAt(row + i);
				else
					_pybase_mem.RemoveAt(row + i);
			}
			MoveSectionSize(section, -rowcount);
		}
		
		private int SeekToSection(PyFileSection section, int pos)
		{
			int ps = _py_sections[(int)section].Begin + pos + 1;
			if (pos == -1) ps = _py_sections[(int)section].End - 1;
			if (ps < 0) ps = _py_sections[(int)section].Begin + 1;
			if (ps > _py_sections[(int)section].End)
				ps = _py_sections[(int)section].End - 1;
			
			return ps;
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
				
		private void SetDefaultIndexes()
		{
			/*
			# --- Header : begin ---
			# --- Header : end ---

			# --- InitBase : begin ---
			# --- InitBase : end ---

			# --- Properties : begin ---
			# --- Properties : end ---

			# --- Layout : begin ---
			# --- Layout : end ---


			# --- App : begin ---
			# --- App : end ---
			*/
			_py_sections[0].Begin = 1;
			_py_sections[1].Begin = 4;
			_py_sections[2].Begin = 7;
			_py_sections[3].Begin = 10;
			_py_sections[4].Begin = 13;
			_py_sections[5].Begin = 1;
			_py_sections[6].Begin = 4;
			_py_sections[7].Begin = 7;
		}
		
		public void ParsePyFile(string filename)
		{
			StreamReader read = new StreamReader(filename);
			while (!read.EndOfStream)
			{
				_py_mem.Add(read.ReadLine());
			}
			read.Close();
		}
		
		public void ParsePyBaseFile(string filename)
		{
			StreamReader read = new StreamReader(filename);
			while (!read.EndOfStream)
			{
				_pybase_mem.Add(read.ReadLine());
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
			_py_sections[0].Begin = 0;			
			InsertSingleLineToSection(-1, PyFileSection.PY_HEADER_SECTION, "#!/usr/bin/env python\n");
			InsertSingleLineToSection(-1, PyFileSection.PY_HEADER_SECTION, "# --- begin Header section ---\n");
			InsertSingleLineToSection(-1, PyFileSection.PY_HEADER_SECTION, "# generated by mkdb\n");
			InsertSingleLineToSection(-1, PyFileSection.PY_HEADER_SECTION, "\n");
			InsertSingleLineToSection(-1, PyFileSection.PY_HEADER_SECTION, "import wx\n");
			InsertSingleLineToSection(-1, PyFileSection.PY_HEADER_SECTION, "\n");
			InsertSingleLineToSection(-1, PyFileSection.PY_HEADER_SECTION, "# --- end Header section ---\n");
			InsertBlankLine(PyFileSection.PY_HEADER_SECTION);
			// Init			
			InsertSingleLineToSection(-1, PyFileSection.PY_INIT_SECTION, "# --- begin Init section ---\n");
			InsertSingleLineToSection(-1, PyFileSection.PY_INIT_SECTION, "# --- end Init section ---\n");
			InsertBlankLine(PyFileSection.PY_INIT_SECTION);
			// Event Decl			
			InsertSingleLineToSection(-1, PyFileSection.PY_EVENTDECL_SECTION, "# --- begin EventDecl section ---\n");
			InsertSingleLineToSection(-1, PyFileSection.PY_EVENTDECL_SECTION, "# --- end EventDecl section ---\n");
			InsertBlankLine(PyFileSection.PY_EVENTDECL_SECTION);
			// Event Impl			
			InsertSingleLineToSection(-1, PyFileSection.PY_EVENTIMPL_SECTION, "# --- begin EventImpl section ---\n");
			InsertSingleLineToSection(-1, PyFileSection.PY_EVENTIMPL_SECTION, "# --- end EventImpl section ---\n");
			InsertBlankLine(PyFileSection.PY_EVENTIMPL_SECTION);
			// App			
			InsertSingleLineToSection(-1, PyFileSection.PY_APP_SECTION, "# --- begin App section ---\n");
			InsertSingleLineToSection(-1, PyFileSection.PY_APP_SECTION, "# --- end App section ---\n");
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
