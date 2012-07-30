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
	[Table(Name="dbo.VolSubRequest")]
	public partial class VolSubRequest : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int? _AttendId;
		
		private int? _RequestorId;
		
		private DateTime _Requested;
		
		private int _SubstituteId;
		
		private DateTime? _Responded;
		
		private bool? _CanSub;
		
   		
    	
		private EntityRef< Person> _Person;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnAttendIdChanging(int? value);
		partial void OnAttendIdChanged();
		
		partial void OnRequestorIdChanging(int? value);
		partial void OnRequestorIdChanged();
		
		partial void OnRequestedChanging(DateTime value);
		partial void OnRequestedChanged();
		
		partial void OnSubstituteIdChanging(int value);
		partial void OnSubstituteIdChanged();
		
		partial void OnRespondedChanging(DateTime? value);
		partial void OnRespondedChanged();
		
		partial void OnCanSubChanging(bool? value);
		partial void OnCanSubChanged();
		
    #endregion
		public VolSubRequest()
		{
			
			
			this._Person = default(EntityRef< Person>); 
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="AttendId", UpdateCheck=UpdateCheck.Never, Storage="_AttendId", DbType="int")]
		public int? AttendId
		{
			get { return this._AttendId; }

			set
			{
				if (this._AttendId != value)
				{
				
                    this.OnAttendIdChanging(value);
					this.SendPropertyChanging();
					this._AttendId = value;
					this.SendPropertyChanged("AttendId");
					this.OnAttendIdChanged();
				}

			}

		}

		
		[Column(Name="RequestorId", UpdateCheck=UpdateCheck.Never, Storage="_RequestorId", DbType="int")]
		public int? RequestorId
		{
			get { return this._RequestorId; }

			set
			{
				if (this._RequestorId != value)
				{
				
					if (this._Person.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnRequestorIdChanging(value);
					this.SendPropertyChanging();
					this._RequestorId = value;
					this.SendPropertyChanged("RequestorId");
					this.OnRequestorIdChanged();
				}

			}

		}

		
		[Column(Name="Requested", UpdateCheck=UpdateCheck.Never, Storage="_Requested", DbType="datetime NOT NULL")]
		public DateTime Requested
		{
			get { return this._Requested; }

			set
			{
				if (this._Requested != value)
				{
				
                    this.OnRequestedChanging(value);
					this.SendPropertyChanging();
					this._Requested = value;
					this.SendPropertyChanged("Requested");
					this.OnRequestedChanged();
				}

			}

		}

		
		[Column(Name="SubstituteId", UpdateCheck=UpdateCheck.Never, Storage="_SubstituteId", DbType="int NOT NULL")]
		public int SubstituteId
		{
			get { return this._SubstituteId; }

			set
			{
				if (this._SubstituteId != value)
				{
				
                    this.OnSubstituteIdChanging(value);
					this.SendPropertyChanging();
					this._SubstituteId = value;
					this.SendPropertyChanged("SubstituteId");
					this.OnSubstituteIdChanged();
				}

			}

		}

		
		[Column(Name="Responded", UpdateCheck=UpdateCheck.Never, Storage="_Responded", DbType="datetime")]
		public DateTime? Responded
		{
			get { return this._Responded; }

			set
			{
				if (this._Responded != value)
				{
				
                    this.OnRespondedChanging(value);
					this.SendPropertyChanging();
					this._Responded = value;
					this.SendPropertyChanged("Responded");
					this.OnRespondedChanged();
				}

			}

		}

		
		[Column(Name="CanSub", UpdateCheck=UpdateCheck.Never, Storage="_CanSub", DbType="bit")]
		public bool? CanSub
		{
			get { return this._CanSub; }

			set
			{
				if (this._CanSub != value)
				{
				
                    this.OnCanSubChanging(value);
					this.SendPropertyChanging();
					this._CanSub = value;
					this.SendPropertyChanged("CanSub");
					this.OnCanSubChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="FK_VolSubRequest_People", Storage="_Person", ThisKey="RequestorId", IsForeignKey=true)]
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
						previousValue.VolSubRequests.Remove(this);
					}

					this._Person.Entity = value;
					if (value != null)
					{
						value.VolSubRequests.Add(this);
						
						this._RequestorId = value.PeopleId;
						
					}

					else
					{
						
						this._RequestorId = default(int?);
						
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

