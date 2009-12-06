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
	[Table(Name="disc.PageVisit")]
	public partial class PageVisit : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private DateTime _CreatedOn;
		
		private string _PageTitle;
		
		private int _Id;
		
		private string _PageUrl;
		
		private DateTime? _VisitTime;
		
		private int? _UserId;
		
		private int? _CUserid;
		
   		
    	
		private EntityRef< User> _User;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnCreatedOnChanging(DateTime value);
		partial void OnCreatedOnChanged();
		
		partial void OnPageTitleChanging(string value);
		partial void OnPageTitleChanged();
		
		partial void OnIdChanging(int value);
		partial void OnIdChanged();
		
		partial void OnPageUrlChanging(string value);
		partial void OnPageUrlChanged();
		
		partial void OnVisitTimeChanging(DateTime? value);
		partial void OnVisitTimeChanged();
		
		partial void OnUserIdChanging(int? value);
		partial void OnUserIdChanged();
		
		partial void OnCUseridChanging(int? value);
		partial void OnCUseridChanged();
		
    #endregion
		public PageVisit()
		{
			
			
			this._User = default(EntityRef< User>); 
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="CreatedOn", UpdateCheck=UpdateCheck.Never, Storage="_CreatedOn", DbType="datetime NOT NULL")]
		public DateTime CreatedOn
		{
			get { return this._CreatedOn; }

			set
			{
				if (this._CreatedOn != value)
				{
				
                    this.OnCreatedOnChanging(value);
					this.SendPropertyChanging();
					this._CreatedOn = value;
					this.SendPropertyChanged("CreatedOn");
					this.OnCreatedOnChanged();
				}

			}

		}

		
		[Column(Name="PageTitle", UpdateCheck=UpdateCheck.Never, Storage="_PageTitle", DbType="varchar(100) NOT NULL")]
		public string PageTitle
		{
			get { return this._PageTitle; }

			set
			{
				if (this._PageTitle != value)
				{
				
                    this.OnPageTitleChanging(value);
					this.SendPropertyChanging();
					this._PageTitle = value;
					this.SendPropertyChanged("PageTitle");
					this.OnPageTitleChanged();
				}

			}

		}

		
		[Column(Name="Id", UpdateCheck=UpdateCheck.Never, Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
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

		
		[Column(Name="PageUrl", UpdateCheck=UpdateCheck.Never, Storage="_PageUrl", DbType="varchar(150)")]
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

		
		[Column(Name="VisitTime", UpdateCheck=UpdateCheck.Never, Storage="_VisitTime", DbType="datetime")]
		public DateTime? VisitTime
		{
			get { return this._VisitTime; }

			set
			{
				if (this._VisitTime != value)
				{
				
                    this.OnVisitTimeChanging(value);
					this.SendPropertyChanging();
					this._VisitTime = value;
					this.SendPropertyChanged("VisitTime");
					this.OnVisitTimeChanged();
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
				
					if (this._User.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
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
    	
		[Association(Name="FK_PageVisit_Users", Storage="_User", ThisKey="UserId", IsForeignKey=true)]
		public User User
		{
			get { return this._User.Entity; }

			set
			{
				User previousValue = this._User.Entity;
				if (((previousValue != value) 
							|| (this._User.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._User.Entity = null;
						previousValue.PageVisits.Remove(this);
					}

					this._User.Entity = value;
					if (value != null)
					{
						value.PageVisits.Add(this);
						
						this._UserId = value.UserId;
						
					}

					else
					{
						
						this._UserId = default(int?);
						
					}

					this.SendPropertyChanged("User");
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

