using System; 
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;

namespace DiscData
{
	[Table(Name="dbo.ForumNotify")]
	public partial class ForumNotify : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _ThreadId;
		
		private string _UserId;
		
		private int? _CUserid;
		
   		
    	
		private EntityRef< ForumEntry> _ForumEntry;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnThreadIdChanging(int value);
		partial void OnThreadIdChanged();
		
		partial void OnUserIdChanging(string value);
		partial void OnUserIdChanged();
		
		partial void OnCUseridChanging(int? value);
		partial void OnCUseridChanged();
		
    #endregion
		public ForumNotify()
		{
			
			
			this._ForumEntry = default(EntityRef< ForumEntry>); 
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="ThreadId", UpdateCheck=UpdateCheck.Never, Storage="_ThreadId", DbType="int NOT NULL", IsPrimaryKey=true)]
		public int ThreadId
		{
			get { return this._ThreadId; }

			set
			{
				if (this._ThreadId != value)
				{
				
					if (this._ForumEntry.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnThreadIdChanging(value);
					this.SendPropertyChanging();
					this._ThreadId = value;
					this.SendPropertyChanged("ThreadId");
					this.OnThreadIdChanged();
				}

			}

		}

		
		[Column(Name="UserId", UpdateCheck=UpdateCheck.Never, Storage="_UserId", DbType="nvarchar(20) NOT NULL", IsPrimaryKey=true)]
		public string UserId
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

		
		[Column(Name="cUserid", UpdateCheck=UpdateCheck.Never, Storage="_CUserid", DbType="int")]
		public int? CUserid
		{
			get { return this._CUserid; }

			set
			{
				if (this._CUserid != value)
				{
				
                    this.OnCUseridChanging(value);
					this.SendPropertyChanging();
					this._CUserid = value;
					this.SendPropertyChanged("CUserid");
					this.OnCUseridChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="FK_ForumNotify_ForumEntry", Storage="_ForumEntry", ThisKey="ThreadId", IsForeignKey=true)]
		public ForumEntry ForumEntry
		{
			get { return this._ForumEntry.Entity; }

			set
			{
				ForumEntry previousValue = this._ForumEntry.Entity;
				if (((previousValue != value) 
							|| (this._ForumEntry.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._ForumEntry.Entity = null;
						previousValue.ForumNotifications.Remove(this);
					}

					this._ForumEntry.Entity = value;
					if (value != null)
					{
						value.ForumNotifications.Add(this);
						
						this._ThreadId = value.Id;
						
					}

					else
					{
						
						this._ThreadId = default(int);
						
					}

					this.SendPropertyChanged("ForumEntry");
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

