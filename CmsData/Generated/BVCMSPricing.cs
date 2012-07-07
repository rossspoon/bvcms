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
	[Table(Name="dbo.BVCMSPricing")]
	public partial class BVCMSPricing : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _Id;
		
		private int _MinWorship;
		
		private int _MaxWorship;
		
		private int _MinActive;
		
		private int _MaxActive;
		
		private decimal _PriceMonthly;
		
		private decimal _PriceAnnual;
		
   		
    	
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnIdChanging(int value);
		partial void OnIdChanged();
		
		partial void OnMinWorshipChanging(int value);
		partial void OnMinWorshipChanged();
		
		partial void OnMaxWorshipChanging(int value);
		partial void OnMaxWorshipChanged();
		
		partial void OnMinActiveChanging(int value);
		partial void OnMinActiveChanged();
		
		partial void OnMaxActiveChanging(int value);
		partial void OnMaxActiveChanged();
		
		partial void OnPriceMonthlyChanging(decimal value);
		partial void OnPriceMonthlyChanged();
		
		partial void OnPriceAnnualChanging(decimal value);
		partial void OnPriceAnnualChanged();
		
    #endregion
		public BVCMSPricing()
		{
			
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="ID", UpdateCheck=UpdateCheck.Never, Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
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

		
		[Column(Name="MinWorship", UpdateCheck=UpdateCheck.Never, Storage="_MinWorship", DbType="int NOT NULL")]
		public int MinWorship
		{
			get { return this._MinWorship; }

			set
			{
				if (this._MinWorship != value)
				{
				
                    this.OnMinWorshipChanging(value);
					this.SendPropertyChanging();
					this._MinWorship = value;
					this.SendPropertyChanged("MinWorship");
					this.OnMinWorshipChanged();
				}

			}

		}

		
		[Column(Name="MaxWorship", UpdateCheck=UpdateCheck.Never, Storage="_MaxWorship", DbType="int NOT NULL")]
		public int MaxWorship
		{
			get { return this._MaxWorship; }

			set
			{
				if (this._MaxWorship != value)
				{
				
                    this.OnMaxWorshipChanging(value);
					this.SendPropertyChanging();
					this._MaxWorship = value;
					this.SendPropertyChanged("MaxWorship");
					this.OnMaxWorshipChanged();
				}

			}

		}

		
		[Column(Name="MinActive", UpdateCheck=UpdateCheck.Never, Storage="_MinActive", DbType="int NOT NULL")]
		public int MinActive
		{
			get { return this._MinActive; }

			set
			{
				if (this._MinActive != value)
				{
				
                    this.OnMinActiveChanging(value);
					this.SendPropertyChanging();
					this._MinActive = value;
					this.SendPropertyChanged("MinActive");
					this.OnMinActiveChanged();
				}

			}

		}

		
		[Column(Name="MaxActive", UpdateCheck=UpdateCheck.Never, Storage="_MaxActive", DbType="int NOT NULL")]
		public int MaxActive
		{
			get { return this._MaxActive; }

			set
			{
				if (this._MaxActive != value)
				{
				
                    this.OnMaxActiveChanging(value);
					this.SendPropertyChanging();
					this._MaxActive = value;
					this.SendPropertyChanged("MaxActive");
					this.OnMaxActiveChanged();
				}

			}

		}

		
		[Column(Name="PriceMonthly", UpdateCheck=UpdateCheck.Never, Storage="_PriceMonthly", DbType="decimal NOT NULL")]
		public decimal PriceMonthly
		{
			get { return this._PriceMonthly; }

			set
			{
				if (this._PriceMonthly != value)
				{
				
                    this.OnPriceMonthlyChanging(value);
					this.SendPropertyChanging();
					this._PriceMonthly = value;
					this.SendPropertyChanged("PriceMonthly");
					this.OnPriceMonthlyChanged();
				}

			}

		}

		
		[Column(Name="PriceAnnual", UpdateCheck=UpdateCheck.Never, Storage="_PriceAnnual", DbType="decimal NOT NULL")]
		public decimal PriceAnnual
		{
			get { return this._PriceAnnual; }

			set
			{
				if (this._PriceAnnual != value)
				{
				
                    this.OnPriceAnnualChanging(value);
					this.SendPropertyChanging();
					this._PriceAnnual = value;
					this.SendPropertyChanged("PriceAnnual");
					this.OnPriceAnnualChanged();
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

