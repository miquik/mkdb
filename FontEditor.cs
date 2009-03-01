/*
 * Creato da SharpDevelop.
 * Utente: Family Rose
 * Data: 01/02/2009
 * Ora: 21.02
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing;
using System.ComponentModel.Design.Serialization;
using System.Reflection;


namespace mkdb
{
	
	public class wxFont : wx.Font
	{
		public wxFont()
		{
		}
		
		public wxFont(string name, int size)
		{
			this.FaceName = name;
			this.PointSize = size;
		}		
		
		public override string ToString()
		{
			return this.FaceName + "; " + this.PointSize + "pt";
		}		
	}
	

	public class wxFontEditor : UITypeEditor
	{
		
		public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
		{
			// We will use a window for property editing.
			return UITypeEditorEditStyle.Modal;
		} 		

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context == null || context.Instance == null || provider == null)
                return value;
            
            wxFont wfnt = (wxFont)value;
            wx.FontData fntdata = new wx.FontData();
            fntdata.InitialFont = wfnt;
            wx.FontDialog fntdialog = new wx.FontDialog(null, fntdata);
            fntdialog.Font = wfnt;
            if (fntdialog.ShowModal() != wx.Dialog.wxCANCEL)
            {
            	wx.FontData rfd = fntdialog.FontData;
            	wxFont retfont = new wxFont();
            	retfont.Family = rfd.ChosenFont.Family;
            	retfont.FaceName = rfd.ChosenFont.FaceName;
            	retfont.Encoding = rfd.ChosenFont.Encoding;
            	retfont.PointSize = rfd.ChosenFont.PointSize;
            	retfont.Style = rfd.ChosenFont.Style;
            	return retfont;
            }
			return value;
		}
	}	

}
