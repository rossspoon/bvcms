using System; 
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;

namespace CmsData
{
	[Table(Name="dbo.TagTag")]
	public partial class TagTag : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _Id;
		
		private int _ParentTagId;
		
   		
    	
		private EntityRef< Tag> _ParentTag;
		
		private EntityRef< Tag> _Tag;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnIdChanging(int value);
		partial void OnIdChanged();
		
		partial void OnParentTagIdChanging(int value);
		partial void OnParentTagIdChanged();
		
    #endregion
		public TagTag()
		{
			
			
			this._ParentTag = default(EntityRef< Tag>); 
			
			this._Tag = default(EntityRef< Tag>); 
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="Id", UpdateCheck=UpdateCheck.Never, Storage="_Id", DbType="int NOT NULL", IsPrimaryKey=true)]
		public int Id
		{
			get { return this._Id; }

			set
			{
				if (this._Id != value)
				{
				
					if (this._Tag.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}

			}

		}

		
		[Column(Name="ParentTagId", UpdateCheck=UpdateCheck.Never, Storage="_ParentTagId", DbType="int NOT NULL", IsPrimaryKey=true)]
		public int ParentTagId
		{
			get { return this._ParentTagId; }

			set
			{
				if (this._ParentTagId != value)
				{
				
					if (this._ParentTag.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnParentTagIdChanging(value);
					this.SendPropertyChanging();
					this._ParentTagId = value;
					this.SendPropertyChanged("ParentTagId");
					this.OnParentTagIdChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="Tags__ParentTag", Storage="_ParentTag", ThisKey="ParentTagId", IsForeignKey=true)]
		public Tag ParentTag
		{
			get { return this._ParentTag.Entity; }

			set
			{
				Tag previousValue = this._ParentTag.Entity;
				if (((previousValue != value) 
							|| (this._ParentTag.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._ParentTag.Entity = null;
						previousValue.Tags.Remove(this);
					}

					this._ParentTag.Entity = value;
					if (value != null)
					{
						value.Tags.Add(this);
						
						this._ParentTagId = value.Id;
						
					}

					else
					{
						
						this._ParentTagId = default(int);
						
					}

					this.SendPropertyChanged("ParentTag");
				}

			}

		}

		
		[Association(Name="TagTags__Tag", Storage="_Tag", ThisKey="Id", IsForeignKey=true)]
		public Tag Tag
		{
			get { return this._Tag.Entity; }

			set
			{
				Tag previousValue = this._Tag.Entity;
				if (((previousValue != value) 
							|| (this._Tag.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._Tag.Entity = null;
						previousValue.TagTags.Remove(this);
					}

					this._Tag.Entity = value;
					if (value != null)
					{
						value.TagTags.Add(this);
						
						this._Id = value.Id;
						
					}

					else
					{
						
						this._Id = default(int);
						
					}

					this.SendPropertyChanged("Tag");
				}

			}

		}

		
	#endregion
	
		public event PropertyChangingEventHandler PropertyChanging;
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
				this.PropertyChanging(this, emptyChangingEventArgs);
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

   		
	}

}

