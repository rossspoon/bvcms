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
	[Table(Name="dbo.TemporaryToken")]
	public partial class TemporaryToken : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private Guid _Id;
		
		private bool _Expired;
		
		private DateTime _CreatedOn;
		
		private int? _CreatedBy;
		
		private int? _CUserid;
		
   		
    	
		private EntityRef< User> _User;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnIdChanging(Guid value);
		partial void OnIdChanged();
		
		partial void OnExpiredChanging(bool value);
		partial void OnExpiredChanged();
		
		partial void OnCreatedOnChanging(DateTime value);
		partial void OnCreatedOnChanged();
		
		partial void OnCreatedByChanging(int? value);
		partial void OnCreatedByChanged();
		
		partial void OnCUseridChanging(int? value);
		partial void OnCUseridChanged();
		
    #endregion
		public TemporaryToken()
		{
			
			
			this._User = default(EntityRef< User>); 
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="id", UpdateCheck=UpdateCheck.Never, Storage="_Id", DbType="uniqueidentifier NOT NULL", IsPrimaryKey=true)]
		public Guid Id
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

		
		[Column(Name="expired", UpdateCheck=UpdateCheck.Never, Storage="_Expired", DbType="bit NOT NULL")]
		public bool Expired
		{
			get { return this._Expired; }

			set
			{
				if (this._Expired != value)
				{
				
                    this.OnExpiredChanging(value);
					this.SendPropertyChanging();
					this._Expired = value;
					this.SendPropertyChanged("Expired");
					this.OnExpiredChanged();
				}

			}

		}

		
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

		
		[Column(Name="CreatedBy", UpdateCheck=UpdateCheck.Never, Storage="_CreatedBy", DbType="int")]
		public int? CreatedBy
		{
			get { return this._CreatedBy; }

			set
			{
				if (this._CreatedBy != value)
				{
				
					if (this._User.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnCreatedByChanging(value);
					this.SendPropertyChanging();
					this._CreatedBy = value;
					this.SendPropertyChanged("CreatedBy");
					this.OnCreatedByChanged();
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
    	
		[Association(Name="FK_TemporaryToken_Users", Storage="_User", ThisKey="CreatedBy", IsForeignKey=true)]
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
						previousValue.TemporaryTokens.Remove(this);
					}

					this._User.Entity = value;
					if (value != null)
					{
						value.TemporaryTokens.Add(this);
						
						this._CreatedBy = value.UserId;
						
					}

					else
					{
						
						this._CreatedBy = default(int?);
						
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

