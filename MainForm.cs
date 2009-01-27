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
			Common.Instance().ObjPropsPanel = objprops;
		}
		
		void ToolStripButton3Click(object sender, EventArgs e)
		{
			// Create a wxWindow on the top of canvas panel.
			wdbFrame frame = new wdbFrame();
			frame.InsertWidget();
			IntPtr wxh = Win32Utils.FindWindow("wxWindowClassNR", frame.Element.Title);
			Win32Utils.SetParent(wxh, canvas.Handle);
			Common.Instance().ChangeCurrentWindow(frame.Element);
			objtree.SelectedNode.Nodes.Add(frame);
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
				// WidgetElem elem = (WidgetElem)Common.Instance().WidgetList[e.Node.Text];
				// WidgetElem elem = (WidgetElem)objtree.Nodes.Find(e.Node.Text, true);
				WidgetElem elem = (WidgetElem)objtree.SelectedNode;
				if (elem != null)
				{
					objprops.SelectedObject = elem.Properties;
					// Common.Instance().ChangeCurrentWindow();
				}
			}
		}
	}
}
