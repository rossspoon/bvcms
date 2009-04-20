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
	[Table(Name="dbo.BlogComment")]
	public partial class BlogComment : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _Id;
		
		private string _Comment;
		
		private int? _BlogPostId;
		
		private DateTime _DatePosted;
		
		private int? _PosterId;
		
   		
    	
		private EntityRef< BlogPost> _BlogPost;
		
		private EntityRef< User> _User;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnIdChanging(int value);
		partial void OnIdChanged();
		
		partial void OnCommentChanging(string value);
		partial void OnCommentChanged();
		
		partial void OnBlogPostIdChanging(int? value);
		partial void OnBlogPostIdChanged();
		
		partial void OnDatePostedChanging(DateTime value);
		partial void OnDatePostedChanged();
		
		partial void OnPosterIdChanging(int? value);
		partial void OnPosterIdChanged();
		
    #endregion
		public BlogComment()
		{
			
			
			this._BlogPost = default(EntityRef< BlogPost>); 
			
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

		
		[Column(Name="Comment", UpdateCheck=UpdateCheck.Never, Storage="_Comment", DbType="varchar NOT NULL")]
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

		
		[Column(Name="BlogPostId", UpdateCheck=UpdateCheck.Never, Storage="_BlogPostId", DbType="int")]
		public int? BlogPostId
		{
			get { return this._BlogPostId; }

			set
			{
				if (this._BlogPostId != value)
				{
				
					if (this._BlogPost.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnBlogPostIdChanging(value);
					this.SendPropertyChanging();
					this._BlogPostId = value;
					this.SendPropertyChanged("BlogPostId");
					this.OnBlogPostIdChanged();
				}

			}

		}

		
		[Column(Name="DatePosted", UpdateCheck=UpdateCheck.Never, Storage="_DatePosted", DbType="datetime NOT NULL")]
		public DateTime DatePosted
		{
			get { return this._DatePosted; }

			set
			{
				if (this._DatePosted != value)
				{
				
                    this.OnDatePostedChanging(value);
					this.SendPropertyChanging();
					this._DatePosted = value;
					this.SendPropertyChanged("DatePosted");
					this.OnDatePostedChanged();
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

		
    #endregion
        
    #region Foreign Key Tables
   		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="FK_BlogComment_BlogPost", Storage="_BlogPost", ThisKey="BlogPostId", IsForeignKey=true)]
		public BlogPost BlogPost
		{
			get { return this._BlogPost.Entity; }

			set
			{
				BlogPost previousValue = this._BlogPost.Entity;
				if (((previousValue != value) 
							|| (this._BlogPost.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._BlogPost.Entity = null;
						previousValue.BlogComments.Remove(this);
					}

					this._BlogPost.Entity = value;
					if (value != null)
					{
						value.BlogComments.Add(this);
						
						this._BlogPostId = value.Id;
						
					}

					else
					{
						
						this._BlogPostId = default(int?);
						
					}

					this.SendPropertyChanged("BlogPost");
				}

			}

		}

		
		[Association(Name="FK_BlogComment_Users", Storage="_User", ThisKey="PosterId", IsForeignKey=true)]
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
						previousValue.BlogComments.Remove(this);
					}

					this._User.Entity = value;
					if (value != null)
					{
						value.BlogComments.Add(this);
						
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

   		
	}

}

