/*
 * Creato da SharpDevelop.
 * Utente: Family Rose
 * Data: 24/12/2008
 * Ora: 13.51
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace mkdb
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.nuovoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.apriToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.salvaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.salvaConNomeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.esciToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.objtree = new System.Windows.Forms.TreeView();
			this.label1 = new System.Windows.Forms.Label();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.canvas = new System.Windows.Forms.Panel();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.label4 = new System.Windows.Forms.Label();
			this.objprops = new System.Windows.Forms.PropertyGrid();
			this.label3 = new System.Windows.Forms.Label();
			this.palette = new System.Windows.Forms.ToolStrip();
			this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
			this.label2 = new System.Windows.Forms.Label();
			this.objtreeimages = new System.Windows.Forms.ImageList(this.components);
			this.menuStrip1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.palette.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusStrip1
			// 
			this.statusStrip1.Location = new System.Drawing.Point(0, 381);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(719, 22);
			this.statusStrip1.TabIndex = 0;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.fileToolStripMenuItem,
									this.aboutToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(719, 24);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.nuovoToolStripMenuItem,
									this.apriToolStripMenuItem,
									this.salvaToolStripMenuItem,
									this.salvaConNomeToolStripMenuItem,
									this.esciToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// nuovoToolStripMenuItem
			// 
			this.nuovoToolStripMenuItem.Name = "nuovoToolStripMenuItem";
			this.nuovoToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
			this.nuovoToolStripMenuItem.Text = "Nuovo";
			// 
			// apriToolStripMenuItem
			// 
			this.apriToolStripMenuItem.Name = "apriToolStripMenuItem";
			this.apriToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
			this.apriToolStripMenuItem.Text = "Apri";
			// 
			// salvaToolStripMenuItem
			// 
			this.salvaToolStripMenuItem.Name = "salvaToolStripMenuItem";
			this.salvaToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
			this.salvaToolStripMenuItem.Text = "Salva";
			// 
			// salvaConNomeToolStripMenuItem
			// 
			this.salvaConNomeToolStripMenuItem.Name = "salvaConNomeToolStripMenuItem";
			this.salvaConNomeToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
			this.salvaConNomeToolStripMenuItem.Text = "Salva con nome";
			// 
			// esciToolStripMenuItem
			// 
			this.esciToolStripMenuItem.Name = "esciToolStripMenuItem";
			this.esciToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
			this.esciToolStripMenuItem.Text = "Esci";
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
			this.aboutToolStripMenuItem.Text = "About";
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.toolStripButton1,
									this.toolStripButton2});
			this.toolStrip1.Location = new System.Drawing.Point(0, 24);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(719, 25);
			this.toolStrip1.TabIndex = 2;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
			this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton1.Text = "toolStripButton1";
			// 
			// toolStripButton2
			// 
			this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
			this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton2.Name = "toolStripButton2";
			this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton2.Text = "toolStripButton2";
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 49);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.objtree);
			this.splitContainer1.Panel1.Controls.Add(this.label1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
			this.splitContainer1.Panel2.Controls.Add(this.palette);
			this.splitContainer1.Panel2.Controls.Add(this.label2);
			this.splitContainer1.Size = new System.Drawing.Size(719, 332);
			this.splitContainer1.SplitterDistance = 214;
			this.splitContainer1.TabIndex = 3;
			// 
			// objtree
			// 
			this.objtree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.objtree.ImageIndex = 0;
			this.objtree.ImageList = this.objtreeimages;
			this.objtree.Location = new System.Drawing.Point(0, 21);
			this.objtree.Name = "objtree";
			this.objtree.SelectedImageIndex = 0;
			this.objtree.Size = new System.Drawing.Size(214, 311);
			this.objtree.TabIndex = 1;
			this.objtree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ObjtreeAfterSelect);
			this.objtree.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.ObjtreeBeforeSelect);
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.SystemColors.HotTrack;
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(214, 21);
			this.label1.TabIndex = 0;
			this.label1.Text = "Object Tree";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point(0, 48);
			this.splitContainer2.Name = "splitContainer2";
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.tabControl1);
			this.splitContainer2.Panel1.Controls.Add(this.label4);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.objprops);
			this.splitContainer2.Panel2.Controls.Add(this.label3);
			this.splitContainer2.Size = new System.Drawing.Size(501, 284);
			this.splitContainer2.SplitterDistance = 324;
			this.splitContainer2.TabIndex = 2;
			// 
			// tabControl1
			// 
			this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 23);
			this.tabControl1.Multiline = true;
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(324, 261);
			this.tabControl1.TabIndex = 1;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.canvas);
			this.tabPage1.Location = new System.Drawing.Point(4, 4);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(316, 235);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Editor";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// canvas
			// 
			this.canvas.BackColor = System.Drawing.Color.DimGray;
			this.canvas.Dock = System.Windows.Forms.DockStyle.Fill;
			this.canvas.Location = new System.Drawing.Point(3, 3);
			this.canvas.Margin = new System.Windows.Forms.Padding(0);
			this.canvas.Name = "canvas";
			this.canvas.Size = new System.Drawing.Size(310, 229);
			this.canvas.TabIndex = 0;
			this.canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.CanvasPaint);
			// 
			// tabPage2
			// 
			this.tabPage2.Location = new System.Drawing.Point(4, 4);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(316, 235);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Python";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.SystemColors.HotTrack;
			this.label4.Dock = System.Windows.Forms.DockStyle.Top;
			this.label4.Location = new System.Drawing.Point(0, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(324, 23);
			this.label4.TabIndex = 0;
			this.label4.Text = "Editor";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// objprops
			// 
			this.objprops.Dock = System.Windows.Forms.DockStyle.Fill;
			this.objprops.Location = new System.Drawing.Point(0, 23);
			this.objprops.Name = "objprops";
			this.objprops.Size = new System.Drawing.Size(173, 261);
			this.objprops.TabIndex = 1;
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.SystemColors.HotTrack;
			this.label3.Dock = System.Windows.Forms.DockStyle.Top;
			this.label3.Location = new System.Drawing.Point(0, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(173, 23);
			this.label3.TabIndex = 0;
			this.label3.Text = "Properties";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// palette
			// 
			this.palette.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.toolStripButton3,
									this.toolStripButton4});
			this.palette.Location = new System.Drawing.Point(0, 23);
			this.palette.Name = "palette";
			this.palette.Size = new System.Drawing.Size(501, 25);
			this.palette.TabIndex = 1;
			this.palette.Text = "toolStrip2";
			// 
			// toolStripButton3
			// 
			this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
			this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton3.Name = "toolStripButton3";
			this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton3.Text = "toolStripButton3";
			this.toolStripButton3.Click += new System.EventHandler(this.ToolStripButton3Click);
			// 
			// toolStripButton4
			// 
			this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
			this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton4.Name = "toolStripButton4";
			this.toolStripButton4.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton4.Text = "toolStripButton4";
			this.toolStripButton4.Click += new System.EventHandler(this.ToolStripButton4Click);
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.SystemColors.HotTrack;
			this.label2.Dock = System.Windows.Forms.DockStyle.Top;
			this.label2.Location = new System.Drawing.Point(0, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(501, 23);
			this.label2.TabIndex = 0;
			this.label2.Text = "Palette";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// objtreeimages
			// 
			this.objtreeimages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("objtreeimages.ImageStream")));
			this.objtreeimages.TransparentColor = System.Drawing.Color.Transparent;
			this.objtreeimages.Images.SetKeyName(0, "button.jpg");
			this.objtreeimages.Images.SetKeyName(1, "choice.jpg");
			this.objtreeimages.Images.SetKeyName(2, "combo_box.jpg");
			this.objtreeimages.Images.SetKeyName(3, "dialog.jpg");
			this.objtreeimages.Images.SetKeyName(4, "flex_grid_sizer.jpg");
			this.objtreeimages.Images.SetKeyName(5, "frame.jpg");
			this.objtreeimages.Images.SetKeyName(6, "grid_bag_sizer.jpg");
			this.objtreeimages.Images.SetKeyName(7, "grid_sizer.jpg");
			this.objtreeimages.Images.SetKeyName(8, "panel.jpg");
			this.objtreeimages.Images.SetKeyName(9, "sizer.jpg");
			this.objtreeimages.Images.SetKeyName(10, "sizer16x16.jpg");
			this.objtreeimages.Images.SetKeyName(11, "spacer.jpg");
			this.objtreeimages.Images.SetKeyName(12, "staticbox_sizer.jpg");
			this.objtreeimages.Images.SetKeyName(13, "stddialogbuttonsizer.jpg");
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(719, 403);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "MainForm";
			this.Text = "dbGui";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			this.splitContainer2.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.palette.ResumeLayout(false);
			this.palette.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.ImageList objtreeimages;
		private System.Windows.Forms.ToolStripButton toolStripButton4;
		private System.Windows.Forms.ToolStripButton toolStripButton3;
		private System.Windows.Forms.ToolStripButton toolStripButton2;
		private System.Windows.Forms.ToolStripButton toolStripButton1;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem esciToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem salvaConNomeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem salvaToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem apriToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem nuovoToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.PropertyGrid objprops;
		private System.Windows.Forms.ToolStrip palette;
		private System.Windows.Forms.Panel canvas;
		private System.Windows.Forms.TreeView objtree;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.StatusStrip statusStrip1;
	}
}
