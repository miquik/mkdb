/*
 * Creato da SharpDevelop.
 * Utente: michele
 * Data: 29/01/2009
 * Ora: 8.41
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;
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
	
	public class wxFlags : Hashtable
	{
		public override string ToString()
		{
			string str = "";
			foreach (DictionaryEntry d in this) 
			{ 
				str += " ;" + d.Key;
			}
			return str;
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
	
	/*
	/// <summary>
	/// Implements a custom type editor for selecting a <see cref="SemiNavigationPage"/> in a list
	/// </summary>
	public class wxFlagsEditor : UITypeEditor
	{

		private IWindowsFormsEditorService edSvc = null;
		private CheckedListBox clb;
		private ToolTip tooltipControl;
		
		/// <summary>
		/// Internal class used for storing custom data in listviewitems
		/// </summary>
		internal class clbItem
		{
			private string _name;
			private int	_val;
			public clbItem(string name, int val)
			{
				_name = name;
				_val = val;
			}
			public string Name
			{
				get	{	return _name;	}
			}
			public int Value
			{
				get	{	return _val;	}
			}
		}
		

		/// <summary>
		/// Overrides the method used to provide basic behaviour for selecting editor.
		/// Shows our custom control for editing the value.
		/// </summary>
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value) 
		{
			if (context != null || context.Instance != null || provider != null) 
				return value;

			
			edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
			if (edSvc != null) 
			{					
				// Create a CheckedListBox and populate it with all the enum values			
				clb = new CheckedListBox();
				clb.BorderStyle = BorderStyle.FixedSingle;
				clb.CheckOnClick = true;
				clb.MouseDown += new MouseEventHandler(this.OnMouseDown);
				clb.MouseMove += new MouseEventHandler(this.OnMouseMoved);
				
				Hashtable ht = (Hashtable)value;
				
				foreach (DictionaryEntry d in ht) 
				{ 
					clbItem = new clbItem(d.Key, d.Value);
					clb.Items.Add(clbItem, true);
				}
				

					foreach(string name in Enum.GetNames(context.PropertyDescriptor.PropertyType))
					{
						// Get the enum value
						object enumVal = Enum.Parse(context.PropertyDescriptor.PropertyType, name);
						// Get the int value 
						int intVal = (int) Convert.ChangeType(enumVal, typeof(int));
						
						// Get the description attribute for this field
						System.Reflection.FieldInfo fi = context.PropertyDescriptor.PropertyType.GetField(name);
						DescriptionAttribute[] attrs = (DescriptionAttribute[]) fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

						// Store the the description
						string tooltip = attrs.Length > 0 ? attrs[0].Description : string.Empty;

						// Get the int value of the current enum value (the one being edited)
						int intEdited = (int) Convert.ChangeType(value, typeof(int));

						// Creates a clbItem that stores the name, the int value and the tooltip
						clbItem item = new clbItem(enumVal.ToString(), intVal, tooltip);

						// Get the checkstate from the value being edited
						bool checkedItem = (intEdited & intVal) > 0;

						// Add the item with the right check state
						clb.Items.Add(item, checkedItem);
					}					

					// Show our CheckedListbox as a DropDownControl. 
					// This methods returns only when the dropdowncontrol is closed
					edSvc.DropDownControl(clb);

					// Get the sum of all checked flags
					int result = 0;
					foreach(clbItem obj in clb.CheckedItems)
					{
						result += obj.Value;
					}
					
					// return the right enum value corresponding to the result
					return Enum.ToObject(context.PropertyDescriptor.PropertyType, result);
				}
			}

			return value;
		}

		/// <summary>
		/// Shows a dropdown icon in the property editor
		/// </summary>
		/// <param name="context">The context of the editing control</param>
		/// <returns>Returns <c>UITypeEditorEditStyle.DropDown</c></returns>
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) 
		{
			return UITypeEditorEditStyle.DropDown;			
		}

		private bool handleLostfocus = false;

		/// <summary>
		/// When got the focus, handle the lost focus event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnMouseDown(object sender, MouseEventArgs e) 
		{
			if(!handleLostfocus && clb.ClientRectangle.Contains(clb.PointToClient(new Point(e.X, e.Y))))
			{
				clb.LostFocus += new EventHandler(this.ValueChanged);
				handleLostfocus = true;
			}
		}

		/// <summary>
		/// Occurs when the mouse is moved over the checkedlistbox. 
		/// Sets the tooltip of the item under the pointer
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnMouseMoved(object sender, MouseEventArgs e) 
		{			
			int index = clb.IndexFromPoint(e.X, e.Y);
			if(index >= 0)
				tooltipControl.SetToolTip(clb, ((clbItem) clb.Items[index]).Tooltip);
		}

		/// <summary>
		/// Close the dropdowncontrol when the user has selected a value
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ValueChanged(object sender, EventArgs e) 
		{
			if (edSvc != null) 
			{
				edSvc.CloseDropDown();
			}
		}
	}
	*/
}
