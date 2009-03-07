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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;

namespace mkdb.Python
{
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
		private string _name;
		private int _begin;
		private int _size;
		private	StringCollection _lines;
		private List<PySection> _children;		
		
		public PySection(string name)
		{
			_name = name;
			_begin = 0;
			_size = 0;
			_children = new List<PySection>();
			_lines = new StringCollection();
		}
		
		public string Name
		{
			get	{	return _name;	}
			set	{	_name = value;	}
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
		public List<PySection> Children
		{
			get	{	return _children;	}
		}
		public StringCollection Lines
		{
			get	{	return _lines;	}
		}
		
		public void AddSection(PySection sec)
		{
			_children.Add(sec);
			Size += sec.Size;
		}
		
		public void RemSection(PySection sec)
		{
			_children.Remove(sec);
			Size -= sec.Size;
		}
		
		public PySection FindChildByName(string childname)
		{
			return _children.Find(delegate(PySection ps){return ps.Name == childname;});
		}
		
		public Match RegexFindAndReplace(string pattern, string newstr, string groupName)
		{
			Regex r = new Regex(pattern);
			for (int i=0; i<_lines.Count; i++)
			{				
				Match m = r.Match(_lines[i]);
				if (m.Success)
				{
					// Change name
					Group gr = m.Groups[groupName];
					string temp = _lines[i].Replace(gr.Value, newstr);
					_lines[i] = temp;
					return m;
				}
			}		
			return null;
		}
		
		public void RegexFindAndReplaceSub(string pattern, string newstr, string groupName)
		{
			Regex r = new Regex(pattern);
			for (int i=0; i<_lines.Count; i++)
			{				
				Match m = r.Match(_lines[i]);
				if (m.Success)
				{
					// Change name
					Group gr = m.Groups[groupName];
					string temp = _lines[i].Replace(gr.Value, newstr);
					_lines[i] = temp;
				}
			}		
			foreach (PySection s in _children)
			{
				s.RegexFindAndReplaceSub(pattern, newstr, groupName);
			}
		}		
		
		public bool FindAndReplace(string oldstr, string newstr)
		{
			for (int i=0; i<_lines.Count; i++)
			{								
				if (_lines[i].Contains(oldstr))
				{
					// Change name
					string temp = _lines[i].Replace(oldstr, newstr);
					_lines[i] = temp;
					return true;
				}
			}		
			return false;			
		}
		
		public void FindAndReplaceSub(string oldstr, string newstr)
		{
			for (int i=0; i<_lines.Count; i++)
			{								
				if (_lines[i].Contains(oldstr))
				{
					// Change name
					string temp = _lines[i].Replace(oldstr, newstr);
					_lines[i] = temp;
				}
			}		
			foreach (PySection s in _children)
			{
				s.FindAndReplaceSub(oldstr, newstr);
			}
		}
		
		
		public void MoveBeginIndex(int count)
		{
			_begin += count;
			if (_begin < 0) _begin = 0;
		}
		
		public void MoveSizeIndex(int count)
		{
			_size += count;
			if (_size < 0) _size = 0;
		}
	}
	
}
