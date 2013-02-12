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
	[Table(Name="dbo.SMSList")]
	public partial class SMSList : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _Id;
		
		private DateTime _Created;
		
		private int _SenderID;
		
		private DateTime _SendAt;
		
		private int _SendGroupID;
		
		private string _Message;
		
		private int _SentSMS;
		
		private int _SentNone;
		
   		
    	
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnIdChanging(int value);
		partial void OnIdChanged();
		
		partial void OnCreatedChanging(DateTime value);
		partial void OnCreatedChanged();
		
		partial void OnSenderIDChanging(int value);
		partial void OnSenderIDChanged();
		
		partial void OnSendAtChanging(DateTime value);
		partial void OnSendAtChanged();
		
		partial void OnSendGroupIDChanging(int value);
		partial void OnSendGroupIDChanged();
		
		partial void OnMessageChanging(string value);
		partial void OnMessageChanged();
		
		partial void OnSentSMSChanging(int value);
		partial void OnSentSMSChanged();
		
		partial void OnSentNoneChanging(int value);
		partial void OnSentNoneChanged();
		
    #endregion
		public SMSList()
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

		
		[Column(Name="Created", UpdateCheck=UpdateCheck.Never, Storage="_Created", DbType="datetime NOT NULL")]
		public DateTime Created
		{
			get { return this._Created; }

			set
			{
				if (this._Created != value)
				{
				
                    this.OnCreatedChanging(value);
					this.SendPropertyChanging();
					this._Created = value;
					this.SendPropertyChanged("Created");
					this.OnCreatedChanged();
				}

			}

		}

		
		[Column(Name="SenderID", UpdateCheck=UpdateCheck.Never, Storage="_SenderID", DbType="int NOT NULL")]
		public int SenderID
		{
			get { return this._SenderID; }

			set
			{
				if (this._SenderID != value)
				{
				
                    this.OnSenderIDChanging(value);
					this.SendPropertyChanging();
					this._SenderID = value;
					this.SendPropertyChanged("SenderID");
					this.OnSenderIDChanged();
				}

			}

		}

		
		[Column(Name="SendAt", UpdateCheck=UpdateCheck.Never, Storage="_SendAt", DbType="datetime NOT NULL")]
		public DateTime SendAt
		{
			get { return this._SendAt; }

			set
			{
				if (this._SendAt != value)
				{
				
                    this.OnSendAtChanging(value);
					this.SendPropertyChanging();
					this._SendAt = value;
					this.SendPropertyChanged("SendAt");
					this.OnSendAtChanged();
				}

			}

		}

		
		[Column(Name="SendGroupID", UpdateCheck=UpdateCheck.Never, Storage="_SendGroupID", DbType="int NOT NULL")]
		public int SendGroupID
		{
			get { return this._SendGroupID; }

			set
			{
				if (this._SendGroupID != value)
				{
				
                    this.OnSendGroupIDChanging(value);
					this.SendPropertyChanging();
					this._SendGroupID = value;
					this.SendPropertyChanged("SendGroupID");
					this.OnSendGroupIDChanged();
				}

			}

		}

		
		[Column(Name="Message", UpdateCheck=UpdateCheck.Never, Storage="_Message", DbType="varchar(160) NOT NULL")]
		public string Message
		{
			get { return this._Message; }

			set
			{
				if (this._Message != value)
				{
				
                    this.OnMessageChanging(value);
					this.SendPropertyChanging();
					this._Message = value;
					this.SendPropertyChanged("Message");
					this.OnMessageChanged();
				}

			}

		}

		
		[Column(Name="SentSMS", UpdateCheck=UpdateCheck.Never, Storage="_SentSMS", DbType="int NOT NULL")]
		public int SentSMS
		{
			get { return this._SentSMS; }

			set
			{
				if (this._SentSMS != value)
				{
				
                    this.OnSentSMSChanging(value);
					this.SendPropertyChanging();
					this._SentSMS = value;
					this.SendPropertyChanged("SentSMS");
					this.OnSentSMSChanged();
				}

			}

		}

		
		[Column(Name="SentNone", UpdateCheck=UpdateCheck.Never, Storage="_SentNone", DbType="int NOT NULL")]
		public int SentNone
		{
			get { return this._SentNone; }

			set
			{
				if (this._SentNone != value)
				{
				
                    this.OnSentNoneChanging(value);
					this.SendPropertyChanging();
					this._SentNone = value;
					this.SendPropertyChanged("SentNone");
					this.OnSentNoneChanged();
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

