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
using System.Windows.Forms.Design;
using System.Drawing;
using System.ComponentModel.Design.Serialization;
using System.Reflection;

using wx;

namespace mkdb
{

	public class wxFlagsItem
	{
		private string _name;
		private uint _val;
		private bool _state;
			
		public wxFlagsItem(string name, uint val, bool state)
		{
			_name = name;
			_val = val;
			_state = state;
		}			
		public string Name
		{
			get	{	return _name;	}
		}
		public uint Value
		{
			get	{	return _val;	}
		}
		public bool Checked
		{
			get	{	return (_state == true);	}
			set	{	_state = value;	}
		}
		
		public override string ToString()
		{
			return _name;
		}
	}	
	
	
	public class wxFlags : ArrayList
	{				
		private uint _long_flag;
		
		public wxFlags() : base()
		{
			_long_flag = 0;
		}
		
		public int AddItem(string name, uint val, bool state)
		{
			return this.Add(new wxFlagsItem(name, val, state));
		}
		
		public uint ToLong
		{
			get	{	return _long_flag;	}
			set	{	_long_flag = value;	}
		}
		
		public override string ToString()
		{
			string str = "";
			foreach (wxFlagsItem item in this) 
			{ 
				if (item.Checked)
				{
					str += item.Name + "; ";
				}
			}
			return str;		
		}
	}
		
	
	public class wxFlagsEditor : UITypeEditor
	{
		private IWindowsFormsEditorService edSvc = null;
		private CheckedListBox clb;
				
		/// <summary>
		/// Overrides the method used to provide basic behaviour for selecting editor.
		/// Shows our custom control for editing the value.
		/// </summary>
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value) 
		{
			if (context == null || context.Instance == null || provider == null) 
				return value;
			
			edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
			if (edSvc != null) 
			{					
				// Create a CheckedListBox and populate it with all the enum values			
				clb = new CheckedListBox();
				clb.BorderStyle = BorderStyle.FixedSingle;
				clb.CheckOnClick = true;
				clb.MouseDown += new MouseEventHandler(this.OnMouseDown);
				
				wxFlags ht = (wxFlags)value;
				foreach (wxFlagsItem d in ht) 
				{ 
					clb.Items.Add(new wxFlagsItem(d.Name, d.Value, d.Checked), d.Checked);
				}				

				// Show our CheckedListbox as a DropDownControl. 
				// This methods returns only when the dropdowncontrol is closed
				edSvc.DropDownControl(clb);
				
				wxFlags retflags = new wxFlags();				
				int i = 0;	
				retflags.ToLong = 0;
				foreach(wxFlagsItem item in clb.Items)
				{	
					bool b;
					if (clb.GetItemChecked(i))	
					{
						b = true;
						retflags.ToLong |= item.Value;
					}  else 
					{
						b = false;
						// retflags.ToLong = retflags.ToLong & (~item.Value);
					}
					retflags.AddItem(item.Name, item.Value, b);
					i++;
				}					
				return retflags;
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
			if (!handleLostfocus && clb.ClientRectangle.Contains(clb.PointToClient(new Point(e.X, e.Y))))
			{
				clb.LostFocus += new EventHandler(this.ValueChanged);
				handleLostfocus = true;
			}
		}

		private void ValueChanged(object sender, EventArgs e) 
		{
			if (edSvc != null) 
			{
				edSvc.CloseDropDown();
			}
		}
	}

	
	
	public class wxFlagsTypeConverter : TypeConverter
	{

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))
				return true;
			else
				return base.CanConvertFrom(context, sourceType);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if ((destinationType == typeof(string)) |
				(destinationType == typeof(InstanceDescriptor)))
				return true;
			else
				return base.CanConvertTo(context, destinationType);
		}		
		
		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			// no value so we return a new Longitude instance
			if (value == null)
				return new wxFlags();

			// convert from a string
			if (value is string)
			{
				// get strongly typed value
				string StringValue = value as string;

				// empty string so we return a new Longitude instance
				if (StringValue.Length <= 0)
					return new wxFlags();

				// create a new Longitude instance with these values and return it
				return new wxFlags();
			} else
				return base.ConvertFrom(context, culture, value);
		}
		
		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			// check that the value we got passed on is of type Longitude
			if (value != null)
				if (!(value is wxFlags))
					throw new Exception("wrong");

			// convert to a string
			if (destinationType == typeof(string))
			{
				// no value so we return an empty string
				if (value == null)
					return string.Empty;

				wxFlags flags = (wxFlags)value;
				// convert to a string and return
				return flags.ToString();
			}

			// convert to a instance descriptor
			if (destinationType == typeof(InstanceDescriptor))
			{ 
				// no value so we return no instance descriptor
				if (value == null)
					return null;

				// strongly typed
				wxFlags flags = (wxFlags)value;

				// used to descripe the constructor
				MemberInfo Member = null;
				object[] Arguments = null;

				// get the constructor for the type
				Member = typeof(wxFlags).GetConstructor(new Type[] {});

				// create the arguments to pass along
				Arguments = new object[] {};

				// return the instance descriptor
				if (Member != null)
					return new InstanceDescriptor(Member, Arguments);
				else
					return null;
			}

			// call the base converter
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
	
}
