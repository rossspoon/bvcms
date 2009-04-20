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
	[Table(Name="dbo.BlogCategory")]
	public partial class BlogCategory : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private string _Category;
		
		private int _BlogPostId;
		
   		
    	
		private EntityRef< BlogPost> _BlogPost;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnCategoryChanging(string value);
		partial void OnCategoryChanged();
		
		partial void OnBlogPostIdChanging(int value);
		partial void OnBlogPostIdChanged();
		
    #endregion
		public BlogCategory()
		{
			
			
			this._BlogPost = default(EntityRef< BlogPost>); 
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="Category", UpdateCheck=UpdateCheck.Never, Storage="_Category", DbType="nvarchar(50) NOT NULL", IsPrimaryKey=true)]
		public string Category
		{
			get { return this._Category; }

			set
			{
				if (this._Category != value)
				{
				
                    this.OnCategoryChanging(value);
					this.SendPropertyChanging();
					this._Category = value;
					this.SendPropertyChanged("Category");
					this.OnCategoryChanged();
				}

			}

		}

		
		[Column(Name="BlogPostId", UpdateCheck=UpdateCheck.Never, Storage="_BlogPostId", DbType="int NOT NULL", IsPrimaryKey=true)]
		public int BlogPostId
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

		
    #endregion
        
    #region Foreign Key Tables
   		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="FK_BlogCategory_Blog", Storage="_BlogPost", ThisKey="BlogPostId", IsForeignKey=true)]
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
						previousValue.BlogCategories.Remove(this);
					}

					this._BlogPost.Entity = value;
					if (value != null)
					{
						value.BlogCategories.Add(this);
						
						this._BlogPostId = value.Id;
						
					}

					else
					{
						
						this._BlogPostId = default(int);
						
					}

					this.SendPropertyChanged("BlogPost");
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

