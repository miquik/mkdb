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
using System.Collections.Specialized;
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
		protected Python.SyntaxRichTextBox richTextBox1;
		protected Python.SyntaxRichTextBox richTextBox2;
		
		public MainForm()
		{
			InitializeComponent();
			InitSyntaxRichTextBox();
			InitSyntaxHighlighter();
			
			_layout = new ArrayList();
			Common.Instance().ObjPropsPanel = objprops;
			Common.Instance().ObjTree = objtree;
			Common.Instance().Canvas = this.canvas;			
			Common.Instance().ObjTreeImageList = this.objtreeimages;
			
			int idx = objtree.Nodes.Add(new WidgetTreeNode("Project"));
			WidgetTreeNode node = (WidgetTreeNode)objtree.Nodes[idx];
			node.Widget = new wiwApp(null, null);
			node.Widget.InsertWidget();
			objtree.SelectedNode = node;
			
			AddToToolStrip(new Widgets.Frame.wtbFrame("Frame", ""));
			AddToToolStrip(new Widgets.BoxSizer.wtbBoxSizer("BoxSizer", ""));
			AddToToolStrip(new Widgets.GridSizer.wtbGridSizer("GridSizer", ""));			
			AddToToolStrip(new Widgets.Button.wtbButton("Button", ""));			
			AddToToolStrip(new Widgets.Label.wtbLabel("Label", ""));			
			AddToToolStrip(new Widgets.TextEdit.wtbTextEdit("TextEdit", ""));			
			AddToToolStrip(new Widgets.Combobox.wtbCombobox("ComboBox", ""));
			AddToToolStrip(new Widgets.Listbox.wtbListbox("ListBox", ""));						
			AddToToolStrip(new Widgets.Ado.wtbAdo("Ado", ""));						
			
			// Init Python Editor
			InitPythonEditor();
			wiwApp theapp = (wiwApp)node.Widget;
			theapp.AppSection = Common.Instance().PyEditor.ApplicationSection;
		}

		void InitSyntaxRichTextBox()
		{
			this.SuspendLayout();
			// First Text Box
			this.richTextBox1 = new mkdb.Python.SyntaxRichTextBox();
			this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.richTextBox1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.richTextBox1.Location = new System.Drawing.Point(0, 0);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.ReadOnly = true;
			this.tabPage2.Controls.Add(richTextBox1);			
			// Second Text Box
			this.richTextBox2 = new mkdb.Python.SyntaxRichTextBox();
			this.richTextBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.richTextBox2.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.richTextBox2.Location = new System.Drawing.Point(0, 0);
			this.richTextBox2.Name = "richTextBox2";
			this.richTextBox2.ReadOnly = true;
			this.tabPage3.Controls.Add(richTextBox2);			
			this.ResumeLayout();
		}
		
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
				
		void DeleteToolStripMenuItemClick(object sender, EventArgs e)
		{
			WidgetTreeNode node = (WidgetTreeNode)Common.Instance().ObjTree.SelectedNode;
			node.OnDelete();
		}
		
		void InitPythonEditor()
		{			
			Python.PyFileEditor pyed = Common.Instance().PyEditor;
			// NOTHING TO DO
		}		
		
		void TabPage2Paint(object sender, PaintEventArgs e)
		{
			StringCollection coll = new StringCollection();
			richTextBox1.Clear();
			Python.PyFileEditor pyed = Common.Instance().PyEditor;
			pyed.Parser.Render(coll, pyed.ApplicationSection);
			foreach (string it in coll)
			{
				richTextBox1.AppendText(it);
			}
		}
		
		
		void TabPage3Paint(object sender, PaintEventArgs e)
		{
			richTextBox2.Clear();
		}		
		
		void InitSyntaxHighlighter()
		{
			// Add the keywords to the list.
    		richTextBox1.Settings.Keywords.Add("if");
    		richTextBox1.Settings.Keywords.Add("def");
    		richTextBox1.Settings.Keywords.Add("class");

   	 		// Set the comment identifier. 
    		richTextBox1.Settings.Comment = "# ---";

    		// Set the colors that will be used.
    		richTextBox1.Settings.KeywordColor = Color.Blue;
    		richTextBox1.Settings.CommentColor = Color.Green;
    		richTextBox1.Settings.StringColor = Color.Gray;
    		richTextBox1.Settings.IntegerColor = Color.Red;

    		// Let's not process strings and integers.
    		richTextBox1.Settings.EnableStrings = true;
    		richTextBox1.Settings.EnableIntegers = true;

    		// Let's make the settings we just set valid by compiling
    		// the keywords to a regular expression.
			richTextBox1.CompileKeywords();
			
			// Add the keywords to the list.
    		richTextBox2.Settings.Keywords.Add("if");
    		richTextBox2.Settings.Keywords.Add("def");
    		richTextBox2.Settings.Keywords.Add("class");

   	 		// Set the comment identifier. 
    		richTextBox2.Settings.Comment = "# ---";

    		// Set the colors that will be used.
    		richTextBox2.Settings.KeywordColor = Color.Blue;
    		richTextBox2.Settings.CommentColor = Color.Green;
    		richTextBox2.Settings.StringColor = Color.Gray;
    		richTextBox2.Settings.IntegerColor = Color.Red;

    		// Let's not process strings and integers.
    		richTextBox2.Settings.EnableStrings = true;
    		richTextBox2.Settings.EnableIntegers = true;

    		// Let's make the settings we just set valid by compiling
    		// the keywords to a regular expression.
			richTextBox1.CompileKeywords();
		}
	}
}
