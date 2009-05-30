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
	[Table(Name="dbo.VolInterestCodes")]
	public partial class VolInterestCode : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _Id;
		
		private int? _OpportunityId;
		
		private string _Description;
		
   		
   		private EntitySet< VolInterestInterestCode> _VolInterestInterestCodes;
		
    	
		private EntityRef< VolOpportunity> _VolOpportunity;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnIdChanging(int value);
		partial void OnIdChanged();
		
		partial void OnOpportunityIdChanging(int? value);
		partial void OnOpportunityIdChanged();
		
		partial void OnDescriptionChanging(string value);
		partial void OnDescriptionChanged();
		
    #endregion
		public VolInterestCode()
		{
			
			this._VolInterestInterestCodes = new EntitySet< VolInterestInterestCode>(new Action< VolInterestInterestCode>(this.attach_VolInterestInterestCodes), new Action< VolInterestInterestCode>(this.detach_VolInterestInterestCodes)); 
			
			
			this._VolOpportunity = default(EntityRef< VolOpportunity>); 
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="Id", UpdateCheck=UpdateCheck.Never, Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get { return this._Id; }

			set
			{
				if (this._Id != value)
				{
				
                    this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}

			}

		}

		
		[Column(Name="OpportunityId", UpdateCheck=UpdateCheck.Never, Storage="_OpportunityId", DbType="int")]
		public int? OpportunityId
		{
			get { return this._OpportunityId; }

			set
			{
				if (this._OpportunityId != value)
				{
				
					if (this._VolOpportunity.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnOpportunityIdChanging(value);
					this.SendPropertyChanging();
					this._OpportunityId = value;
					this.SendPropertyChanged("OpportunityId");
					this.OnOpportunityIdChanged();
				}

			}

		}

		
		[Column(Name="Description", UpdateCheck=UpdateCheck.Never, Storage="_Description", DbType="varchar(50)")]
		public string Description
		{
			get { return this._Description; }

			set
			{
				if (this._Description != value)
				{
				
                    this.OnDescriptionChanging(value);
					this.SendPropertyChanging();
					this._Description = value;
					this.SendPropertyChanged("Description");
					this.OnDescriptionChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
   		[Association(Name="FK_VolInterestInterestCodes_VolInterestCodes", Storage="_VolInterestInterestCodes", OtherKey="InterestCodeId")]
   		public EntitySet< VolInterestInterestCode> VolInterestInterestCodes
   		{
   		    get { return this._VolInterestInterestCodes; }

			set	{ this._VolInterestInterestCodes.Assign(value); }

   		}

		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="FK_VolInterestCodes_VolOpportunity", Storage="_VolOpportunity", ThisKey="OpportunityId", IsForeignKey=true)]
		public VolOpportunity VolOpportunity
		{
			get { return this._VolOpportunity.Entity; }

			set
			{
				VolOpportunity previousValue = this._VolOpportunity.Entity;
				if (((previousValue != value) 
							|| (this._VolOpportunity.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._VolOpportunity.Entity = null;
						previousValue.VolInterestCodes.Remove(this);
					}

					this._VolOpportunity.Entity = value;
					if (value != null)
					{
						value.VolInterestCodes.Add(this);
						
						this._OpportunityId = value.Id;
						
					}

					else
					{
						
						this._OpportunityId = default(int?);
						
					}

					this.SendPropertyChanged("VolOpportunity");
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

   		
		private void attach_VolInterestInterestCodes(VolInterestInterestCode entity)
		{
			this.SendPropertyChanging();
			entity.VolInterestCode = this;
		}

		private void detach_VolInterestInterestCodes(VolInterestInterestCode entity)
		{
			this.SendPropertyChanging();
			entity.VolInterestCode = null;
		}

		
	}

}

