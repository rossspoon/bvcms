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
	[Table(Name="lookup.CountryLookup")]
	public partial class CountryLookup : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private string _CountryName;
		
		private string _CountryCode;
		
		private int? _DisplayFlag;
		
   		
    	
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnCountryNameChanging(string value);
		partial void OnCountryNameChanged();
		
		partial void OnCountryCodeChanging(string value);
		partial void OnCountryCodeChanged();
		
		partial void OnDisplayFlagChanging(int? value);
		partial void OnDisplayFlagChanged();
		
    #endregion
		public CountryLookup()
		{
			
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="CountryName", UpdateCheck=UpdateCheck.Never, Storage="_CountryName", DbType="varchar(30) NOT NULL")]
		public string CountryName
		{
			get { return this._CountryName; }

			set
			{
				if (this._CountryName != value)
				{
				
                    this.OnCountryNameChanging(value);
					this.SendPropertyChanging();
					this._CountryName = value;
					this.SendPropertyChanged("CountryName");
					this.OnCountryNameChanged();
				}

			}

		}

		
		[Column(Name="CountryCode", UpdateCheck=UpdateCheck.Never, Storage="_CountryCode", DbType="varchar(10) NOT NULL", IsPrimaryKey=true)]
		public string CountryCode
		{
			get { return this._CountryCode; }

			set
			{
				if (this._CountryCode != value)
				{
				
                    this.OnCountryCodeChanging(value);
					this.SendPropertyChanging();
					this._CountryCode = value;
					this.SendPropertyChanged("CountryCode");
					this.OnCountryCodeChanged();
				}

			}

		}

		
		[Column(Name="DisplayFlag", UpdateCheck=UpdateCheck.Never, Storage="_DisplayFlag", DbType="int")]
		public int? DisplayFlag
		{
			get { return this._DisplayFlag; }

			set
			{
				if (this._DisplayFlag != value)
				{
				
                    this.OnDisplayFlagChanging(value);
					this.SendPropertyChanging();
					this._DisplayFlag = value;
					this.SendPropertyChanged("DisplayFlag");
					this.OnDisplayFlagChanged();
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

