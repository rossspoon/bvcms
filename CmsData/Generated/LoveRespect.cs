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
	[Table(Name="dbo.LoveRespect")]
	public partial class LoveRespect : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _Id;
		
		private DateTime _Created;
		
		private int? _HimId;
		
		private string _HisEmail;
		
		private bool? _HisEmailPreferred;
		
		private int? _HerId;
		
		private string _HerEmail;
		
		private bool? _HerEmailPreferred;
		
		private int? _OrgId;
		
		private int? _Relationship;
		
		private int? _PreferNight;
		
   		
    	
		private EntityRef< Organization> _Organization;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnIdChanging(int value);
		partial void OnIdChanged();
		
		partial void OnCreatedChanging(DateTime value);
		partial void OnCreatedChanged();
		
		partial void OnHimIdChanging(int? value);
		partial void OnHimIdChanged();
		
		partial void OnHisEmailChanging(string value);
		partial void OnHisEmailChanged();
		
		partial void OnHisEmailPreferredChanging(bool? value);
		partial void OnHisEmailPreferredChanged();
		
		partial void OnHerIdChanging(int? value);
		partial void OnHerIdChanged();
		
		partial void OnHerEmailChanging(string value);
		partial void OnHerEmailChanged();
		
		partial void OnHerEmailPreferredChanging(bool? value);
		partial void OnHerEmailPreferredChanged();
		
		partial void OnOrgIdChanging(int? value);
		partial void OnOrgIdChanged();
		
		partial void OnRelationshipChanging(int? value);
		partial void OnRelationshipChanged();
		
		partial void OnPreferNightChanging(int? value);
		partial void OnPreferNightChanged();
		
    #endregion
		public LoveRespect()
		{
			
			
			this._Organization = default(EntityRef< Organization>); 
			
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

		
		[Column(Name="Created", UpdateCheck=UpdateCheck.Never, Storage="_Created", DbType="datetime NOT NULL")]
		public DateTime Created
		{
			get { return this._Created; }

			set
			{
				if (this._Created != value)
				{
				
                    this.OnCreatedChanging(value);
					this.SendPropertyChanging();
					this._Created = value;
					this.SendPropertyChanged("Created");
					this.OnCreatedChanged();
				}

			}

		}

		
		[Column(Name="HimId", UpdateCheck=UpdateCheck.Never, Storage="_HimId", DbType="int")]
		public int? HimId
		{
			get { return this._HimId; }

			set
			{
				if (this._HimId != value)
				{
				
                    this.OnHimIdChanging(value);
					this.SendPropertyChanging();
					this._HimId = value;
					this.SendPropertyChanged("HimId");
					this.OnHimIdChanged();
				}

			}

		}

		
		[Column(Name="HisEmail", UpdateCheck=UpdateCheck.Never, Storage="_HisEmail", DbType="varchar(50)")]
		public string HisEmail
		{
			get { return this._HisEmail; }

			set
			{
				if (this._HisEmail != value)
				{
				
                    this.OnHisEmailChanging(value);
					this.SendPropertyChanging();
					this._HisEmail = value;
					this.SendPropertyChanged("HisEmail");
					this.OnHisEmailChanged();
				}

			}

		}

		
		[Column(Name="HisEmailPreferred", UpdateCheck=UpdateCheck.Never, Storage="_HisEmailPreferred", DbType="bit")]
		public bool? HisEmailPreferred
		{
			get { return this._HisEmailPreferred; }

			set
			{
				if (this._HisEmailPreferred != value)
				{
				
                    this.OnHisEmailPreferredChanging(value);
					this.SendPropertyChanging();
					this._HisEmailPreferred = value;
					this.SendPropertyChanged("HisEmailPreferred");
					this.OnHisEmailPreferredChanged();
				}

			}

		}

		
		[Column(Name="HerId", UpdateCheck=UpdateCheck.Never, Storage="_HerId", DbType="int")]
		public int? HerId
		{
			get { return this._HerId; }

			set
			{
				if (this._HerId != value)
				{
				
                    this.OnHerIdChanging(value);
					this.SendPropertyChanging();
					this._HerId = value;
					this.SendPropertyChanged("HerId");
					this.OnHerIdChanged();
				}

			}

		}

		
		[Column(Name="HerEmail", UpdateCheck=UpdateCheck.Never, Storage="_HerEmail", DbType="varchar(50)")]
		public string HerEmail
		{
			get { return this._HerEmail; }

			set
			{
				if (this._HerEmail != value)
				{
				
                    this.OnHerEmailChanging(value);
					this.SendPropertyChanging();
					this._HerEmail = value;
					this.SendPropertyChanged("HerEmail");
					this.OnHerEmailChanged();
				}

			}

		}

		
		[Column(Name="HerEmailPreferred", UpdateCheck=UpdateCheck.Never, Storage="_HerEmailPreferred", DbType="bit")]
		public bool? HerEmailPreferred
		{
			get { return this._HerEmailPreferred; }

			set
			{
				if (this._HerEmailPreferred != value)
				{
				
                    this.OnHerEmailPreferredChanging(value);
					this.SendPropertyChanging();
					this._HerEmailPreferred = value;
					this.SendPropertyChanged("HerEmailPreferred");
					this.OnHerEmailPreferredChanged();
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

		
		[Column(Name="Relationship", UpdateCheck=UpdateCheck.Never, Storage="_Relationship", DbType="int")]
		public int? Relationship
		{
			get { return this._Relationship; }

			set
			{
				if (this._Relationship != value)
				{
				
                    this.OnRelationshipChanging(value);
					this.SendPropertyChanging();
					this._Relationship = value;
					this.SendPropertyChanged("Relationship");
					this.OnRelationshipChanged();
				}

			}

		}

		
		[Column(Name="PreferNight", UpdateCheck=UpdateCheck.Never, Storage="_PreferNight", DbType="int")]
		public int? PreferNight
		{
			get { return this._PreferNight; }

			set
			{
				if (this._PreferNight != value)
				{
				
                    this.OnPreferNightChanging(value);
					this.SendPropertyChanging();
					this._PreferNight = value;
					this.SendPropertyChanged("PreferNight");
					this.OnPreferNightChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="FK_LoveRespect_Organizations", Storage="_Organization", ThisKey="OrgId", IsForeignKey=true)]
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
						previousValue.LoveRespects.Remove(this);
					}

					this._Organization.Entity = value;
					if (value != null)
					{
						value.LoveRespects.Add(this);
						
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

