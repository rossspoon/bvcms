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
	[Table(Name="disc.Invitation")]
	public partial class Invitation : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private string _Password;
		
		private DateTime? _Expires;
		
		private int _GroupId;
		
   		
    	
		private EntityRef< Group> _Group;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnPasswordChanging(string value);
		partial void OnPasswordChanged();
		
		partial void OnExpiresChanging(DateTime? value);
		partial void OnExpiresChanged();
		
		partial void OnGroupIdChanging(int value);
		partial void OnGroupIdChanged();
		
    #endregion
		public Invitation()
		{
			
			
			this._Group = default(EntityRef< Group>); 
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="password", UpdateCheck=UpdateCheck.Never, Storage="_Password", DbType="nvarchar(50) NOT NULL", IsPrimaryKey=true)]
		public string Password
		{
			get { return this._Password; }

			set
			{
				if (this._Password != value)
				{
				
                    this.OnPasswordChanging(value);
					this.SendPropertyChanging();
					this._Password = value;
					this.SendPropertyChanged("Password");
					this.OnPasswordChanged();
				}

			}

		}

		
		[Column(Name="expires", UpdateCheck=UpdateCheck.Never, Storage="_Expires", DbType="datetime")]
		public DateTime? Expires
		{
			get { return this._Expires; }

			set
			{
				if (this._Expires != value)
				{
				
                    this.OnExpiresChanging(value);
					this.SendPropertyChanging();
					this._Expires = value;
					this.SendPropertyChanged("Expires");
					this.OnExpiresChanged();
				}

			}

		}

		
		[Column(Name="GroupId", UpdateCheck=UpdateCheck.Never, Storage="_GroupId", DbType="int NOT NULL", IsPrimaryKey=true)]
		public int GroupId
		{
			get { return this._GroupId; }

			set
			{
				if (this._GroupId != value)
				{
				
					if (this._Group.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnGroupIdChanging(value);
					this.SendPropertyChanging();
					this._GroupId = value;
					this.SendPropertyChanged("GroupId");
					this.OnGroupIdChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="FK_Invitation_Group", Storage="_Group", ThisKey="GroupId", IsForeignKey=true)]
		public Group Group
		{
			get { return this._Group.Entity; }

			set
			{
				Group previousValue = this._Group.Entity;
				if (((previousValue != value) 
							|| (this._Group.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._Group.Entity = null;
						previousValue.Invitations.Remove(this);
					}

					this._Group.Entity = value;
					if (value != null)
					{
						value.Invitations.Add(this);
						
						this._GroupId = value.Id;
						
					}

					else
					{
						
						this._GroupId = default(int);
						
					}

					this.SendPropertyChanged("Group");
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

