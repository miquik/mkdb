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

namespace mkdb.Python
{
	public enum PyFileSection
	{
		PY_HEADER_SECTION,
		PY_INIT_SECTION,
		PY_EVENTDECL_SECTION,
		PY_EVENTIMPL_SECTION,
		PY_APP_SECTION,
		//
		PYBASE_INIT_SECTION,
		PYBASE_PROPS_SECTION,
		PYBASE_LAYOUT_SECTION
	}
	
	public class PyFileEditor
	{
		private string _pyname;
		private string _pybasename;
		private MemoryStream _py_mem;
		private MemoryStream _py_base_mem;
		
		#region InFile indexes
		// Begin : begin include 'Comment'
		// Size : size include 'Comment' --> size == 1 => no code		
		// pyfile.py
		private long _py_header_begin;
		private long _py_header_size;
		private long _py_init_begin;
		private long _py_init_size;
		private long _py_eventdecl_begin;
		private long _py_eventdecl_size;		
		private long _py_eventimpl_begin;
		private long _py_eventimpl_size;		
		private long _py_app_begin;
		private long _py_app_size;				
		// pybasefile.py
		private long _pybase_init_begin;
		private long _pybase_init_size;
		private long _pybase_props_begin;
		private long _pybase_props_size;
		private long _pybase_layout_begin;
		private long _pybase_layout_size;
		#endregion
		
		public PyFileEditor()
		{
			// _mem = new MemoryStream();
			_py_mem = new MemoryStream();
			_py_base_mem = new MemoryStream();
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
		
		private void WriteLineToHeader(string str)
		{
			_py_mem.Seek(_py_header_begin, SeekOrigin.Begin);
			_py_mem.Write(Encoding.ASCII.GetBytes(str.ToCharArray()), 0, str.Length);
			MoveSectionSize(PyFileSection.PY_HEADER_SECTION, 1);
		}
		
		public void WriteLineToSection(PyFileSection section, string str, bool multiline)
		{
			string[] arr = Regex.Split(str, "\r\n");
			int count = arr.Length;
		}
		
		private void SeekToSection(PyFileSection section)
		{
			switch (startFrom)
			{
				case PyFileSection.PY_HEADER_SECTION :
					if (count > 0)
					{
						_py_header_begin += count;
						MoveIndexes(PyFileSection.PY_INIT_SECTION, count);
					}
					break;
				case PyFileSection.PY_INIT_SECTION :
					_py_init_begin += count; // _py_header_size + _py_header_size + 1;
					MoveIndexes(PyFileSection.PY_EVENTDECL_SECTION, count);
					break;
					
				case PyFileSection.PY_EVENTDECL_SECTION :
					_py_eventdecl_begin += count; // _py_init_begin + _py_init_size + 1;
					MoveIndexes(PyFileSection.PY_EVENTIMPL_SECTION, count);
					break;
					
				case PyFileSection.PY_EVENTIMPL_SECTION :
					_py_eventimpl_begin += count; // _py_eventdecl_begin + _py_eventdecl_size + 1;
					MoveIndexes(PyFileSection.PY_APP_SECTION, count);
					break;
					
				case PyFileSection.PY_APP_SECTION :
					_py_app_begin += count; // _py_eventimpl_begin + _py_eventimpl_size + 2;
					break;
					
				case PyFileSection.PYBASE_INIT_SECTION :
					if (count > 0)
					{
						_pybase_init_begin += count;
						MoveIndexes(PyFileSection.PYBASE_PROPS_SECTION, count);
					}
					break;					
					
				case PyFileSection.PYBASE_PROPS_SECTION :
					_pybase_props_begin += count; // _py_eventimpl_begin + _py_eventimpl_size + 2;
					break;
					
				case PyFileSection.PYBASE_LAYOUT_SECTION :
					_pybase_layout_begin += count; // _py_eventimpl_begin + _py_eventimpl_size + 2;
					break;
			}		
		}
		
		public void MoveIndexes(PyFileSection startFrom, int count)
		{
			// move the indexes of 'count', starting from 'startFrom'
			switch (startFrom)
			{
				case PyFileSection.PY_HEADER_SECTION :
					if (count > 0)
					{
						_py_header_begin += count;
						MoveIndexes(PyFileSection.PY_INIT_SECTION, count);
					}
					break;
				case PyFileSection.PY_INIT_SECTION :
					_py_init_begin += count; // _py_header_size + _py_header_size + 1;
					MoveIndexes(PyFileSection.PY_EVENTDECL_SECTION, count);
					break;
					
				case PyFileSection.PY_EVENTDECL_SECTION :
					_py_eventdecl_begin += count; // _py_init_begin + _py_init_size + 1;
					MoveIndexes(PyFileSection.PY_EVENTIMPL_SECTION, count);
					break;
					
				case PyFileSection.PY_EVENTIMPL_SECTION :
					_py_eventimpl_begin += count; // _py_eventdecl_begin + _py_eventdecl_size + 1;
					MoveIndexes(PyFileSection.PY_APP_SECTION, count);
					break;
					
				case PyFileSection.PY_APP_SECTION :
					_py_app_begin += count; // _py_eventimpl_begin + _py_eventimpl_size + 2;
					break;
					
				case PyFileSection.PYBASE_INIT_SECTION :
					if (count > 0)
					{
						_pybase_init_begin += count;
						MoveIndexes(PyFileSection.PYBASE_PROPS_SECTION, count);
					}
					break;					
					
				case PyFileSection.PYBASE_PROPS_SECTION :
					_pybase_props_begin += count; // _py_eventimpl_begin + _py_eventimpl_size + 2;
					break;
					
				case PyFileSection.PYBASE_LAYOUT_SECTION :
					_pybase_layout_begin += count; // _py_eventimpl_begin + _py_eventimpl_size + 2;
					break;
			}
		}
		
		public void MoveSectionSize(PyFileSection startFrom, int count)
		{
			// move the indexes of 'count', starting from 'startFrom'
			switch (startFrom)
			{
				case PyFileSection.PY_HEADER_SECTION :
					if (count > 0)
					{
						_py_header_size += count;
						MoveIndexes(PyFileSection.PY_INIT_SECTION, count);
					}
					break;
				case PyFileSection.PY_INIT_SECTION :
					_py_init_size += count; // _py_header_size + _py_header_size + 1;
					MoveIndexes(PyFileSection.PY_EVENTDECL_SECTION, count);
					break;
					
				case PyFileSection.PY_EVENTDECL_SECTION :
					_py_eventdecl_size += count; // _py_init_begin + _py_init_size + 1;
					MoveIndexes(PyFileSection.PY_EVENTIMPL_SECTION, count);
					break;
					
				case PyFileSection.PY_EVENTIMPL_SECTION :
					_py_eventimpl_size += count; // _py_eventdecl_begin + _py_eventdecl_size + 1;
					MoveIndexes(PyFileSection.PY_APP_SECTION, count);
					break;
					
				case PyFileSection.PY_APP_SECTION :
					_py_app_size += count; // _py_eventimpl_begin + _py_eventimpl_size + 2;
					break;
					
				case PyFileSection.PYBASE_INIT_SECTION :
					if (count > 0)
					{
						_pybase_init_size += count;
						MoveIndexes(PyFileSection.PYBASE_PROPS_SECTION, count);
					}
					break;					
					
				case PyFileSection.PYBASE_PROPS_SECTION :
					_pybase_props_size += count; // _py_eventimpl_begin + _py_eventimpl_size + 2;
					break;
					
				case PyFileSection.PYBASE_LAYOUT_SECTION :
					_pybase_layout_size += count; // _py_eventimpl_begin + _py_eventimpl_size + 2;
					break;
			}
		}
		
		
		private void SetDefaultIndexes()
		{
			MoveIndexes(PyFileSection.PY_HEADER_SECTION, 0);
		}
	}
}
