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
	[Table(Name="dbo.Users")]
	public partial class User : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _UserId;
		
		private int? _PeopleId;
		
		private string _Username;
		
		private string _Comment;
		
		private string _Password;
		
		private string _PasswordQuestion;
		
		private string _PasswordAnswer;
		
		private bool _IsApproved;
		
		private DateTime? _LastActivityDate;
		
		private DateTime? _LastLoginDate;
		
		private DateTime? _LastPasswordChangedDate;
		
		private DateTime? _CreationDate;
		
		private bool _IsLockedOut;
		
		private DateTime? _LastLockedOutDate;
		
		private int _FailedPasswordAttemptCount;
		
		private DateTime? _FailedPasswordAttemptWindowStart;
		
		private int _FailedPasswordAnswerAttemptCount;
		
		private DateTime? _FailedPasswordAnswerAttemptWindowStart;
		
		private string _EmailAddress;
		
		private int? _ItemsInGrid;
		
		private string _CurrentCart;
		
		private bool _MustChangePassword;
		
		private string _Host;
		
		private string _TempPassword;
		
		private bool? _NotifyAll;
		
		private string _FirstName;
		
		private string _LastName;
		
		private DateTime? _BirthDay;
		
		private string _DefaultGroup;
		
		private bool? _NotifyEnabled;
		
		private bool? _ForceLogin;
		
		private int? _CUserId;
		
   		
   		private EntitySet< PageContent> _CreatedPages;
		
   		private EntitySet< Blog> _Blogs;
		
   		private EntitySet< BlogComment> _BlogComments;
		
   		private EntitySet< BlogNotify> _BlogNotifications;
		
   		private EntitySet< BlogPost> _BlogPosts;
		
   		private EntitySet< Forum> _Forums;
		
   		private EntitySet< ForumEntry> _ForumEntries;
		
   		private EntitySet< ForumUserRead> _ForumUserReads;
		
   		private EntitySet< PageVisit> _PageVisits;
		
   		private EntitySet< PendingNotification> _PendingNotifications;
		
   		private EntitySet< PodCast> _PodCasts;
		
   		private EntitySet< PrayerSlot> _PrayerSlots;
		
   		private EntitySet< TemporaryToken> _TemporaryTokens;
		
   		private EntitySet< UserGroupRole> _UserGroupRoles;
		
   		private EntitySet< UserRole> _UserRoles;
		
   		private EntitySet< Verse> _Verses;
		
   		private EntitySet< VerseCategory> _VerseCategories;
		
   		private EntitySet< PageContent> _ModifiedPages;
		
    	
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnUserIdChanging(int value);
		partial void OnUserIdChanged();
		
		partial void OnPeopleIdChanging(int? value);
		partial void OnPeopleIdChanged();
		
		partial void OnUsernameChanging(string value);
		partial void OnUsernameChanged();
		
		partial void OnCommentChanging(string value);
		partial void OnCommentChanged();
		
		partial void OnPasswordChanging(string value);
		partial void OnPasswordChanged();
		
		partial void OnPasswordQuestionChanging(string value);
		partial void OnPasswordQuestionChanged();
		
		partial void OnPasswordAnswerChanging(string value);
		partial void OnPasswordAnswerChanged();
		
		partial void OnIsApprovedChanging(bool value);
		partial void OnIsApprovedChanged();
		
		partial void OnLastActivityDateChanging(DateTime? value);
		partial void OnLastActivityDateChanged();
		
		partial void OnLastLoginDateChanging(DateTime? value);
		partial void OnLastLoginDateChanged();
		
		partial void OnLastPasswordChangedDateChanging(DateTime? value);
		partial void OnLastPasswordChangedDateChanged();
		
		partial void OnCreationDateChanging(DateTime? value);
		partial void OnCreationDateChanged();
		
		partial void OnIsLockedOutChanging(bool value);
		partial void OnIsLockedOutChanged();
		
		partial void OnLastLockedOutDateChanging(DateTime? value);
		partial void OnLastLockedOutDateChanged();
		
		partial void OnFailedPasswordAttemptCountChanging(int value);
		partial void OnFailedPasswordAttemptCountChanged();
		
		partial void OnFailedPasswordAttemptWindowStartChanging(DateTime? value);
		partial void OnFailedPasswordAttemptWindowStartChanged();
		
		partial void OnFailedPasswordAnswerAttemptCountChanging(int value);
		partial void OnFailedPasswordAnswerAttemptCountChanged();
		
		partial void OnFailedPasswordAnswerAttemptWindowStartChanging(DateTime? value);
		partial void OnFailedPasswordAnswerAttemptWindowStartChanged();
		
		partial void OnEmailAddressChanging(string value);
		partial void OnEmailAddressChanged();
		
		partial void OnItemsInGridChanging(int? value);
		partial void OnItemsInGridChanged();
		
		partial void OnCurrentCartChanging(string value);
		partial void OnCurrentCartChanged();
		
		partial void OnMustChangePasswordChanging(bool value);
		partial void OnMustChangePasswordChanged();
		
		partial void OnHostChanging(string value);
		partial void OnHostChanged();
		
		partial void OnTempPasswordChanging(string value);
		partial void OnTempPasswordChanged();
		
		partial void OnNotifyAllChanging(bool? value);
		partial void OnNotifyAllChanged();
		
		partial void OnFirstNameChanging(string value);
		partial void OnFirstNameChanged();
		
		partial void OnLastNameChanging(string value);
		partial void OnLastNameChanged();
		
		partial void OnBirthDayChanging(DateTime? value);
		partial void OnBirthDayChanged();
		
		partial void OnDefaultGroupChanging(string value);
		partial void OnDefaultGroupChanged();
		
		partial void OnNotifyEnabledChanging(bool? value);
		partial void OnNotifyEnabledChanged();
		
		partial void OnForceLoginChanging(bool? value);
		partial void OnForceLoginChanged();
		
		partial void OnCUserIdChanging(int? value);
		partial void OnCUserIdChanged();
		
    #endregion
		public User()
		{
			
			this._CreatedPages = new EntitySet< PageContent>(new Action< PageContent>(this.attach_CreatedPages), new Action< PageContent>(this.detach_CreatedPages)); 
			
			this._Blogs = new EntitySet< Blog>(new Action< Blog>(this.attach_Blogs), new Action< Blog>(this.detach_Blogs)); 
			
			this._BlogComments = new EntitySet< BlogComment>(new Action< BlogComment>(this.attach_BlogComments), new Action< BlogComment>(this.detach_BlogComments)); 
			
			this._BlogNotifications = new EntitySet< BlogNotify>(new Action< BlogNotify>(this.attach_BlogNotifications), new Action< BlogNotify>(this.detach_BlogNotifications)); 
			
			this._BlogPosts = new EntitySet< BlogPost>(new Action< BlogPost>(this.attach_BlogPosts), new Action< BlogPost>(this.detach_BlogPosts)); 
			
			this._Forums = new EntitySet< Forum>(new Action< Forum>(this.attach_Forums), new Action< Forum>(this.detach_Forums)); 
			
			this._ForumEntries = new EntitySet< ForumEntry>(new Action< ForumEntry>(this.attach_ForumEntries), new Action< ForumEntry>(this.detach_ForumEntries)); 
			
			this._ForumUserReads = new EntitySet< ForumUserRead>(new Action< ForumUserRead>(this.attach_ForumUserReads), new Action< ForumUserRead>(this.detach_ForumUserReads)); 
			
			this._PageVisits = new EntitySet< PageVisit>(new Action< PageVisit>(this.attach_PageVisits), new Action< PageVisit>(this.detach_PageVisits)); 
			
			this._PendingNotifications = new EntitySet< PendingNotification>(new Action< PendingNotification>(this.attach_PendingNotifications), new Action< PendingNotification>(this.detach_PendingNotifications)); 
			
			this._PodCasts = new EntitySet< PodCast>(new Action< PodCast>(this.attach_PodCasts), new Action< PodCast>(this.detach_PodCasts)); 
			
			this._PrayerSlots = new EntitySet< PrayerSlot>(new Action< PrayerSlot>(this.attach_PrayerSlots), new Action< PrayerSlot>(this.detach_PrayerSlots)); 
			
			this._TemporaryTokens = new EntitySet< TemporaryToken>(new Action< TemporaryToken>(this.attach_TemporaryTokens), new Action< TemporaryToken>(this.detach_TemporaryTokens)); 
			
			this._UserGroupRoles = new EntitySet< UserGroupRole>(new Action< UserGroupRole>(this.attach_UserGroupRoles), new Action< UserGroupRole>(this.detach_UserGroupRoles)); 
			
			this._UserRoles = new EntitySet< UserRole>(new Action< UserRole>(this.attach_UserRoles), new Action< UserRole>(this.detach_UserRoles)); 
			
			this._Verses = new EntitySet< Verse>(new Action< Verse>(this.attach_Verses), new Action< Verse>(this.detach_Verses)); 
			
			this._VerseCategories = new EntitySet< VerseCategory>(new Action< VerseCategory>(this.attach_VerseCategories), new Action< VerseCategory>(this.detach_VerseCategories)); 
			
			this._ModifiedPages = new EntitySet< PageContent>(new Action< PageContent>(this.attach_ModifiedPages), new Action< PageContent>(this.detach_ModifiedPages)); 
			
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="UserId", UpdateCheck=UpdateCheck.Never, Storage="_UserId", AutoSync=AutoSync.OnInsert, DbType="int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int UserId
		{
			get { return this._UserId; }

			set
			{
				if (this._UserId != value)
				{
				
                    this.OnUserIdChanging(value);
					this.SendPropertyChanging();
					this._UserId = value;
					this.SendPropertyChanged("UserId");
					this.OnUserIdChanged();
				}

			}

		}

		
		[Column(Name="PeopleId", UpdateCheck=UpdateCheck.Never, Storage="_PeopleId", DbType="int")]
		public int? PeopleId
		{
			get { return this._PeopleId; }

			set
			{
				if (this._PeopleId != value)
				{
				
                    this.OnPeopleIdChanging(value);
					this.SendPropertyChanging();
					this._PeopleId = value;
					this.SendPropertyChanged("PeopleId");
					this.OnPeopleIdChanged();
				}

			}

		}

		
		[Column(Name="Username", UpdateCheck=UpdateCheck.Never, Storage="_Username", DbType="varchar(50) NOT NULL")]
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

		
		[Column(Name="Comment", UpdateCheck=UpdateCheck.Never, Storage="_Comment", DbType="varchar(255)")]
		public string Comment
		{
			get { return this._Comment; }

			set
			{
				if (this._Comment != value)
				{
				
                    this.OnCommentChanging(value);
					this.SendPropertyChanging();
					this._Comment = value;
					this.SendPropertyChanged("Comment");
					this.OnCommentChanged();
				}

			}

		}

		
		[Column(Name="Password", UpdateCheck=UpdateCheck.Never, Storage="_Password", DbType="varchar(128) NOT NULL")]
		public string Password
		{
			get { return this._Password; }

			set
			{
				if (this._Password != value)
				{
				
                    this.OnPasswordChanging(value);
					this.SendPropertyChanging();
					this._Password = value;
					this.SendPropertyChanged("Password");
					this.OnPasswordChanged();
				}

			}

		}

		
		[Column(Name="PasswordQuestion", UpdateCheck=UpdateCheck.Never, Storage="_PasswordQuestion", DbType="varchar(255)")]
		public string PasswordQuestion
		{
			get { return this._PasswordQuestion; }

			set
			{
				if (this._PasswordQuestion != value)
				{
				
                    this.OnPasswordQuestionChanging(value);
					this.SendPropertyChanging();
					this._PasswordQuestion = value;
					this.SendPropertyChanged("PasswordQuestion");
					this.OnPasswordQuestionChanged();
				}

			}

		}

		
		[Column(Name="PasswordAnswer", UpdateCheck=UpdateCheck.Never, Storage="_PasswordAnswer", DbType="varchar(255)")]
		public string PasswordAnswer
		{
			get { return this._PasswordAnswer; }

			set
			{
				if (this._PasswordAnswer != value)
				{
				
                    this.OnPasswordAnswerChanging(value);
					this.SendPropertyChanging();
					this._PasswordAnswer = value;
					this.SendPropertyChanged("PasswordAnswer");
					this.OnPasswordAnswerChanged();
				}

			}

		}

		
		[Column(Name="IsApproved", UpdateCheck=UpdateCheck.Never, Storage="_IsApproved", DbType="bit NOT NULL")]
		public bool IsApproved
		{
			get { return this._IsApproved; }

			set
			{
				if (this._IsApproved != value)
				{
				
                    this.OnIsApprovedChanging(value);
					this.SendPropertyChanging();
					this._IsApproved = value;
					this.SendPropertyChanged("IsApproved");
					this.OnIsApprovedChanged();
				}

			}

		}

		
		[Column(Name="LastActivityDate", UpdateCheck=UpdateCheck.Never, Storage="_LastActivityDate", DbType="datetime")]
		public DateTime? LastActivityDate
		{
			get { return this._LastActivityDate; }

			set
			{
				if (this._LastActivityDate != value)
				{
				
                    this.OnLastActivityDateChanging(value);
					this.SendPropertyChanging();
					this._LastActivityDate = value;
					this.SendPropertyChanged("LastActivityDate");
					this.OnLastActivityDateChanged();
				}

			}

		}

		
		[Column(Name="LastLoginDate", UpdateCheck=UpdateCheck.Never, Storage="_LastLoginDate", DbType="datetime")]
		public DateTime? LastLoginDate
		{
			get { return this._LastLoginDate; }

			set
			{
				if (this._LastLoginDate != value)
				{
				
                    this.OnLastLoginDateChanging(value);
					this.SendPropertyChanging();
					this._LastLoginDate = value;
					this.SendPropertyChanged("LastLoginDate");
					this.OnLastLoginDateChanged();
				}

			}

		}

		
		[Column(Name="LastPasswordChangedDate", UpdateCheck=UpdateCheck.Never, Storage="_LastPasswordChangedDate", DbType="datetime")]
		public DateTime? LastPasswordChangedDate
		{
			get { return this._LastPasswordChangedDate; }

			set
			{
				if (this._LastPasswordChangedDate != value)
				{
				
                    this.OnLastPasswordChangedDateChanging(value);
					this.SendPropertyChanging();
					this._LastPasswordChangedDate = value;
					this.SendPropertyChanged("LastPasswordChangedDate");
					this.OnLastPasswordChangedDateChanged();
				}

			}

		}

		
		[Column(Name="CreationDate", UpdateCheck=UpdateCheck.Never, Storage="_CreationDate", DbType="datetime")]
		public DateTime? CreationDate
		{
			get { return this._CreationDate; }

			set
			{
				if (this._CreationDate != value)
				{
				
                    this.OnCreationDateChanging(value);
					this.SendPropertyChanging();
					this._CreationDate = value;
					this.SendPropertyChanged("CreationDate");
					this.OnCreationDateChanged();
				}

			}

		}

		
		[Column(Name="IsLockedOut", UpdateCheck=UpdateCheck.Never, Storage="_IsLockedOut", DbType="bit NOT NULL")]
		public bool IsLockedOut
		{
			get { return this._IsLockedOut; }

			set
			{
				if (this._IsLockedOut != value)
				{
				
                    this.OnIsLockedOutChanging(value);
					this.SendPropertyChanging();
					this._IsLockedOut = value;
					this.SendPropertyChanged("IsLockedOut");
					this.OnIsLockedOutChanged();
				}

			}

		}

		
		[Column(Name="LastLockedOutDate", UpdateCheck=UpdateCheck.Never, Storage="_LastLockedOutDate", DbType="datetime")]
		public DateTime? LastLockedOutDate
		{
			get { return this._LastLockedOutDate; }

			set
			{
				if (this._LastLockedOutDate != value)
				{
				
                    this.OnLastLockedOutDateChanging(value);
					this.SendPropertyChanging();
					this._LastLockedOutDate = value;
					this.SendPropertyChanged("LastLockedOutDate");
					this.OnLastLockedOutDateChanged();
				}

			}

		}

		
		[Column(Name="FailedPasswordAttemptCount", UpdateCheck=UpdateCheck.Never, Storage="_FailedPasswordAttemptCount", DbType="int NOT NULL")]
		public int FailedPasswordAttemptCount
		{
			get { return this._FailedPasswordAttemptCount; }

			set
			{
				if (this._FailedPasswordAttemptCount != value)
				{
				
                    this.OnFailedPasswordAttemptCountChanging(value);
					this.SendPropertyChanging();
					this._FailedPasswordAttemptCount = value;
					this.SendPropertyChanged("FailedPasswordAttemptCount");
					this.OnFailedPasswordAttemptCountChanged();
				}

			}

		}

		
		[Column(Name="FailedPasswordAttemptWindowStart", UpdateCheck=UpdateCheck.Never, Storage="_FailedPasswordAttemptWindowStart", DbType="datetime")]
		public DateTime? FailedPasswordAttemptWindowStart
		{
			get { return this._FailedPasswordAttemptWindowStart; }

			set
			{
				if (this._FailedPasswordAttemptWindowStart != value)
				{
				
                    this.OnFailedPasswordAttemptWindowStartChanging(value);
					this.SendPropertyChanging();
					this._FailedPasswordAttemptWindowStart = value;
					this.SendPropertyChanged("FailedPasswordAttemptWindowStart");
					this.OnFailedPasswordAttemptWindowStartChanged();
				}

			}

		}

		
		[Column(Name="FailedPasswordAnswerAttemptCount", UpdateCheck=UpdateCheck.Never, Storage="_FailedPasswordAnswerAttemptCount", DbType="int NOT NULL")]
		public int FailedPasswordAnswerAttemptCount
		{
			get { return this._FailedPasswordAnswerAttemptCount; }

			set
			{
				if (this._FailedPasswordAnswerAttemptCount != value)
				{
				
                    this.OnFailedPasswordAnswerAttemptCountChanging(value);
					this.SendPropertyChanging();
					this._FailedPasswordAnswerAttemptCount = value;
					this.SendPropertyChanged("FailedPasswordAnswerAttemptCount");
					this.OnFailedPasswordAnswerAttemptCountChanged();
				}

			}

		}

		
		[Column(Name="FailedPasswordAnswerAttemptWindowStart", UpdateCheck=UpdateCheck.Never, Storage="_FailedPasswordAnswerAttemptWindowStart", DbType="datetime")]
		public DateTime? FailedPasswordAnswerAttemptWindowStart
		{
			get { return this._FailedPasswordAnswerAttemptWindowStart; }

			set
			{
				if (this._FailedPasswordAnswerAttemptWindowStart != value)
				{
				
                    this.OnFailedPasswordAnswerAttemptWindowStartChanging(value);
					this.SendPropertyChanging();
					this._FailedPasswordAnswerAttemptWindowStart = value;
					this.SendPropertyChanged("FailedPasswordAnswerAttemptWindowStart");
					this.OnFailedPasswordAnswerAttemptWindowStartChanged();
				}

			}

		}

		
		[Column(Name="EmailAddress", UpdateCheck=UpdateCheck.Never, Storage="_EmailAddress", DbType="varchar(50)")]
		public string EmailAddress
		{
			get { return this._EmailAddress; }

			set
			{
				if (this._EmailAddress != value)
				{
				
                    this.OnEmailAddressChanging(value);
					this.SendPropertyChanging();
					this._EmailAddress = value;
					this.SendPropertyChanged("EmailAddress");
					this.OnEmailAddressChanged();
				}

			}

		}

		
		[Column(Name="ItemsInGrid", UpdateCheck=UpdateCheck.Never, Storage="_ItemsInGrid", DbType="int")]
		public int? ItemsInGrid
		{
			get { return this._ItemsInGrid; }

			set
			{
				if (this._ItemsInGrid != value)
				{
				
                    this.OnItemsInGridChanging(value);
					this.SendPropertyChanging();
					this._ItemsInGrid = value;
					this.SendPropertyChanged("ItemsInGrid");
					this.OnItemsInGridChanged();
				}

			}

		}

		
		[Column(Name="CurrentCart", UpdateCheck=UpdateCheck.Never, Storage="_CurrentCart", DbType="varchar(100)")]
		public string CurrentCart
		{
			get { return this._CurrentCart; }

			set
			{
				if (this._CurrentCart != value)
				{
				
                    this.OnCurrentCartChanging(value);
					this.SendPropertyChanging();
					this._CurrentCart = value;
					this.SendPropertyChanged("CurrentCart");
					this.OnCurrentCartChanged();
				}

			}

		}

		
		[Column(Name="MustChangePassword", UpdateCheck=UpdateCheck.Never, Storage="_MustChangePassword", DbType="bit NOT NULL")]
		public bool MustChangePassword
		{
			get { return this._MustChangePassword; }

			set
			{
				if (this._MustChangePassword != value)
				{
				
                    this.OnMustChangePasswordChanging(value);
					this.SendPropertyChanging();
					this._MustChangePassword = value;
					this.SendPropertyChanged("MustChangePassword");
					this.OnMustChangePasswordChanged();
				}

			}

		}

		
		[Column(Name="Host", UpdateCheck=UpdateCheck.Never, Storage="_Host", DbType="varchar(100)")]
		public string Host
		{
			get { return this._Host; }

			set
			{
				if (this._Host != value)
				{
				
                    this.OnHostChanging(value);
					this.SendPropertyChanging();
					this._Host = value;
					this.SendPropertyChanged("Host");
					this.OnHostChanged();
				}

			}

		}

		
		[Column(Name="TempPassword", UpdateCheck=UpdateCheck.Never, Storage="_TempPassword", DbType="varchar(128)")]
		public string TempPassword
		{
			get { return this._TempPassword; }

			set
			{
				if (this._TempPassword != value)
				{
				
                    this.OnTempPasswordChanging(value);
					this.SendPropertyChanging();
					this._TempPassword = value;
					this.SendPropertyChanged("TempPassword");
					this.OnTempPasswordChanged();
				}

			}

		}

		
		[Column(Name="NotifyAll", UpdateCheck=UpdateCheck.Never, Storage="_NotifyAll", DbType="bit")]
		public bool? NotifyAll
		{
			get { return this._NotifyAll; }

			set
			{
				if (this._NotifyAll != value)
				{
				
                    this.OnNotifyAllChanging(value);
					this.SendPropertyChanging();
					this._NotifyAll = value;
					this.SendPropertyChanged("NotifyAll");
					this.OnNotifyAllChanged();
				}

			}

		}

		
		[Column(Name="FirstName", UpdateCheck=UpdateCheck.Never, Storage="_FirstName", DbType="varchar(50)")]
		public string FirstName
		{
			get { return this._FirstName; }

			set
			{
				if (this._FirstName != value)
				{
				
                    this.OnFirstNameChanging(value);
					this.SendPropertyChanging();
					this._FirstName = value;
					this.SendPropertyChanged("FirstName");
					this.OnFirstNameChanged();
				}

			}

		}

		
		[Column(Name="LastName", UpdateCheck=UpdateCheck.Never, Storage="_LastName", DbType="varchar(50)")]
		public string LastName
		{
			get { return this._LastName; }

			set
			{
				if (this._LastName != value)
				{
				
                    this.OnLastNameChanging(value);
					this.SendPropertyChanging();
					this._LastName = value;
					this.SendPropertyChanged("LastName");
					this.OnLastNameChanged();
				}

			}

		}

		
		[Column(Name="BirthDay", UpdateCheck=UpdateCheck.Never, Storage="_BirthDay", DbType="datetime")]
		public DateTime? BirthDay
		{
			get { return this._BirthDay; }

			set
			{
				if (this._BirthDay != value)
				{
				
                    this.OnBirthDayChanging(value);
					this.SendPropertyChanging();
					this._BirthDay = value;
					this.SendPropertyChanged("BirthDay");
					this.OnBirthDayChanged();
				}

			}

		}

		
		[Column(Name="DefaultGroup", UpdateCheck=UpdateCheck.Never, Storage="_DefaultGroup", DbType="varchar(50)")]
		public string DefaultGroup
		{
			get { return this._DefaultGroup; }

			set
			{
				if (this._DefaultGroup != value)
				{
				
                    this.OnDefaultGroupChanging(value);
					this.SendPropertyChanging();
					this._DefaultGroup = value;
					this.SendPropertyChanged("DefaultGroup");
					this.OnDefaultGroupChanged();
				}

			}

		}

		
		[Column(Name="NotifyEnabled", UpdateCheck=UpdateCheck.Never, Storage="_NotifyEnabled", DbType="bit")]
		public bool? NotifyEnabled
		{
			get { return this._NotifyEnabled; }

			set
			{
				if (this._NotifyEnabled != value)
				{
				
                    this.OnNotifyEnabledChanging(value);
					this.SendPropertyChanging();
					this._NotifyEnabled = value;
					this.SendPropertyChanged("NotifyEnabled");
					this.OnNotifyEnabledChanged();
				}

			}

		}

		
		[Column(Name="ForceLogin", UpdateCheck=UpdateCheck.Never, Storage="_ForceLogin", DbType="bit")]
		public bool? ForceLogin
		{
			get { return this._ForceLogin; }

			set
			{
				if (this._ForceLogin != value)
				{
				
                    this.OnForceLoginChanging(value);
					this.SendPropertyChanging();
					this._ForceLogin = value;
					this.SendPropertyChanged("ForceLogin");
					this.OnForceLoginChanged();
				}

			}

		}

		
		[Column(Name="cUserId", UpdateCheck=UpdateCheck.Never, Storage="_CUserId", DbType="int")]
		public int? CUserId
		{
			get { return this._CUserId; }

			set
			{
				if (this._CUserId != value)
				{
				
                    this.OnCUserIdChanging(value);
					this.SendPropertyChanging();
					this._CUserId = value;
					this.SendPropertyChanged("CUserId");
					this.OnCUserIdChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
   		[Association(Name="CreatedPages__CreatedBy", Storage="_CreatedPages", OtherKey="CreatedById")]
   		public EntitySet< PageContent> CreatedPages
   		{
   		    get { return this._CreatedPages; }

			set	{ this._CreatedPages.Assign(value); }

   		}

		
   		[Association(Name="FK_Blog_Users", Storage="_Blogs", OtherKey="OwnerId")]
   		public EntitySet< Blog> Blogs
   		{
   		    get { return this._Blogs; }

			set	{ this._Blogs.Assign(value); }

   		}

		
   		[Association(Name="FK_BlogComment_Users", Storage="_BlogComments", OtherKey="PosterId")]
   		public EntitySet< BlogComment> BlogComments
   		{
   		    get { return this._BlogComments; }

			set	{ this._BlogComments.Assign(value); }

   		}

		
   		[Association(Name="FK_BlogNotify_Users", Storage="_BlogNotifications", OtherKey="UserId")]
   		public EntitySet< BlogNotify> BlogNotifications
   		{
   		    get { return this._BlogNotifications; }

			set	{ this._BlogNotifications.Assign(value); }

   		}

		
   		[Association(Name="FK_BlogPost_Users", Storage="_BlogPosts", OtherKey="PosterId")]
   		public EntitySet< BlogPost> BlogPosts
   		{
   		    get { return this._BlogPosts; }

			set	{ this._BlogPosts.Assign(value); }

   		}

		
   		[Association(Name="FK_Forum_Users", Storage="_Forums", OtherKey="CreatedBy")]
   		public EntitySet< Forum> Forums
   		{
   		    get { return this._Forums; }

			set	{ this._Forums.Assign(value); }

   		}

		
   		[Association(Name="FK_ForumEntry_Users", Storage="_ForumEntries", OtherKey="CreatedBy")]
   		public EntitySet< ForumEntry> ForumEntries
   		{
   		    get { return this._ForumEntries; }

			set	{ this._ForumEntries.Assign(value); }

   		}

		
   		[Association(Name="FK_ForumUserRead_Users", Storage="_ForumUserReads", OtherKey="UserId")]
   		public EntitySet< ForumUserRead> ForumUserReads
   		{
   		    get { return this._ForumUserReads; }

			set	{ this._ForumUserReads.Assign(value); }

   		}

		
   		[Association(Name="FK_PageVisit_Users", Storage="_PageVisits", OtherKey="UserId")]
   		public EntitySet< PageVisit> PageVisits
   		{
   		    get { return this._PageVisits; }

			set	{ this._PageVisits.Assign(value); }

   		}

		
   		[Association(Name="FK_PendingNotifications_Users", Storage="_PendingNotifications", OtherKey="UserId")]
   		public EntitySet< PendingNotification> PendingNotifications
   		{
   		    get { return this._PendingNotifications; }

			set	{ this._PendingNotifications.Assign(value); }

   		}

		
   		[Association(Name="FK_PodCast_Users", Storage="_PodCasts", OtherKey="UserId")]
   		public EntitySet< PodCast> PodCasts
   		{
   		    get { return this._PodCasts; }

			set	{ this._PodCasts.Assign(value); }

   		}

		
   		[Association(Name="FK_PrayerSlot_Users", Storage="_PrayerSlots", OtherKey="UserId")]
   		public EntitySet< PrayerSlot> PrayerSlots
   		{
   		    get { return this._PrayerSlots; }

			set	{ this._PrayerSlots.Assign(value); }

   		}

		
   		[Association(Name="FK_TemporaryToken_Users", Storage="_TemporaryTokens", OtherKey="CreatedBy")]
   		public EntitySet< TemporaryToken> TemporaryTokens
   		{
   		    get { return this._TemporaryTokens; }

			set	{ this._TemporaryTokens.Assign(value); }

   		}

		
   		[Association(Name="FK_UserGroupRole_Users", Storage="_UserGroupRoles", OtherKey="UserId")]
   		public EntitySet< UserGroupRole> UserGroupRoles
   		{
   		    get { return this._UserGroupRoles; }

			set	{ this._UserGroupRoles.Assign(value); }

   		}

		
   		[Association(Name="FK_UserRole_Users", Storage="_UserRoles", OtherKey="UserId")]
   		public EntitySet< UserRole> UserRoles
   		{
   		    get { return this._UserRoles; }

			set	{ this._UserRoles.Assign(value); }

   		}

		
   		[Association(Name="FK_Verse_Users", Storage="_Verses", OtherKey="CreatedBy")]
   		public EntitySet< Verse> Verses
   		{
   		    get { return this._Verses; }

			set	{ this._Verses.Assign(value); }

   		}

		
   		[Association(Name="FK_VerseCategory_Users", Storage="_VerseCategories", OtherKey="CreatedBy")]
   		public EntitySet< VerseCategory> VerseCategories
   		{
   		    get { return this._VerseCategories; }

			set	{ this._VerseCategories.Assign(value); }

   		}

		
   		[Association(Name="ModifiedPages__ModifiedBy", Storage="_ModifiedPages", OtherKey="ModifiedById")]
   		public EntitySet< PageContent> ModifiedPages
   		{
   		    get { return this._ModifiedPages; }

			set	{ this._ModifiedPages.Assign(value); }

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

   		
		private void attach_CreatedPages(PageContent entity)
		{
			this.SendPropertyChanging();
			entity.CreatedBy = this;
		}

		private void detach_CreatedPages(PageContent entity)
		{
			this.SendPropertyChanging();
			entity.CreatedBy = null;
		}

		
		private void attach_Blogs(Blog entity)
		{
			this.SendPropertyChanging();
			entity.User = this;
		}

		private void detach_Blogs(Blog entity)
		{
			this.SendPropertyChanging();
			entity.User = null;
		}

		
		private void attach_BlogComments(BlogComment entity)
		{
			this.SendPropertyChanging();
			entity.User = this;
		}

		private void detach_BlogComments(BlogComment entity)
		{
			this.SendPropertyChanging();
			entity.User = null;
		}

		
		private void attach_BlogNotifications(BlogNotify entity)
		{
			this.SendPropertyChanging();
			entity.User = this;
		}

		private void detach_BlogNotifications(BlogNotify entity)
		{
			this.SendPropertyChanging();
			entity.User = null;
		}

		
		private void attach_BlogPosts(BlogPost entity)
		{
			this.SendPropertyChanging();
			entity.User = this;
		}

		private void detach_BlogPosts(BlogPost entity)
		{
			this.SendPropertyChanging();
			entity.User = null;
		}

		
		private void attach_Forums(Forum entity)
		{
			this.SendPropertyChanging();
			entity.User = this;
		}

		private void detach_Forums(Forum entity)
		{
			this.SendPropertyChanging();
			entity.User = null;
		}

		
		private void attach_ForumEntries(ForumEntry entity)
		{
			this.SendPropertyChanging();
			entity.User = this;
		}

		private void detach_ForumEntries(ForumEntry entity)
		{
			this.SendPropertyChanging();
			entity.User = null;
		}

		
		private void attach_ForumUserReads(ForumUserRead entity)
		{
			this.SendPropertyChanging();
			entity.User = this;
		}

		private void detach_ForumUserReads(ForumUserRead entity)
		{
			this.SendPropertyChanging();
			entity.User = null;
		}

		
		private void attach_PageVisits(PageVisit entity)
		{
			this.SendPropertyChanging();
			entity.User = this;
		}

		private void detach_PageVisits(PageVisit entity)
		{
			this.SendPropertyChanging();
			entity.User = null;
		}

		
		private void attach_PendingNotifications(PendingNotification entity)
		{
			this.SendPropertyChanging();
			entity.User = this;
		}

		private void detach_PendingNotifications(PendingNotification entity)
		{
			this.SendPropertyChanging();
			entity.User = null;
		}

		
		private void attach_PodCasts(PodCast entity)
		{
			this.SendPropertyChanging();
			entity.User = this;
		}

		private void detach_PodCasts(PodCast entity)
		{
			this.SendPropertyChanging();
			entity.User = null;
		}

		
		private void attach_PrayerSlots(PrayerSlot entity)
		{
			this.SendPropertyChanging();
			entity.User = this;
		}

		private void detach_PrayerSlots(PrayerSlot entity)
		{
			this.SendPropertyChanging();
			entity.User = null;
		}

		
		private void attach_TemporaryTokens(TemporaryToken entity)
		{
			this.SendPropertyChanging();
			entity.User = this;
		}

		private void detach_TemporaryTokens(TemporaryToken entity)
		{
			this.SendPropertyChanging();
			entity.User = null;
		}

		
		private void attach_UserGroupRoles(UserGroupRole entity)
		{
			this.SendPropertyChanging();
			entity.User = this;
		}

		private void detach_UserGroupRoles(UserGroupRole entity)
		{
			this.SendPropertyChanging();
			entity.User = null;
		}

		
		private void attach_UserRoles(UserRole entity)
		{
			this.SendPropertyChanging();
			entity.User = this;
		}

		private void detach_UserRoles(UserRole entity)
		{
			this.SendPropertyChanging();
			entity.User = null;
		}

		
		private void attach_Verses(Verse entity)
		{
			this.SendPropertyChanging();
			entity.User = this;
		}

		private void detach_Verses(Verse entity)
		{
			this.SendPropertyChanging();
			entity.User = null;
		}

		
		private void attach_VerseCategories(VerseCategory entity)
		{
			this.SendPropertyChanging();
			entity.User = this;
		}

		private void detach_VerseCategories(VerseCategory entity)
		{
			this.SendPropertyChanging();
			entity.User = null;
		}

		
		private void attach_ModifiedPages(PageContent entity)
		{
			this.SendPropertyChanging();
			entity.ModifiedBy = this;
		}

		private void detach_ModifiedPages(PageContent entity)
		{
			this.SendPropertyChanging();
			entity.ModifiedBy = null;
		}

		
	}

}

