/*
 * Creato da SharpDevelop.
 * Utente: michele
 * Data: 23/02/2009
 * Ora: 9.08
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;
using System.Drawing;

namespace mkdb.Widgets.BoxSizer
{	
	/// <summary>
	/// Tool Button wdbBoxSizer
	/// </summary>
	public class wtbBoxSizer : ToolStripButton
	{
		protected int _img_index;
		
		public wtbBoxSizer(string name, string image_resource) : base()
		{
			this.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			// this.Image = ((System.Drawing.Image)(resources.GetObject(image_resource)));
			this.Image = (System.Drawing.Image)GetEmbeddedImage();
			this.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.Name = name;
			this.Size = new System.Drawing.Size(23, 22);
			this.Text = name;
			this.Click += new System.EventHandler(this.ToolStripButtonClick);	
			Common.Instance().ObjTreeImageList.Images.Add(name, this.Image);						
			_img_index = Common.Instance().ObjTreeImageList.Images.Count - 1;					
			// _img_index = Common.Instance().ObjTreeImageList.Images.IndexOf(this.Image);
		}
		
		// BoxSizer
		// Così per ogni sizer
		void ToolStripButtonClick(object sender, EventArgs e)
		{	
			TreeView objtree = Common.Instance().ObjTree;
			WidgetTreeNode ps = Common.Instance().FindBestParentSizer((WidgetTreeNode)objtree.SelectedNode, true);
			WidgetTreeNode pc = Common.Instance().FindBestParentContainer((WidgetTreeNode)objtree.SelectedNode, true);
			if (pc == null && ps == null)	
				return;
			
			// Add this sizer
			wx.Sizer siz = null;
			wx.Window win = null;
			Common.Instance().CheckParentForSizer(pc, ps, out win, out siz);
			wdbBoxSizer bsizer = new wdbBoxSizer(win, siz);
			if (ps != null)	
				ps.Nodes.Add(bsizer);
			else			
				pc.Nodes.Add(bsizer);
			bsizer.ImageIndex = _img_index;
			bsizer.SelectedImageIndex = _img_index;			
			objtree.SelectedNode = bsizer;
		}

		public Image GetEmbeddedImage()
		{
    		try
    		{
    			System.Reflection.Assembly a = System.Reflection.Assembly.GetExecutingAssembly();
        		// myNamespace.resources.images.top_left_corner.gif 
        		System.IO.Stream str = a.GetManifestResourceStream("mkdb.Widgets.BoxSizer.sizer.jpg");
        		if(str == null)
            		throw new Exception("Could not locate embedded resource ");
        		return new System.Drawing.Bitmap(str);
    		}
    		catch(Exception e)
    		{
        		throw new Exception("mkdb : " + e.Message);
    		}
		}
	}
}
