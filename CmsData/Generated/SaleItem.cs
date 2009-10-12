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
	[Table(Name="dbo.SaleItem")]
	public partial class SaleItem : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _Id;
		
		private string _Description;
		
		private decimal _Price;
		
		private bool? _Available;
		
		private string _Url;
		
		private int? _MaxItems;
		
		private int? _DefaultItems;
		
		private string _Email;
		
   		
   		private EntitySet< SaleTransaction> _SaleTransactions;
		
    	
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnIdChanging(int value);
		partial void OnIdChanged();
		
		partial void OnDescriptionChanging(string value);
		partial void OnDescriptionChanged();
		
		partial void OnPriceChanging(decimal value);
		partial void OnPriceChanged();
		
		partial void OnAvailableChanging(bool? value);
		partial void OnAvailableChanged();
		
		partial void OnUrlChanging(string value);
		partial void OnUrlChanged();
		
		partial void OnMaxItemsChanging(int? value);
		partial void OnMaxItemsChanged();
		
		partial void OnDefaultItemsChanging(int? value);
		partial void OnDefaultItemsChanged();
		
		partial void OnEmailChanging(string value);
		partial void OnEmailChanged();
		
    #endregion
		public SaleItem()
		{
			
			this._SaleTransactions = new EntitySet< SaleTransaction>(new Action< SaleTransaction>(this.attach_SaleTransactions), new Action< SaleTransaction>(this.detach_SaleTransactions)); 
			
			
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

		
		[Column(Name="Price", UpdateCheck=UpdateCheck.Never, Storage="_Price", DbType="money NOT NULL")]
		public decimal Price
		{
			get { return this._Price; }

			set
			{
				if (this._Price != value)
				{
				
                    this.OnPriceChanging(value);
					this.SendPropertyChanging();
					this._Price = value;
					this.SendPropertyChanged("Price");
					this.OnPriceChanged();
				}

			}

		}

		
		[Column(Name="Available", UpdateCheck=UpdateCheck.Never, Storage="_Available", DbType="bit")]
		public bool? Available
		{
			get { return this._Available; }

			set
			{
				if (this._Available != value)
				{
				
                    this.OnAvailableChanging(value);
					this.SendPropertyChanging();
					this._Available = value;
					this.SendPropertyChanged("Available");
					this.OnAvailableChanged();
				}

			}

		}

		
		[Column(Name="URL", UpdateCheck=UpdateCheck.Never, Storage="_Url", DbType="varchar(150)")]
		public string Url
		{
			get { return this._Url; }

			set
			{
				if (this._Url != value)
				{
				
                    this.OnUrlChanging(value);
					this.SendPropertyChanging();
					this._Url = value;
					this.SendPropertyChanged("Url");
					this.OnUrlChanged();
				}

			}

		}

		
		[Column(Name="MaxItems", UpdateCheck=UpdateCheck.Never, Storage="_MaxItems", DbType="int")]
		public int? MaxItems
		{
			get { return this._MaxItems; }

			set
			{
				if (this._MaxItems != value)
				{
				
                    this.OnMaxItemsChanging(value);
					this.SendPropertyChanging();
					this._MaxItems = value;
					this.SendPropertyChanged("MaxItems");
					this.OnMaxItemsChanged();
				}

			}

		}

		
		[Column(Name="DefaultItems", UpdateCheck=UpdateCheck.Never, Storage="_DefaultItems", DbType="int")]
		public int? DefaultItems
		{
			get { return this._DefaultItems; }

			set
			{
				if (this._DefaultItems != value)
				{
				
                    this.OnDefaultItemsChanging(value);
					this.SendPropertyChanging();
					this._DefaultItems = value;
					this.SendPropertyChanged("DefaultItems");
					this.OnDefaultItemsChanged();
				}

			}

		}

		
		[Column(Name="Email", UpdateCheck=UpdateCheck.Never, Storage="_Email", DbType="varchar(50)")]
		public string Email
		{
			get { return this._Email; }

			set
			{
				if (this._Email != value)
				{
				
                    this.OnEmailChanging(value);
					this.SendPropertyChanging();
					this._Email = value;
					this.SendPropertyChanged("Email");
					this.OnEmailChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
   		[Association(Name="FK_SaleTransaction_SaleItem", Storage="_SaleTransactions", OtherKey="ItemId")]
   		public EntitySet< SaleTransaction> SaleTransactions
   		{
   		    get { return this._SaleTransactions; }

			set	{ this._SaleTransactions.Assign(value); }

   		}

		
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

   		
		private void attach_SaleTransactions(SaleTransaction entity)
		{
			this.SendPropertyChanging();
			entity.SaleItem = this;
		}

		private void detach_SaleTransactions(SaleTransaction entity)
		{
			this.SendPropertyChanging();
			entity.SaleItem = null;
		}

		
	}

}

