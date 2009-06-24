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
	[Table(Name="dbo.SoulMate")]
	public partial class SoulMate : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _Id;
		
		private int? _HimId;
		
		private int? _HerId;
		
		private int? _EventId;
		
		private int? _Relationship;
		
		private int? _ChildcareId;
		
   		
    	
		private EntityRef< Meeting> _ChildCareMeeting;
		
		private EntityRef< Meeting> _Meeting;
		
		private EntityRef< Person> _Her;
		
		private EntityRef< Person> _Him;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnIdChanging(int value);
		partial void OnIdChanged();
		
		partial void OnHimIdChanging(int? value);
		partial void OnHimIdChanged();
		
		partial void OnHerIdChanging(int? value);
		partial void OnHerIdChanged();
		
		partial void OnEventIdChanging(int? value);
		partial void OnEventIdChanged();
		
		partial void OnRelationshipChanging(int? value);
		partial void OnRelationshipChanged();
		
		partial void OnChildcareIdChanging(int? value);
		partial void OnChildcareIdChanged();
		
    #endregion
		public SoulMate()
		{
			
			
			this._ChildCareMeeting = default(EntityRef< Meeting>); 
			
			this._Meeting = default(EntityRef< Meeting>); 
			
			this._Her = default(EntityRef< Person>); 
			
			this._Him = default(EntityRef< Person>); 
			
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
				
                    this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}

			}

		}

		
		[Column(Name="HimId", UpdateCheck=UpdateCheck.Never, Storage="_HimId", DbType="int")]
		public int? HimId
		{
			get { return this._HimId; }

			set
			{
				if (this._HimId != value)
				{
				
					if (this._Him.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnHimIdChanging(value);
					this.SendPropertyChanging();
					this._HimId = value;
					this.SendPropertyChanged("HimId");
					this.OnHimIdChanged();
				}

			}

		}

		
		[Column(Name="HerId", UpdateCheck=UpdateCheck.Never, Storage="_HerId", DbType="int")]
		public int? HerId
		{
			get { return this._HerId; }

			set
			{
				if (this._HerId != value)
				{
				
					if (this._Her.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnHerIdChanging(value);
					this.SendPropertyChanging();
					this._HerId = value;
					this.SendPropertyChanged("HerId");
					this.OnHerIdChanged();
				}

			}

		}

		
		[Column(Name="EventId", UpdateCheck=UpdateCheck.Never, Storage="_EventId", DbType="int")]
		public int? EventId
		{
			get { return this._EventId; }

			set
			{
				if (this._EventId != value)
				{
				
					if (this._Meeting.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnEventIdChanging(value);
					this.SendPropertyChanging();
					this._EventId = value;
					this.SendPropertyChanged("EventId");
					this.OnEventIdChanged();
				}

			}

		}

		
		[Column(Name="Relationship", UpdateCheck=UpdateCheck.Never, Storage="_Relationship", DbType="int")]
		public int? Relationship
		{
			get { return this._Relationship; }

			set
			{
				if (this._Relationship != value)
				{
				
                    this.OnRelationshipChanging(value);
					this.SendPropertyChanging();
					this._Relationship = value;
					this.SendPropertyChanged("Relationship");
					this.OnRelationshipChanged();
				}

			}

		}

		
		[Column(Name="ChildcareId", UpdateCheck=UpdateCheck.Never, Storage="_ChildcareId", DbType="int")]
		public int? ChildcareId
		{
			get { return this._ChildcareId; }

			set
			{
				if (this._ChildcareId != value)
				{
				
					if (this._ChildCareMeeting.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnChildcareIdChanging(value);
					this.SendPropertyChanging();
					this._ChildcareId = value;
					this.SendPropertyChanged("ChildcareId");
					this.OnChildcareIdChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="ChildSoulMates__ChildCareMeeting", Storage="_ChildCareMeeting", ThisKey="ChildcareId", IsForeignKey=true)]
		public Meeting ChildCareMeeting
		{
			get { return this._ChildCareMeeting.Entity; }

			set
			{
				Meeting previousValue = this._ChildCareMeeting.Entity;
				if (((previousValue != value) 
							|| (this._ChildCareMeeting.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._ChildCareMeeting.Entity = null;
						previousValue.ChildSoulMates.Remove(this);
					}

					this._ChildCareMeeting.Entity = value;
					if (value != null)
					{
						value.ChildSoulMates.Add(this);
						
						this._ChildcareId = value.MeetingId;
						
					}

					else
					{
						
						this._ChildcareId = default(int?);
						
					}

					this.SendPropertyChanged("ChildCareMeeting");
				}

			}

		}

		
		[Association(Name="FK_SoulMate_Meetings", Storage="_Meeting", ThisKey="EventId", IsForeignKey=true)]
		public Meeting Meeting
		{
			get { return this._Meeting.Entity; }

			set
			{
				Meeting previousValue = this._Meeting.Entity;
				if (((previousValue != value) 
							|| (this._Meeting.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._Meeting.Entity = null;
						previousValue.SoulMates.Remove(this);
					}

					this._Meeting.Entity = value;
					if (value != null)
					{
						value.SoulMates.Add(this);
						
						this._EventId = value.MeetingId;
						
					}

					else
					{
						
						this._EventId = default(int?);
						
					}

					this.SendPropertyChanged("Meeting");
				}

			}

		}

		
		[Association(Name="HerSoulMates__Her", Storage="_Her", ThisKey="HerId", IsForeignKey=true)]
		public Person Her
		{
			get { return this._Her.Entity; }

			set
			{
				Person previousValue = this._Her.Entity;
				if (((previousValue != value) 
							|| (this._Her.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._Her.Entity = null;
						previousValue.HerSoulMates.Remove(this);
					}

					this._Her.Entity = value;
					if (value != null)
					{
						value.HerSoulMates.Add(this);
						
						this._HerId = value.PeopleId;
						
					}

					else
					{
						
						this._HerId = default(int?);
						
					}

					this.SendPropertyChanged("Her");
				}

			}

		}

		
		[Association(Name="HisSoulMates__Him", Storage="_Him", ThisKey="HimId", IsForeignKey=true)]
		public Person Him
		{
			get { return this._Him.Entity; }

			set
			{
				Person previousValue = this._Him.Entity;
				if (((previousValue != value) 
							|| (this._Him.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._Him.Entity = null;
						previousValue.HisSoulMates.Remove(this);
					}

					this._Him.Entity = value;
					if (value != null)
					{
						value.HisSoulMates.Add(this);
						
						this._HimId = value.PeopleId;
						
					}

					else
					{
						
						this._HimId = default(int?);
						
					}

					this.SendPropertyChanged("Him");
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

