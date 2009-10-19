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
	[Table(Name="dbo.Forum")]
	public partial class Forum : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _Id;
		
		private string _Description;
		
		private DateTime? _CreatedOn;
		
		private int? _GroupId;
		
		private int? _CreatedBy;
		
   		
   		private EntitySet< ForumEntry> _ForumEntries;
		
    	
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
		
		partial void OnCreatedOnChanging(DateTime? value);
		partial void OnCreatedOnChanged();
		
		partial void OnGroupIdChanging(int? value);
		partial void OnGroupIdChanged();
		
		partial void OnCreatedByChanging(int? value);
		partial void OnCreatedByChanged();
		
    #endregion
		public Forum()
		{
			
			this._ForumEntries = new EntitySet< ForumEntry>(new Action< ForumEntry>(this.attach_ForumEntries), new Action< ForumEntry>(this.detach_ForumEntries)); 
			
			
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

		
		[Column(Name="Description", UpdateCheck=UpdateCheck.Never, Storage="_Description", DbType="nvarchar(50)")]
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
   		
   		[Association(Name="FK_ForumEntry_Forum", Storage="_ForumEntries", OtherKey="ForumId")]
   		public EntitySet< ForumEntry> ForumEntries
   		{
   		    get { return this._ForumEntries; }

			set	{ this._ForumEntries.Assign(value); }

   		}

		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="FK_Forum_Group", Storage="_Group", ThisKey="GroupId", IsForeignKey=true)]
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
						previousValue.Forums.Remove(this);
					}

					this._Group.Entity = value;
					if (value != null)
					{
						value.Forums.Add(this);
						
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

		
		[Association(Name="FK_Forum_Users", Storage="_User", ThisKey="CreatedBy", IsForeignKey=true)]
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
						previousValue.Forums.Remove(this);
					}

					this._User.Entity = value;
					if (value != null)
					{
						value.Forums.Add(this);
						
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

   		
		private void attach_ForumEntries(ForumEntry entity)
		{
			this.SendPropertyChanging();
			entity.Forum = this;
		}

		private void detach_ForumEntries(ForumEntry entity)
		{
			this.SendPropertyChanging();
			entity.Forum = null;
		}

		
	}

}

