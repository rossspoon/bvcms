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
	[Table(Name="lookup.WeeklySchedule2")]
	public partial class WeeklySchedule2 : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _Id;
		
		private string _Description;
		
		private int _Day;
		
		private DateTime _MeetingTime;
		
		private string _Code;
		
		private int? _ScheduleId;
		
   		
    	
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnIdChanging(int value);
		partial void OnIdChanged();
		
		partial void OnDescriptionChanging(string value);
		partial void OnDescriptionChanged();
		
		partial void OnDayChanging(int value);
		partial void OnDayChanged();
		
		partial void OnMeetingTimeChanging(DateTime value);
		partial void OnMeetingTimeChanged();
		
		partial void OnCodeChanging(string value);
		partial void OnCodeChanged();
		
		partial void OnScheduleIdChanging(int? value);
		partial void OnScheduleIdChanged();
		
    #endregion
		public WeeklySchedule2()
		{
			
			
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

		
		[Column(Name="Day", UpdateCheck=UpdateCheck.Never, Storage="_Day", DbType="int NOT NULL")]
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

		
		[Column(Name="MeetingTime", UpdateCheck=UpdateCheck.Never, Storage="_MeetingTime", DbType="datetime NOT NULL")]
		public DateTime MeetingTime
		{
			get { return this._MeetingTime; }

			set
			{
				if (this._MeetingTime != value)
				{
				
                    this.OnMeetingTimeChanging(value);
					this.SendPropertyChanging();
					this._MeetingTime = value;
					this.SendPropertyChanged("MeetingTime");
					this.OnMeetingTimeChanged();
				}

			}

		}

		
		[Column(Name="Code", UpdateCheck=UpdateCheck.Never, Storage="_Code", DbType="varchar(10)")]
		public string Code
		{
			get { return this._Code; }

			set
			{
				if (this._Code != value)
				{
				
                    this.OnCodeChanging(value);
					this.SendPropertyChanging();
					this._Code = value;
					this.SendPropertyChanged("Code");
					this.OnCodeChanged();
				}

			}

		}

		
		[Column(Name="ScheduleId", UpdateCheck=UpdateCheck.Never, Storage="_ScheduleId", DbType="int", IsDbGenerated=true)]
		public int? ScheduleId
		{
			get { return this._ScheduleId; }

			set
			{
				if (this._ScheduleId != value)
				{
				
                    this.OnScheduleIdChanging(value);
					this.SendPropertyChanging();
					this._ScheduleId = value;
					this.SendPropertyChanged("ScheduleId");
					this.OnScheduleIdChanged();
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

