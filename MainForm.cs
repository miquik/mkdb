/*
 * Creato da SharpDevelop.
 * Utente: Family Rose
 * Data: 24/12/2008
 * Ora: 13.51
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;
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
		protected ArrayList _layout;
		
		public MainForm()
		{
			InitializeComponent();
			_layout = new ArrayList();
			Common.Instance().ObjPropsPanel = objprops;
			Common.Instance().ObjTree = objtree;
			Common.Instance().Canvas = this.canvas;			
			Common.Instance().ObjTreeImageList = this.objtreeimages;
			
			int idx = objtree.Nodes.Add(new wdbApp(null, null));
			wdbApp wdba = (wdbApp)objtree.Nodes[idx];
			objtree.SelectedNode = wdba;
			AddToToolStrip(new Widgets.Frame.wtbFrame("Frame", ""));
			AddToToolStrip(new Widgets.BoxSizer.wtbBoxSizer("BoxSizer", ""));
			AddToToolStrip(new Widgets.GridSizer.wtbGridSizer("GridSizer", ""));			
			AddToToolStrip(new Widgets.Button.wtbButton("Button", ""));			
			AddToToolStrip(new Widgets.Label.wtbLabel("Label", ""));			
		}
		/*
		// Frame
		void ToolStripButton3Click(object sender, EventArgs e)
		{	
			wdbApp _tapp = (wdbApp)objtree.Nodes[0];						
			wdbFrame wdbf = new wdbFrame(null, null);
			// wdbf.CreateWidget(_tapp.Widget);
			ChangeCurrentWindow(wdbf);
			_tapp.Nodes.Add(wdbf);
			objtree.SelectedNode = wdbf;
		}
		
		// BoxSizer
		// Così per ogni sizer
		void ToolStripButton4Click(object sender, EventArgs e)
		{
			WidgetTreeNode ps = FindBestParentSizer((WidgetTreeNode)objtree.SelectedNode, true);
			WidgetTreeNode pc = FindBestParentContainer((WidgetTreeNode)objtree.SelectedNode, true);
			if (pc == null && ps == null)	
				return;
			
			// Add this sizer
			wx.Sizer siz = null;
			wx.Window win = null;
			CheckParentForSizer(pc, ps, out win, out siz);
			wdbBoxSizer bsizer = new wdbBoxSizer(win, siz);
			if (ps != null)	
				ps.Nodes.Add(bsizer);
			else			
				pc.Nodes.Add(bsizer);
			objtree.SelectedNode = bsizer;
		}
		// Grid Sizer
		void ToolStripButton6Click(object sender, EventArgs e)
		{
			WidgetTreeNode ps = FindBestParentSizer((WidgetTreeNode)objtree.SelectedNode, true);
			WidgetTreeNode pc = FindBestParentContainer((WidgetTreeNode)objtree.SelectedNode, true);
			if (pc == null && ps == null)	
				return;
			
			// Add this sizer
			wx.Sizer siz = null;
			wx.Window win = null;
			CheckParentForSizer(pc, ps, out win, out siz);
			wdbGridSizer gsizer = new wdbGridSizer(win, siz);
			if (ps != null)	
				ps.Nodes.Add(gsizer);
			else			
				pc.Nodes.Add(gsizer);
			objtree.SelectedNode = gsizer;			
		}
		
		
		// Button
		// Così per ogni widget
		void ToolStripButton5Click(object sender, EventArgs e)
		{
			WidgetTreeNode ps = FindBestParentSizer((WidgetTreeNode)objtree.SelectedNode, false);
			WidgetTreeNode pc = FindBestParentContainer((WidgetTreeNode)objtree.SelectedNode, false);			
			if (ps == null)
				return;
			
			wx.Window win;
			CheckParentForWidget(pc, ps, out win);
			wdbButton btn = new wdbButton(win, (wx.Sizer)ps.Widget);
			ps.Nodes.Add(btn);
			objtree.SelectedNode = btn;
		}		
		*/
		
		void ObjtreeBeforeSelect(object sender, TreeViewCancelEventArgs e)
		{			
			IWDBBase elem = (IWDBBase)Common.Instance().CurrentElement;
			if (elem != null)
			{
				elem.IsSelected = false;
				elem.HighlightSelection();
			}
		}		
		
		void ObjtreeAfterSelect(object sender, TreeViewEventArgs e)
		{
			WidgetTreeNode elem = (WidgetTreeNode)objtree.SelectedNode;
			if (elem != null)
			{
				ChangeCurrentWindow(elem);
				objprops.SelectedObject = elem.Widget.Properties;
				elem.Widget.IsSelected = true;
				elem.Widget.HighlightSelection();
				Common.Instance().CurrentElement = elem.Widget;
			}
		}
		
		public void ChangeCurrentWindow(WidgetTreeNode neww)
    	{
			if (neww.Widget.WidgetType == (int)StandardWidgetType.WID_APP)
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
 	  			Common.Instance().CurrentWindow = (wx.Window)_cur;
				Common.Instance().CurrentWindow.Show();
  			} 	  		
    	}

		public IWDBBase FindTopMostFrame(WidgetTreeNode node)
		{
			if (node.Level == 1)
			{
				if (node.Widget.WidgetType == (int)StandardWidgetType.WID_FRAME)
				{
					return node.Widget;
				}
				else
				{
					return null;
				}
			}
			if (node.Parent != null)
			{
				return FindTopMostFrame((WidgetTreeNode)node.Parent);
			}
			return null;
		}
		
		void CanvasPaint(object sender, PaintEventArgs e)
		{
			CanvasPaintRecursive((WidgetTreeNode)objtree.Nodes[0]);
		}
		
		void CanvasPaintRecursive(WidgetTreeNode elem)
		{
			foreach (WidgetTreeNode e in elem.Nodes)
			{
				CanvasPaintRecursive(e);
			}
			if (elem.Widget.IsSelected)
			{
				elem.Widget.HighlightSelection();
			}
		}
		
		WidgetTreeNode FindBestParentContainer(WidgetTreeNode curNode, bool sizerIsAsking)
		{
			if (sizerIsAsking == false)
			{
				return null;
			}
			if (curNode.Widget.IsSizer == false)
			{
				if (curNode.Widget.CanAcceptChildren())
				{
					return curNode;
				} else 
				{
					return null;
				}
			} else
			{
				if (curNode.Parent == null)
				{
					return null;
				} else
				{
					return FindBestParentContainer((WidgetTreeNode)curNode.Parent, true);
				}
			}
		}
		
		WidgetTreeNode FindBestParentSizer(WidgetTreeNode curNode, bool sizerIsAsking)
		{
			if (curNode.Widget.IsSizer == false)
			{
				return null;
			}
			if (curNode.Widget.CanAcceptChildren())
			{
				return curNode;
			} else
			{
				if (curNode.Parent == null)
				{
					return null;
				} else
				{
					return FindBestParentSizer((WidgetTreeNode)curNode.Parent, false);
				}
			}
		}						
		
		void CheckParentForSizer(WidgetTreeNode _c, WidgetTreeNode _s, out wx.Window _rc, out wx.Sizer _rs)
		{
			_rc = null;
			_rs = null;
			if (_c != null) _rc = (wx.Window)_c.Widget;
			if (_s != null) 
			{
				_rs = (wx.Sizer)_s.Widget;
				_rc = _s.Widget.ParentContainer;
			}			
		}
		
		void CheckParentForWidget(WidgetTreeNode _c, WidgetTreeNode _s, out wx.Window _rc)
		{
			_rc = null;
			if (_c == null)
			{
				_rc = (wx.Window)_s.Widget.ParentContainer;
			} else
			{
				_rc = (wx.Window)_c.Widget;
			}
		}
		
		// Menu Layout
		void AddToToolStrip(ToolStripButton button)
		{		
			_layout.Add(button);
			palette.Items.Add(button);
		}
		
	}
}
