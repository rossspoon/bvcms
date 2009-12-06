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
	[Table(Name="disc.PageContent")]
	public partial class PageContent : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _PageID;
		
		private string _Title;
		
		private string _Body;
		
		private string _PageUrl;
		
		private DateTime _CreatedOn;
		
		private DateTime? _ModifiedOn;
		
		private bool _Deleted;
		
		private int? _CreatedById;
		
		private int? _ModifiedById;
		
		private int? _CUserid;
		
		private int? _CUserid2;
		
   		
    	
		private EntityRef< User> _CreatedBy;
		
		private EntityRef< User> _ModifiedBy;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnPageIDChanging(int value);
		partial void OnPageIDChanged();
		
		partial void OnTitleChanging(string value);
		partial void OnTitleChanged();
		
		partial void OnBodyChanging(string value);
		partial void OnBodyChanged();
		
		partial void OnPageUrlChanging(string value);
		partial void OnPageUrlChanged();
		
		partial void OnCreatedOnChanging(DateTime value);
		partial void OnCreatedOnChanged();
		
		partial void OnModifiedOnChanging(DateTime? value);
		partial void OnModifiedOnChanged();
		
		partial void OnDeletedChanging(bool value);
		partial void OnDeletedChanged();
		
		partial void OnCreatedByIdChanging(int? value);
		partial void OnCreatedByIdChanged();
		
		partial void OnModifiedByIdChanging(int? value);
		partial void OnModifiedByIdChanged();
		
		partial void OnCUseridChanging(int? value);
		partial void OnCUseridChanged();
		
		partial void OnCUserid2Changing(int? value);
		partial void OnCUserid2Changed();
		
    #endregion
		public PageContent()
		{
			
			
			this._CreatedBy = default(EntityRef< User>); 
			
			this._ModifiedBy = default(EntityRef< User>); 
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="PageID", UpdateCheck=UpdateCheck.Never, Storage="_PageID", AutoSync=AutoSync.OnInsert, DbType="int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int PageID
		{
			get { return this._PageID; }

			set
			{
				if (this._PageID != value)
				{
				
                    this.OnPageIDChanging(value);
					this.SendPropertyChanging();
					this._PageID = value;
					this.SendPropertyChanged("PageID");
					this.OnPageIDChanged();
				}

			}

		}

		
		[Column(Name="Title", UpdateCheck=UpdateCheck.Never, Storage="_Title", DbType="nvarchar(500) NOT NULL")]
		public string Title
		{
			get { return this._Title; }

			set
			{
				if (this._Title != value)
				{
				
                    this.OnTitleChanging(value);
					this.SendPropertyChanging();
					this._Title = value;
					this.SendPropertyChanged("Title");
					this.OnTitleChanged();
				}

			}

		}

		
		[Column(Name="Body", UpdateCheck=UpdateCheck.Never, Storage="_Body", DbType="nvarchar")]
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

		
		[Column(Name="PageUrl", UpdateCheck=UpdateCheck.Never, Storage="_PageUrl", DbType="nvarchar(500) NOT NULL")]
		public string PageUrl
		{
			get { return this._PageUrl; }

			set
			{
				if (this._PageUrl != value)
				{
				
                    this.OnPageUrlChanging(value);
					this.SendPropertyChanging();
					this._PageUrl = value;
					this.SendPropertyChanged("PageUrl");
					this.OnPageUrlChanged();
				}

			}

		}

		
		[Column(Name="CreatedOn", UpdateCheck=UpdateCheck.Never, Storage="_CreatedOn", DbType="datetime NOT NULL")]
		public DateTime CreatedOn
		{
			get { return this._CreatedOn; }

			set
			{
				if (this._CreatedOn != value)
				{
				
                    this.OnCreatedOnChanging(value);
					this.SendPropertyChanging();
					this._CreatedOn = value;
					this.SendPropertyChanged("CreatedOn");
					this.OnCreatedOnChanged();
				}

			}

		}

		
		[Column(Name="ModifiedOn", UpdateCheck=UpdateCheck.Never, Storage="_ModifiedOn", DbType="datetime")]
		public DateTime? ModifiedOn
		{
			get { return this._ModifiedOn; }

			set
			{
				if (this._ModifiedOn != value)
				{
				
                    this.OnModifiedOnChanging(value);
					this.SendPropertyChanging();
					this._ModifiedOn = value;
					this.SendPropertyChanged("ModifiedOn");
					this.OnModifiedOnChanged();
				}

			}

		}

		
		[Column(Name="Deleted", UpdateCheck=UpdateCheck.Never, Storage="_Deleted", DbType="bit NOT NULL")]
		public bool Deleted
		{
			get { return this._Deleted; }

			set
			{
				if (this._Deleted != value)
				{
				
                    this.OnDeletedChanging(value);
					this.SendPropertyChanging();
					this._Deleted = value;
					this.SendPropertyChanged("Deleted");
					this.OnDeletedChanged();
				}

			}

		}

		
		[Column(Name="CreatedById", UpdateCheck=UpdateCheck.Never, Storage="_CreatedById", DbType="int")]
		public int? CreatedById
		{
			get { return this._CreatedById; }

			set
			{
				if (this._CreatedById != value)
				{
				
					if (this._CreatedBy.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnCreatedByIdChanging(value);
					this.SendPropertyChanging();
					this._CreatedById = value;
					this.SendPropertyChanged("CreatedById");
					this.OnCreatedByIdChanged();
				}

			}

		}

		
		[Column(Name="ModifiedById", UpdateCheck=UpdateCheck.Never, Storage="_ModifiedById", DbType="int")]
		public int? ModifiedById
		{
			get { return this._ModifiedById; }

			set
			{
				if (this._ModifiedById != value)
				{
				
					if (this._ModifiedBy.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnModifiedByIdChanging(value);
					this.SendPropertyChanging();
					this._ModifiedById = value;
					this.SendPropertyChanged("ModifiedById");
					this.OnModifiedByIdChanged();
				}

			}

		}

		
		[Column(Name="cUserid", UpdateCheck=UpdateCheck.Never, Storage="_CUserid", DbType="int")]
		public int? CUserid
		{
			get { return this._CUserid; }

			set
			{
				if (this._CUserid != value)
				{
				
                    this.OnCUseridChanging(value);
					this.SendPropertyChanging();
					this._CUserid = value;
					this.SendPropertyChanged("CUserid");
					this.OnCUseridChanged();
				}

			}

		}

		
		[Column(Name="cUserid2", UpdateCheck=UpdateCheck.Never, Storage="_CUserid2", DbType="int")]
		public int? CUserid2
		{
			get { return this._CUserid2; }

			set
			{
				if (this._CUserid2 != value)
				{
				
                    this.OnCUserid2Changing(value);
					this.SendPropertyChanging();
					this._CUserid2 = value;
					this.SendPropertyChanged("CUserid2");
					this.OnCUserid2Changed();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="CreatedPages__CreatedBy", Storage="_CreatedBy", ThisKey="CreatedById", IsForeignKey=true)]
		public User CreatedBy
		{
			get { return this._CreatedBy.Entity; }

			set
			{
				User previousValue = this._CreatedBy.Entity;
				if (((previousValue != value) 
							|| (this._CreatedBy.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._CreatedBy.Entity = null;
						previousValue.CreatedPages.Remove(this);
					}

					this._CreatedBy.Entity = value;
					if (value != null)
					{
						value.CreatedPages.Add(this);
						
						this._CreatedById = value.UserId;
						
					}

					else
					{
						
						this._CreatedById = default(int?);
						
					}

					this.SendPropertyChanged("CreatedBy");
				}

			}

		}

		
		[Association(Name="ModifiedPages__ModifiedBy", Storage="_ModifiedBy", ThisKey="ModifiedById", IsForeignKey=true)]
		public User ModifiedBy
		{
			get { return this._ModifiedBy.Entity; }

			set
			{
				User previousValue = this._ModifiedBy.Entity;
				if (((previousValue != value) 
							|| (this._ModifiedBy.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._ModifiedBy.Entity = null;
						previousValue.ModifiedPages.Remove(this);
					}

					this._ModifiedBy.Entity = value;
					if (value != null)
					{
						value.ModifiedPages.Add(this);
						
						this._ModifiedById = value.UserId;
						
					}

					else
					{
						
						this._ModifiedById = default(int?);
						
					}

					this.SendPropertyChanged("ModifiedBy");
				}

			}

		}

		
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

