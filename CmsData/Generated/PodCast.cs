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
	[Table(Name="disc.PodCast")]
	public partial class PodCast : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _Id;
		
		private string _S3Name;
		
		private string _Title;
		
		private string _Description;
		
		private DateTime? _PubDate;
		
		private int? _Length;
		
		private int? _PostId;
		
		private int? _UserId;
		
		private int? _CUserid;
		
   		
    	
		private EntityRef< BlogPost> _BlogPost;
		
		private EntityRef< User> _User;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnIdChanging(int value);
		partial void OnIdChanged();
		
		partial void OnS3NameChanging(string value);
		partial void OnS3NameChanged();
		
		partial void OnTitleChanging(string value);
		partial void OnTitleChanged();
		
		partial void OnDescriptionChanging(string value);
		partial void OnDescriptionChanged();
		
		partial void OnPubDateChanging(DateTime? value);
		partial void OnPubDateChanged();
		
		partial void OnLengthChanging(int? value);
		partial void OnLengthChanged();
		
		partial void OnPostIdChanging(int? value);
		partial void OnPostIdChanged();
		
		partial void OnUserIdChanging(int? value);
		partial void OnUserIdChanged();
		
		partial void OnCUseridChanging(int? value);
		partial void OnCUseridChanged();
		
    #endregion
		public PodCast()
		{
			
			
			this._BlogPost = default(EntityRef< BlogPost>); 
			
			this._User = default(EntityRef< User>); 
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="id", UpdateCheck=UpdateCheck.Never, Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
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

		
		[Column(Name="S3Name", UpdateCheck=UpdateCheck.Never, Storage="_S3Name", DbType="varchar(150)")]
		public string S3Name
		{
			get { return this._S3Name; }

			set
			{
				if (this._S3Name != value)
				{
				
                    this.OnS3NameChanging(value);
					this.SendPropertyChanging();
					this._S3Name = value;
					this.SendPropertyChanged("S3Name");
					this.OnS3NameChanged();
				}

			}

		}

		
		[Column(Name="Title", UpdateCheck=UpdateCheck.Never, Storage="_Title", DbType="varchar(100)")]
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

		
		[Column(Name="Description", UpdateCheck=UpdateCheck.Never, Storage="_Description", DbType="varchar(3000)")]
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

		
		[Column(Name="pubDate", UpdateCheck=UpdateCheck.Never, Storage="_PubDate", DbType="datetime")]
		public DateTime? PubDate
		{
			get { return this._PubDate; }

			set
			{
				if (this._PubDate != value)
				{
				
                    this.OnPubDateChanging(value);
					this.SendPropertyChanging();
					this._PubDate = value;
					this.SendPropertyChanged("PubDate");
					this.OnPubDateChanged();
				}

			}

		}

		
		[Column(Name="length", UpdateCheck=UpdateCheck.Never, Storage="_Length", DbType="int")]
		public int? Length
		{
			get { return this._Length; }

			set
			{
				if (this._Length != value)
				{
				
                    this.OnLengthChanging(value);
					this.SendPropertyChanging();
					this._Length = value;
					this.SendPropertyChanged("Length");
					this.OnLengthChanged();
				}

			}

		}

		
		[Column(Name="postId", UpdateCheck=UpdateCheck.Never, Storage="_PostId", DbType="int")]
		public int? PostId
		{
			get { return this._PostId; }

			set
			{
				if (this._PostId != value)
				{
				
					if (this._BlogPost.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnPostIdChanging(value);
					this.SendPropertyChanging();
					this._PostId = value;
					this.SendPropertyChanged("PostId");
					this.OnPostIdChanged();
				}

			}

		}

		
		[Column(Name="UserId", UpdateCheck=UpdateCheck.Never, Storage="_UserId", DbType="int")]
		public int? UserId
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
   		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="FK_PodCast_BlogPost", Storage="_BlogPost", ThisKey="PostId", IsForeignKey=true)]
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
						previousValue.PodCasts.Remove(this);
					}

					this._BlogPost.Entity = value;
					if (value != null)
					{
						value.PodCasts.Add(this);
						
						this._PostId = value.Id;
						
					}

					else
					{
						
						this._PostId = default(int?);
						
					}

					this.SendPropertyChanged("BlogPost");
				}

			}

		}

		
		[Association(Name="FK_PodCast_Users", Storage="_User", ThisKey="UserId", IsForeignKey=true)]
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
						previousValue.PodCasts.Remove(this);
					}

					this._User.Entity = value;
					if (value != null)
					{
						value.PodCasts.Add(this);
						
						this._UserId = value.UserId;
						
					}

					else
					{
						
						this._UserId = default(int?);
						
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

