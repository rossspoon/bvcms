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
	[Table(Name="dbo.CheckInTimes")]
	public partial class CheckInTime : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _Id;
		
		private int? _PeopleId;
		
		private int? _OrganizationId;
		
		private DateTime? _CheckInDay;
		
		private DateTime? _CheckInTimeX;
		
   		
    	
		private EntityRef< Organization> _Organization;
		
		private EntityRef< Person> _Person;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnIdChanging(int value);
		partial void OnIdChanged();
		
		partial void OnPeopleIdChanging(int? value);
		partial void OnPeopleIdChanged();
		
		partial void OnOrganizationIdChanging(int? value);
		partial void OnOrganizationIdChanged();
		
		partial void OnCheckInDayChanging(DateTime? value);
		partial void OnCheckInDayChanged();
		
		partial void OnCheckInTimeXChanging(DateTime? value);
		partial void OnCheckInTimeXChanged();
		
    #endregion
		public CheckInTime()
		{
			
			
			this._Organization = default(EntityRef< Organization>); 
			
			this._Person = default(EntityRef< Person>); 
			
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

		
		[Column(Name="PeopleId", UpdateCheck=UpdateCheck.Never, Storage="_PeopleId", DbType="int")]
		public int? PeopleId
		{
			get { return this._PeopleId; }

			set
			{
				if (this._PeopleId != value)
				{
				
					if (this._Person.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnPeopleIdChanging(value);
					this.SendPropertyChanging();
					this._PeopleId = value;
					this.SendPropertyChanged("PeopleId");
					this.OnPeopleIdChanged();
				}

			}

		}

		
		[Column(Name="OrganizationId", UpdateCheck=UpdateCheck.Never, Storage="_OrganizationId", DbType="int")]
		public int? OrganizationId
		{
			get { return this._OrganizationId; }

			set
			{
				if (this._OrganizationId != value)
				{
				
					if (this._Organization.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnOrganizationIdChanging(value);
					this.SendPropertyChanging();
					this._OrganizationId = value;
					this.SendPropertyChanged("OrganizationId");
					this.OnOrganizationIdChanged();
				}

			}

		}

		
		[Column(Name="CheckInDay", UpdateCheck=UpdateCheck.Never, Storage="_CheckInDay", DbType="datetime")]
		public DateTime? CheckInDay
		{
			get { return this._CheckInDay; }

			set
			{
				if (this._CheckInDay != value)
				{
				
                    this.OnCheckInDayChanging(value);
					this.SendPropertyChanging();
					this._CheckInDay = value;
					this.SendPropertyChanged("CheckInDay");
					this.OnCheckInDayChanged();
				}

			}

		}

		
		[Column(Name="CheckInTime", UpdateCheck=UpdateCheck.Never, Storage="_CheckInTimeX", DbType="datetime")]
		public DateTime? CheckInTimeX
		{
			get { return this._CheckInTimeX; }

			set
			{
				if (this._CheckInTimeX != value)
				{
				
                    this.OnCheckInTimeXChanging(value);
					this.SendPropertyChanging();
					this._CheckInTimeX = value;
					this.SendPropertyChanged("CheckInTimeX");
					this.OnCheckInTimeXChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="FK_CheckInTimes_Organizations", Storage="_Organization", ThisKey="OrganizationId", IsForeignKey=true)]
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
						previousValue.CheckInTimes.Remove(this);
					}

					this._Organization.Entity = value;
					if (value != null)
					{
						value.CheckInTimes.Add(this);
						
						this._OrganizationId = value.OrganizationId;
						
					}

					else
					{
						
						this._OrganizationId = default(int?);
						
					}

					this.SendPropertyChanged("Organization");
				}

			}

		}

		
		[Association(Name="FK_CheckInTimes_People", Storage="_Person", ThisKey="PeopleId", IsForeignKey=true)]
		public Person Person
		{
			get { return this._Person.Entity; }

			set
			{
				Person previousValue = this._Person.Entity;
				if (((previousValue != value) 
							|| (this._Person.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._Person.Entity = null;
						previousValue.CheckInTimes.Remove(this);
					}

					this._Person.Entity = value;
					if (value != null)
					{
						value.CheckInTimes.Add(this);
						
						this._PeopleId = value.PeopleId;
						
					}

					else
					{
						
						this._PeopleId = default(int?);
						
					}

					this.SendPropertyChanged("Person");
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

