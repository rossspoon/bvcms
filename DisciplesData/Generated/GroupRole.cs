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
	[Table(Name="dbo.GroupRoles")]
	public partial class GroupRole : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private string _RoleName;
		
		private int _RoleId;
		
		private int? _GroupId;
		
   		
   		private EntitySet< UserGroupRole> _UserGroupRoles;
		
    	
		private EntityRef< Group> _Group;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnRoleNameChanging(string value);
		partial void OnRoleNameChanged();
		
		partial void OnRoleIdChanging(int value);
		partial void OnRoleIdChanged();
		
		partial void OnGroupIdChanging(int? value);
		partial void OnGroupIdChanged();
		
    #endregion
		public GroupRole()
		{
			
			this._UserGroupRoles = new EntitySet< UserGroupRole>(new Action< UserGroupRole>(this.attach_UserGroupRoles), new Action< UserGroupRole>(this.detach_UserGroupRoles)); 
			
			
			this._Group = default(EntityRef< Group>); 
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="RoleName", UpdateCheck=UpdateCheck.Never, Storage="_RoleName", DbType="varchar(50)")]
		public string RoleName
		{
			get { return this._RoleName; }

			set
			{
				if (this._RoleName != value)
				{
				
                    this.OnRoleNameChanging(value);
					this.SendPropertyChanging();
					this._RoleName = value;
					this.SendPropertyChanged("RoleName");
					this.OnRoleNameChanged();
				}

			}

		}

		
		[Column(Name="RoleId", UpdateCheck=UpdateCheck.Never, Storage="_RoleId", AutoSync=AutoSync.OnInsert, DbType="int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int RoleId
		{
			get { return this._RoleId; }

			set
			{
				if (this._RoleId != value)
				{
				
                    this.OnRoleIdChanging(value);
					this.SendPropertyChanging();
					this._RoleId = value;
					this.SendPropertyChanged("RoleId");
					this.OnRoleIdChanged();
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

		
    #endregion
        
    #region Foreign Key Tables
   		
   		[Association(Name="FK_UserGroupRole_GroupRoles", Storage="_UserGroupRoles", OtherKey="RoleId")]
   		public EntitySet< UserGroupRole> UserGroupRoles
   		{
   		    get { return this._UserGroupRoles; }

			set	{ this._UserGroupRoles.Assign(value); }

   		}

		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="FK_GroupRoles_Group", Storage="_Group", ThisKey="GroupId", IsForeignKey=true)]
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
						previousValue.GroupRoles.Remove(this);
					}

					this._Group.Entity = value;
					if (value != null)
					{
						value.GroupRoles.Add(this);
						
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

   		
		private void attach_UserGroupRoles(UserGroupRole entity)
		{
			this.SendPropertyChanging();
			entity.GroupRole = this;
		}

		private void detach_UserGroupRoles(UserGroupRole entity)
		{
			this.SendPropertyChanging();
			entity.GroupRole = null;
		}

		
	}

}

