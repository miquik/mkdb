/*
 * Creato da SharpDevelop.
 * Utente: michele
 * Data: 06/02/2009
 * Ora: 15.23
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;

namespace mkdb
{
	public enum CommandFlags
	{
		TB_NONE = 0,
		TB_CMD_SIZER
	}
		
	public abstract class Command
	{
		private string _cmd_name;
		
		public Command()	{}		
		public Command(string _name)
		{
			_cmd_name = _name;
		}		
		
		public virtual bool Execute()
		{
			return true;
		}
		
		public virtual void Undo()		{}		
		public virtual void Redo()		{}
		
		public string Name
		{
			get	{	return _cmd_name;	}
			set	{	_cmd_name = value;	}
		}				
	}
	
	public class CommandManager
	{			
		private ArrayList _cmd_list;
		
		public CommandManager()
		{
			_cmd_list = new ArrayList();
		}
	
		public int AddCommand(Command cmd)
		{
			return _cmd_list.Add(cmd);
		}
		
		public void RemCommand(Command cmd)
		{
			_cmd_list.Remove(cmd);
		}
		
		public ArrayList CommandList
		{
			get	{	return _cmd_list;	}
		}
	}
}
