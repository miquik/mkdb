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
	public abstract class WidgetElem
	{
		protected wx.Window _elem;
		protected WidgetProps _props;
		
		public WidgetElem()
		{
			_elem = null;
			_props = null;
		}
		
		public abstract bool InsertWidget(System.Windows.Forms.Panel _canvas);
		public abstract bool DeleteWidget();		
		public abstract long FindBlockInText();
		
		protected abstract bool InsertWidgetInEditor(System.Windows.Forms.Panel _canvas);
		protected abstract bool InsertWidgetInText();
		
		protected abstract bool DeleteWidgetFromEditor();
		protected abstract bool DeleteWidgetFromText();
	}
}
