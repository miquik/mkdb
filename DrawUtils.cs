/*
 * Creato da SharpDevelop.
 * Utente: michele
 * Data: 28/01/2009
 * Ora: 16.07
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Reflection;

namespace mkdb
{
	
	public class wxPoint
	{
		private int _x;
		private int _y;
		public wxPoint()
		{
			_x = 0; _y = 0;
		}
		
		public wxPoint(int x, int y)
		{
			_x = x; _y = y;
		}
		
		public int X
		{
			get	{	return _x;	}
			set	{	_x = value;	}
		}

		public int Y
		{
			get	{	return _y;	}
			set	{	_y = value;	}
		}
			
		public static implicit operator System.Drawing.Point(wxPoint pt) 
   		{
			return new System.Drawing.Point(pt._x, pt._y);
   		}			
	}
	
	public class wxSize
	{
		private int _width;
		private int _height;
		public wxSize()
		{
			_width = 0; _height = 0;
		}
		
		public wxSize(int w, int h)
		{
			_width = w; _height = h;
		}
		
		public int Width
		{
			get	{	return _width;	}
			set	{	_width = value;	}
		}

		public int Height
		{
			get	{	return _height;	}
			set	{	_height = value;	}
		}
			
		public static implicit operator System.Drawing.Size(wxSize sz) 
   		{
			return new System.Drawing.Size(sz._width, sz._height);
   		}			
	}
	
	// wxPointConverter
	class wxPointTypeConverter : TypeConverter
	{
		const string WrongType = "The value type is not of type wxPoint!";
		/// <summary>
		/// we can convert from a System.Drawing.Point to this type
		/// </summary>
		/// <param name="context">context descriptor</param>
		/// <param name="sourceType">source type</param>
		/// <returns></returns>
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(System.Drawing.Point))
				return true;
			else
				return base.CanConvertFrom(context, sourceType);
		}

		/// <summary>
		/// we can convert to a System.Drawing.Point and a InstanceDescriptor
		/// </summary>
		/// <param name="context">context descriptor</param>
		/// <param name="destinationType">the destination type</param>
		/// <returns></returns>
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if ((destinationType == typeof(System.Drawing.Point)) |
				(destinationType == typeof(InstanceDescriptor)))
				return true;
			else
				return base.CanConvertTo(context, destinationType);
		}

		/// <summary>
		/// convert from a System.Drawing.Point to this type
		/// </summary>
		/// <param name="context">context descriptor</param>
		/// <param name="culture">culture info to use</param>
		/// <param name="value">the source value</param>
		/// <returns></returns>
		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			
			// if no value passed along then return a new Latitute instance
			if (value == null)
				return new wxPoint();

			// if the source is a string then convert to our type
			if (value is System.Drawing.Point)
			{
				System.Drawing.Point pt = (System.Drawing.Point)value;
				return new wxPoint(pt.X, pt.Y);
			} else
			{
				return base.ConvertFrom(context, culture, value);
			}
		}

		/// <summary>
		/// convert from wxPoint type to a System.Drawing.Point or instance descriptor
		/// </summary>
		/// <param name="context">conext descriptor</param>
		/// <param name="culture">culture info</param>
		/// <param name="value">source value</param>
		/// <param name="destinationType">destination type</param>
		/// <returns></returns>
		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			// check that the value passed along is of our type
			if (value != null)
				if (!(value is wxPoint))
					throw new Exception(WrongType);

			// convert to a string
			if (destinationType == typeof(System.Drawing.Point))
			{
				// no Latitude instance so we return an empty string
				if (value == null)
					return System.Drawing.Point.Empty;

				wxPoint pt = (wxPoint)value;
				return new System.Drawing.Point(pt.X, pt.Y);				
			}

			if (destinationType == typeof(InstanceDescriptor))
   			{
				// no Latitude instance
				if (value == null)
					return null;
				
         		wxPoint pt = (wxPoint)value;
      			ConstructorInfo ctor = typeof(System.Drawing.Point).GetConstructor(
							new Type[] {typeof(int), typeof(int)});
      			if (ctor != null) 
      			{
         			return new InstanceDescriptor(ctor, new object[] {pt.X, pt.Y});
				}
			}
			// call base converter to convert
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
	
}
