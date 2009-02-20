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
		
		// Frame
		void ToolStripButton3Click(object sender, EventArgs e)
		{	
			wdbApp _tapp = (wdbApp)objtree.Nodes[0];						
			wdbFrame wdbf = new wdbFrame(null, null);
			// wdbf.CreateWidget(_tapp.WDBBase);
			ChangeCurrentWindow(wdbf);
			_tapp.Nodes.Add(wdbf);
			objtree.SelectedNode = wdbf;
		}
		
		// BoxSizer
		// Così per ogni sizer
		void ToolStripButton4Click(object sender, EventArgs e)
		{
			/* Create Sizer */
			WidgetElem ps = FindBestParentSizer((WidgetElem)objtree.SelectedNode, true);
			WidgetElem pc = FindBestParentContainer((WidgetElem)objtree.SelectedNode, true);
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
			/* Create Sizer */
			WidgetElem ps = FindBestParentSizer((WidgetElem)objtree.SelectedNode, true);
			WidgetElem pc = FindBestParentContainer((WidgetElem)objtree.SelectedNode, true);
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
			/* Create Sizer */
			WidgetElem ps = FindBestParentSizer((WidgetElem)objtree.SelectedNode, false);
			WidgetElem pc = FindBestParentContainer((WidgetElem)objtree.SelectedNode, false);			
			if (ps == null)
				return;
			
			wx.Window win;
			CheckParentForWidget(pc, ps, out win);
			wdbButton btn = new wdbButton(win, (wx.Sizer)ps.WDBBase);
			ps.Nodes.Add(btn);
			objtree.SelectedNode = btn;
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
  				Common.Instance().CurrentWindow = _cur.Me;
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
		
		WidgetElem FindBestParentContainer(WidgetElem curNode, bool sizerIsAsking)
		{
			if (sizerIsAsking == false)
			{
				return null;
			}
			if (curNode.WDBBase.IsSizer == false)
			{
				if (curNode.WDBBase.CanAcceptChildren())
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
					return FindBestParentContainer((WidgetElem)curNode.Parent, true);
				}
			}
		}
		
		WidgetElem FindBestParentSizer(WidgetElem curNode, bool sizerIsAsking)
		{
			if (curNode.WDBBase.IsSizer == false)
			{
				return null;
			}
			if (curNode.WDBBase.CanAcceptChildren())
			{
				return curNode;
			} else
			{
				if (curNode.Parent == null)
				{
					return null;
				} else
				{
					return FindBestParentSizer((WidgetElem)curNode.Parent, false);
				}
			}
		}						
		
		void CheckParentForSizer(WidgetElem _c, WidgetElem _s, out wx.Window _rc, out wx.Sizer _rs)
		{
			_rc = null;
			_rs = null;
			if (_c != null) _rc = (wx.Window)_c.WDBBase;
			if (_s != null) 
			{
				_rs = (wx.Sizer)_s.WDBBase;
				_rc = _s.WDBBase.ParentContainer;
			}			
		}
		
		void CheckParentForWidget(WidgetElem _c, WidgetElem _s, out wx.Window _rc)
		{
			_rc = null;
			if (_c == null)
			{
				_rc = (wx.Window)_s.WDBBase.ParentContainer;
			} else
			{
				_rc = (wx.Window)_c.WDBBase;
			}
		}
		
		
	}
}
