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
	[Table(Name="dbo.SMSItems")]
	public partial class SMSItem : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _Id;
		
		private int _ListID;
		
		private int _PeopleID;
		
		private string _Number;
		
		private bool _Sent;
		
   		
    	
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnIdChanging(int value);
		partial void OnIdChanged();
		
		partial void OnListIDChanging(int value);
		partial void OnListIDChanged();
		
		partial void OnPeopleIDChanging(int value);
		partial void OnPeopleIDChanged();
		
		partial void OnNumberChanging(string value);
		partial void OnNumberChanged();
		
		partial void OnSentChanging(bool value);
		partial void OnSentChanged();
		
    #endregion
		public SMSItem()
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

		
		[Column(Name="ListID", UpdateCheck=UpdateCheck.Never, Storage="_ListID", DbType="int NOT NULL")]
		public int ListID
		{
			get { return this._ListID; }

			set
			{
				if (this._ListID != value)
				{
				
                    this.OnListIDChanging(value);
					this.SendPropertyChanging();
					this._ListID = value;
					this.SendPropertyChanged("ListID");
					this.OnListIDChanged();
				}

			}

		}

		
		[Column(Name="PeopleID", UpdateCheck=UpdateCheck.Never, Storage="_PeopleID", DbType="int NOT NULL")]
		public int PeopleID
		{
			get { return this._PeopleID; }

			set
			{
				if (this._PeopleID != value)
				{
				
                    this.OnPeopleIDChanging(value);
					this.SendPropertyChanging();
					this._PeopleID = value;
					this.SendPropertyChanged("PeopleID");
					this.OnPeopleIDChanged();
				}

			}

		}

		
		[Column(Name="Number", UpdateCheck=UpdateCheck.Never, Storage="_Number", DbType="varchar(25) NOT NULL")]
		public string Number
		{
			get { return this._Number; }

			set
			{
				if (this._Number != value)
				{
				
                    this.OnNumberChanging(value);
					this.SendPropertyChanging();
					this._Number = value;
					this.SendPropertyChanged("Number");
					this.OnNumberChanged();
				}

			}

		}

		
		[Column(Name="Sent", UpdateCheck=UpdateCheck.Never, Storage="_Sent", DbType="bit NOT NULL")]
		public bool Sent
		{
			get { return this._Sent; }

			set
			{
				if (this._Sent != value)
				{
				
                    this.OnSentChanging(value);
					this.SendPropertyChanging();
					this._Sent = value;
					this.SendPropertyChanged("Sent");
					this.OnSentChanged();
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

