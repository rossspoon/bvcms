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
	[Table(Name="dbo.CheckInActivity")]
	public partial class CheckInActivity : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _CheckinId;
		
		private int _ActivityId;
		
   		
    	
		private EntityRef< Activity> _Activity;
		
		private EntityRef< CheckInTime> _CheckInTime;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnCheckinIdChanging(int value);
		partial void OnCheckinIdChanged();
		
		partial void OnActivityIdChanging(int value);
		partial void OnActivityIdChanged();
		
    #endregion
		public CheckInActivity()
		{
			
			
			this._Activity = default(EntityRef< Activity>); 
			
			this._CheckInTime = default(EntityRef< CheckInTime>); 
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="CheckinId", UpdateCheck=UpdateCheck.Never, Storage="_CheckinId", DbType="int NOT NULL", IsPrimaryKey=true)]
		public int CheckinId
		{
			get { return this._CheckinId; }

			set
			{
				if (this._CheckinId != value)
				{
				
					if (this._CheckInTime.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnCheckinIdChanging(value);
					this.SendPropertyChanging();
					this._CheckinId = value;
					this.SendPropertyChanged("CheckinId");
					this.OnCheckinIdChanged();
				}

			}

		}

		
		[Column(Name="ActivityId", UpdateCheck=UpdateCheck.Never, Storage="_ActivityId", DbType="int NOT NULL", IsPrimaryKey=true)]
		public int ActivityId
		{
			get { return this._ActivityId; }

			set
			{
				if (this._ActivityId != value)
				{
				
					if (this._Activity.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnActivityIdChanging(value);
					this.SendPropertyChanging();
					this._ActivityId = value;
					this.SendPropertyChanged("ActivityId");
					this.OnActivityIdChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="FK_CheckInActivity_Activity", Storage="_Activity", ThisKey="ActivityId", IsForeignKey=true)]
		public Activity Activity
		{
			get { return this._Activity.Entity; }

			set
			{
				Activity previousValue = this._Activity.Entity;
				if (((previousValue != value) 
							|| (this._Activity.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._Activity.Entity = null;
						previousValue.CheckInActivities.Remove(this);
					}

					this._Activity.Entity = value;
					if (value != null)
					{
						value.CheckInActivities.Add(this);
						
						this._ActivityId = value.Id;
						
					}

					else
					{
						
						this._ActivityId = default(int);
						
					}

					this.SendPropertyChanged("Activity");
				}

			}

		}

		
		[Association(Name="FK_CheckInActivity_CheckInTimes", Storage="_CheckInTime", ThisKey="CheckinId", IsForeignKey=true)]
		public CheckInTime CheckInTime
		{
			get { return this._CheckInTime.Entity; }

			set
			{
				CheckInTime previousValue = this._CheckInTime.Entity;
				if (((previousValue != value) 
							|| (this._CheckInTime.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._CheckInTime.Entity = null;
						previousValue.CheckInActivities.Remove(this);
					}

					this._CheckInTime.Entity = value;
					if (value != null)
					{
						value.CheckInActivities.Add(this);
						
						this._CheckinId = value.Id;
						
					}

					else
					{
						
						this._CheckinId = default(int);
						
					}

					this.SendPropertyChanged("CheckInTime");
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

