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
	[Table(Name="dbo.BlogCategoryXref")]
	public partial class BlogCategoryXref : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _CatId;
		
		private int _BlogPostId;
		
   		
    	
		private EntityRef< BlogPost> _BlogPost;
		
		private EntityRef< BlogCategory> _BlogCategory;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnCatIdChanging(int value);
		partial void OnCatIdChanged();
		
		partial void OnBlogPostIdChanging(int value);
		partial void OnBlogPostIdChanged();
		
    #endregion
		public BlogCategoryXref()
		{
			
			
			this._BlogPost = default(EntityRef< BlogPost>); 
			
			this._BlogCategory = default(EntityRef< BlogCategory>); 
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="CatId", UpdateCheck=UpdateCheck.Never, Storage="_CatId", DbType="int NOT NULL", IsPrimaryKey=true)]
		public int CatId
		{
			get { return this._CatId; }

			set
			{
				if (this._CatId != value)
				{
				
					if (this._BlogCategory.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnCatIdChanging(value);
					this.SendPropertyChanging();
					this._CatId = value;
					this.SendPropertyChanged("CatId");
					this.OnCatIdChanged();
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
    	
		[Association(Name="FK_BlogCategoryXref_BlogPost", Storage="_BlogPost", ThisKey="BlogPostId", IsForeignKey=true)]
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
						previousValue.BlogCategoryXrefs.Remove(this);
					}

					this._BlogPost.Entity = value;
					if (value != null)
					{
						value.BlogCategoryXrefs.Add(this);
						
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

		
		[Association(Name="FK_BlogCategoryXref_Category", Storage="_BlogCategory", ThisKey="CatId", IsForeignKey=true)]
		public BlogCategory BlogCategory
		{
			get { return this._BlogCategory.Entity; }

			set
			{
				BlogCategory previousValue = this._BlogCategory.Entity;
				if (((previousValue != value) 
							|| (this._BlogCategory.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._BlogCategory.Entity = null;
						previousValue.BlogCategoryXrefs.Remove(this);
					}

					this._BlogCategory.Entity = value;
					if (value != null)
					{
						value.BlogCategoryXrefs.Add(this);
						
						this._CatId = value.Id;
						
					}

					else
					{
						
						this._CatId = default(int);
						
					}

					this.SendPropertyChanged("BlogCategory");
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

