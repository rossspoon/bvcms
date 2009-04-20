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
	[Table(Name="dbo.Zips")]
	public partial class Zip : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private string _ZipCode;
		
		private int? _MetroMarginalCode;
		
   		
    	
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnZipCodeChanging(string value);
		partial void OnZipCodeChanged();
		
		partial void OnMetroMarginalCodeChanging(int? value);
		partial void OnMetroMarginalCodeChanged();
		
    #endregion
		public Zip()
		{
			
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="ZipCode", UpdateCheck=UpdateCheck.Never, Storage="_ZipCode", DbType="varchar(10) NOT NULL", IsPrimaryKey=true)]
		public string ZipCode
		{
			get { return this._ZipCode; }

			set
			{
				if (this._ZipCode != value)
				{
				
                    this.OnZipCodeChanging(value);
					this.SendPropertyChanging();
					this._ZipCode = value;
					this.SendPropertyChanged("ZipCode");
					this.OnZipCodeChanged();
				}

			}

		}

		
		[Column(Name="MetroMarginalCode", UpdateCheck=UpdateCheck.Never, Storage="_MetroMarginalCode", DbType="int")]
		public int? MetroMarginalCode
		{
			get { return this._MetroMarginalCode; }

			set
			{
				if (this._MetroMarginalCode != value)
				{
				
                    this.OnMetroMarginalCodeChanging(value);
					this.SendPropertyChanging();
					this._MetroMarginalCode = value;
					this.SendPropertyChanged("MetroMarginalCode");
					this.OnMetroMarginalCodeChanged();
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

