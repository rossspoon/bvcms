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
	[Table(Name="dbo.EmailQueue")]
	public partial class EmailQueue : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _Id;
		
		private DateTime? _SendWhen;
		
		private string _Subject;
		
		private string _Body;
		
		private string _FromAddr;
		
		private DateTime? _Sent;
		
		private DateTime? _Started;
		
		private DateTime _Queued;
		
		private string _FromName;
		
   		
   		private EntitySet< EmailQueueTo> _EmailQueueTos;
		
    	
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnIdChanging(int value);
		partial void OnIdChanged();
		
		partial void OnSendWhenChanging(DateTime? value);
		partial void OnSendWhenChanged();
		
		partial void OnSubjectChanging(string value);
		partial void OnSubjectChanged();
		
		partial void OnBodyChanging(string value);
		partial void OnBodyChanged();
		
		partial void OnFromAddrChanging(string value);
		partial void OnFromAddrChanged();
		
		partial void OnSentChanging(DateTime? value);
		partial void OnSentChanged();
		
		partial void OnStartedChanging(DateTime? value);
		partial void OnStartedChanged();
		
		partial void OnQueuedChanging(DateTime value);
		partial void OnQueuedChanged();
		
		partial void OnFromNameChanging(string value);
		partial void OnFromNameChanged();
		
    #endregion
		public EmailQueue()
		{
			
			this._EmailQueueTos = new EntitySet< EmailQueueTo>(new Action< EmailQueueTo>(this.attach_EmailQueueTos), new Action< EmailQueueTo>(this.detach_EmailQueueTos)); 
			
			
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

		
		[Column(Name="SendWhen", UpdateCheck=UpdateCheck.Never, Storage="_SendWhen", DbType="datetime")]
		public DateTime? SendWhen
		{
			get { return this._SendWhen; }

			set
			{
				if (this._SendWhen != value)
				{
				
                    this.OnSendWhenChanging(value);
					this.SendPropertyChanging();
					this._SendWhen = value;
					this.SendPropertyChanged("SendWhen");
					this.OnSendWhenChanged();
				}

			}

		}

		
		[Column(Name="Subject", UpdateCheck=UpdateCheck.Never, Storage="_Subject", DbType="varchar(200)")]
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

		
		[Column(Name="Body", UpdateCheck=UpdateCheck.Never, Storage="_Body", DbType="varchar")]
		public string Body
		{
			get { return this._Body; }

			set
			{
				if (this._Body != value)
				{
				
                    this.OnBodyChanging(value);
					this.SendPropertyChanging();
					this._Body = value;
					this.SendPropertyChanged("Body");
					this.OnBodyChanged();
				}

			}

		}

		
		[Column(Name="FromAddr", UpdateCheck=UpdateCheck.Never, Storage="_FromAddr", DbType="varchar(100)")]
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

		
		[Column(Name="Sent", UpdateCheck=UpdateCheck.Never, Storage="_Sent", DbType="datetime")]
		public DateTime? Sent
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

		
		[Column(Name="Started", UpdateCheck=UpdateCheck.Never, Storage="_Started", DbType="datetime")]
		public DateTime? Started
		{
			get { return this._Started; }

			set
			{
				if (this._Started != value)
				{
				
                    this.OnStartedChanging(value);
					this.SendPropertyChanging();
					this._Started = value;
					this.SendPropertyChanged("Started");
					this.OnStartedChanged();
				}

			}

		}

		
		[Column(Name="Queued", UpdateCheck=UpdateCheck.Never, Storage="_Queued", DbType="datetime NOT NULL")]
		public DateTime Queued
		{
			get { return this._Queued; }

			set
			{
				if (this._Queued != value)
				{
				
                    this.OnQueuedChanging(value);
					this.SendPropertyChanging();
					this._Queued = value;
					this.SendPropertyChanged("Queued");
					this.OnQueuedChanged();
				}

			}

		}

		
		[Column(Name="FromName", UpdateCheck=UpdateCheck.Never, Storage="_FromName", DbType="varchar(60)")]
		public string FromName
		{
			get { return this._FromName; }

			set
			{
				if (this._FromName != value)
				{
				
                    this.OnFromNameChanging(value);
					this.SendPropertyChanging();
					this._FromName = value;
					this.SendPropertyChanged("FromName");
					this.OnFromNameChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
   		[Association(Name="FK_EmailQueueTo_EmailQueue", Storage="_EmailQueueTos", OtherKey="Id")]
   		public EntitySet< EmailQueueTo> EmailQueueTos
   		{
   		    get { return this._EmailQueueTos; }

			set	{ this._EmailQueueTos.Assign(value); }

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

   		
		private void attach_EmailQueueTos(EmailQueueTo entity)
		{
			this.SendPropertyChanging();
			entity.EmailQueue = this;
		}

		private void detach_EmailQueueTos(EmailQueueTo entity)
		{
			this.SendPropertyChanging();
			entity.EmailQueue = null;
		}

		
	}

}

