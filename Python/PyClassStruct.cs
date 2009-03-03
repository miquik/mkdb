/*
 * Creato da SharpDevelop.
 * Utente: michele
 * Data: 03/03/2009
 * Ora: 13.54
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;

namespace textops
{
	public enum PyClassSection
	{
		PYDEF_NONE = -1, 
		PYDEF_INIT_SECTION = 0, 
		PYDEF_PROPS_SECTION = 1,
		PYDEF_LAYOUT_SECTION = 2,
		PYDEF_EVENT_SECTION = 3
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
		private string _sec_begin_label;
		private string _sec_end_label;
		
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
		public string BeginLabel
		{
			get	{	return _sec_begin_label;	}
			set	{	_sec_begin_label = value;	}
		}
		public string EndLabel
		{
			get	{	return _sec_end_label;	}
			set	{	_sec_end_label = value;	}
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
	
	public class PyClass
	{
		private string _classname;
		private StringCollection _lines;
		// pyfile.py
		private const int _sec_count = 4;
		public PySection[] _sections;
		public PyClassSection _cur_section;
		public int _cur_begin;
		public int _cur_size;
		
		public PyClass()
		{
			_lines = new StringCollection();
			_sections = new PySection[_sec_count];
			for (int i=0; i<_sec_count; i++)
			{
				_sections[i] = new PySection();
				_sections[i].Begin = 1 + i*2;				
			}
			_cur_begin = -1;
			_cur_size = -1;
			_cur_section = PyClassSection.PYDEF_NONE;
		}
		
		public string Name
		{
			get	{	return _classname;		}
			set	{	_classname = value;	}
		}
		public StringCollection Lines
		{
			get	{	return _lines;	}
		}
		
		public void BeginSection(PyClassSection section)
		{
			// make this section as current;
			_cur_section = section;
			_cur_begin = _sections[(int)section].Begin;
			_cur_size = _sections[(int)section].Size;
		}
		
		public void EndSection()
		{
			// make this section as current;
			_cur_section = PyClassSection.PYDEF_NONE;
			_cur_begin = -1;
			_cur_size = -1;
		}
		
		public void SetBeginLabel(string label)
		{
			// _lines[_cur_begin].A
		}
		
		private int SeekToSection(int pos)
		{
			/*
	 		* B = 0, S = 0, E = B + S + 1
	 		* Insert PS = B + 1 + Pos, If (Pos = -1) PS = E;
	 		*/
	 		int B = _cur_begin;
	 		int E = _cur_begin + _cur_size + 1;
			
			int ps = B + pos + 1;
			if (pos == -1) 	ps = E;
			if (ps < 0) 	ps = B + 1;
			if (ps > E) 	ps = E;
			
			return ps;
		}		
		
		public void InsertSingleLine(int pos, string str)
		{
			// pos : relative to section, -1 mean end of section
			int row = SeekToSection(pos);
			if (row >= _lines.Count)
			{	
				_lines.Add(str);
			} else {
				_lines.Insert(row, str);
			}
			MoveSectionSize(1);
		}
				
		public void InsertLines(int pos, string str)
		{
			// pos : relative to section, -1 mean end of section
			string[] arr = Regex.Split(str, "\r\n");
			int count = arr.Length;
			int row = SeekToSection(pos);
			for (int i=0; i<count; i++)
			{
				if (row + i >= _lines.Count)
				{	
					_lines.Add(arr[i]);
				} else {
					_lines.Insert(row + i, arr[i]);
				}
			}
			MoveSectionSize(count);
		}
						
		public void DeleteLine(int pos, int rowcount)
		{
			int row = SeekToSection(pos);
			for (int i=0; i<rowcount; i++)
			{
				_lines.RemoveAt(row + i);
			}
			MoveSectionSize(-rowcount);
		}
		
		public void ReplaceEntireLine(int pos, string str)
		{
			// pos : relative to section, -1 mean end of section
			int row = SeekToSection(pos);
			if (row < _lines.Count)
			{	
				_lines[row] = str;
			}
		}				
				
		public void InsertBeforeSection(string str)
		{
			// pos : relative to section, -1 mean end of section
			int row = SeekToSection(0);
			if (row - 1 < 0)  row = 0; else row--;
			_lines.Insert(row, str);
			MoveIndexes(1);
		}
		
		public void InsertAfterSection(string str)
		{
			// pos : relative to section, -1 mean end of section
			int row = SeekToSection( -1);
			row++;
			if (row >= _lines.Count)
			{
				_lines.Add(str);
			} else {
				_lines.Insert(row, str);
			}
			for (int i = (int)(_cur_section) + 1; i<_sec_count; i++)
			{
				_sections[i].MoveBeginIndex(1);
			}
		}				
				
		public void MoveIndexes(int count)
		{
			for (int i = (int)_cur_section; i<_sec_count; i++)
			{
				_sections[i].MoveBeginIndex(count);
			}
			_cur_begin = _sections[(int)_cur_section].Begin;
		}
		
		public void MoveSectionSize(int count)
		{
			_sections[(int)_cur_section].MoveSizeIndex(count);
			for (int i = (int)(_cur_section) + 1; i<_sec_count; i++)
			{
				_sections[i].MoveBeginIndex(count);
			}
			_cur_size = _sections[(int)_cur_section].Size;
		}
	}
}
