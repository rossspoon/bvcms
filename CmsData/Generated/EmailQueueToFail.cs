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
	[Table(Name="dbo.EmailQueueToFail")]
	public partial class EmailQueueToFail : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _Id;
		
		private int _PeopleId;
		
		private DateTime? _Time;
		
		private string _EventX;
		
		private string _Reason;
		
		private string _Bouncetype;
		
   		
    	
		private EntityRef< EmailQueue> _EmailQueue;
		
		private EntityRef< EmailQueueTo> _EmailQueueTo;
		
		private EntityRef< Person> _Person;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnIdChanging(int value);
		partial void OnIdChanged();
		
		partial void OnPeopleIdChanging(int value);
		partial void OnPeopleIdChanged();
		
		partial void OnTimeChanging(DateTime? value);
		partial void OnTimeChanged();
		
		partial void OnEventXChanging(string value);
		partial void OnEventXChanged();
		
		partial void OnReasonChanging(string value);
		partial void OnReasonChanged();
		
		partial void OnBouncetypeChanging(string value);
		partial void OnBouncetypeChanged();
		
    #endregion
		public EmailQueueToFail()
		{
			
			
			this._EmailQueue = default(EntityRef< EmailQueue>); 
			
			this._EmailQueueTo = default(EntityRef< EmailQueueTo>); 
			
			this._Person = default(EntityRef< Person>); 
			
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
				
					if (this._EmailQueueTo.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
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

		
		[Column(Name="time", UpdateCheck=UpdateCheck.Never, Storage="_Time", DbType="datetime")]
		public DateTime? Time
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

		
		[Column(Name="event", UpdateCheck=UpdateCheck.Never, Storage="_EventX", DbType="varchar(20)")]
		public string EventX
		{
			get { return this._EventX; }

			set
			{
				if (this._EventX != value)
				{
				
                    this.OnEventXChanging(value);
					this.SendPropertyChanging();
					this._EventX = value;
					this.SendPropertyChanged("EventX");
					this.OnEventXChanged();
				}

			}

		}

		
		[Column(Name="reason", UpdateCheck=UpdateCheck.Never, Storage="_Reason", DbType="varchar(200)")]
		public string Reason
		{
			get { return this._Reason; }

			set
			{
				if (this._Reason != value)
				{
				
                    this.OnReasonChanging(value);
					this.SendPropertyChanging();
					this._Reason = value;
					this.SendPropertyChanged("Reason");
					this.OnReasonChanged();
				}

			}

		}

		
		[Column(Name="bouncetype", UpdateCheck=UpdateCheck.Never, Storage="_Bouncetype", DbType="varchar(20)")]
		public string Bouncetype
		{
			get { return this._Bouncetype; }

			set
			{
				if (this._Bouncetype != value)
				{
				
                    this.OnBouncetypeChanging(value);
					this.SendPropertyChanging();
					this._Bouncetype = value;
					this.SendPropertyChanged("Bouncetype");
					this.OnBouncetypeChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="FK_EmailQueueToFail_EmailQueue", Storage="_EmailQueue", ThisKey="Id", IsForeignKey=true)]
		public EmailQueue EmailQueue
		{
			get { return this._EmailQueue.Entity; }

			set
			{
				EmailQueue previousValue = this._EmailQueue.Entity;
				if (((previousValue != value) 
							|| (this._EmailQueue.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._EmailQueue.Entity = null;
						previousValue.EmailQueueToFails.Remove(this);
					}

					this._EmailQueue.Entity = value;
					if (value != null)
					{
						value.EmailQueueToFails.Add(this);
						
						this._Id = value.Id;
						
					}

					else
					{
						
						this._Id = default(int);
						
					}

					this.SendPropertyChanged("EmailQueue");
				}

			}

		}

		
		[Association(Name="FK_EmailQueueToFail_EmailQueueTo", Storage="_EmailQueueTo", ThisKey="Id,PeopleId", IsForeignKey=true)]
		public EmailQueueTo EmailQueueTo
		{
			get { return this._EmailQueueTo.Entity; }

			set
			{
				EmailQueueTo previousValue = this._EmailQueueTo.Entity;
				if (((previousValue != value) 
							|| (this._EmailQueueTo.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._EmailQueueTo.Entity = null;
						previousValue.EmailQueueToFails.Remove(this);
					}

					this._EmailQueueTo.Entity = value;
					if (value != null)
					{
						value.EmailQueueToFails.Add(this);
						
						this._Id = value.Id;
						
						this._PeopleId = value.PeopleId;
						
					}

					else
					{
						
						this._Id = default(int);
						
						this._PeopleId = default(int);
						
					}

					this.SendPropertyChanged("EmailQueueTo");
				}

			}

		}

		
		[Association(Name="FK_EmailQueueToFail_People", Storage="_Person", ThisKey="PeopleId", IsForeignKey=true)]
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
						previousValue.EmailQueueToFails.Remove(this);
					}

					this._Person.Entity = value;
					if (value != null)
					{
						value.EmailQueueToFails.Add(this);
						
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

