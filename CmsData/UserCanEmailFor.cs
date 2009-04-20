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
	[Table(Name="dbo.UserCanEmailFor")]
	public partial class UserCanEmailFor : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _UserId;
		
		private int _CanEmailFor;
		
   		
    	
		private EntityRef< User> _Assistant;
		
		private EntityRef< User> _Boss;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnUserIdChanging(int value);
		partial void OnUserIdChanged();
		
		partial void OnCanEmailForChanging(int value);
		partial void OnCanEmailForChanged();
		
    #endregion
		public UserCanEmailFor()
		{
			
			
			this._Assistant = default(EntityRef< User>); 
			
			this._Boss = default(EntityRef< User>); 
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="UserId", UpdateCheck=UpdateCheck.Never, Storage="_UserId", DbType="int NOT NULL", IsPrimaryKey=true)]
		public int UserId
		{
			get { return this._UserId; }

			set
			{
				if (this._UserId != value)
				{
				
					if (this._Assistant.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnUserIdChanging(value);
					this.SendPropertyChanging();
					this._UserId = value;
					this.SendPropertyChanged("UserId");
					this.OnUserIdChanged();
				}

			}

		}

		
		[Column(Name="CanEmailFor", UpdateCheck=UpdateCheck.Never, Storage="_CanEmailFor", DbType="int NOT NULL", IsPrimaryKey=true)]
		public int CanEmailFor
		{
			get { return this._CanEmailFor; }

			set
			{
				if (this._CanEmailFor != value)
				{
				
					if (this._Boss.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnCanEmailForChanging(value);
					this.SendPropertyChanging();
					this._CanEmailFor = value;
					this.SendPropertyChanged("CanEmailFor");
					this.OnCanEmailForChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="UsersICanEmailFor__Assistant", Storage="_Assistant", ThisKey="UserId", IsForeignKey=true)]
		public User Assistant
		{
			get { return this._Assistant.Entity; }

			set
			{
				User previousValue = this._Assistant.Entity;
				if (((previousValue != value) 
							|| (this._Assistant.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._Assistant.Entity = null;
						previousValue.UsersICanEmailFor.Remove(this);
					}

					this._Assistant.Entity = value;
					if (value != null)
					{
						value.UsersICanEmailFor.Add(this);
						
						this._UserId = value.UserId;
						
					}

					else
					{
						
						this._UserId = default(int);
						
					}

					this.SendPropertyChanged("Assistant");
				}

			}

		}

		
		[Association(Name="UsersWhoCanEmailForMe__Boss", Storage="_Boss", ThisKey="CanEmailFor", IsForeignKey=true)]
		public User Boss
		{
			get { return this._Boss.Entity; }

			set
			{
				User previousValue = this._Boss.Entity;
				if (((previousValue != value) 
							|| (this._Boss.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._Boss.Entity = null;
						previousValue.UsersWhoCanEmailForMe.Remove(this);
					}

					this._Boss.Entity = value;
					if (value != null)
					{
						value.UsersWhoCanEmailForMe.Add(this);
						
						this._CanEmailFor = value.UserId;
						
					}

					else
					{
						
						this._CanEmailFor = default(int);
						
					}

					this.SendPropertyChanged("Boss");
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

