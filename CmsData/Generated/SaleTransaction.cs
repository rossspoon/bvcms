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
	[Table(Name="dbo.SaleTransaction")]
	public partial class SaleTransaction : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _Id;
		
		private DateTime _SaleDate;
		
		private decimal _Amount;
		
		private string _TransactionId;
		
		private int _PeopleId;
		
		private string _Username;
		
		private string _Password;
		
		private int _ItemId;
		
		private int _Quantity;
		
		private string _EmailAddress;
		
		private string _ItemDescription;
		
   		
    	
		private EntityRef< Person> _Person;
		
		private EntityRef< SaleItem> _SaleItem;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnIdChanging(int value);
		partial void OnIdChanged();
		
		partial void OnSaleDateChanging(DateTime value);
		partial void OnSaleDateChanged();
		
		partial void OnAmountChanging(decimal value);
		partial void OnAmountChanged();
		
		partial void OnTransactionIdChanging(string value);
		partial void OnTransactionIdChanged();
		
		partial void OnPeopleIdChanging(int value);
		partial void OnPeopleIdChanged();
		
		partial void OnUsernameChanging(string value);
		partial void OnUsernameChanged();
		
		partial void OnPasswordChanging(string value);
		partial void OnPasswordChanged();
		
		partial void OnItemIdChanging(int value);
		partial void OnItemIdChanged();
		
		partial void OnQuantityChanging(int value);
		partial void OnQuantityChanged();
		
		partial void OnEmailAddressChanging(string value);
		partial void OnEmailAddressChanged();
		
		partial void OnItemDescriptionChanging(string value);
		partial void OnItemDescriptionChanged();
		
    #endregion
		public SaleTransaction()
		{
			
			
			this._Person = default(EntityRef< Person>); 
			
			this._SaleItem = default(EntityRef< SaleItem>); 
			
			OnCreated();
		}

		
    #region Columns
		
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

		
		[Column(Name="SaleDate", UpdateCheck=UpdateCheck.Never, Storage="_SaleDate", DbType="datetime NOT NULL")]
		public DateTime SaleDate
		{
			get { return this._SaleDate; }

			set
			{
				if (this._SaleDate != value)
				{
				
                    this.OnSaleDateChanging(value);
					this.SendPropertyChanging();
					this._SaleDate = value;
					this.SendPropertyChanged("SaleDate");
					this.OnSaleDateChanged();
				}

			}

		}

		
		[Column(Name="Amount", UpdateCheck=UpdateCheck.Never, Storage="_Amount", DbType="money NOT NULL")]
		public decimal Amount
		{
			get { return this._Amount; }

			set
			{
				if (this._Amount != value)
				{
				
                    this.OnAmountChanging(value);
					this.SendPropertyChanging();
					this._Amount = value;
					this.SendPropertyChanged("Amount");
					this.OnAmountChanged();
				}

			}

		}

		
		[Column(Name="TransactionId", UpdateCheck=UpdateCheck.Never, Storage="_TransactionId", DbType="varchar(50)")]
		public string TransactionId
		{
			get { return this._TransactionId; }

			set
			{
				if (this._TransactionId != value)
				{
				
                    this.OnTransactionIdChanging(value);
					this.SendPropertyChanging();
					this._TransactionId = value;
					this.SendPropertyChanged("TransactionId");
					this.OnTransactionIdChanged();
				}

			}

		}

		
		[Column(Name="PeopleId", UpdateCheck=UpdateCheck.Never, Storage="_PeopleId", DbType="int NOT NULL")]
		public int PeopleId
		{
			get { return this._PeopleId; }

			set
			{
				if (this._PeopleId != value)
				{
				
					if (this._Person.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnPeopleIdChanging(value);
					this.SendPropertyChanging();
					this._PeopleId = value;
					this.SendPropertyChanged("PeopleId");
					this.OnPeopleIdChanged();
				}

			}

		}

		
		[Column(Name="Username", UpdateCheck=UpdateCheck.Never, Storage="_Username", DbType="varchar(50)")]
		public string Username
		{
			get { return this._Username; }

			set
			{
				if (this._Username != value)
				{
				
                    this.OnUsernameChanging(value);
					this.SendPropertyChanging();
					this._Username = value;
					this.SendPropertyChanged("Username");
					this.OnUsernameChanged();
				}

			}

		}

		
		[Column(Name="Password", UpdateCheck=UpdateCheck.Never, Storage="_Password", DbType="varchar(50)")]
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

		
		[Column(Name="ItemId", UpdateCheck=UpdateCheck.Never, Storage="_ItemId", DbType="int NOT NULL")]
		public int ItemId
		{
			get { return this._ItemId; }

			set
			{
				if (this._ItemId != value)
				{
				
					if (this._SaleItem.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnItemIdChanging(value);
					this.SendPropertyChanging();
					this._ItemId = value;
					this.SendPropertyChanged("ItemId");
					this.OnItemIdChanged();
				}

			}

		}

		
		[Column(Name="Quantity", UpdateCheck=UpdateCheck.Never, Storage="_Quantity", DbType="int NOT NULL")]
		public int Quantity
		{
			get { return this._Quantity; }

			set
			{
				if (this._Quantity != value)
				{
				
                    this.OnQuantityChanging(value);
					this.SendPropertyChanging();
					this._Quantity = value;
					this.SendPropertyChanged("Quantity");
					this.OnQuantityChanged();
				}

			}

		}

		
		[Column(Name="EmailAddress", UpdateCheck=UpdateCheck.Never, Storage="_EmailAddress", DbType="varchar(50) NOT NULL")]
		public string EmailAddress
		{
			get { return this._EmailAddress; }

			set
			{
				if (this._EmailAddress != value)
				{
				
                    this.OnEmailAddressChanging(value);
					this.SendPropertyChanging();
					this._EmailAddress = value;
					this.SendPropertyChanged("EmailAddress");
					this.OnEmailAddressChanged();
				}

			}

		}

		
		[Column(Name="ItemDescription", UpdateCheck=UpdateCheck.Never, Storage="_ItemDescription", DbType="varchar(50) NOT NULL")]
		public string ItemDescription
		{
			get { return this._ItemDescription; }

			set
			{
				if (this._ItemDescription != value)
				{
				
                    this.OnItemDescriptionChanging(value);
					this.SendPropertyChanging();
					this._ItemDescription = value;
					this.SendPropertyChanged("ItemDescription");
					this.OnItemDescriptionChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="FK_SaleTransaction_People", Storage="_Person", ThisKey="PeopleId", IsForeignKey=true)]
		public Person Person
		{
			get { return this._Person.Entity; }

			set
			{
				Person previousValue = this._Person.Entity;
				if (((previousValue != value) 
							|| (this._Person.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._Person.Entity = null;
						previousValue.SaleTransactions.Remove(this);
					}

					this._Person.Entity = value;
					if (value != null)
					{
						value.SaleTransactions.Add(this);
						
						this._PeopleId = value.PeopleId;
						
					}

					else
					{
						
						this._PeopleId = default(int);
						
					}

					this.SendPropertyChanged("Person");
				}

			}

		}

		
		[Association(Name="FK_SaleTransaction_SaleItem", Storage="_SaleItem", ThisKey="ItemId", IsForeignKey=true)]
		public SaleItem SaleItem
		{
			get { return this._SaleItem.Entity; }

			set
			{
				SaleItem previousValue = this._SaleItem.Entity;
				if (((previousValue != value) 
							|| (this._SaleItem.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._SaleItem.Entity = null;
						previousValue.SaleTransactions.Remove(this);
					}

					this._SaleItem.Entity = value;
					if (value != null)
					{
						value.SaleTransactions.Add(this);
						
						this._ItemId = value.Id;
						
					}

					else
					{
						
						this._ItemId = default(int);
						
					}

					this.SendPropertyChanged("SaleItem");
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

