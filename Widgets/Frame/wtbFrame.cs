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
	/*
	this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
	this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
	this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
	this.toolStripButton3.Name = "toolStripButton3";
	this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
	this.toolStripButton3.Text = "toolStripButton3";
	this.toolStripButton3.Click += new System.EventHandler(this.ToolStripButton3Click);
	*/
	
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
			/*
			if (Common.Instance().ObjTreeImageList.Images.IndexOfKey(name) == -1)
			{
				Common.Instance().ObjTreeImageList.Images.Add(name, this.Image);	
			}
			*/
			Common.Instance().ObjTreeImageList.Images.Add(name, this.Image);									
			_img_index = Common.Instance().ObjTreeImageList.Images.Count - 1;		
			// _img_index = Common.Instance().ObjTreeImageList.Images.IndexOf(this.Image);
		}
		
		void ToolStripButtonClick(object sender, EventArgs e)
		{	
			wdbApp _tapp = (wdbApp)Common.Instance().ObjTree.Nodes[0];
			wdbFrame wdbf = new wdbFrame(null, null);
			wdbf.ImageIndex = _img_index;
			wdbf.SelectedImageIndex = _img_index;
			_tapp.Nodes.Add(wdbf);			
			Common.Instance().ObjTree.SelectedNode = wdbf;
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
