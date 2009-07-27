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
	[Table(Name="dbo.RecAgeDivision")]
	public partial class RecAgeDivision : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _Id;
		
		private int? _DivId;
		
		private int? _OrgId;
		
		private int? _StartAge;
		
		private int? _EndAge;
		
		private string _AgeDate;
		
		private int? _GenderId;
		
   		
    	
		private EntityRef< Division> _Division;
		
		private EntityRef< Gender> _Gender;
		
		private EntityRef< Organization> _Organization;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnIdChanging(int value);
		partial void OnIdChanged();
		
		partial void OnDivIdChanging(int? value);
		partial void OnDivIdChanged();
		
		partial void OnOrgIdChanging(int? value);
		partial void OnOrgIdChanged();
		
		partial void OnStartAgeChanging(int? value);
		partial void OnStartAgeChanged();
		
		partial void OnEndAgeChanging(int? value);
		partial void OnEndAgeChanged();
		
		partial void OnAgeDateChanging(string value);
		partial void OnAgeDateChanged();
		
		partial void OnGenderIdChanging(int? value);
		partial void OnGenderIdChanged();
		
    #endregion
		public RecAgeDivision()
		{
			
			
			this._Division = default(EntityRef< Division>); 
			
			this._Gender = default(EntityRef< Gender>); 
			
			this._Organization = default(EntityRef< Organization>); 
			
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

		
		[Column(Name="DivId", UpdateCheck=UpdateCheck.Never, Storage="_DivId", DbType="int")]
		public int? DivId
		{
			get { return this._DivId; }

			set
			{
				if (this._DivId != value)
				{
				
					if (this._Division.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnDivIdChanging(value);
					this.SendPropertyChanging();
					this._DivId = value;
					this.SendPropertyChanged("DivId");
					this.OnDivIdChanged();
				}

			}

		}

		
		[Column(Name="OrgId", UpdateCheck=UpdateCheck.Never, Storage="_OrgId", DbType="int")]
		public int? OrgId
		{
			get { return this._OrgId; }

			set
			{
				if (this._OrgId != value)
				{
				
					if (this._Organization.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnOrgIdChanging(value);
					this.SendPropertyChanging();
					this._OrgId = value;
					this.SendPropertyChanged("OrgId");
					this.OnOrgIdChanged();
				}

			}

		}

		
		[Column(Name="StartAge", UpdateCheck=UpdateCheck.Never, Storage="_StartAge", DbType="int")]
		public int? StartAge
		{
			get { return this._StartAge; }

			set
			{
				if (this._StartAge != value)
				{
				
                    this.OnStartAgeChanging(value);
					this.SendPropertyChanging();
					this._StartAge = value;
					this.SendPropertyChanged("StartAge");
					this.OnStartAgeChanged();
				}

			}

		}

		
		[Column(Name="EndAge", UpdateCheck=UpdateCheck.Never, Storage="_EndAge", DbType="int")]
		public int? EndAge
		{
			get { return this._EndAge; }

			set
			{
				if (this._EndAge != value)
				{
				
                    this.OnEndAgeChanging(value);
					this.SendPropertyChanging();
					this._EndAge = value;
					this.SendPropertyChanged("EndAge");
					this.OnEndAgeChanged();
				}

			}

		}

		
		[Column(Name="AgeDate", UpdateCheck=UpdateCheck.Never, Storage="_AgeDate", DbType="varchar(50)")]
		public string AgeDate
		{
			get { return this._AgeDate; }

			set
			{
				if (this._AgeDate != value)
				{
				
                    this.OnAgeDateChanging(value);
					this.SendPropertyChanging();
					this._AgeDate = value;
					this.SendPropertyChanged("AgeDate");
					this.OnAgeDateChanged();
				}

			}

		}

		
		[Column(Name="GenderId", UpdateCheck=UpdateCheck.Never, Storage="_GenderId", DbType="int")]
		public int? GenderId
		{
			get { return this._GenderId; }

			set
			{
				if (this._GenderId != value)
				{
				
					if (this._Gender.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnGenderIdChanging(value);
					this.SendPropertyChanging();
					this._GenderId = value;
					this.SendPropertyChanged("GenderId");
					this.OnGenderIdChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="FK_Recreation_Division", Storage="_Division", ThisKey="DivId", IsForeignKey=true)]
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
						previousValue.RecAgeDivisions.Remove(this);
					}

					this._Division.Entity = value;
					if (value != null)
					{
						value.RecAgeDivisions.Add(this);
						
						this._DivId = value.Id;
						
					}

					else
					{
						
						this._DivId = default(int?);
						
					}

					this.SendPropertyChanged("Division");
				}

			}

		}

		
		[Association(Name="FK_Recreation_Gender", Storage="_Gender", ThisKey="GenderId", IsForeignKey=true)]
		public Gender Gender
		{
			get { return this._Gender.Entity; }

			set
			{
				Gender previousValue = this._Gender.Entity;
				if (((previousValue != value) 
							|| (this._Gender.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._Gender.Entity = null;
						previousValue.RecAgeDivisions.Remove(this);
					}

					this._Gender.Entity = value;
					if (value != null)
					{
						value.RecAgeDivisions.Add(this);
						
						this._GenderId = value.Id;
						
					}

					else
					{
						
						this._GenderId = default(int?);
						
					}

					this.SendPropertyChanged("Gender");
				}

			}

		}

		
		[Association(Name="FK_Recreation_Organizations", Storage="_Organization", ThisKey="OrgId", IsForeignKey=true)]
		public Organization Organization
		{
			get { return this._Organization.Entity; }

			set
			{
				Organization previousValue = this._Organization.Entity;
				if (((previousValue != value) 
							|| (this._Organization.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._Organization.Entity = null;
						previousValue.RecAgeDivisions.Remove(this);
					}

					this._Organization.Entity = value;
					if (value != null)
					{
						value.RecAgeDivisions.Add(this);
						
						this._OrgId = value.OrganizationId;
						
					}

					else
					{
						
						this._OrgId = default(int?);
						
					}

					this.SendPropertyChanged("Organization");
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

