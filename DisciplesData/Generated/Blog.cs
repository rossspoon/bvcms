using System; 
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;

namespace DiscData
{
	[Table(Name="dbo.Blog")]
	public partial class Blog : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _Id;
		
		private string _Description;
		
		private string _Name;
		
		private string _Title;
		
		private int? _GroupId;
		
		private bool _NotOnMenu;
		
		private int? _OwnerId;
		
		private int _PrivacyLevel;
		
		private int? _CUserid;
		
   		
   		private EntitySet< BlogNotify> _BlogNotifications;
		
   		private EntitySet< BlogPost> _BlogPosts;
		
   		private EntitySet< OtherNotify> _OtherNotifications;
		
    	
		private EntityRef< Group> _Group;
		
		private EntityRef< User> _User;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnIdChanging(int value);
		partial void OnIdChanged();
		
		partial void OnDescriptionChanging(string value);
		partial void OnDescriptionChanged();
		
		partial void OnNameChanging(string value);
		partial void OnNameChanged();
		
		partial void OnTitleChanging(string value);
		partial void OnTitleChanged();
		
		partial void OnGroupIdChanging(int? value);
		partial void OnGroupIdChanged();
		
		partial void OnNotOnMenuChanging(bool value);
		partial void OnNotOnMenuChanged();
		
		partial void OnOwnerIdChanging(int? value);
		partial void OnOwnerIdChanged();
		
		partial void OnPrivacyLevelChanging(int value);
		partial void OnPrivacyLevelChanged();
		
		partial void OnCUseridChanging(int? value);
		partial void OnCUseridChanged();
		
    #endregion
		public Blog()
		{
			
			this._BlogNotifications = new EntitySet< BlogNotify>(new Action< BlogNotify>(this.attach_BlogNotifications), new Action< BlogNotify>(this.detach_BlogNotifications)); 
			
			this._BlogPosts = new EntitySet< BlogPost>(new Action< BlogPost>(this.attach_BlogPosts), new Action< BlogPost>(this.detach_BlogPosts)); 
			
			this._OtherNotifications = new EntitySet< OtherNotify>(new Action< OtherNotify>(this.attach_OtherNotifications), new Action< OtherNotify>(this.detach_OtherNotifications)); 
			
			
			this._Group = default(EntityRef< Group>); 
			
			this._User = default(EntityRef< User>); 
			
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

		
		[Column(Name="Description", UpdateCheck=UpdateCheck.Never, Storage="_Description", DbType="nvarchar(250)")]
		public string Description
		{
			get { return this._Description; }

			set
			{
				if (this._Description != value)
				{
				
                    this.OnDescriptionChanging(value);
					this.SendPropertyChanging();
					this._Description = value;
					this.SendPropertyChanged("Description");
					this.OnDescriptionChanged();
				}

			}

		}

		
		[Column(Name="Name", UpdateCheck=UpdateCheck.Never, Storage="_Name", DbType="nvarchar(50)")]
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

		
		[Column(Name="Title", UpdateCheck=UpdateCheck.Never, Storage="_Title", DbType="nvarchar(50)")]
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

		
		[Column(Name="GroupId", UpdateCheck=UpdateCheck.Never, Storage="_GroupId", DbType="int")]
		public int? GroupId
		{
			get { return this._GroupId; }

			set
			{
				if (this._GroupId != value)
				{
				
					if (this._Group.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnGroupIdChanging(value);
					this.SendPropertyChanging();
					this._GroupId = value;
					this.SendPropertyChanged("GroupId");
					this.OnGroupIdChanged();
				}

			}

		}

		
		[Column(Name="NotOnMenu", UpdateCheck=UpdateCheck.Never, Storage="_NotOnMenu", DbType="bit NOT NULL")]
		public bool NotOnMenu
		{
			get { return this._NotOnMenu; }

			set
			{
				if (this._NotOnMenu != value)
				{
				
                    this.OnNotOnMenuChanging(value);
					this.SendPropertyChanging();
					this._NotOnMenu = value;
					this.SendPropertyChanged("NotOnMenu");
					this.OnNotOnMenuChanged();
				}

			}

		}

		
		[Column(Name="OwnerId", UpdateCheck=UpdateCheck.Never, Storage="_OwnerId", DbType="int")]
		public int? OwnerId
		{
			get { return this._OwnerId; }

			set
			{
				if (this._OwnerId != value)
				{
				
					if (this._User.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnOwnerIdChanging(value);
					this.SendPropertyChanging();
					this._OwnerId = value;
					this.SendPropertyChanged("OwnerId");
					this.OnOwnerIdChanged();
				}

			}

		}

		
		[Column(Name="PrivacyLevel", UpdateCheck=UpdateCheck.Never, Storage="_PrivacyLevel", DbType="int NOT NULL")]
		public int PrivacyLevel
		{
			get { return this._PrivacyLevel; }

			set
			{
				if (this._PrivacyLevel != value)
				{
				
                    this.OnPrivacyLevelChanging(value);
					this.SendPropertyChanging();
					this._PrivacyLevel = value;
					this.SendPropertyChanged("PrivacyLevel");
					this.OnPrivacyLevelChanged();
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

		
    #endregion
        
    #region Foreign Key Tables
   		
   		[Association(Name="FK_BlogNotify_Blog", Storage="_BlogNotifications", OtherKey="BlogId")]
   		public EntitySet< BlogNotify> BlogNotifications
   		{
   		    get { return this._BlogNotifications; }

			set	{ this._BlogNotifications.Assign(value); }

   		}

		
   		[Association(Name="FK_BlogPost_Blog", Storage="_BlogPosts", OtherKey="BlogId")]
   		public EntitySet< BlogPost> BlogPosts
   		{
   		    get { return this._BlogPosts; }

			set	{ this._BlogPosts.Assign(value); }

   		}

		
   		[Association(Name="FK_OtherNotify_Blog", Storage="_OtherNotifications", OtherKey="BlogId")]
   		public EntitySet< OtherNotify> OtherNotifications
   		{
   		    get { return this._OtherNotifications; }

			set	{ this._OtherNotifications.Assign(value); }

   		}

		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="FK_Blog_Group", Storage="_Group", ThisKey="GroupId", IsForeignKey=true)]
		public Group Group
		{
			get { return this._Group.Entity; }

			set
			{
				Group previousValue = this._Group.Entity;
				if (((previousValue != value) 
							|| (this._Group.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._Group.Entity = null;
						previousValue.Blogs.Remove(this);
					}

					this._Group.Entity = value;
					if (value != null)
					{
						value.Blogs.Add(this);
						
						this._GroupId = value.Id;
						
					}

					else
					{
						
						this._GroupId = default(int?);
						
					}

					this.SendPropertyChanged("Group");
				}

			}

		}

		
		[Association(Name="FK_Blog_Users", Storage="_User", ThisKey="OwnerId", IsForeignKey=true)]
		public User User
		{
			get { return this._User.Entity; }

			set
			{
				User previousValue = this._User.Entity;
				if (((previousValue != value) 
							|| (this._User.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._User.Entity = null;
						previousValue.Blogs.Remove(this);
					}

					this._User.Entity = value;
					if (value != null)
					{
						value.Blogs.Add(this);
						
						this._OwnerId = value.UserId;
						
					}

					else
					{
						
						this._OwnerId = default(int?);
						
					}

					this.SendPropertyChanged("User");
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

   		
		private void attach_BlogNotifications(BlogNotify entity)
		{
			this.SendPropertyChanging();
			entity.Blog = this;
		}

		private void detach_BlogNotifications(BlogNotify entity)
		{
			this.SendPropertyChanging();
			entity.Blog = null;
		}

		
		private void attach_BlogPosts(BlogPost entity)
		{
			this.SendPropertyChanging();
			entity.Blog = this;
		}

		private void detach_BlogPosts(BlogPost entity)
		{
			this.SendPropertyChanging();
			entity.Blog = null;
		}

		
		private void attach_OtherNotifications(OtherNotify entity)
		{
			this.SendPropertyChanging();
			entity.Blog = this;
		}

		private void detach_OtherNotifications(OtherNotify entity)
		{
			this.SendPropertyChanging();
			entity.Blog = null;
		}

		
	}

}

