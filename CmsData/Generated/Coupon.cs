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
	[Table(Name="dbo.Coupons")]
	public partial class Coupon : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private string _Id;
		
		private DateTime? _Used;
		
		private DateTime? _Canceled;
		
		private decimal? _Amount;
		
		private int? _PeopleId;
		
		private string _Name;
		
		private int? _Divid;
		
		private int? _Orgid;
		
		private DateTime _Created;
		
   		
    	
		private EntityRef< Division> _Division;
		
		private EntityRef< Organization> _Organization;
		
		private EntityRef< Person> _Person;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnIdChanging(string value);
		partial void OnIdChanged();
		
		partial void OnUsedChanging(DateTime? value);
		partial void OnUsedChanged();
		
		partial void OnCanceledChanging(DateTime? value);
		partial void OnCanceledChanged();
		
		partial void OnAmountChanging(decimal? value);
		partial void OnAmountChanged();
		
		partial void OnPeopleIdChanging(int? value);
		partial void OnPeopleIdChanged();
		
		partial void OnNameChanging(string value);
		partial void OnNameChanged();
		
		partial void OnDividChanging(int? value);
		partial void OnDividChanged();
		
		partial void OnOrgidChanging(int? value);
		partial void OnOrgidChanged();
		
		partial void OnCreatedChanging(DateTime value);
		partial void OnCreatedChanged();
		
    #endregion
		public Coupon()
		{
			
			
			this._Division = default(EntityRef< Division>); 
			
			this._Organization = default(EntityRef< Organization>); 
			
			this._Person = default(EntityRef< Person>); 
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="Id", UpdateCheck=UpdateCheck.Never, Storage="_Id", DbType="varchar(50) NOT NULL", IsPrimaryKey=true)]
		public string Id
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

		
		[Column(Name="Used", UpdateCheck=UpdateCheck.Never, Storage="_Used", DbType="datetime")]
		public DateTime? Used
		{
			get { return this._Used; }

			set
			{
				if (this._Used != value)
				{
				
                    this.OnUsedChanging(value);
					this.SendPropertyChanging();
					this._Used = value;
					this.SendPropertyChanged("Used");
					this.OnUsedChanged();
				}

			}

		}

		
		[Column(Name="Canceled", UpdateCheck=UpdateCheck.Never, Storage="_Canceled", DbType="datetime")]
		public DateTime? Canceled
		{
			get { return this._Canceled; }

			set
			{
				if (this._Canceled != value)
				{
				
                    this.OnCanceledChanging(value);
					this.SendPropertyChanging();
					this._Canceled = value;
					this.SendPropertyChanged("Canceled");
					this.OnCanceledChanged();
				}

			}

		}

		
		[Column(Name="Amount", UpdateCheck=UpdateCheck.Never, Storage="_Amount", DbType="money")]
		public decimal? Amount
		{
			get { return this._Amount; }

			set
			{
				if (this._Amount != value)
				{
				
                    this.OnAmountChanging(value);
					this.SendPropertyChanging();
					this._Amount = value;
					this.SendPropertyChanged("Amount");
					this.OnAmountChanged();
				}

			}

		}

		
		[Column(Name="PeopleId", UpdateCheck=UpdateCheck.Never, Storage="_PeopleId", DbType="int")]
		public int? PeopleId
		{
			get { return this._PeopleId; }

			set
			{
				if (this._PeopleId != value)
				{
				
					if (this._Person.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnPeopleIdChanging(value);
					this.SendPropertyChanging();
					this._PeopleId = value;
					this.SendPropertyChanged("PeopleId");
					this.OnPeopleIdChanged();
				}

			}

		}

		
		[Column(Name="Name", UpdateCheck=UpdateCheck.Never, Storage="_Name", DbType="varchar(80)")]
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

		
		[Column(Name="divid", UpdateCheck=UpdateCheck.Never, Storage="_Divid", DbType="int")]
		public int? Divid
		{
			get { return this._Divid; }

			set
			{
				if (this._Divid != value)
				{
				
					if (this._Division.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnDividChanging(value);
					this.SendPropertyChanging();
					this._Divid = value;
					this.SendPropertyChanged("Divid");
					this.OnDividChanged();
				}

			}

		}

		
		[Column(Name="orgid", UpdateCheck=UpdateCheck.Never, Storage="_Orgid", DbType="int")]
		public int? Orgid
		{
			get { return this._Orgid; }

			set
			{
				if (this._Orgid != value)
				{
				
					if (this._Organization.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnOrgidChanging(value);
					this.SendPropertyChanging();
					this._Orgid = value;
					this.SendPropertyChanged("Orgid");
					this.OnOrgidChanged();
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

		
    #endregion
        
    #region Foreign Key Tables
   		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="FK_Coupons_Division", Storage="_Division", ThisKey="Divid", IsForeignKey=true)]
		public Division Division
		{
			get { return this._Division.Entity; }

			set
			{
				Division previousValue = this._Division.Entity;
				if (((previousValue != value) 
							|| (this._Division.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._Division.Entity = null;
						previousValue.Coupons.Remove(this);
					}

					this._Division.Entity = value;
					if (value != null)
					{
						value.Coupons.Add(this);
						
						this._Divid = value.Id;
						
					}

					else
					{
						
						this._Divid = default(int?);
						
					}

					this.SendPropertyChanged("Division");
				}

			}

		}

		
		[Association(Name="FK_Coupons_Organizations", Storage="_Organization", ThisKey="Orgid", IsForeignKey=true)]
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
						previousValue.Coupons.Remove(this);
					}

					this._Organization.Entity = value;
					if (value != null)
					{
						value.Coupons.Add(this);
						
						this._Orgid = value.OrganizationId;
						
					}

					else
					{
						
						this._Orgid = default(int?);
						
					}

					this.SendPropertyChanged("Organization");
				}

			}

		}

		
		[Association(Name="FK_Coupons_People", Storage="_Person", ThisKey="PeopleId", IsForeignKey=true)]
		public Person Person
		{
			get { return this._Person.Entity; }

			set
			{
				Person previousValue = this._Person.Entity;
				if (((previousValue != value) 
							|| (this._Person.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._Person.Entity = null;
						previousValue.Coupons.Remove(this);
					}

					this._Person.Entity = value;
					if (value != null)
					{
						value.Coupons.Add(this);
						
						this._PeopleId = value.PeopleId;
						
					}

					else
					{
						
						this._PeopleId = default(int?);
						
					}

					this.SendPropertyChanged("Person");
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

