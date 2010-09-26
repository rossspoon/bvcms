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
	[Table(Name="dbo.BadET")]
	public partial class BadET : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _Id;
		
		private int _PeopleId;
		
		private int? _OrgId;
		
		private int _TranId;
		
		private int _Flag;
		
		private bool? _Status;
		
   		
    	
		private EntityRef< Organization> _Organization;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnIdChanging(int value);
		partial void OnIdChanged();
		
		partial void OnPeopleIdChanging(int value);
		partial void OnPeopleIdChanged();
		
		partial void OnOrgIdChanging(int? value);
		partial void OnOrgIdChanged();
		
		partial void OnTranIdChanging(int value);
		partial void OnTranIdChanged();
		
		partial void OnFlagChanging(int value);
		partial void OnFlagChanged();
		
		partial void OnStatusChanging(bool? value);
		partial void OnStatusChanged();
		
    #endregion
		public BadET()
		{
			
			
			this._Organization = default(EntityRef< Organization>); 
			
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

		
		[Column(Name="PeopleId", UpdateCheck=UpdateCheck.Never, Storage="_PeopleId", DbType="int NOT NULL")]
		public int PeopleId
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

		
		[Column(Name="OrgId", UpdateCheck=UpdateCheck.Never, Storage="_OrgId", DbType="int")]
		public int? OrgId
		{
			get { return this._OrgId; }

			set
			{
				if (this._OrgId != value)
				{
				
					if (this._Organization.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnOrgIdChanging(value);
					this.SendPropertyChanging();
					this._OrgId = value;
					this.SendPropertyChanged("OrgId");
					this.OnOrgIdChanged();
				}

			}

		}

		
		[Column(Name="TranId", UpdateCheck=UpdateCheck.Never, Storage="_TranId", DbType="int NOT NULL")]
		public int TranId
		{
			get { return this._TranId; }

			set
			{
				if (this._TranId != value)
				{
				
                    this.OnTranIdChanging(value);
					this.SendPropertyChanging();
					this._TranId = value;
					this.SendPropertyChanged("TranId");
					this.OnTranIdChanged();
				}

			}

		}

		
		[Column(Name="Flag", UpdateCheck=UpdateCheck.Never, Storage="_Flag", DbType="int NOT NULL")]
		public int Flag
		{
			get { return this._Flag; }

			set
			{
				if (this._Flag != value)
				{
				
                    this.OnFlagChanging(value);
					this.SendPropertyChanging();
					this._Flag = value;
					this.SendPropertyChanged("Flag");
					this.OnFlagChanged();
				}

			}

		}

		
		[Column(Name="Status", UpdateCheck=UpdateCheck.Never, Storage="_Status", DbType="bit")]
		public bool? Status
		{
			get { return this._Status; }

			set
			{
				if (this._Status != value)
				{
				
                    this.OnStatusChanging(value);
					this.SendPropertyChanging();
					this._Status = value;
					this.SendPropertyChanged("Status");
					this.OnStatusChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="FK_BadET_Organizations", Storage="_Organization", ThisKey="OrgId", IsForeignKey=true)]
		public Organization Organization
		{
			get { return this._Organization.Entity; }

			set
			{
				Organization previousValue = this._Organization.Entity;
				if (((previousValue != value) 
							|| (this._Organization.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._Organization.Entity = null;
						previousValue.BadETs.Remove(this);
					}

					this._Organization.Entity = value;
					if (value != null)
					{
						value.BadETs.Add(this);
						
						this._OrgId = value.OrganizationId;
						
					}

					else
					{
						
						this._OrgId = default(int?);
						
					}

					this.SendPropertyChanged("Organization");
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

