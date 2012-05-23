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
		
		private DateTime? _CheckInTimeX;
		
		private int? _GuestOfId;
		
		private string _Location;
		
   		
   		private EntitySet< CheckInActivity> _CheckInActivities;
		
    	
		private EntityRef< Person> _GuestOf;
		
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
		
		partial void OnCheckInTimeXChanging(DateTime? value);
		partial void OnCheckInTimeXChanged();
		
		partial void OnGuestOfIdChanging(int? value);
		partial void OnGuestOfIdChanged();
		
		partial void OnLocationChanging(string value);
		partial void OnLocationChanged();
		
    #endregion
		public CheckInTime()
		{
			
			this._CheckInActivities = new EntitySet< CheckInActivity>(new Action< CheckInActivity>(this.attach_CheckInActivities), new Action< CheckInActivity>(this.detach_CheckInActivities)); 
			
			
			this._GuestOf = default(EntityRef< Person>); 
			
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

		
		[Column(Name="GuestOfId", UpdateCheck=UpdateCheck.Never, Storage="_GuestOfId", DbType="int")]
		public int? GuestOfId
		{
			get { return this._GuestOfId; }

			set
			{
				if (this._GuestOfId != value)
				{
				
					if (this._GuestOf.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnGuestOfIdChanging(value);
					this.SendPropertyChanging();
					this._GuestOfId = value;
					this.SendPropertyChanged("GuestOfId");
					this.OnGuestOfIdChanged();
				}

			}

		}

		
		[Column(Name="location", UpdateCheck=UpdateCheck.Never, Storage="_Location", DbType="varchar(50)")]
		public string Location
		{
			get { return this._Location; }

			set
			{
				if (this._Location != value)
				{
				
                    this.OnLocationChanging(value);
					this.SendPropertyChanging();
					this._Location = value;
					this.SendPropertyChanged("Location");
					this.OnLocationChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
   		[Association(Name="FK_CheckInActivity_CheckInTimes", Storage="_CheckInActivities", OtherKey="Id")]
   		public EntitySet< CheckInActivity> CheckInActivities
   		{
   		    get { return this._CheckInActivities; }

			set	{ this._CheckInActivities.Assign(value); }

   		}

		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="CheckinTimes__GuestOf", Storage="_GuestOf", ThisKey="GuestOfId", IsForeignKey=true)]
		public Person GuestOf
		{
			get { return this._GuestOf.Entity; }

			set
			{
				Person previousValue = this._GuestOf.Entity;
				if (((previousValue != value) 
							|| (this._GuestOf.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._GuestOf.Entity = null;
						previousValue.CheckinTimes.Remove(this);
					}

					this._GuestOf.Entity = value;
					if (value != null)
					{
						value.CheckinTimes.Add(this);
						
						this._GuestOfId = value.PeopleId;
						
					}

					else
					{
						
						this._GuestOfId = default(int?);
						
					}

					this.SendPropertyChanged("GuestOf");
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

   		
		private void attach_CheckInActivities(CheckInActivity entity)
		{
			this.SendPropertyChanging();
			entity.CheckInTime = this;
		}

		private void detach_CheckInActivities(CheckInActivity entity)
		{
			this.SendPropertyChanging();
			entity.CheckInTime = null;
		}

		
	}

}

