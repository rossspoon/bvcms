using System; 
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;

namespace DiscData
{
	[Table(Name="dbo.VerseCategoryXref")]
	public partial class VerseCategoryXref : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _VerseCategoryId;
		
		private int _VerseId;
		
   		
    	
		private EntityRef< Verse> _Verse;
		
		private EntityRef< VerseCategory> _VerseCategory;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnVerseCategoryIdChanging(int value);
		partial void OnVerseCategoryIdChanged();
		
		partial void OnVerseIdChanging(int value);
		partial void OnVerseIdChanged();
		
    #endregion
		public VerseCategoryXref()
		{
			
			
			this._Verse = default(EntityRef< Verse>); 
			
			this._VerseCategory = default(EntityRef< VerseCategory>); 
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="VerseCategoryId", UpdateCheck=UpdateCheck.Never, Storage="_VerseCategoryId", DbType="int NOT NULL", IsPrimaryKey=true)]
		public int VerseCategoryId
		{
			get { return this._VerseCategoryId; }

			set
			{
				if (this._VerseCategoryId != value)
				{
				
					if (this._VerseCategory.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnVerseCategoryIdChanging(value);
					this.SendPropertyChanging();
					this._VerseCategoryId = value;
					this.SendPropertyChanged("VerseCategoryId");
					this.OnVerseCategoryIdChanged();
				}

			}

		}

		
		[Column(Name="VerseId", UpdateCheck=UpdateCheck.Never, Storage="_VerseId", DbType="int NOT NULL", IsPrimaryKey=true)]
		public int VerseId
		{
			get { return this._VerseId; }

			set
			{
				if (this._VerseId != value)
				{
				
					if (this._Verse.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnVerseIdChanging(value);
					this.SendPropertyChanging();
					this._VerseId = value;
					this.SendPropertyChanged("VerseId");
					this.OnVerseIdChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="FK_VerseCategoryXref_Verse", Storage="_Verse", ThisKey="VerseId", IsForeignKey=true)]
		public Verse Verse
		{
			get { return this._Verse.Entity; }

			set
			{
				Verse previousValue = this._Verse.Entity;
				if (((previousValue != value) 
							|| (this._Verse.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._Verse.Entity = null;
						previousValue.VerseCategoryXrefs.Remove(this);
					}

					this._Verse.Entity = value;
					if (value != null)
					{
						value.VerseCategoryXrefs.Add(this);
						
						this._VerseId = value.Id;
						
					}

					else
					{
						
						this._VerseId = default(int);
						
					}

					this.SendPropertyChanged("Verse");
				}

			}

		}

		
		[Association(Name="FK_VerseCategoryXref_VerseCategory", Storage="_VerseCategory", ThisKey="VerseCategoryId", IsForeignKey=true)]
		public VerseCategory VerseCategory
		{
			get { return this._VerseCategory.Entity; }

			set
			{
				VerseCategory previousValue = this._VerseCategory.Entity;
				if (((previousValue != value) 
							|| (this._VerseCategory.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._VerseCategory.Entity = null;
						previousValue.VerseCategoryXrefs.Remove(this);
					}

					this._VerseCategory.Entity = value;
					if (value != null)
					{
						value.VerseCategoryXrefs.Add(this);
						
						this._VerseCategoryId = value.Id;
						
					}

					else
					{
						
						this._VerseCategoryId = default(int);
						
					}

					this.SendPropertyChanged("VerseCategory");
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

