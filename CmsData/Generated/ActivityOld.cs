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
	[Table(Name="dbo.ActivityOld")]
	public partial class ActivityOld : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private long _Id;
		
		private DateTime? _ActivityDate;
		
		private int? _UserId;
		
		private string _Activity;
		
		private string _PageUrl;
		
		private string _Machine;
		
		private int? _OrgId;
		
		private int? _PeopleId;
		
   		
    	
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnIdChanging(long value);
		partial void OnIdChanged();
		
		partial void OnActivityDateChanging(DateTime? value);
		partial void OnActivityDateChanged();
		
		partial void OnUserIdChanging(int? value);
		partial void OnUserIdChanged();
		
		partial void OnActivityChanging(string value);
		partial void OnActivityChanged();
		
		partial void OnPageUrlChanging(string value);
		partial void OnPageUrlChanged();
		
		partial void OnMachineChanging(string value);
		partial void OnMachineChanged();
		
		partial void OnOrgIdChanging(int? value);
		partial void OnOrgIdChanged();
		
		partial void OnPeopleIdChanging(int? value);
		partial void OnPeopleIdChanged();
		
    #endregion
		public ActivityOld()
		{
			
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="Id", UpdateCheck=UpdateCheck.Never, Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="bigint NOT NULL IDENTITY", IsDbGenerated=true)]
		public long Id
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

		
		[Column(Name="ActivityDate", UpdateCheck=UpdateCheck.Never, Storage="_ActivityDate", DbType="datetime")]
		public DateTime? ActivityDate
		{
			get { return this._ActivityDate; }

			set
			{
				if (this._ActivityDate != value)
				{
				
                    this.OnActivityDateChanging(value);
					this.SendPropertyChanging();
					this._ActivityDate = value;
					this.SendPropertyChanged("ActivityDate");
					this.OnActivityDateChanged();
				}

			}

		}

		
		[Column(Name="UserId", UpdateCheck=UpdateCheck.Never, Storage="_UserId", DbType="int")]
		public int? UserId
		{
			get { return this._UserId; }

			set
			{
				if (this._UserId != value)
				{
				
                    this.OnUserIdChanging(value);
					this.SendPropertyChanging();
					this._UserId = value;
					this.SendPropertyChanged("UserId");
					this.OnUserIdChanged();
				}

			}

		}

		
		[Column(Name="Activity", UpdateCheck=UpdateCheck.Never, Storage="_Activity", DbType="varchar(200)")]
		public string Activity
		{
			get { return this._Activity; }

			set
			{
				if (this._Activity != value)
				{
				
                    this.OnActivityChanging(value);
					this.SendPropertyChanging();
					this._Activity = value;
					this.SendPropertyChanged("Activity");
					this.OnActivityChanged();
				}

			}

		}

		
		[Column(Name="PageUrl", UpdateCheck=UpdateCheck.Never, Storage="_PageUrl", DbType="varchar(410)")]
		public string PageUrl
		{
			get { return this._PageUrl; }

			set
			{
				if (this._PageUrl != value)
				{
				
                    this.OnPageUrlChanging(value);
					this.SendPropertyChanging();
					this._PageUrl = value;
					this.SendPropertyChanged("PageUrl");
					this.OnPageUrlChanged();
				}

			}

		}

		
		[Column(Name="Machine", UpdateCheck=UpdateCheck.Never, Storage="_Machine", DbType="varchar(50)")]
		public string Machine
		{
			get { return this._Machine; }

			set
			{
				if (this._Machine != value)
				{
				
                    this.OnMachineChanging(value);
					this.SendPropertyChanging();
					this._Machine = value;
					this.SendPropertyChanged("Machine");
					this.OnMachineChanged();
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
				
                    this.OnOrgIdChanging(value);
					this.SendPropertyChanging();
					this._OrgId = value;
					this.SendPropertyChanged("OrgId");
					this.OnOrgIdChanged();
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

