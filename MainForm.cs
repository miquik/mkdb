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
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void ToolStripButton3Click(object sender, EventArgs e)
		{
			// Create a wxWindow on the top of canvas panel.
			Common.Instance().ObjPropsPanel = objprops;
			wdbFrame testframe = new wdbFrame();
			testframe.InsertWidget(canvas);
			/*
			IntPtr wxh;
			wx.Frame testframe = new wx.Frame(null, 10001,"Prova", new Point(0,0));
			testframe.Title = "wxFrame1";
			// testframe.Move
			wxh = Win32Utils.FindWindow("wxWindowClassNR", "wxFrame1");
			Win32Utils.SetParent(wxh, canvas.Handle);
			testframe.Show();
			*/
			// wxWindowClassNR
		}
		
		void ToolStripButton4Click(object sender, EventArgs e)
		{
			wxWindowProps testprop = new wxWindowProps();
			objprops.SelectedObject = testprop;
			// objprops.SelectedObject = new CheckBoxInPropertyGrid();
		}
	}
}
