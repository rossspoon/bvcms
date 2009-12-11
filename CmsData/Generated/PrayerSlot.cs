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
	[Table(Name="disc.PrayerSlot")]
	public partial class PrayerSlot : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _Day;
		
		private DateTime _Time;
		
		private int _PeopleId;
		
   		
    	
		private EntityRef< Person> _Person;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnDayChanging(int value);
		partial void OnDayChanged();
		
		partial void OnTimeChanging(DateTime value);
		partial void OnTimeChanged();
		
		partial void OnPeopleIdChanging(int value);
		partial void OnPeopleIdChanged();
		
    #endregion
		public PrayerSlot()
		{
			
			
			this._Person = default(EntityRef< Person>); 
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="Day", UpdateCheck=UpdateCheck.Never, Storage="_Day", DbType="int NOT NULL", IsPrimaryKey=true)]
		public int Day
		{
			get { return this._Day; }

			set
			{
				if (this._Day != value)
				{
				
                    this.OnDayChanging(value);
					this.SendPropertyChanging();
					this._Day = value;
					this.SendPropertyChanged("Day");
					this.OnDayChanged();
				}

			}

		}

		
		[Column(Name="Time", UpdateCheck=UpdateCheck.Never, Storage="_Time", DbType="datetime NOT NULL", IsPrimaryKey=true)]
		public DateTime Time
		{
			get { return this._Time; }

			set
			{
				if (this._Time != value)
				{
				
                    this.OnTimeChanging(value);
					this.SendPropertyChanging();
					this._Time = value;
					this.SendPropertyChanged("Time");
					this.OnTimeChanged();
				}

			}

		}

		
		[Column(Name="PeopleId", UpdateCheck=UpdateCheck.Never, Storage="_PeopleId", DbType="int NOT NULL", IsPrimaryKey=true)]
		public int PeopleId
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

		
    #endregion
        
    #region Foreign Key Tables
   		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="FK_PrayerSlot_People", Storage="_Person", ThisKey="PeopleId", IsForeignKey=true)]
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
						previousValue.PrayerSlots.Remove(this);
					}

					this._Person.Entity = value;
					if (value != null)
					{
						value.PrayerSlots.Add(this);
						
						this._PeopleId = value.PeopleId;
						
					}

					else
					{
						
						this._PeopleId = default(int);
						
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

