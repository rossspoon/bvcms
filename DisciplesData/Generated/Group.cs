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
	[Table(Name="dbo.Group")]
	public partial class Group : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _Id;
		
		private string _Name;
		
		private int? _ContentId;
		
   		
   		private EntitySet< Blog> _Blogs;
		
   		private EntitySet< Forum> _Forums;
		
   		private EntitySet< GroupRole> _GroupRoles;
		
   		private EntitySet< Invitation> _Invitations;
		
    	
		private EntityRef< ParaContent> _WelcomeText;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnIdChanging(int value);
		partial void OnIdChanged();
		
		partial void OnNameChanging(string value);
		partial void OnNameChanged();
		
		partial void OnContentIdChanging(int? value);
		partial void OnContentIdChanged();
		
    #endregion
		public Group()
		{
			
			this._Blogs = new EntitySet< Blog>(new Action< Blog>(this.attach_Blogs), new Action< Blog>(this.detach_Blogs)); 
			
			this._Forums = new EntitySet< Forum>(new Action< Forum>(this.attach_Forums), new Action< Forum>(this.detach_Forums)); 
			
			this._GroupRoles = new EntitySet< GroupRole>(new Action< GroupRole>(this.attach_GroupRoles), new Action< GroupRole>(this.detach_GroupRoles)); 
			
			this._Invitations = new EntitySet< Invitation>(new Action< Invitation>(this.attach_Invitations), new Action< Invitation>(this.detach_Invitations)); 
			
			
			this._WelcomeText = default(EntityRef< ParaContent>); 
			
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

		
		[Column(Name="Name", UpdateCheck=UpdateCheck.Never, Storage="_Name", DbType="varchar(50)")]
		public string Name
		{
			get { return this._Name; }

			set
			{
				if (this._Name != value)
				{
				
                    this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}

			}

		}

		
		[Column(Name="ContentId", UpdateCheck=UpdateCheck.Never, Storage="_ContentId", DbType="int")]
		public int? ContentId
		{
			get { return this._ContentId; }

			set
			{
				if (this._ContentId != value)
				{
				
					if (this._WelcomeText.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnContentIdChanging(value);
					this.SendPropertyChanging();
					this._ContentId = value;
					this.SendPropertyChanged("ContentId");
					this.OnContentIdChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
   		[Association(Name="FK_Blog_Group", Storage="_Blogs", OtherKey="GroupId")]
   		public EntitySet< Blog> Blogs
   		{
   		    get { return this._Blogs; }

			set	{ this._Blogs.Assign(value); }

   		}

		
   		[Association(Name="FK_Forum_Group", Storage="_Forums", OtherKey="GroupId")]
   		public EntitySet< Forum> Forums
   		{
   		    get { return this._Forums; }

			set	{ this._Forums.Assign(value); }

   		}

		
   		[Association(Name="FK_GroupRoles_Group", Storage="_GroupRoles", OtherKey="GroupId")]
   		public EntitySet< GroupRole> GroupRoles
   		{
   		    get { return this._GroupRoles; }

			set	{ this._GroupRoles.Assign(value); }

   		}

		
   		[Association(Name="FK_Invitation_Group", Storage="_Invitations", OtherKey="GroupId")]
   		public EntitySet< Invitation> Invitations
   		{
   		    get { return this._Invitations; }

			set	{ this._Invitations.Assign(value); }

   		}

		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="Groups__WelcomeText", Storage="_WelcomeText", ThisKey="ContentId", IsForeignKey=true)]
		public ParaContent WelcomeText
		{
			get { return this._WelcomeText.Entity; }

			set
			{
				ParaContent previousValue = this._WelcomeText.Entity;
				if (((previousValue != value) 
							|| (this._WelcomeText.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._WelcomeText.Entity = null;
						previousValue.Groups.Remove(this);
					}

					this._WelcomeText.Entity = value;
					if (value != null)
					{
						value.Groups.Add(this);
						
						this._ContentId = value.ContentID;
						
					}

					else
					{
						
						this._ContentId = default(int?);
						
					}

					this.SendPropertyChanged("WelcomeText");
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

   		
		private void attach_Blogs(Blog entity)
		{
			this.SendPropertyChanging();
			entity.Group = this;
		}

		private void detach_Blogs(Blog entity)
		{
			this.SendPropertyChanging();
			entity.Group = null;
		}

		
		private void attach_Forums(Forum entity)
		{
			this.SendPropertyChanging();
			entity.Group = this;
		}

		private void detach_Forums(Forum entity)
		{
			this.SendPropertyChanging();
			entity.Group = null;
		}

		
		private void attach_GroupRoles(GroupRole entity)
		{
			this.SendPropertyChanging();
			entity.Group = this;
		}

		private void detach_GroupRoles(GroupRole entity)
		{
			this.SendPropertyChanging();
			entity.Group = null;
		}

		
		private void attach_Invitations(Invitation entity)
		{
			this.SendPropertyChanging();
			entity.Group = this;
		}

		private void detach_Invitations(Invitation entity)
		{
			this.SendPropertyChanging();
			entity.Group = null;
		}

		
	}

}

