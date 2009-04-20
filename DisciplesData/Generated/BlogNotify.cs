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
	[Table(Name="dbo.BlogNotify")]
	public partial class BlogNotify : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _BlogId;
		
		private int _UserId;
		
   		
    	
		private EntityRef< Blog> _Blog;
		
		private EntityRef< User> _User;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnBlogIdChanging(int value);
		partial void OnBlogIdChanged();
		
		partial void OnUserIdChanging(int value);
		partial void OnUserIdChanged();
		
    #endregion
		public BlogNotify()
		{
			
			
			this._Blog = default(EntityRef< Blog>); 
			
			this._User = default(EntityRef< User>); 
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="BlogId", UpdateCheck=UpdateCheck.Never, Storage="_BlogId", DbType="int NOT NULL", IsPrimaryKey=true)]
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

		
		[Column(Name="UserId", UpdateCheck=UpdateCheck.Never, Storage="_UserId", DbType="int NOT NULL", IsPrimaryKey=true)]
		public int UserId
		{
			get { return this._UserId; }

			set
			{
				if (this._UserId != value)
				{
				
					if (this._User.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnUserIdChanging(value);
					this.SendPropertyChanging();
					this._UserId = value;
					this.SendPropertyChanged("UserId");
					this.OnUserIdChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="FK_BlogNotify_Blog", Storage="_Blog", ThisKey="BlogId", IsForeignKey=true)]
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
						previousValue.BlogNotifications.Remove(this);
					}

					this._Blog.Entity = value;
					if (value != null)
					{
						value.BlogNotifications.Add(this);
						
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

		
		[Association(Name="FK_BlogNotify_Users", Storage="_User", ThisKey="UserId", IsForeignKey=true)]
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
						previousValue.BlogNotifications.Remove(this);
					}

					this._User.Entity = value;
					if (value != null)
					{
						value.BlogNotifications.Add(this);
						
						this._UserId = value.UserId;
						
					}

					else
					{
						
						this._UserId = default(int);
						
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

