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
	[Table(Name="dbo.ForumEntry")]
	public partial class ForumEntry : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _Id;
		
		private string _Title;
		
		private string _Entry;
		
		private int? _ForumId;
		
		private int? _ReplyToId;
		
		private int? _ThreadId;
		
		private int? _DisplayOrder;
		
		private int? _DisplayDepth;
		
		private DateTime? _CreatedOn;
		
		private int? _CreatedBy;
		
   		
   		private EntitySet< ForumNotify> _ForumNotifications;
		
   		private EntitySet< ForumUserRead> _ForumUserReads;
		
   		private EntitySet< ForumEntry> _Replies;
		
   		private EntitySet< ForumEntry> _ThreadEntries;
		
    	
		private EntityRef< Forum> _Forum;
		
		private EntityRef< User> _User;
		
		private EntityRef< ForumEntry> _RepliedTo;
		
		private EntityRef< ForumEntry> _ThreadPost;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnIdChanging(int value);
		partial void OnIdChanged();
		
		partial void OnTitleChanging(string value);
		partial void OnTitleChanged();
		
		partial void OnEntryChanging(string value);
		partial void OnEntryChanged();
		
		partial void OnForumIdChanging(int? value);
		partial void OnForumIdChanged();
		
		partial void OnReplyToIdChanging(int? value);
		partial void OnReplyToIdChanged();
		
		partial void OnThreadIdChanging(int? value);
		partial void OnThreadIdChanged();
		
		partial void OnDisplayOrderChanging(int? value);
		partial void OnDisplayOrderChanged();
		
		partial void OnDisplayDepthChanging(int? value);
		partial void OnDisplayDepthChanged();
		
		partial void OnCreatedOnChanging(DateTime? value);
		partial void OnCreatedOnChanged();
		
		partial void OnCreatedByChanging(int? value);
		partial void OnCreatedByChanged();
		
    #endregion
		public ForumEntry()
		{
			
			this._ForumNotifications = new EntitySet< ForumNotify>(new Action< ForumNotify>(this.attach_ForumNotifications), new Action< ForumNotify>(this.detach_ForumNotifications)); 
			
			this._ForumUserReads = new EntitySet< ForumUserRead>(new Action< ForumUserRead>(this.attach_ForumUserReads), new Action< ForumUserRead>(this.detach_ForumUserReads)); 
			
			this._Replies = new EntitySet< ForumEntry>(new Action< ForumEntry>(this.attach_Replies), new Action< ForumEntry>(this.detach_Replies)); 
			
			this._ThreadEntries = new EntitySet< ForumEntry>(new Action< ForumEntry>(this.attach_ThreadEntries), new Action< ForumEntry>(this.detach_ThreadEntries)); 
			
			
			this._Forum = default(EntityRef< Forum>); 
			
			this._User = default(EntityRef< User>); 
			
			this._RepliedTo = default(EntityRef< ForumEntry>); 
			
			this._ThreadPost = default(EntityRef< ForumEntry>); 
			
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

		
		[Column(Name="Entry", UpdateCheck=UpdateCheck.Never, Storage="_Entry", DbType="text(2147483647)")]
		public string Entry
		{
			get { return this._Entry; }

			set
			{
				if (this._Entry != value)
				{
				
                    this.OnEntryChanging(value);
					this.SendPropertyChanging();
					this._Entry = value;
					this.SendPropertyChanged("Entry");
					this.OnEntryChanged();
				}

			}

		}

		
		[Column(Name="ForumId", UpdateCheck=UpdateCheck.Never, Storage="_ForumId", DbType="int")]
		public int? ForumId
		{
			get { return this._ForumId; }

			set
			{
				if (this._ForumId != value)
				{
				
					if (this._Forum.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnForumIdChanging(value);
					this.SendPropertyChanging();
					this._ForumId = value;
					this.SendPropertyChanged("ForumId");
					this.OnForumIdChanged();
				}

			}

		}

		
		[Column(Name="ReplyToId", UpdateCheck=UpdateCheck.Never, Storage="_ReplyToId", DbType="int")]
		public int? ReplyToId
		{
			get { return this._ReplyToId; }

			set
			{
				if (this._ReplyToId != value)
				{
				
					if (this._RepliedTo.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnReplyToIdChanging(value);
					this.SendPropertyChanging();
					this._ReplyToId = value;
					this.SendPropertyChanged("ReplyToId");
					this.OnReplyToIdChanged();
				}

			}

		}

		
		[Column(Name="ThreadId", UpdateCheck=UpdateCheck.Never, Storage="_ThreadId", DbType="int")]
		public int? ThreadId
		{
			get { return this._ThreadId; }

			set
			{
				if (this._ThreadId != value)
				{
				
					if (this._ThreadPost.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnThreadIdChanging(value);
					this.SendPropertyChanging();
					this._ThreadId = value;
					this.SendPropertyChanged("ThreadId");
					this.OnThreadIdChanged();
				}

			}

		}

		
		[Column(Name="DisplayOrder", UpdateCheck=UpdateCheck.Never, Storage="_DisplayOrder", DbType="int")]
		public int? DisplayOrder
		{
			get { return this._DisplayOrder; }

			set
			{
				if (this._DisplayOrder != value)
				{
				
                    this.OnDisplayOrderChanging(value);
					this.SendPropertyChanging();
					this._DisplayOrder = value;
					this.SendPropertyChanged("DisplayOrder");
					this.OnDisplayOrderChanged();
				}

			}

		}

		
		[Column(Name="DisplayDepth", UpdateCheck=UpdateCheck.Never, Storage="_DisplayDepth", DbType="int")]
		public int? DisplayDepth
		{
			get { return this._DisplayDepth; }

			set
			{
				if (this._DisplayDepth != value)
				{
				
                    this.OnDisplayDepthChanging(value);
					this.SendPropertyChanging();
					this._DisplayDepth = value;
					this.SendPropertyChanged("DisplayDepth");
					this.OnDisplayDepthChanged();
				}

			}

		}

		
		[Column(Name="CreatedOn", UpdateCheck=UpdateCheck.Never, Storage="_CreatedOn", DbType="datetime")]
		public DateTime? CreatedOn
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

		
		[Column(Name="CreatedBy", UpdateCheck=UpdateCheck.Never, Storage="_CreatedBy", DbType="int")]
		public int? CreatedBy
		{
			get { return this._CreatedBy; }

			set
			{
				if (this._CreatedBy != value)
				{
				
					if (this._User.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnCreatedByChanging(value);
					this.SendPropertyChanging();
					this._CreatedBy = value;
					this.SendPropertyChanged("CreatedBy");
					this.OnCreatedByChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
   		[Association(Name="FK_ForumNotify_ForumEntry", Storage="_ForumNotifications", OtherKey="ThreadId")]
   		public EntitySet< ForumNotify> ForumNotifications
   		{
   		    get { return this._ForumNotifications; }

			set	{ this._ForumNotifications.Assign(value); }

   		}

		
   		[Association(Name="FK_ForumUserRead_ForumEntry", Storage="_ForumUserReads", OtherKey="ForumEntryId")]
   		public EntitySet< ForumUserRead> ForumUserReads
   		{
   		    get { return this._ForumUserReads; }

			set	{ this._ForumUserReads.Assign(value); }

   		}

		
   		[Association(Name="Replies__RepliedTo", Storage="_Replies", OtherKey="ReplyToId")]
   		public EntitySet< ForumEntry> Replies
   		{
   		    get { return this._Replies; }

			set	{ this._Replies.Assign(value); }

   		}

		
   		[Association(Name="ThreadEntries__ThreadPost", Storage="_ThreadEntries", OtherKey="ThreadId")]
   		public EntitySet< ForumEntry> ThreadEntries
   		{
   		    get { return this._ThreadEntries; }

			set	{ this._ThreadEntries.Assign(value); }

   		}

		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="FK_ForumEntry_Forum", Storage="_Forum", ThisKey="ForumId", IsForeignKey=true)]
		public Forum Forum
		{
			get { return this._Forum.Entity; }

			set
			{
				Forum previousValue = this._Forum.Entity;
				if (((previousValue != value) 
							|| (this._Forum.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._Forum.Entity = null;
						previousValue.ForumEntries.Remove(this);
					}

					this._Forum.Entity = value;
					if (value != null)
					{
						value.ForumEntries.Add(this);
						
						this._ForumId = value.Id;
						
					}

					else
					{
						
						this._ForumId = default(int?);
						
					}

					this.SendPropertyChanged("Forum");
				}

			}

		}

		
		[Association(Name="FK_ForumEntry_Users", Storage="_User", ThisKey="CreatedBy", IsForeignKey=true)]
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
						previousValue.ForumEntries.Remove(this);
					}

					this._User.Entity = value;
					if (value != null)
					{
						value.ForumEntries.Add(this);
						
						this._CreatedBy = value.UserId;
						
					}

					else
					{
						
						this._CreatedBy = default(int?);
						
					}

					this.SendPropertyChanged("User");
				}

			}

		}

		
		[Association(Name="Replies__RepliedTo", Storage="_RepliedTo", ThisKey="ReplyToId", IsForeignKey=true)]
		public ForumEntry RepliedTo
		{
			get { return this._RepliedTo.Entity; }

			set
			{
				ForumEntry previousValue = this._RepliedTo.Entity;
				if (((previousValue != value) 
							|| (this._RepliedTo.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._RepliedTo.Entity = null;
						previousValue.Replies.Remove(this);
					}

					this._RepliedTo.Entity = value;
					if (value != null)
					{
						value.Replies.Add(this);
						
						this._ReplyToId = value.Id;
						
					}

					else
					{
						
						this._ReplyToId = default(int?);
						
					}

					this.SendPropertyChanged("RepliedTo");
				}

			}

		}

		
		[Association(Name="ThreadEntries__ThreadPost", Storage="_ThreadPost", ThisKey="ThreadId", IsForeignKey=true)]
		public ForumEntry ThreadPost
		{
			get { return this._ThreadPost.Entity; }

			set
			{
				ForumEntry previousValue = this._ThreadPost.Entity;
				if (((previousValue != value) 
							|| (this._ThreadPost.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._ThreadPost.Entity = null;
						previousValue.ThreadEntries.Remove(this);
					}

					this._ThreadPost.Entity = value;
					if (value != null)
					{
						value.ThreadEntries.Add(this);
						
						this._ThreadId = value.Id;
						
					}

					else
					{
						
						this._ThreadId = default(int?);
						
					}

					this.SendPropertyChanged("ThreadPost");
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

   		
		private void attach_ForumNotifications(ForumNotify entity)
		{
			this.SendPropertyChanging();
			entity.ForumEntry = this;
		}

		private void detach_ForumNotifications(ForumNotify entity)
		{
			this.SendPropertyChanging();
			entity.ForumEntry = null;
		}

		
		private void attach_ForumUserReads(ForumUserRead entity)
		{
			this.SendPropertyChanging();
			entity.ForumEntry = this;
		}

		private void detach_ForumUserReads(ForumUserRead entity)
		{
			this.SendPropertyChanging();
			entity.ForumEntry = null;
		}

		
		private void attach_Replies(ForumEntry entity)
		{
			this.SendPropertyChanging();
			entity.RepliedTo = this;
		}

		private void detach_Replies(ForumEntry entity)
		{
			this.SendPropertyChanging();
			entity.RepliedTo = null;
		}

		
		private void attach_ThreadEntries(ForumEntry entity)
		{
			this.SendPropertyChanging();
			entity.ThreadPost = this;
		}

		private void detach_ThreadEntries(ForumEntry entity)
		{
			this.SendPropertyChanging();
			entity.ThreadPost = null;
		}

		
	}

}

