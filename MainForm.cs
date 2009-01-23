/*
 * Creato da SharpDevelop.
 * Utente: Family Rose
 * Data: 24/12/2008
 * Ora: 13.51
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using mkdb.Widgets;


namespace mkdb
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			objtree.Nodes.Add("Application");
		}
		
		void ToolStripButton3Click(object sender, EventArgs e)
		{
			// Create a wxWindow on the top of canvas panel.
			wdbFrame testframe = new wdbFrame();
			Common.Instance().ObjPropsPanel = objprops;
			testframe.InsertWidget(canvas);
			objtree.SelectedNode.Nodes.Add(testframe.Label);
			Common.Instance().WidgetList.Add(testframe.Label, testframe);
		}
		
		void ToolStripButton4Click(object sender, EventArgs e)
		{
			wxWindowProps testprop = new wxWindowProps();
			objprops.SelectedObject = testprop;
			// objprops.SelectedObject = new CheckBoxInPropertyGrid();
		}
		
		void ObjtreeAfterSelect(object sender, TreeViewEventArgs e)
		{
			if (e.Action == TreeViewAction.ByMouse)
			{
				WidgetElem elem = (WidgetElem)Common.Instance().WidgetList[e.Node.Text];
				if (elem != null)
				{
					objprops.SelectedObject = elem.Props;
					// Common.Instance().ChangeCurrentWindow();
				}
			}
		}
	}
}
