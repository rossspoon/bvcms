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
	[Table(Name="dbo.EmailSent")]
	public partial class EmailSent : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _Id;
		
		private string _Username;
		
		private DateTime? _Time;
		
		private string _FromAddr;
		
		private string _Subject;
		
		private string _Message;
		
		private string _Name;
		
		private string _ToAddr;
		
		private int? _FromPid;
		
		private int? _ToPid;
		
   		
    	
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnIdChanging(int value);
		partial void OnIdChanged();
		
		partial void OnUsernameChanging(string value);
		partial void OnUsernameChanged();
		
		partial void OnTimeChanging(DateTime? value);
		partial void OnTimeChanged();
		
		partial void OnFromAddrChanging(string value);
		partial void OnFromAddrChanged();
		
		partial void OnSubjectChanging(string value);
		partial void OnSubjectChanged();
		
		partial void OnMessageChanging(string value);
		partial void OnMessageChanged();
		
		partial void OnNameChanging(string value);
		partial void OnNameChanged();
		
		partial void OnToAddrChanging(string value);
		partial void OnToAddrChanged();
		
		partial void OnFromPidChanging(int? value);
		partial void OnFromPidChanged();
		
		partial void OnToPidChanging(int? value);
		partial void OnToPidChanged();
		
    #endregion
		public EmailSent()
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

		
		[Column(Name="Time", UpdateCheck=UpdateCheck.Never, Storage="_Time", DbType="datetime")]
		public DateTime? Time
		{
			get { return this._Time; }

			set
			{
				if (this._Time != value)
				{
				
                    this.OnTimeChanging(value);
					this.SendPropertyChanging();
					this._Time = value;
					this.SendPropertyChanged("Time");
					this.OnTimeChanged();
				}

			}

		}

		
		[Column(Name="FromAddr", UpdateCheck=UpdateCheck.Never, Storage="_FromAddr", DbType="varchar(50)")]
		public string FromAddr
		{
			get { return this._FromAddr; }

			set
			{
				if (this._FromAddr != value)
				{
				
                    this.OnFromAddrChanging(value);
					this.SendPropertyChanging();
					this._FromAddr = value;
					this.SendPropertyChanged("FromAddr");
					this.OnFromAddrChanged();
				}

			}

		}

		
		[Column(Name="Subject", UpdateCheck=UpdateCheck.Never, Storage="_Subject", DbType="varchar(100)")]
		public string Subject
		{
			get { return this._Subject; }

			set
			{
				if (this._Subject != value)
				{
				
                    this.OnSubjectChanging(value);
					this.SendPropertyChanging();
					this._Subject = value;
					this.SendPropertyChanged("Subject");
					this.OnSubjectChanged();
				}

			}

		}

		
		[Column(Name="Message", UpdateCheck=UpdateCheck.Never, Storage="_Message", DbType="varchar")]
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

		
		[Column(Name="Name", UpdateCheck=UpdateCheck.Never, Storage="_Name", DbType="varchar(100)")]
		public string Name
		{
			get { return this._Name; }

			set
			{
				if (this._Name != value)
				{
				
                    this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}

			}

		}

		
		[Column(Name="ToAddr", UpdateCheck=UpdateCheck.Never, Storage="_ToAddr", DbType="varchar(200)")]
		public string ToAddr
		{
			get { return this._ToAddr; }

			set
			{
				if (this._ToAddr != value)
				{
				
                    this.OnToAddrChanging(value);
					this.SendPropertyChanging();
					this._ToAddr = value;
					this.SendPropertyChanged("ToAddr");
					this.OnToAddrChanged();
				}

			}

		}

		
		[Column(Name="FromPid", UpdateCheck=UpdateCheck.Never, Storage="_FromPid", DbType="int")]
		public int? FromPid
		{
			get { return this._FromPid; }

			set
			{
				if (this._FromPid != value)
				{
				
                    this.OnFromPidChanging(value);
					this.SendPropertyChanging();
					this._FromPid = value;
					this.SendPropertyChanged("FromPid");
					this.OnFromPidChanged();
				}

			}

		}

		
		[Column(Name="ToPid", UpdateCheck=UpdateCheck.Never, Storage="_ToPid", DbType="int")]
		public int? ToPid
		{
			get { return this._ToPid; }

			set
			{
				if (this._ToPid != value)
				{
				
                    this.OnToPidChanging(value);
					this.SendPropertyChanging();
					this._ToPid = value;
					this.SendPropertyChanged("ToPid");
					this.OnToPidChanged();
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

