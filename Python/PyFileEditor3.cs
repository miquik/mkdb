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
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

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
			_size = -1;
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
	
	public class PyStream
	{
		private MemoryStream _mem;
		private StreamReader _read;
		private StreamWriter _write;
		
		public PyStream()
		{
			_mem = new MemoryStream();
			_read = new StreamReader(_mem);
			_write = new StreamWriter(_mem);
			_write.AutoFlush = true;
		}
		
		public MemoryStream Memory
		{
			get	{	return _mem;	}
		}
		public StreamReader Reader
		{
			get	{	return _read;	}
		}
		public StreamWriter Writer
		{
			get	{	return _write;	}
		}
	}
	
	
	public class PyFileEditor
	{
		private string _pyname;
		private string _pybasename;
		private PyStream _py_mem;
		private PyStream _py_base_mem;
		
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
			_py_mem = new PyStream();
			_py_base_mem = new PyStream();
			_py_sections = new PySection[_sec_count];
			for (int i=0; i<_sec_count; i++)
			{
				_py_sections[i].Section = (PyFileSection)i;
			}
			SetDefaultIndexes();
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
			SeekToSection(section, pos);
			_py_mem.Writer.WriteLine(str);
		}
		
		public void InsertLinesToSection(int pos, PyFileSection section, string str)
		{
			// pos : relative to section, -1 mean end of section
			string[] arr = Regex.Split(str, "\r\n");
			int count = arr.Length;
			SeekToSection(section, pos);
			for (int i=0; i<count; i++)
			{
				_py_mem.Writer.WriteLine(arr[i]);
			}
		}
		
		
		public void DeleteLineToSection(PyFileSection section, int rowcount)
		{
			// MemoryStream temp = new MemoryStream();
			MoveSectionSize(section, rowcount);
		}
		
		private long ConvertSectionToPosition(PyFileSection section, int pos)
		{
			int row = 0;
			long posf = 0;
			while (row < pos)
			{
				if ((int)section < 5)
				{
					posf += _py_mem.Reader.ReadLine().Length;
				} else {					
					posf += _py_base_mem.Reader.ReadLine().Length;				
				}
				row++;
			}		
			return posf;
		}
		
		private void SeekToSection(PyFileSection section, int pos)
		{
			// Seek to position
			// First : get position as row
			int ps = _py_sections[(int)section].Begin + pos + 1;
			if (pos == -1) ps = _py_sections[(int)section].End;
			if (ps > _py_sections[(int)section].End)
				ps = _py_sections[(int)section].End;
			
			if ((int)section < 5)
				_py_mem.Memory.Seek(0, SeekOrigin.Begin);
			else
				_py_base_mem.Memory.Seek(0, SeekOrigin.Begin);
			long posf = ConvertSectionToPosition(section, ps);
			if ((int)section < 5)
				_py_mem.Memory.Seek(posf, SeekOrigin.Begin);
			else
				_py_base_mem.Memory.Seek(posf, SeekOrigin.Begin);
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
			MoveIndexes(PyFileSection.PY_HEADER_SECTION, 0);
		}
		
		public void ParsePyFile(string filename)
		{
			StreamReader read = new StreamReader(filename);
			char[] byt = read.ReadToEnd().ToCharArray();
			_py_mem.Writer.Write(byt, 0, (int)byt.Length);
			read.Close();
		}
		
		public void ParsePyBaseFile(string filename)
		{
			StreamReader read = new StreamReader(filename);
			char[] byt = read.ReadToEnd().ToCharArray();
			_py_mem.Writer.Write(byt, 0, (int)byt.Length);
			read.Close();
		}		
	}
}
