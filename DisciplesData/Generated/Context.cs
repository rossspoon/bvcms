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
	[DatabaseAttribute(Name="Disc")]
	public partial class DiscDataContext : DataContext
	{
		private static MappingSource mappingSource = new AttributeMappingSource();
		
#region Extensibility Method Definitions
        partial void OnCreated();
		
        partial void InsertBlog(Blog instance);
        partial void UpdateBlog(Blog instance);
        partial void DeleteBlog(Blog instance);
        
        partial void InsertBlogCategory(BlogCategory instance);
        partial void UpdateBlogCategory(BlogCategory instance);
        partial void DeleteBlogCategory(BlogCategory instance);
        
        partial void InsertBlogCategoryXref(BlogCategoryXref instance);
        partial void UpdateBlogCategoryXref(BlogCategoryXref instance);
        partial void DeleteBlogCategoryXref(BlogCategoryXref instance);
        
        partial void InsertBlogComment(BlogComment instance);
        partial void UpdateBlogComment(BlogComment instance);
        partial void DeleteBlogComment(BlogComment instance);
        
        partial void InsertBlogNotify(BlogNotify instance);
        partial void UpdateBlogNotify(BlogNotify instance);
        partial void DeleteBlogNotify(BlogNotify instance);
        
        partial void InsertBlogPost(BlogPost instance);
        partial void UpdateBlogPost(BlogPost instance);
        partial void DeleteBlogPost(BlogPost instance);
        
        partial void InsertForum(Forum instance);
        partial void UpdateForum(Forum instance);
        partial void DeleteForum(Forum instance);
        
        partial void InsertForumEntry(ForumEntry instance);
        partial void UpdateForumEntry(ForumEntry instance);
        partial void DeleteForumEntry(ForumEntry instance);
        
        partial void InsertForumNotify(ForumNotify instance);
        partial void UpdateForumNotify(ForumNotify instance);
        partial void DeleteForumNotify(ForumNotify instance);
        
        partial void InsertForumUserRead(ForumUserRead instance);
        partial void UpdateForumUserRead(ForumUserRead instance);
        partial void DeleteForumUserRead(ForumUserRead instance);
        
        partial void InsertGroup(Group instance);
        partial void UpdateGroup(Group instance);
        partial void DeleteGroup(Group instance);
        
        partial void InsertGroupRole(GroupRole instance);
        partial void UpdateGroupRole(GroupRole instance);
        partial void DeleteGroupRole(GroupRole instance);
        
        partial void InsertInvitation(Invitation instance);
        partial void UpdateInvitation(Invitation instance);
        partial void DeleteInvitation(Invitation instance);
        
        partial void InsertOtherNotify(OtherNotify instance);
        partial void UpdateOtherNotify(OtherNotify instance);
        partial void DeleteOtherNotify(OtherNotify instance);
        
        partial void InsertPageContent(PageContent instance);
        partial void UpdatePageContent(PageContent instance);
        partial void DeletePageContent(PageContent instance);
        
        partial void InsertPageVisit(PageVisit instance);
        partial void UpdatePageVisit(PageVisit instance);
        partial void DeletePageVisit(PageVisit instance);
        
        partial void InsertParaContent(ParaContent instance);
        partial void UpdateParaContent(ParaContent instance);
        partial void DeleteParaContent(ParaContent instance);
        
        partial void InsertPendingNotification(PendingNotification instance);
        partial void UpdatePendingNotification(PendingNotification instance);
        partial void DeletePendingNotification(PendingNotification instance);
        
        partial void InsertPodCast(PodCast instance);
        partial void UpdatePodCast(PodCast instance);
        partial void DeletePodCast(PodCast instance);
        
        partial void InsertPrayerSlot(PrayerSlot instance);
        partial void UpdatePrayerSlot(PrayerSlot instance);
        partial void DeletePrayerSlot(PrayerSlot instance);
        
        partial void InsertReadPlan(ReadPlan instance);
        partial void UpdateReadPlan(ReadPlan instance);
        partial void DeleteReadPlan(ReadPlan instance);
        
        partial void InsertRole(Role instance);
        partial void UpdateRole(Role instance);
        partial void DeleteRole(Role instance);
        
        partial void InsertTemporaryToken(TemporaryToken instance);
        partial void UpdateTemporaryToken(TemporaryToken instance);
        partial void DeleteTemporaryToken(TemporaryToken instance);
        
        partial void InsertUploadAuthenticationXref(UploadAuthenticationXref instance);
        partial void UpdateUploadAuthenticationXref(UploadAuthenticationXref instance);
        partial void DeleteUploadAuthenticationXref(UploadAuthenticationXref instance);
        
        partial void InsertUserGroupRole(UserGroupRole instance);
        partial void UpdateUserGroupRole(UserGroupRole instance);
        partial void DeleteUserGroupRole(UserGroupRole instance);
        
        partial void InsertUserRole(UserRole instance);
        partial void UpdateUserRole(UserRole instance);
        partial void DeleteUserRole(UserRole instance);
        
        partial void InsertUser(User instance);
        partial void UpdateUser(User instance);
        partial void DeleteUser(User instance);
        
        partial void InsertVerse(Verse instance);
        partial void UpdateVerse(Verse instance);
        partial void DeleteVerse(Verse instance);
        
        partial void InsertVerseCategory(VerseCategory instance);
        partial void UpdateVerseCategory(VerseCategory instance);
        partial void DeleteVerseCategory(VerseCategory instance);
        
        partial void InsertVerseCategoryXref(VerseCategoryXref instance);
        partial void UpdateVerseCategoryXref(VerseCategoryXref instance);
        partial void DeleteVerseCategoryXref(VerseCategoryXref instance);
        
        partial void InsertWord(Word instance);
        partial void UpdateWord(Word instance);
        partial void DeleteWord(Word instance);
        
#endregion
		
		public DiscDataContext() : 
				base(System.Configuration.ConfigurationManager.ConnectionStrings["Disc"].ConnectionString, mappingSource)
		{
			OnCreated();
		}

		
		public DiscDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}

		
		public DiscDataContext(IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}

		
		public DiscDataContext(string connection, MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}

		
		public DiscDataContext(IDbConnection connection, MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}

    #region Tables
		
		public Table< Blog> Blogs
		{
			get	{ return this.GetTable< Blog>(); }

		}

		public Table< BlogCategory> BlogCategories
		{
			get	{ return this.GetTable< BlogCategory>(); }

		}

		public Table< BlogCategoryXref> BlogCategoryXrefs
		{
			get	{ return this.GetTable< BlogCategoryXref>(); }

		}

		public Table< BlogComment> BlogComments
		{
			get	{ return this.GetTable< BlogComment>(); }

		}

		public Table< BlogNotify> BlogNotifications
		{
			get	{ return this.GetTable< BlogNotify>(); }

		}

		public Table< BlogPost> BlogPosts
		{
			get	{ return this.GetTable< BlogPost>(); }

		}

		public Table< Forum> Forums
		{
			get	{ return this.GetTable< Forum>(); }

		}

		public Table< ForumEntry> ForumEntries
		{
			get	{ return this.GetTable< ForumEntry>(); }

		}

		public Table< ForumNotify> ForumNotifications
		{
			get	{ return this.GetTable< ForumNotify>(); }

		}

		public Table< ForumUserRead> ForumUserReads
		{
			get	{ return this.GetTable< ForumUserRead>(); }

		}

		public Table< Group> Groups
		{
			get	{ return this.GetTable< Group>(); }

		}

		public Table< GroupRole> GroupRoles
		{
			get	{ return this.GetTable< GroupRole>(); }

		}

		public Table< Invitation> Invitations
		{
			get	{ return this.GetTable< Invitation>(); }

		}

		public Table< OtherNotify> OtherNotifications
		{
			get	{ return this.GetTable< OtherNotify>(); }

		}

		public Table< PageContent> PageContents
		{
			get	{ return this.GetTable< PageContent>(); }

		}

		public Table< PageVisit> PageVisits
		{
			get	{ return this.GetTable< PageVisit>(); }

		}

		public Table< ParaContent> ParaContents
		{
			get	{ return this.GetTable< ParaContent>(); }

		}

		public Table< PendingNotification> PendingNotifications
		{
			get	{ return this.GetTable< PendingNotification>(); }

		}

		public Table< PodCast> PodCasts
		{
			get	{ return this.GetTable< PodCast>(); }

		}

		public Table< PrayerSlot> PrayerSlots
		{
			get	{ return this.GetTable< PrayerSlot>(); }

		}

		public Table< ReadPlan> ReadPlans
		{
			get	{ return this.GetTable< ReadPlan>(); }

		}

		public Table< Role> Roles
		{
			get	{ return this.GetTable< Role>(); }

		}

		public Table< TemporaryToken> TemporaryTokens
		{
			get	{ return this.GetTable< TemporaryToken>(); }

		}

		public Table< UploadAuthenticationXref> UploadAuthenticationXrefs
		{
			get	{ return this.GetTable< UploadAuthenticationXref>(); }

		}

		public Table< UserGroupRole> UserGroupRoles
		{
			get	{ return this.GetTable< UserGroupRole>(); }

		}

		public Table< UserRole> UserRoles
		{
			get	{ return this.GetTable< UserRole>(); }

		}

		public Table< User> Users
		{
			get	{ return this.GetTable< User>(); }

		}

		public Table< Verse> Verses
		{
			get	{ return this.GetTable< Verse>(); }

		}

		public Table< VerseCategory> VerseCategories
		{
			get	{ return this.GetTable< VerseCategory>(); }

		}

		public Table< VerseCategoryXref> VerseCategoryXrefs
		{
			get	{ return this.GetTable< VerseCategoryXref>(); }

		}

		public Table< Word> Words
		{
			get	{ return this.GetTable< Word>(); }

		}

	#endregion
	#region Views
		
	    public Table< View.BlogCategoriesView> ViewBlogCategoriesViews
	    {
		    get { return this.GetTable< View.BlogCategoriesView>(); }

	    }

	    public Table< View.PodcastSummary> ViewPodcastSummaries
	    {
		    get { return this.GetTable< View.PodcastSummary>(); }

	    }

	    public Table< View.UserList> ViewUserLists
	    {
		    get { return this.GetTable< View.UserList>(); }

	    }

	    public Table< View.VerseCategoriesView> ViewVerseCategoriesViews
	    {
		    get { return this.GetTable< View.VerseCategoriesView>(); }

	    }

	    public Table< View.VerseSummary> ViewVerseSummaries
	    {
		    get { return this.GetTable< View.VerseSummary>(); }

	    }

	    public Table< View.ViewPageVisit> ViewViewPageVisits
	    {
		    get { return this.GetTable< View.ViewPageVisit>(); }

	    }

    #endregion
	#region Table Functions
		
		[Function(Name="dbo.VerseSummaryForCategory2", IsComposable = true)]
		public IQueryable< View.VerseSummaryForCategory2 > VerseSummaryForCategory2(
            [Parameter(DbType="int")] int? catid
            )
		{
			return this.CreateMethodCallQuery< View.VerseSummaryForCategory2>(this, 
			    ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                catid
                );
		}

    #endregion
	#region Scalar Functions
		
		[Function(Name="dbo.DayOfYear", IsComposable = true)]
		[return: Parameter(DbType = "int")]
		public int? DayOfYear(
            [Parameter(Name = "DateX", DbType="datetime")] DateTime? DateX
            )
		{
			return ((Int32)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                DateX
                ).ReturnValue));
		}

		[Function(Name="dbo.fn_diagramobjects", IsComposable = true)]
		[return: Parameter(DbType = "int")]
		public int? FnDiagramobjects(
            )
		{
			return ((Int32)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod()))
                ).ReturnValue));
		}

		[Function(Name="dbo.GetProfile", IsComposable = true)]
		[return: Parameter(DbType = "varchar")]
		public string GetProfile(
            [Parameter(Name = "Username", DbType="varchar")] string Username,
            [Parameter(Name = "PropertyName", DbType="varchar")] string PropertyName
            )
		{
			return ((String)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                Username,
                PropertyName
                ).ReturnValue));
		}

		[Function(Name="dbo.VersePos", IsComposable = true)]
		[return: Parameter(DbType = "int")]
		public int? VersePos(
            [Parameter(Name = "id", DbType="int")] int? id
            )
		{
			return ((Int32)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                id
                ).ReturnValue));
		}

		[Function(Name="dbo.VerseInCategory", IsComposable = true)]
		[return: Parameter(DbType = "bit")]
		public bool? VerseInCategory(
            [Parameter(Name = "vid", DbType="int")] int? vid,
            [Parameter(Name = "catid", DbType="int")] int? catid
            )
		{
			return ((Boolean)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                vid,
                catid
                ).ReturnValue));
		}

    #endregion
	#region Stored Procedures
		
		[Function(Name="dbo.ForumNewEntry")]
		public ISingleResult< ForumEntry> ForumNewEntry(
            [Parameter(Name = "forumid", DbType="int")] int? forumid,
            [Parameter(Name = "replytoid", DbType="int")] int? replytoid,
            [Parameter(Name = "title", DbType="nvarchar")] string title,
            [Parameter(Name = "entry", DbType="text")] string entry,
            [Parameter(Name = "created", DbType="datetime")] DateTime? created,
            [Parameter(Name = "createdby", DbType="nvarchar")] string createdby
            )
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                forumid,
                replytoid,
                title,
                entry,
                created,
                createdby
			);
			return ((ISingleResult< ForumEntry>)(result.ReturnValue));
		}

    #endregion
   }

}

