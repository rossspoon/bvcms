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
		
		private DateTime? _SaleDate;
		
		private decimal? _Amount;
		
		private string _TransactionId;
		
		private int? _PeopleId;
		
		private string _Username;
		
		private string _Password;
		
		private int? _ItemId;
		
   		
    	
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnIdChanging(int value);
		partial void OnIdChanged();
		
		partial void OnSaleDateChanging(DateTime? value);
		partial void OnSaleDateChanged();
		
		partial void OnAmountChanging(decimal? value);
		partial void OnAmountChanged();
		
		partial void OnTransactionIdChanging(string value);
		partial void OnTransactionIdChanged();
		
		partial void OnPeopleIdChanging(int? value);
		partial void OnPeopleIdChanged();
		
		partial void OnUsernameChanging(string value);
		partial void OnUsernameChanged();
		
		partial void OnPasswordChanging(string value);
		partial void OnPasswordChanged();
		
		partial void OnItemIdChanging(int? value);
		partial void OnItemIdChanged();
		
    #endregion
		public SaleTransaction()
		{
			
			
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

		
		[Column(Name="SaleDate", UpdateCheck=UpdateCheck.Never, Storage="_SaleDate", DbType="datetime")]
		public DateTime? SaleDate
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

		
		[Column(Name="Amount", UpdateCheck=UpdateCheck.Never, Storage="_Amount", DbType="money")]
		public decimal? Amount
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

		
		[Column(Name="ItemId", UpdateCheck=UpdateCheck.Never, Storage="_ItemId", DbType="int")]
		public int? ItemId
		{
			get { return this._ItemId; }

			set
			{
				if (this._ItemId != value)
				{
				
                    this.OnItemIdChanging(value);
					this.SendPropertyChanging();
					this._ItemId = value;
					this.SendPropertyChanged("ItemId");
					this.OnItemIdChanged();
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

