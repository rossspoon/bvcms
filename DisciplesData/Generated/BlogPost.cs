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
	[Table(Name="dbo.BlogPost")]
	public partial class BlogPost : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _Id;
		
		private string _Title;
		
		private int _BlogId;
		
		private string _Post;
		
		private DateTime? _EntryDate;
		
		private DateTime? _Updated;
		
		private string _EnclosureUrl;
		
		private int? _EnclosureLength;
		
		private string _EnclosureType;
		
		private bool _IsPublic;
		
		private bool _NotifyLater;
		
		private int? _PosterId;
		
		private int? _CUserid;
		
   		
   		private EntitySet< BlogCategoryXref> _BlogCategoryXrefs;
		
   		private EntitySet< BlogComment> _BlogComments;
		
   		private EntitySet< PodCast> _PodCasts;
		
    	
		private EntityRef< Blog> _Blog;
		
		private EntityRef< User> _User;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnIdChanging(int value);
		partial void OnIdChanged();
		
		partial void OnTitleChanging(string value);
		partial void OnTitleChanged();
		
		partial void OnBlogIdChanging(int value);
		partial void OnBlogIdChanged();
		
		partial void OnPostChanging(string value);
		partial void OnPostChanged();
		
		partial void OnEntryDateChanging(DateTime? value);
		partial void OnEntryDateChanged();
		
		partial void OnUpdatedChanging(DateTime? value);
		partial void OnUpdatedChanged();
		
		partial void OnEnclosureUrlChanging(string value);
		partial void OnEnclosureUrlChanged();
		
		partial void OnEnclosureLengthChanging(int? value);
		partial void OnEnclosureLengthChanged();
		
		partial void OnEnclosureTypeChanging(string value);
		partial void OnEnclosureTypeChanged();
		
		partial void OnIsPublicChanging(bool value);
		partial void OnIsPublicChanged();
		
		partial void OnNotifyLaterChanging(bool value);
		partial void OnNotifyLaterChanged();
		
		partial void OnPosterIdChanging(int? value);
		partial void OnPosterIdChanged();
		
		partial void OnCUseridChanging(int? value);
		partial void OnCUseridChanged();
		
    #endregion
		public BlogPost()
		{
			
			this._BlogCategoryXrefs = new EntitySet< BlogCategoryXref>(new Action< BlogCategoryXref>(this.attach_BlogCategoryXrefs), new Action< BlogCategoryXref>(this.detach_BlogCategoryXrefs)); 
			
			this._BlogComments = new EntitySet< BlogComment>(new Action< BlogComment>(this.attach_BlogComments), new Action< BlogComment>(this.detach_BlogComments)); 
			
			this._PodCasts = new EntitySet< PodCast>(new Action< PodCast>(this.attach_PodCasts), new Action< PodCast>(this.detach_PodCasts)); 
			
			
			this._Blog = default(EntityRef< Blog>); 
			
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

		
		[Column(Name="Title", UpdateCheck=UpdateCheck.Never, Storage="_Title", DbType="nvarchar(250)")]
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

		
		[Column(Name="BlogId", UpdateCheck=UpdateCheck.Never, Storage="_BlogId", DbType="int NOT NULL")]
		public int BlogId
		{
			get { return this._BlogId; }

			set
			{
				if (this._BlogId != value)
				{
				
					if (this._Blog.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnBlogIdChanging(value);
					this.SendPropertyChanging();
					this._BlogId = value;
					this.SendPropertyChanged("BlogId");
					this.OnBlogIdChanged();
				}

			}

		}

		
		[Column(Name="Post", UpdateCheck=UpdateCheck.Never, Storage="_Post", DbType="text(2147483647)")]
		public string Post
		{
			get { return this._Post; }

			set
			{
				if (this._Post != value)
				{
				
                    this.OnPostChanging(value);
					this.SendPropertyChanging();
					this._Post = value;
					this.SendPropertyChanged("Post");
					this.OnPostChanged();
				}

			}

		}

		
		[Column(Name="EntryDate", UpdateCheck=UpdateCheck.Never, Storage="_EntryDate", DbType="datetime")]
		public DateTime? EntryDate
		{
			get { return this._EntryDate; }

			set
			{
				if (this._EntryDate != value)
				{
				
                    this.OnEntryDateChanging(value);
					this.SendPropertyChanging();
					this._EntryDate = value;
					this.SendPropertyChanged("EntryDate");
					this.OnEntryDateChanged();
				}

			}

		}

		
		[Column(Name="Updated", UpdateCheck=UpdateCheck.Never, Storage="_Updated", DbType="datetime")]
		public DateTime? Updated
		{
			get { return this._Updated; }

			set
			{
				if (this._Updated != value)
				{
				
                    this.OnUpdatedChanging(value);
					this.SendPropertyChanging();
					this._Updated = value;
					this.SendPropertyChanged("Updated");
					this.OnUpdatedChanged();
				}

			}

		}

		
		[Column(Name="EnclosureUrl", UpdateCheck=UpdateCheck.Never, Storage="_EnclosureUrl", DbType="nvarchar(100)")]
		public string EnclosureUrl
		{
			get { return this._EnclosureUrl; }

			set
			{
				if (this._EnclosureUrl != value)
				{
				
                    this.OnEnclosureUrlChanging(value);
					this.SendPropertyChanging();
					this._EnclosureUrl = value;
					this.SendPropertyChanged("EnclosureUrl");
					this.OnEnclosureUrlChanged();
				}

			}

		}

		
		[Column(Name="EnclosureLength", UpdateCheck=UpdateCheck.Never, Storage="_EnclosureLength", DbType="int")]
		public int? EnclosureLength
		{
			get { return this._EnclosureLength; }

			set
			{
				if (this._EnclosureLength != value)
				{
				
                    this.OnEnclosureLengthChanging(value);
					this.SendPropertyChanging();
					this._EnclosureLength = value;
					this.SendPropertyChanged("EnclosureLength");
					this.OnEnclosureLengthChanged();
				}

			}

		}

		
		[Column(Name="EnclosureType", UpdateCheck=UpdateCheck.Never, Storage="_EnclosureType", DbType="nvarchar(50)")]
		public string EnclosureType
		{
			get { return this._EnclosureType; }

			set
			{
				if (this._EnclosureType != value)
				{
				
                    this.OnEnclosureTypeChanging(value);
					this.SendPropertyChanging();
					this._EnclosureType = value;
					this.SendPropertyChanged("EnclosureType");
					this.OnEnclosureTypeChanged();
				}

			}

		}

		
		[Column(Name="IsPublic", UpdateCheck=UpdateCheck.Never, Storage="_IsPublic", DbType="bit NOT NULL")]
		public bool IsPublic
		{
			get { return this._IsPublic; }

			set
			{
				if (this._IsPublic != value)
				{
				
                    this.OnIsPublicChanging(value);
					this.SendPropertyChanging();
					this._IsPublic = value;
					this.SendPropertyChanged("IsPublic");
					this.OnIsPublicChanged();
				}

			}

		}

		
		[Column(Name="NotifyLater", UpdateCheck=UpdateCheck.Never, Storage="_NotifyLater", DbType="bit NOT NULL")]
		public bool NotifyLater
		{
			get { return this._NotifyLater; }

			set
			{
				if (this._NotifyLater != value)
				{
				
                    this.OnNotifyLaterChanging(value);
					this.SendPropertyChanging();
					this._NotifyLater = value;
					this.SendPropertyChanged("NotifyLater");
					this.OnNotifyLaterChanged();
				}

			}

		}

		
		[Column(Name="PosterId", UpdateCheck=UpdateCheck.Never, Storage="_PosterId", DbType="int")]
		public int? PosterId
		{
			get { return this._PosterId; }

			set
			{
				if (this._PosterId != value)
				{
				
					if (this._User.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnPosterIdChanging(value);
					this.SendPropertyChanging();
					this._PosterId = value;
					this.SendPropertyChanged("PosterId");
					this.OnPosterIdChanged();
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
   		
   		[Association(Name="FK_BlogCategoryXref_BlogPost", Storage="_BlogCategoryXrefs", OtherKey="BlogPostId")]
   		public EntitySet< BlogCategoryXref> BlogCategoryXrefs
   		{
   		    get { return this._BlogCategoryXrefs; }

			set	{ this._BlogCategoryXrefs.Assign(value); }

   		}

		
   		[Association(Name="FK_BlogComment_BlogPost", Storage="_BlogComments", OtherKey="BlogPostId")]
   		public EntitySet< BlogComment> BlogComments
   		{
   		    get { return this._BlogComments; }

			set	{ this._BlogComments.Assign(value); }

   		}

		
   		[Association(Name="FK_PodCast_BlogPost", Storage="_PodCasts", OtherKey="PostId")]
   		public EntitySet< PodCast> PodCasts
   		{
   		    get { return this._PodCasts; }

			set	{ this._PodCasts.Assign(value); }

   		}

		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="FK_BlogPost_Blog", Storage="_Blog", ThisKey="BlogId", IsForeignKey=true)]
		public Blog Blog
		{
			get { return this._Blog.Entity; }

			set
			{
				Blog previousValue = this._Blog.Entity;
				if (((previousValue != value) 
							|| (this._Blog.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._Blog.Entity = null;
						previousValue.BlogPosts.Remove(this);
					}

					this._Blog.Entity = value;
					if (value != null)
					{
						value.BlogPosts.Add(this);
						
						this._BlogId = value.Id;
						
					}

					else
					{
						
						this._BlogId = default(int);
						
					}

					this.SendPropertyChanged("Blog");
				}

			}

		}

		
		[Association(Name="FK_BlogPost_Users", Storage="_User", ThisKey="PosterId", IsForeignKey=true)]
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
						previousValue.BlogPosts.Remove(this);
					}

					this._User.Entity = value;
					if (value != null)
					{
						value.BlogPosts.Add(this);
						
						this._PosterId = value.UserId;
						
					}

					else
					{
						
						this._PosterId = default(int?);
						
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

   		
		private void attach_BlogCategoryXrefs(BlogCategoryXref entity)
		{
			this.SendPropertyChanging();
			entity.BlogPost = this;
		}

		private void detach_BlogCategoryXrefs(BlogCategoryXref entity)
		{
			this.SendPropertyChanging();
			entity.BlogPost = null;
		}

		
		private void attach_BlogComments(BlogComment entity)
		{
			this.SendPropertyChanging();
			entity.BlogPost = this;
		}

		private void detach_BlogComments(BlogComment entity)
		{
			this.SendPropertyChanging();
			entity.BlogPost = null;
		}

		
		private void attach_PodCasts(PodCast entity)
		{
			this.SendPropertyChanging();
			entity.BlogPost = this;
		}

		private void detach_PodCasts(PodCast entity)
		{
			this.SendPropertyChanging();
			entity.BlogPost = null;
		}

		
	}

}

