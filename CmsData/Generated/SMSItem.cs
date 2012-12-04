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
		
		private int _SendToID;
		
		private byte _SendBy;
		
		private string _SendAddress;
		
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
		
		partial void OnSendToIDChanging(int value);
		partial void OnSendToIDChanged();
		
		partial void OnSendByChanging(byte value);
		partial void OnSendByChanged();
		
		partial void OnSendAddressChanging(string value);
		partial void OnSendAddressChanged();
		
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

		
		[Column(Name="SendToID", UpdateCheck=UpdateCheck.Never, Storage="_SendToID", DbType="int NOT NULL")]
		public int SendToID
		{
			get { return this._SendToID; }

			set
			{
				if (this._SendToID != value)
				{
				
                    this.OnSendToIDChanging(value);
					this.SendPropertyChanging();
					this._SendToID = value;
					this.SendPropertyChanged("SendToID");
					this.OnSendToIDChanged();
				}

			}

		}

		
		[Column(Name="SendBy", UpdateCheck=UpdateCheck.Never, Storage="_SendBy", DbType="tinyint NOT NULL")]
		public byte SendBy
		{
			get { return this._SendBy; }

			set
			{
				if (this._SendBy != value)
				{
				
                    this.OnSendByChanging(value);
					this.SendPropertyChanging();
					this._SendBy = value;
					this.SendPropertyChanged("SendBy");
					this.OnSendByChanged();
				}

			}

		}

		
		[Column(Name="SendAddress", UpdateCheck=UpdateCheck.Never, Storage="_SendAddress", DbType="varchar(100) NOT NULL")]
		public string SendAddress
		{
			get { return this._SendAddress; }

			set
			{
				if (this._SendAddress != value)
				{
				
                    this.OnSendAddressChanging(value);
					this.SendPropertyChanging();
					this._SendAddress = value;
					this.SendPropertyChanged("SendAddress");
					this.OnSendAddressChanged();
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

