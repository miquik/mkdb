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
using System.Text.RegularExpressions;
using mkdb.Widgets;


namespace mkdb
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{	
		protected ArrayList _layout;
		// protected RichTextBox richTextBox1;
		// protected RichTextBox richTextBox2;
		
		#region Syntax Highlighter
		protected Python.SyntaxSettings _syntax_settings;
		private string m_strLine = "";
		private int m_nContentLength = 0;
		private int m_nLineLength = 0;
		private int m_nLineStart = 0;
		private int m_nLineEnd = 0;
		private string m_strKeywords = "";
		private int m_nCurSelection = 0;
		protected Python.SyntaxRichTextBox richTextBox1;
		protected Python.SyntaxRichTextBox richTextBox2;
		#endregion
		
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
			/*
			this.SuspendLayout();
			// First Text Box
			this.richTextBox1 = new RichTextBox();
			this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.richTextBox1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.richTextBox1.Location = new System.Drawing.Point(0, 0);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.ReadOnly = true;
			this.richTextBox1.TextChanged += new System.EventHandler(this.RichTextBox1TextChanged);
			// this.richTextBox1.TextChanged += new System.Windows.Forms.Text(this.TabPage2Paint);			
			this.tabPage2.Controls.Add(richTextBox1);			
			// Second Text Box
			this.richTextBox2 = new RichTextBox();
			this.richTextBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.richTextBox2.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.richTextBox2.Location = new System.Drawing.Point(0, 0);
			this.richTextBox2.Name = "richTextBox2";
			this.richTextBox2.ReadOnly = true;
			this.richTextBox2.TextChanged += new System.EventHandler(this.RichTextBox2TextChanged);			
			this.tabPage3.Controls.Add(richTextBox2);			
			this.ResumeLayout();
			_syntax_settings = new mkdb.Python.SyntaxSettings();
			*/
			this.SuspendLayout();
			// First Text Box
			this.richTextBox1 = new mkdb.Python.SyntaxRichTextBox();
			this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.richTextBox1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.richTextBox1.Location = new System.Drawing.Point(0, 0);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.ReadOnly = true;
			this.tabPage2.Paint += new System.Windows.Forms.PaintEventHandler(this.TabPage2Paint);			
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
			int nStartPos = 0;
			m_nLineStart = 0;
			foreach (string it in coll)
			{
				m_nLineStart = nStartPos;
				richTextBox1.LineStart = nStartPos;
				richTextBox1.LineEnd = nStartPos + it.Length;
				richTextBox1.AppendText(it);
				nStartPos += it.Length;				
			}
		}
		
		
		void TabPage3Paint(object sender, PaintEventArgs e)
		{
			StringCollection coll = new StringCollection();
			richTextBox2.Clear();
			Python.PyFileEditor pyed = Common.Instance().PyEditor;
			WidgetTreeNode node = (WidgetTreeNode)objtree.SelectedNode;
			wiwFrame frame = (wiwFrame)node.Widget;
			pyed.Parser.Render(coll, frame.FrameClass);
			int nStartPos = 0;
			m_nLineStart = 0;
			foreach (string it in coll)
			{
				m_nLineStart = nStartPos;
				richTextBox2.LineStart = nStartPos;
				richTextBox2.LineEnd = nStartPos + it.Length;
				richTextBox2.AppendText(it);
				nStartPos += it.Length;				
			}
		}		
		
		#region Syntax Highlighter Impl
		void InitSyntaxHighlighter()
		{
    		richTextBox1.Settings.Keywords.Add("if");
    		richTextBox1.Settings.Keywords.Add("def");
    		richTextBox1.Settings.Keywords.Add("class");

   	 		// Set the comment identifier. 
    		richTextBox1.Settings.Comment = "#";

    		// Set the colors that will be used.
    		richTextBox1.Settings.KeywordColor = Color.Blue;
    		richTextBox1.Settings.CommentColor = Color.Green;
    		richTextBox1.Settings.StringColor = Color.Gray;
    		richTextBox1.Settings.IntegerColor = Color.Red;

    		// Let's not process strings and integers.
    		richTextBox1.Settings.EnableStrings = true;
    		richTextBox1.Settings.EnableIntegers = true;
    		richTextBox1.CompileKeywords();
			/*
			// Add the keywords to the list.
    		_syntax_settings.Keywords.Add("if");
    		_syntax_settings.Keywords.Add("def");
    		_syntax_settings.Keywords.Add("class");

   	 		// Set the comment identifier. 
    		_syntax_settings.Comment = "#";

    		// Set the colors that will be used.
    		_syntax_settings.KeywordColor = Color.Blue;
    		_syntax_settings.CommentColor = Color.Green;
    		_syntax_settings.StringColor = Color.Gray;
    		_syntax_settings.IntegerColor = Color.Red;

    		// Let's not process strings and integers.
    		_syntax_settings.EnableStrings = true;
    		_syntax_settings.EnableIntegers = true;
    		*/
    		// CompileKeywords();
		}		
		
		private void CompileKeywords()
		{
			for (int i = 0; i < _syntax_settings.Keywords.Count; i++)
			{
				string strKeyword =  _syntax_settings.Keywords[i];

				if (i ==  _syntax_settings.Keywords.Count-1)
					m_strKeywords += "\\b" + strKeyword + "\\b";
				else
					m_strKeywords += "\\b" + strKeyword + "\\b|";
			}
		}		
		
		void RichTextBox1TextChanged(object sender, EventArgs e)
		{
			// Calculate shit here.
			m_nContentLength = richTextBox1.TextLength;

			int nCurrentSelectionStart = richTextBox1.SelectionStart;
			int nCurrentSelectionLength = richTextBox1.SelectionLength;

			// Find the start of the current line.
			m_nLineStart = nCurrentSelectionStart;
			while ((m_nLineStart > 0) && (richTextBox1.Text[m_nLineStart - 1] != '\n'))
				m_nLineStart--;
			// Find the end of the current line.
			m_nLineEnd = nCurrentSelectionStart;
			while ((m_nLineEnd < richTextBox1.Text.Length) && (richTextBox1.Text[m_nLineEnd] != '\n'))
				m_nLineEnd++;
			// Calculate the length of the line.
			m_nLineLength = m_nLineEnd - m_nLineStart;
			// Get the current line.
			m_strLine = richTextBox1.Text.Substring(m_nLineStart, m_nLineLength);

			// Process this line.
			ProcessLine(richTextBox1);			
		}
		
		void RichTextBox2TextChanged(object sender, EventArgs e)
		{
			// Calculate shit here.
			m_nContentLength = richTextBox2.TextLength;

			int nCurrentSelectionStart = richTextBox2.SelectionStart;
			int nCurrentSelectionLength = richTextBox2.SelectionLength;

			// Find the start of the current line.
			m_nLineStart = nCurrentSelectionStart;
			while ((m_nLineStart > 0) && (richTextBox2.Text[m_nLineStart - 1] != '\n'))
				m_nLineStart--;
			// Find the end of the current line.
			m_nLineEnd = nCurrentSelectionStart;
			while ((m_nLineEnd < richTextBox2.Text.Length) && (richTextBox2.Text[m_nLineEnd] != '\n'))
				m_nLineEnd++;
			// Calculate the length of the line.
			m_nLineLength = m_nLineEnd - m_nLineStart;
			// Get the current line.
			m_strLine = richTextBox2.Text.Substring(m_nLineStart, m_nLineLength);			

			// Process this line.
			ProcessLine(richTextBox2);			
		}		
		
		/// <summary>
		/// Process a line.
		/// </summary>
		private void ProcessLine(RichTextBox box)
		{
			// Save the position and make the whole line black
			int nPosition = box.SelectionStart;
			box.SelectionStart = m_nLineStart;
			box.SelectionLength = m_nLineLength;
			box.SelectionColor = Color.Black;

			// Process the keywords
			ProcessRegex(box, m_strKeywords, _syntax_settings.KeywordColor);
			// Process numbers
			if(_syntax_settings.EnableIntegers)
				ProcessRegex(box, "\\b(?:[0-9]*\\.)?[0-9]+\\b", _syntax_settings.IntegerColor);
			// Process strings
			if(_syntax_settings.EnableStrings)
				ProcessRegex(box, "\"[^\"\\\\\\r\\n]*(?:\\\\.[^\"\\\\\\r\\n]*)*\"", _syntax_settings.StringColor);
			// Process comments
			if(_syntax_settings.EnableComments && !string.IsNullOrEmpty(_syntax_settings.Comment))
				ProcessRegex(box, _syntax_settings.Comment + ".*$", _syntax_settings.CommentColor);

			box.SelectionStart = nPosition;
			box.SelectionLength = 0;
			box.SelectionColor = Color.Black;

			m_nCurSelection = nPosition;
		}		
		
		private void ProcessRegex(RichTextBox box, string strRegex, Color color)
		{
			Regex regKeywords = new Regex(strRegex, RegexOptions.IgnoreCase | RegexOptions.Compiled);
			Match regMatch;

			for (regMatch = regKeywords.Match(m_strLine); regMatch.Success; regMatch = regMatch.NextMatch())
			{
				// Process the words
				int nStart = m_nLineStart + regMatch.Index;
				int nLenght = regMatch.Length;
				box.SelectionStart = nStart;
				box.SelectionLength = nLenght;
				box.SelectionColor = color;
			}
		}		
		#endregion
	}
}
