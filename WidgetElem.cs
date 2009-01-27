/*
 * Creato da SharpDevelop.
 * Utente: Family Rose
 * Data: 26/12/2008
 * Ora: 12.12
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using wx;
using System.Windows.Forms;

namespace mkdb
{
	/// <summary>
	/// Description of WidgetElem.
	/// </summary>
	public abstract class WidgetElem : TreeNode
	{
		protected wx.Window _elem;
		protected WidgetProps _props;
		protected string _label;
		
		public WidgetElem()
		{
			_elem = null;
			_props = null;
			_label = "";
		}
		
		public string Label
		{
			get	{	return _label;	}
		}
		public WidgetProps Properties
		{
			get	{	return _props;	}
		}		
		public wx.Window Element
		{
			get	{	return _elem;	}
		}
		
		public abstract bool InsertWidget();
		public abstract bool DeleteWidget();		
		public abstract long FindBlockInText();
		
		protected abstract bool InsertWidgetInEditor();
		protected abstract bool InsertWidgetInText();
		
		protected abstract bool DeleteWidgetFromEditor();
		protected abstract bool DeleteWidgetFromText();
	}
}
