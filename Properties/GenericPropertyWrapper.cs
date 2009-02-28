using System;
using System.Collections;
using System.ComponentModel;

/*
 * Copyright Mihai Maerean, 2006
*/

namespace mkdb.Properties
{
	public delegate void DelegatePropertyValueChanged( IGenericProperty gp );

	#region IGenericProperty
	/// <summary>
	/// Allows setting and retreaving of a value
	/// </summary>
	public interface IGenericProperty
	{
		string PropertyName
		{
			get;
			set;
		}

		object DefaultValue
		{
			get;
			set;
		}

		object Value
		{
			get;
			set;
		}

		string Category
		{
			get;
			set;
		}

		string Description
		{
			get;
			set;
		}

		IGenericPropertyCollection PropertyParent
		{
			get;
			set;
		}

		Attribute[] Attributes
		{
			get;
			set;
		}

		void AddAttribute( Attribute a );

		IGenericProperty CloneProperty();

		event DelegatePropertyValueChanged PropertyValueChanged;
		void RaisePropertyValueChanged();
	}
	#endregion IGenericProperty

	#region GenericProperty
	[Serializable]
	public class GenericProperty : IGenericProperty
	{
		public GenericProperty( string Name, object Value, string Category, string Description, params Attribute[] attrs )
		{
			this.Category = Category;
			this.PropertyName = Name;
			this.Value = Value;
			this.DefaultValue = Value;
			this.Description = Description;
			this.Attributes = attrs;
		}

		#region IGenericProperty Members

		private string PropertyName_ = "propname";
		public virtual string PropertyName
		{
			get
			{
				return PropertyName_;
			}
			set
			{
				PropertyName_ = value;

				if( PropertyParent != null )
					PropertyParent.PropertyChanged( this );
			}
		}

		protected string Category_ = "Misc";
		public virtual string Category
		{
			get
			{
				return Category_;
			}
			set
			{
				Category_ = value;

				if( PropertyParent != null )
					PropertyParent.PropertyChanged( this );
			}
		}

		protected string Description_ = "Misc";
		public virtual string Description
		{
			get
			{
				return Description_;
			}
			set
			{
				Description_ = value;

				if( PropertyParent != null )
					PropertyParent.PropertyChanged( this );
			}
		}

		protected object Value_ = null;
		public virtual object Value
		{
			get
			{
				return Value_;
			}
			set
			{
				//if( Category_ == " Properties" )					System.Windows.Forms.MessageBox.Show( PropertyName_ + "\n" + Value_ + "\n" + value );
				Value_ = value;

				if( PropertyParent != null )
					PropertyParent.PropertyChanged( this );

				RaisePropertyValueChanged();
			}
		}

		protected object DefaultValue_ = null;
		public virtual object DefaultValue
		{
			get
			{
				return DefaultValue_;
			}
			set
			{
				DefaultValue_ = value;

				if( PropertyParent != null )
					PropertyParent.PropertyChanged( this );
			}
		}

		protected IGenericPropertyCollection PropertyParent_ = null;
		public IGenericPropertyCollection PropertyParent
		{
			get
			{
				return PropertyParent_;
			}
			set
			{
				PropertyParent_ = value;
			}
		}

		protected Attribute[] Attributes_ = null;
		public virtual Attribute[] Attributes
		{
			get
			{
				return Attributes_;
			}
			set
			{
				Attributes_ = value;
			}
		}

		public virtual IGenericProperty CloneProperty()
		{
			return new GenericProperty( this.PropertyName, Value, Category, Description, 
				Attributes != null ? (Attribute[])Attributes.Clone() : null );
		}

		public virtual void AddAttribute( Attribute a )
		{
			AttributeCollection ac = new AttributeCollection( Attributes );
			if( ! ac.Matches( a ) )
			{
				ArrayList al = new ArrayList();
				if( Attributes != null )
					al.AddRange( Attributes );
				al.Add( a );

				Attributes = (Attribute[]) al.ToArray( typeof(Attribute) );
			}
		}

		public event DelegatePropertyValueChanged PropertyValueChanged;
		public void RaisePropertyValueChanged()
		{
			if( PropertyValueChanged != null )
				PropertyValueChanged( this );
		}
		#endregion

		public override string ToString()
		{
			return Value != null ? Value.ToString() : "null" ;
		}
	}
	#endregion GenericProperty

	#region IGenericPropertyCollection
	public interface IGenericPropertyCollection
	{
		void AddProperty( IGenericProperty gp );

		IGenericProperty this[ int index ]
		{
			get;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="category">can be null, in which case it means "any category"</param>
		/// <returns></returns>
		IGenericProperty GetProperty( string name, string category );

		void RemoveProperty( IGenericProperty gp );

		void ClearProperties();

		void PropertyChanged( IGenericProperty prop );

		IEnumerable PropertiesEnumerable
		{
			get;
		}

		IGenericPropertyCollection CloneProperties();

		void AddAttributeToAllProperties( Attribute a );

		PropertyDescriptorCollection PropertyDescriptorCollection
		{
			get;
		}
	}
	#endregion IGenericPropertyCollection

	#region GenericPropertyCollection
	//[PropertyTab(typeof(PropertyUserInterface.DPropertyTab), PropertyTabScope.Component)]
	[Serializable]
	public class GenericPropertyCollection : CollectionBase, IGenericPropertyCollection
	{
		[Category("Important")]
		public virtual string Switch
		{
			get
			{
				return "to the other tab";
			}
		}

		public virtual void AddProperty( IGenericProperty gp )
		{
			List.Add( gp );
			gp.PropertyParent = this;
		}

		public virtual IGenericProperty this[ int index ]
		{
			get
			{
				return (IGenericProperty) List[ index ];
			}
		}

		public virtual IGenericProperty GetProperty( string name, string category )
		{
			name = name.ToLower();
			if( category != null )
				category = category.ToLower();

			foreach( IGenericProperty ip in List )
			{
				if( category == null || ip.Category.ToLower() == category )
					if( ip.PropertyName.ToLower() == name )
						return ip;
			}

			return null;
		}

		public virtual void RemoveProperty( IGenericProperty gp )
		{
			int i = List.IndexOf( gp );
			if( i != -1 )
				List.RemoveAt( i );
		}

		public void ClearProperties()
		{
			List.Clear();
		}

		public virtual void PropertyChanged( IGenericProperty prop )
		{
		}

		[Browsable(false)]
		public virtual IEnumerable PropertiesEnumerable
		{
			get
			{
				return List;
			}
		}

		public virtual IGenericPropertyCollection CloneProperties()
		{
			GenericPropertyCollection clone = new GenericPropertyCollection();
			foreach( IGenericProperty ip in List )
				clone.AddProperty( ip.CloneProperty() );
			return clone;
		}

		public virtual void AddAttributeToAllProperties( Attribute a )
		{
			foreach( IGenericProperty gp in List )
				gp.AddAttribute( a );
		}

		public virtual PropertyDescriptorCollection PropertyDescriptorCollection
		{
			get
			{
				ArrayList propList = new ArrayList();

				foreach( IGenericProperty gpi in PropertiesEnumerable )
				{
					propList.Add(new GenericPropertyDescriptor(gpi));
				}

				// return the collection of PropertyDescriptors.
				return new PropertyDescriptorCollection((PropertyDescriptor[])propList.ToArray(typeof(PropertyDescriptor)));
			}
		}

		[Browsable(false)]
		public new int Count
		{
			get
			{
				return List.Count;
			}
		}
	}
	#endregion GenericPropertyCollection

	#region GenericPropertyDescriptor
	/// <summary>
	/// It's situated between the collection and PropertyGrid
	/// </summary>
	public class GenericPropertyDescriptor : PropertyDescriptor 
	{
		//protected DPropertyTab owner;
		public IGenericProperty Property;
		private ArrayList ats = new ArrayList();

		public GenericPropertyDescriptor(IGenericProperty Property/*, DPropertyTab owner*/ ) :
			base(Property.PropertyName, 
			(Attribute[])
			AddIfCondition(
			ConcatArrayList(
			new Attribute[]
						{
							new CategoryAttribute(Property.Category),
							RefreshPropertiesAttribute.All,
							new DescriptionAttribute(Property.Description)
						}
			,Property.Attributes)
			, Property.Value is IGenericPropertyCollection, new TypeConverterAttribute(typeof(DExpandableObjectConverter))
			).ToArray(typeof(Attribute)) )
		{
			this.Property = Property;
			//this.owner = owner;
		}

		public static ArrayList ConcatArrayList( ICollection a1, ICollection a2 )
		{
			ArrayList a = new ArrayList( a1 );
			if( a2 != null )
				a.AddRange( a2 );
			return a;
		}

		public static ArrayList AddIfCondition( ArrayList al, bool condition, Attribute a )
		{
			if( condition )
				al.Add( a );
			return al;
		}

		/// <summary>
		/// This is the type of property.
		/// </summary>
		public override Type PropertyType
		{
			get 
			{
				return Property.Value != null ? Property.Value.GetType() : typeof(Object);
			}
		}

		/// <summary>
		/// The type of component the framework expects for this property.  Notice
		/// This returns IGenericProperty.  That is because the object that is being browsed
		/// when this property is shown is a IGenericProperty.  So we are faking the PropertyGrid
		/// into thinking this is a property on that type, even though it isn't.
		/// </summary>	
		public override Type ComponentType
		{
			get 
			{
				return typeof(IGenericProperty);
			}
		}

		/// <summary>
		/// We have to override all the abstract members.
		/// </summary>
		public override bool IsReadOnly
		{
			get
			{
				return new AttributeCollection( Property.Attributes ).Matches( new ReadOnlyAttributeEditor() );
			}
		}
		
		/// <summary>
		/// Just get the number of Points we're currently showing.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public override object GetValue(object o) 
		{
			// the object that's passed in is the FunkyButton itself.
			return Property.Value;
		}

		public override void SetValue(object o, object value) 
		{
			Property.Value = value;
			///MessageBox.Show( value.ToString() );			
		}

		/// <summary>
		/// Abstract member override
		/// </summary>
		/// <param name="o"></param>
		public override void ResetValue(object o)
		{
			Property.Value = Property.DefaultValue;
		}


		/// <summary>
		/// This is not a resettable property.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public override bool CanResetValue(object o) 
		{
			return ! Property.Value.Equals( Property.DefaultValue ); //true; // to set to default value from the DB 
		}

		/// <summary>
		/// This property doesn't participate in code generation.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public override bool ShouldSerializeValue(object o) 
		{
			return ! Property.Value.Equals( Property.DefaultValue );
		}

		public override PropertyDescriptorCollection GetChildProperties(object instance, Attribute[] filter)
		{
			System.Windows.Forms.MessageBox.Show("GetChildProperties");
			return base.GetChildProperties (instance, filter);
			///return new PropertyDescriptorCollection( new PropertyDescriptor[] {this, this} );
		}
	}
	#endregion GenericPropertyDescriptor

	#region PropertyBag
	/// <summary>
	/// a GenericPropertyCollection that in a PropertyGid displays the inner generic properties instead of the 
	/// reflection-reachable C# properties
	/// </summary>
	public class PropertyBag : GenericPropertyCollection, ICustomTypeDescriptor
	{
		#region ICustomTypeDescriptor Members

		public TypeConverter GetConverter()
		{
			return TypeDescriptor.GetConverter( this, true );
		}

		public EventDescriptorCollection GetEvents(Attribute[] attributes)
		{
			return TypeDescriptor.GetEvents( this, attributes, true );
		}

		EventDescriptorCollection System.ComponentModel.ICustomTypeDescriptor.GetEvents()
		{
			return TypeDescriptor.GetEvents( this, true );
		}

		public string GetComponentName()
		{
			return TypeDescriptor.GetComponentName( this, true );
		}

		public object GetPropertyOwner(PropertyDescriptor pd)
		{
			return this;
		}

		public AttributeCollection GetAttributes()
		{
			return TypeDescriptor.GetAttributes( this, true );
		}

		public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			return this.PropertyDescriptorCollection;
		}

		PropertyDescriptorCollection System.ComponentModel.ICustomTypeDescriptor.GetProperties()
		{
			return this.PropertyDescriptorCollection;
		}

		public object GetEditor(Type editorBaseType)
		{
			return TypeDescriptor.GetEditor( this, editorBaseType, true );
		}

		public PropertyDescriptor GetDefaultProperty()
		{
			return null;
		}

		public EventDescriptor GetDefaultEvent()
		{
			return null;
		}

		public string GetClassName()
		{
			return TypeDescriptor.GetClassName( this, true );
		}

		#endregion

		public override string ToString()
		{
			return "";
		}

	}
	#endregion GenericPropertyCollection_CustomTypeDescriptor

	#region ReadOnlyAttributeEditor
	[Serializable]
	public class ReadOnlyAttributeEditor : Attribute
	{
	}
	#endregion ReadOnlyAttributeEditor

	[Serializable]
	public class DExpandableObjectConverter : ExpandableObjectConverter
	{
		public DExpandableObjectConverter()
		{
		}

		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			if( value is IGenericPropertyCollection )
			{
				return (value as IGenericPropertyCollection).PropertyDescriptorCollection;
			}
			else
				return base.GetProperties( context, value, attributes );
		}
	}
}
