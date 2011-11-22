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
	[Table(Name="dbo.MeetingExtra")]
	public partial class MeetingExtra : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _MeetingId;
		
		private string _Field;
		
		private string _Data;
		
		private string _DataType;
		
   		
    	
		private EntityRef< Meeting> _Meeting;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnMeetingIdChanging(int value);
		partial void OnMeetingIdChanged();
		
		partial void OnFieldChanging(string value);
		partial void OnFieldChanged();
		
		partial void OnDataChanging(string value);
		partial void OnDataChanged();
		
		partial void OnDataTypeChanging(string value);
		partial void OnDataTypeChanged();
		
    #endregion
		public MeetingExtra()
		{
			
			
			this._Meeting = default(EntityRef< Meeting>); 
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="MeetingId", UpdateCheck=UpdateCheck.Never, Storage="_MeetingId", DbType="int NOT NULL", IsPrimaryKey=true)]
		public int MeetingId
		{
			get { return this._MeetingId; }

			set
			{
				if (this._MeetingId != value)
				{
				
					if (this._Meeting.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnMeetingIdChanging(value);
					this.SendPropertyChanging();
					this._MeetingId = value;
					this.SendPropertyChanged("MeetingId");
					this.OnMeetingIdChanged();
				}

			}

		}

		
		[Column(Name="Field", UpdateCheck=UpdateCheck.Never, Storage="_Field", DbType="varchar(50) NOT NULL", IsPrimaryKey=true)]
		public string Field
		{
			get { return this._Field; }

			set
			{
				if (this._Field != value)
				{
				
                    this.OnFieldChanging(value);
					this.SendPropertyChanging();
					this._Field = value;
					this.SendPropertyChanged("Field");
					this.OnFieldChanged();
				}

			}

		}

		
		[Column(Name="Data", UpdateCheck=UpdateCheck.Never, Storage="_Data", DbType="varchar")]
		public string Data
		{
			get { return this._Data; }

			set
			{
				if (this._Data != value)
				{
				
                    this.OnDataChanging(value);
					this.SendPropertyChanging();
					this._Data = value;
					this.SendPropertyChanged("Data");
					this.OnDataChanged();
				}

			}

		}

		
		[Column(Name="DataType", UpdateCheck=UpdateCheck.Never, Storage="_DataType", DbType="varchar(5)")]
		public string DataType
		{
			get { return this._DataType; }

			set
			{
				if (this._DataType != value)
				{
				
                    this.OnDataTypeChanging(value);
					this.SendPropertyChanging();
					this._DataType = value;
					this.SendPropertyChanged("DataType");
					this.OnDataTypeChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="FK_MeetingExtra_Meetings", Storage="_Meeting", ThisKey="MeetingId", IsForeignKey=true)]
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
						previousValue.MeetingExtras.Remove(this);
					}

					this._Meeting.Entity = value;
					if (value != null)
					{
						value.MeetingExtras.Add(this);
						
						this._MeetingId = value.MeetingId;
						
					}

					else
					{
						
						this._MeetingId = default(int);
						
					}

					this.SendPropertyChanged("Meeting");
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

