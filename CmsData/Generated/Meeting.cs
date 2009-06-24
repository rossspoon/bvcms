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
	[Table(Name="dbo.Meetings")]
	public partial class Meeting : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _MeetingId;
		
		private int _CreatedBy;
		
		private DateTime _CreatedDate;
		
		private int _OrganizationId;
		
		private int _NumPresent;
		
		private int _NumMembers;
		
		private int _NumVstMembers;
		
		private int _NumRepeatVst;
		
		private int _NumNewVisit;
		
		private string _Location;
		
		private DateTime? _MeetingDate;
		
		private bool _GroupMeetingFlag;
		
   		
   		private EntitySet< SoulMate> _ChildSoulMates;
		
   		private EntitySet< Attend> _Attends;
		
   		private EntitySet< SoulMate> _SoulMates;
		
    	
		private EntityRef< Organization> _Organization;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnMeetingIdChanging(int value);
		partial void OnMeetingIdChanged();
		
		partial void OnCreatedByChanging(int value);
		partial void OnCreatedByChanged();
		
		partial void OnCreatedDateChanging(DateTime value);
		partial void OnCreatedDateChanged();
		
		partial void OnOrganizationIdChanging(int value);
		partial void OnOrganizationIdChanged();
		
		partial void OnNumPresentChanging(int value);
		partial void OnNumPresentChanged();
		
		partial void OnNumMembersChanging(int value);
		partial void OnNumMembersChanged();
		
		partial void OnNumVstMembersChanging(int value);
		partial void OnNumVstMembersChanged();
		
		partial void OnNumRepeatVstChanging(int value);
		partial void OnNumRepeatVstChanged();
		
		partial void OnNumNewVisitChanging(int value);
		partial void OnNumNewVisitChanged();
		
		partial void OnLocationChanging(string value);
		partial void OnLocationChanged();
		
		partial void OnMeetingDateChanging(DateTime? value);
		partial void OnMeetingDateChanged();
		
		partial void OnGroupMeetingFlagChanging(bool value);
		partial void OnGroupMeetingFlagChanged();
		
    #endregion
		public Meeting()
		{
			
			this._ChildSoulMates = new EntitySet< SoulMate>(new Action< SoulMate>(this.attach_ChildSoulMates), new Action< SoulMate>(this.detach_ChildSoulMates)); 
			
			this._Attends = new EntitySet< Attend>(new Action< Attend>(this.attach_Attends), new Action< Attend>(this.detach_Attends)); 
			
			this._SoulMates = new EntitySet< SoulMate>(new Action< SoulMate>(this.attach_SoulMates), new Action< SoulMate>(this.detach_SoulMates)); 
			
			
			this._Organization = default(EntityRef< Organization>); 
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="MeetingId", UpdateCheck=UpdateCheck.Never, Storage="_MeetingId", AutoSync=AutoSync.OnInsert, DbType="int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int MeetingId
		{
			get { return this._MeetingId; }

			set
			{
				if (this._MeetingId != value)
				{
				
                    this.OnMeetingIdChanging(value);
					this.SendPropertyChanging();
					this._MeetingId = value;
					this.SendPropertyChanged("MeetingId");
					this.OnMeetingIdChanged();
				}

			}

		}

		
		[Column(Name="CreatedBy", UpdateCheck=UpdateCheck.Never, Storage="_CreatedBy", DbType="int NOT NULL")]
		public int CreatedBy
		{
			get { return this._CreatedBy; }

			set
			{
				if (this._CreatedBy != value)
				{
				
                    this.OnCreatedByChanging(value);
					this.SendPropertyChanging();
					this._CreatedBy = value;
					this.SendPropertyChanged("CreatedBy");
					this.OnCreatedByChanged();
				}

			}

		}

		
		[Column(Name="CreatedDate", UpdateCheck=UpdateCheck.Never, Storage="_CreatedDate", DbType="datetime NOT NULL")]
		public DateTime CreatedDate
		{
			get { return this._CreatedDate; }

			set
			{
				if (this._CreatedDate != value)
				{
				
                    this.OnCreatedDateChanging(value);
					this.SendPropertyChanging();
					this._CreatedDate = value;
					this.SendPropertyChanged("CreatedDate");
					this.OnCreatedDateChanged();
				}

			}

		}

		
		[Column(Name="OrganizationId", UpdateCheck=UpdateCheck.Never, Storage="_OrganizationId", DbType="int NOT NULL")]
		public int OrganizationId
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

		
		[Column(Name="NumPresent", UpdateCheck=UpdateCheck.Never, Storage="_NumPresent", DbType="int NOT NULL")]
		public int NumPresent
		{
			get { return this._NumPresent; }

			set
			{
				if (this._NumPresent != value)
				{
				
                    this.OnNumPresentChanging(value);
					this.SendPropertyChanging();
					this._NumPresent = value;
					this.SendPropertyChanged("NumPresent");
					this.OnNumPresentChanged();
				}

			}

		}

		
		[Column(Name="NumMembers", UpdateCheck=UpdateCheck.Never, Storage="_NumMembers", DbType="int NOT NULL")]
		public int NumMembers
		{
			get { return this._NumMembers; }

			set
			{
				if (this._NumMembers != value)
				{
				
                    this.OnNumMembersChanging(value);
					this.SendPropertyChanging();
					this._NumMembers = value;
					this.SendPropertyChanged("NumMembers");
					this.OnNumMembersChanged();
				}

			}

		}

		
		[Column(Name="NumVstMembers", UpdateCheck=UpdateCheck.Never, Storage="_NumVstMembers", DbType="int NOT NULL")]
		public int NumVstMembers
		{
			get { return this._NumVstMembers; }

			set
			{
				if (this._NumVstMembers != value)
				{
				
                    this.OnNumVstMembersChanging(value);
					this.SendPropertyChanging();
					this._NumVstMembers = value;
					this.SendPropertyChanged("NumVstMembers");
					this.OnNumVstMembersChanged();
				}

			}

		}

		
		[Column(Name="NumRepeatVst", UpdateCheck=UpdateCheck.Never, Storage="_NumRepeatVst", DbType="int NOT NULL")]
		public int NumRepeatVst
		{
			get { return this._NumRepeatVst; }

			set
			{
				if (this._NumRepeatVst != value)
				{
				
                    this.OnNumRepeatVstChanging(value);
					this.SendPropertyChanging();
					this._NumRepeatVst = value;
					this.SendPropertyChanged("NumRepeatVst");
					this.OnNumRepeatVstChanged();
				}

			}

		}

		
		[Column(Name="NumNewVisit", UpdateCheck=UpdateCheck.Never, Storage="_NumNewVisit", DbType="int NOT NULL")]
		public int NumNewVisit
		{
			get { return this._NumNewVisit; }

			set
			{
				if (this._NumNewVisit != value)
				{
				
                    this.OnNumNewVisitChanging(value);
					this.SendPropertyChanging();
					this._NumNewVisit = value;
					this.SendPropertyChanged("NumNewVisit");
					this.OnNumNewVisitChanged();
				}

			}

		}

		
		[Column(Name="Location", UpdateCheck=UpdateCheck.Never, Storage="_Location", DbType="varchar(25)")]
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

		
		[Column(Name="MeetingDate", UpdateCheck=UpdateCheck.Never, Storage="_MeetingDate", DbType="datetime")]
		public DateTime? MeetingDate
		{
			get { return this._MeetingDate; }

			set
			{
				if (this._MeetingDate != value)
				{
				
                    this.OnMeetingDateChanging(value);
					this.SendPropertyChanging();
					this._MeetingDate = value;
					this.SendPropertyChanged("MeetingDate");
					this.OnMeetingDateChanged();
				}

			}

		}

		
		[Column(Name="GroupMeetingFlag", UpdateCheck=UpdateCheck.Never, Storage="_GroupMeetingFlag", DbType="bit NOT NULL")]
		public bool GroupMeetingFlag
		{
			get { return this._GroupMeetingFlag; }

			set
			{
				if (this._GroupMeetingFlag != value)
				{
				
                    this.OnGroupMeetingFlagChanging(value);
					this.SendPropertyChanging();
					this._GroupMeetingFlag = value;
					this.SendPropertyChanged("GroupMeetingFlag");
					this.OnGroupMeetingFlagChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
   		[Association(Name="ChildSoulMates__ChildCareMeeting", Storage="_ChildSoulMates", OtherKey="ChildcareId")]
   		public EntitySet< SoulMate> ChildSoulMates
   		{
   		    get { return this._ChildSoulMates; }

			set	{ this._ChildSoulMates.Assign(value); }

   		}

		
   		[Association(Name="FK_AttendWithAbsents_TBL_MEETINGS_TBL", Storage="_Attends", OtherKey="MeetingId")]
   		public EntitySet< Attend> Attends
   		{
   		    get { return this._Attends; }

			set	{ this._Attends.Assign(value); }

   		}

		
   		[Association(Name="FK_SoulMate_Meetings", Storage="_SoulMates", OtherKey="EventId")]
   		public EntitySet< SoulMate> SoulMates
   		{
   		    get { return this._SoulMates; }

			set	{ this._SoulMates.Assign(value); }

   		}

		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="FK_MEETINGS_TBL_ORGANIZATIONS_TBL", Storage="_Organization", ThisKey="OrganizationId", IsForeignKey=true)]
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
						previousValue.Meetings.Remove(this);
					}

					this._Organization.Entity = value;
					if (value != null)
					{
						value.Meetings.Add(this);
						
						this._OrganizationId = value.OrganizationId;
						
					}

					else
					{
						
						this._OrganizationId = default(int);
						
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

   		
		private void attach_ChildSoulMates(SoulMate entity)
		{
			this.SendPropertyChanging();
			entity.ChildCareMeeting = this;
		}

		private void detach_ChildSoulMates(SoulMate entity)
		{
			this.SendPropertyChanging();
			entity.ChildCareMeeting = null;
		}

		
		private void attach_Attends(Attend entity)
		{
			this.SendPropertyChanging();
			entity.Meeting = this;
		}

		private void detach_Attends(Attend entity)
		{
			this.SendPropertyChanging();
			entity.Meeting = null;
		}

		
		private void attach_SoulMates(SoulMate entity)
		{
			this.SendPropertyChanging();
			entity.Meeting = this;
		}

		private void detach_SoulMates(SoulMate entity)
		{
			this.SendPropertyChanging();
			entity.Meeting = null;
		}

		
	}

}

