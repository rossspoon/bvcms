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
	[Table(Name="lookup.StateLookup")]
	public partial class StateLookup : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private string _StateCode;
		
		private string _StateName;
		
   		
    	
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnStateCodeChanging(string value);
		partial void OnStateCodeChanged();
		
		partial void OnStateNameChanging(string value);
		partial void OnStateNameChanged();
		
    #endregion
		public StateLookup()
		{
			
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="StateCode", UpdateCheck=UpdateCheck.Never, Storage="_StateCode", DbType="varchar(30) NOT NULL", IsPrimaryKey=true)]
		public string StateCode
		{
			get { return this._StateCode; }

			set
			{
				if (this._StateCode != value)
				{
				
                    this.OnStateCodeChanging(value);
					this.SendPropertyChanging();
					this._StateCode = value;
					this.SendPropertyChanged("StateCode");
					this.OnStateCodeChanged();
				}

			}

		}

		
		[Column(Name="StateName", UpdateCheck=UpdateCheck.Never, Storage="_StateName", DbType="varchar(30)")]
		public string StateName
		{
			get { return this._StateName; }

			set
			{
				if (this._StateName != value)
				{
				
                    this.OnStateNameChanging(value);
					this.SendPropertyChanging();
					this._StateName = value;
					this.SendPropertyChanged("StateName");
					this.OnStateNameChanged();
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

