/*
 * Creato da SharpDevelop.
 * Utente: michele
 * Data: 29/01/2009
 * Ora: 8.41
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using wx;

namespace mkdb
{
	// Stupid thing
	public class wxFont : wx.Font
	{
		public override string ToString()
		{
			return this.FaceName + "; " + this.PointSize + "pt";
		}
	}
	
	public class wxColor : wx.Colour
	{
		public override string ToString()
		{
			return this.Red + "; " + this.Green + "; " + this.Blue;
		}
	}
	
	/// <summary>
	/// Description of wxFontEditors.
	/// </summary>
	public class wxColorEditors : UITypeEditor
	{
		
		public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
		{
			// We will use a window for property editing.
			return UITypeEditorEditStyle.Modal;
		} 		

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
			System.Drawing.Color winColor;
            if (context == null || context.Instance == null || provider == null)
                return value;

            wxColor color = (wxColor)value;
            System.Windows.Forms.ColorDialog colDialog = new System.Windows.Forms.ColorDialog();
            colDialog.Color = new System.Drawing.Color();
            colDialog.Color = System.Drawing.Color.FromArgb(255, color.Red, color.Green, color.Blue);
            if (colDialog.ShowDialog() != DialogResult.Cancel)
			{
   				winColor = colDialog.Color;
   				color.Set(winColor.R, winColor.G, winColor.B);
			}            
			return color;
		}
		
		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			return true;
		}
		
		public override void PaintValue(PaintValueEventArgs e)
        {
			if (e.Value != null)
			{			
 				if (e.Value is wxColor) 
   				{
 					System.Drawing.Color col = new System.Drawing.Color();
 					col = System.Drawing.Color.FromArgb(255, ((wxColor)e.Value).Red,
 					                                    ((wxColor)e.Value).Green,
 					                                    ((wxColor)e.Value).Blue);
 					System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(col);
         			e.Graphics.FillRectangle(brush, e.Bounds); 					
 				}
 			}
   			base.PaintValue(e);
			
			/*
			if (e.Value != null)
			{
				HatchStyle style = (HatchStyle)e.Value;
				Rectangle bounds = e.Bounds;
				Brush brush = new HatchBrush(style, 
					SystemColors.WindowText, SystemColors.Window);
                try
				{
					GraphicsState state = e.Graphics.Save();
					e.Graphics.RenderingOrigin = new Point(bounds.X, bounds.Y);
					e.Graphics.FillRectangle(brush, bounds);
					e.Graphics.Restore(state);
                }
				finally
				{
					brush.Dispose();
                }
            }*/            
		}		
	}
	
	
	/// <summary>
	/// Description of wxFontEditors.
	/// </summary>
	public class wxFontEditors : UITypeEditor
	{
		
		public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
		{
			// We will use a window for property editing.
			return UITypeEditorEditStyle.Modal;
		} 		

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            System.Drawing.Font winFont = null;			
            
            if (context == null || context.Instance == null || provider == null)
                return value;
            
            wxFont wfnt = (wxFont)value;
			System.Windows.Forms.FontDialog fontDialog = new System.Windows.Forms.FontDialog();
			fontDialog.Font = new System.Drawing.Font(wfnt.FaceName, wfnt.PointSize);
			if (fontDialog.ShowDialog() != DialogResult.Cancel)
			{
   				winFont = fontDialog.Font;
   				wfnt.FaceName = winFont.Name;
   				if (winFont.Bold) wfnt.Family &= FontFamily.wxBOLD;
   				if (winFont.Italic) wfnt.Family &= FontFamily.wxITALIC;   				
   				if (winFont.Underline) wfnt.Underlined = true; else wfnt.Underlined = false;
   				wfnt.PointSize = (int)winFont.SizeInPoints;
			}            
			return wfnt;
		}
	}	
}
