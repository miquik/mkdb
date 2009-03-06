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

namespace mkdb.Widgets.Frame
{
	
	/// <summary>
	/// Tool Button wdbFrame
	/// </summary>
	public class wtbFrame : ToolStripButton
	{
		protected int _img_index;
		
		public wtbFrame(string name, string image_resource) : base()
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
		
		void ToolStripButtonClick(object sender, EventArgs e)
		{	
			TreeView objtree = Common.Instance().ObjTree;
			WidgetTreeNode node = new WidgetTreeNode("Frame");
			//
			//			
			wx.Frame hfrm = new wx.Frame(null, -1, "");		
			node.Widget = new wiwFrame(hfrm);
			wx.Window win = (wx.Window)node.Widget;
			node.Widget.InsertWidget();
			Win32Utils.SetParent(win.GetHandle(), Common.Instance().Canvas.Handle);
			// wdbFrame wdbf = new wdbFrame(null, null);
			node.ImageIndex = _img_index;
			node.SelectedImageIndex = _img_index;
			WidgetTreeNode app = (WidgetTreeNode)objtree.Nodes[0];
			app.Nodes.Add(node);
			objtree.SelectedNode = node;
			// _tapp.Nodes.Add(wdbf);			
			// Common.Instance().ObjTree.SelectedNode = wdbf;
		}

		public Image GetEmbeddedImage()
		{
    		try
    		{
    			System.Reflection.Assembly a = System.Reflection.Assembly.GetExecutingAssembly();
        		// myNamespace.resources.images.top_left_corner.gif 
        		System.IO.Stream str = a.GetManifestResourceStream("mkdb.Widgets.Frame.frame.jpg");
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
