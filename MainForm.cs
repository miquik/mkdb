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
			InitializeComponent();
			Common.Instance().ObjPropsPanel = objprops;
			Common.Instance().ObjTree = objtree;
			Common.Instance().Canvas = this.canvas;			
			int idx = objtree.Nodes.Add(new wdbApp());
			wdbApp wdba = (wdbApp)objtree.Nodes[idx];
			wdba.CreateWidget(null);			
			objtree.SelectedNode = wdba;
		}
		
		WidgetElem FindBestParent(WidgetElem curNode)
		{
			WidgetElem parent;
			// Find an available Sizer starting from current node
			if (curNode.WDBBase.CanAcceptChildren())
				return curNode;
			parent = (WidgetElem)curNode.Parent;
			if (parent == null)
				return null;		
			return FindBestParent(parent);
		}
		
		void ToolStripButton3Click(object sender, EventArgs e)
		{	
			wdbApp _tapp = (wdbApp)objtree.Nodes[0];			
			wdbFrame wdbf = new wdbFrame();
			wdbf.CreateWidget(_tapp.WDBBase);
			ChangeCurrentWindow(wdbf);
			objtree.SelectedNode = _tapp;
			objtree.SelectedNode.Nodes.Add(wdbf);			
			objtree.SelectedNode = wdbf;
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
				bsizer.CreateWidget(parent.WDBBase);
				objtree.SelectedNode.Nodes.Add(bsizer);
				objtree.SelectedNode = bsizer;
			}
		}
		
		void ObjtreeBeforeSelect(object sender, TreeViewCancelEventArgs e)
		{			
			IWDBBase elem = (IWDBBase)Common.Instance().CurrentElement;
			if (elem != null)
			{
				elem.IsSelected = false;
				elem.PaintOnSelection();
			}
		}		
		
		void ObjtreeAfterSelect(object sender, TreeViewEventArgs e)
		{
			WidgetElem elem = (WidgetElem)objtree.SelectedNode;
			if (elem != null)
			{
				ChangeCurrentWindow(elem);
				objprops.SelectedObject = elem.WDBBase.Properties;
				elem.WDBBase.IsSelected = true;
				elem.WDBBase.PaintOnSelection();
				Common.Instance().CurrentElement = elem.WDBBase;
			}
		}
		
		public void ChangeCurrentWindow(WidgetElem neww)
    	{
			if (neww.WDBBase.WidgetID == (int)StandardWidgetID.WID_APP)
			{
				// Hide, when we select the application.
 	  			if (Common.Instance().CurrentWindow != null)
   					Common.Instance().CurrentWindow.Hide();				
			}
 	  		IWDBBase _cur = FindTopMostFrame(neww);
  			if (_cur != null)
  			{
 	  			if (Common.Instance().CurrentWindow != null)
   					Common.Instance().CurrentWindow.Hide();
  				Common.Instance().CurrentWindow = _cur.WxWindow;
				Common.Instance().CurrentWindow.Show();
  			} 	  		
    	}

		public IWDBBase FindTopMostFrame(WidgetElem node)
		{
			if (node.Level == 1)
			{
				if (node.WDBBase.WidgetID == (int)StandardWidgetID.WID_FRAME)
				{
					return node.WDBBase;
				}
				else
				{
					return null;
				}
			}
			if (node.Parent != null)
			{
				return FindTopMostFrame((WidgetElem)node.Parent);
			}
			return null;
		}
		
		void CanvasPaint(object sender, PaintEventArgs e)
		{
			CanvasPaintRecursive((WidgetElem)objtree.Nodes[0]);
		}
		
		void CanvasPaintRecursive(WidgetElem elem)
		{
			foreach (WidgetElem e in elem.Nodes)
			{
				CanvasPaintRecursive(e);
			}
			if (elem.WDBBase.IsSelected)
			{
				elem.WDBBase.PaintOnSelection();
			}
		}
	}
}
