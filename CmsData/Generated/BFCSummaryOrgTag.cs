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
	[Table(Name="dbo.BFCSummaryOrgTags")]
	public partial class BFCSummaryOrgTag : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _SortOrder;
		
		private int _OrgTagId;
		
   		
    	
		private EntityRef< Division> _Division;
		
		private EntityRef< Tag> _Tag;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnSortOrderChanging(int value);
		partial void OnSortOrderChanged();
		
		partial void OnOrgTagIdChanging(int value);
		partial void OnOrgTagIdChanged();
		
    #endregion
		public BFCSummaryOrgTag()
		{
			
			
			this._Division = default(EntityRef< Division>); 
			
			this._Tag = default(EntityRef< Tag>); 
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="SortOrder", UpdateCheck=UpdateCheck.Never, Storage="_SortOrder", DbType="int NOT NULL")]
		public int SortOrder
		{
			get { return this._SortOrder; }

			set
			{
				if (this._SortOrder != value)
				{
				
                    this.OnSortOrderChanging(value);
					this.SendPropertyChanging();
					this._SortOrder = value;
					this.SendPropertyChanged("SortOrder");
					this.OnSortOrderChanged();
				}

			}

		}

		
		[Column(Name="OrgTagId", UpdateCheck=UpdateCheck.Never, Storage="_OrgTagId", DbType="int NOT NULL", IsPrimaryKey=true)]
		public int OrgTagId
		{
			get { return this._OrgTagId; }

			set
			{
				if (this._OrgTagId != value)
				{
				
					if (this._Tag.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnOrgTagIdChanging(value);
					this.SendPropertyChanging();
					this._OrgTagId = value;
					this.SendPropertyChanged("OrgTagId");
					this.OnOrgTagIdChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="FK_BFCSummaryOrgTags_Division", Storage="_Division", ThisKey="OrgTagId", IsForeignKey=true)]
		public Division Division
		{
			get { return this._Division.Entity; }

			set
			{
				Division previousValue = this._Division.Entity;
				if (((previousValue != value) 
							|| (this._Division.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._Division.Entity = null;
						previousValue.BFCSummaryOrgTags.Remove(this);
					}

					this._Division.Entity = value;
					if (value != null)
					{
						value.BFCSummaryOrgTags.Add(this);
						
						this._OrgTagId = value.Id;
						
					}

					else
					{
						
						this._OrgTagId = default(int);
						
					}

					this.SendPropertyChanged("Division");
				}

			}

		}

		
		[Association(Name="FK_BFCSummaryOrgTags_Tag", Storage="_Tag", ThisKey="OrgTagId", IsForeignKey=true)]
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
						previousValue.BFCSummaryOrgTags.Remove(this);
					}

					this._Tag.Entity = value;
					if (value != null)
					{
						value.BFCSummaryOrgTags.Add(this);
						
						this._OrgTagId = value.Id;
						
					}

					else
					{
						
						this._OrgTagId = default(int);
						
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

