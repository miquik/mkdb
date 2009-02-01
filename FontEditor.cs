/*
 * Creato da SharpDevelop.
 * Utente: Family Rose
 * Data: 01/02/2009
 * Ora: 21.02
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace mkdb
{

	public class wxFont : wx.Font
	{
		public override string ToString()
		{
			return this.FaceName + "; " + this.PointSize + "pt";
		}
	}
	

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
   				if (winFont.Bold) wfnt.Family &= wx.FontFamily.wxBOLD;
   				if (winFont.Italic) wfnt.Family &= wx.FontFamily.wxITALIC;   				
   				if (winFont.Underline) wfnt.Underlined = true; else wfnt.Underlined = false;
   				wfnt.PointSize = (int)winFont.SizeInPoints;
			}            
			return wfnt;
		}
	}	

}
