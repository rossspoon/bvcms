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
		
		private decimal? _Price;
		
		private bool? _Available;
		
		private string _Url;
		
   		
    	
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnIdChanging(int value);
		partial void OnIdChanged();
		
		partial void OnDescriptionChanging(string value);
		partial void OnDescriptionChanged();
		
		partial void OnPriceChanging(decimal? value);
		partial void OnPriceChanged();
		
		partial void OnAvailableChanging(bool? value);
		partial void OnAvailableChanged();
		
		partial void OnUrlChanging(string value);
		partial void OnUrlChanged();
		
    #endregion
		public SaleItem()
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

		
		[Column(Name="Price", UpdateCheck=UpdateCheck.Never, Storage="_Price", DbType="money")]
		public decimal? Price
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

		
		[Column(Name="URL", UpdateCheck=UpdateCheck.Never, Storage="_Url", DbType="varchar(50)")]
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

