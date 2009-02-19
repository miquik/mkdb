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
			int idx = objtree.Nodes.Add(new wdbApp(null, null));
			wdbApp wdba = (wdbApp)objtree.Nodes[idx];
			// wdba.CreateWidget(null);			
			objtree.SelectedNode = wdba;
		}
		
		wx.Window FindBestParentContainer(WidgetElem curNode)
		{
			if (curNode.WDBBase.IsSizer == true)
			{
				return curNode.WDBBase.ParentContainer;
			}
			return (wx.Window)curNode.WDBBase;
			/*
			WidgetElem parent;
			// Find an available Sizer starting from current node
			if (curNode.WDBBase.CanAcceptChildren())
				return curNode;
			parent = (WidgetElem)curNode.Parent;
			if (parent == null)
				return null;		
			return FindBestParent(parent);
			*/
		}
		
		WidgetElem FindBestParentSizer(WidgetElem curNode)
		{
			WidgetElem parent;
			// Find an available Sizer starting from current node
			if (curNode.WDBBase.CanAcceptChildren() && curNode.WDBBase.IsSizer)
				return curNode;
			parent = (WidgetElem)curNode.Parent;
			if (parent == null)
				return null;		
			return FindBestParentSizer(parent);
			/*
			WidgetElem parent;
			// Find an available Sizer starting from current node
			if (curNode.WDBBase.CanAcceptChildren())
				return curNode;
			parent = (WidgetElem)curNode.Parent;
			if (parent == null)
				return null;		
			return FindBestParentSizer(parent);
			*/
		}		
		
		void ToolStripButton3Click(object sender, EventArgs e)
		{	
			wdbApp _tapp = (wdbApp)objtree.Nodes[0];			
			wdbFrame wdbf = new wdbFrame(null, null);
			// wdbf.CreateWidget(_tapp.WDBBase);
			ChangeCurrentWindow(wdbf);
			objtree.SelectedNode = _tapp;
			objtree.SelectedNode.Nodes.Add(wdbf);			
			objtree.SelectedNode = wdbf;
		}
		
		void ToolStripButton4Click(object sender, EventArgs e)
		{
			/* Create Sizer */
			WidgetElem ps;
			wx.Window pc;
			ps = FindBestParentSizer((WidgetElem)objtree.SelectedNode);
			pc = FindBestParentContainer((WidgetElem)objtree.SelectedNode);
			if (ps == null && pc == null)
			{
				// TODO : Error here!!
			} else {
				// Add this sizer
				// !!!
				wx.Sizer siz;
				if (ps == null) siz = null;
				else siz = (wx.Sizer)ps.WDBBase;
				wdbBoxSizer bsizer = new wdbBoxSizer(pc, siz);
				// bsizer.CreateWidget(parent.WDBBase);
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
			if (neww.WDBBase.WidgetType == (int)StandardWidgetType.WID_APP)
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
  				Common.Instance().CurrentWindow = _cur.ParentContainer;
				Common.Instance().CurrentWindow.Show();
  			} 	  		
    	}

		public IWDBBase FindTopMostFrame(WidgetElem node)
		{
			if (node.Level == 1)
			{
				if (node.WDBBase.WidgetType == (int)StandardWidgetType.WID_FRAME)
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
