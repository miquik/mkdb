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
			Common.Instance().ObjPropsPanel = objprops;
			Common.Instance().Canvas = this.canvas;
			// objtree.Nodes.Add("Application");
			objtree.Nodes.Add(new wdbApp());
			wdbApp _tapp = (wdbApp)objtree.Nodes[0];
			_tapp.InsertWidget(null);
		}
		
		WidgetElem FindBestParent(WidgetElem curNode)
		{
			WidgetElem parent;
			// Find an available Sizer starting from current node
			if (curNode.CanAcceptChildren())
				return curNode;
			parent = (WidgetElem)curNode.Parent;
			if (parent == null)
				return null;		
			return FindBestParent(parent);
		}
		
		void ToolStripButton3Click(object sender, EventArgs e)
		{
			// Create a wxWindow on the top of canvas panel.
			wdbApp _tapp = (wdbApp)objtree.Nodes[0];			
			wdbFrame frame = new wdbFrame();
			frame.InsertWidget(_tapp);
			// frame.InsertWidget(null);
			// IntPtr wxh = Win32Utils.FindWindow("wxWindowClassNR", frame.Element.Title);
			// Win32Utils.SetParent(frame.Element.GetHandle(), canvas.Handle);
			Common.Instance().ChangeCurrentWindow(frame);
			objtree.SelectedNode.Nodes.Add(frame);
			objtree.SelectedNode = frame;
			frame.PaintOnSelection();			
		}
		
		void ToolStripButton4Click(object sender, EventArgs e)
		{
			/* Create Sizer */
			WidgetElem parent;
			parent = FindBestParent((WidgetElem)objtree.SelectedNode);
			if (parent == null)
			{
				// TODO : Error here!!
			} else {
				// Add this sizer
				wdbBoxSizer bsizer = new wdbBoxSizer();
				bsizer.InsertWidget(parent);
				parent.Nodes.Add(bsizer);
				objtree.SelectedNode = bsizer;
				bsizer.PaintOnSelection();			
			}
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
					elem.PaintOnSelection();
					// Common.Instance().ChangeCurrentWindow();
				}
			}
		}
	}
}
