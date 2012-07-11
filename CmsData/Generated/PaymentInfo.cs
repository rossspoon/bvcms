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
	[Table(Name="dbo.PaymentInfo")]
	public partial class PaymentInfo : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _PeopleId;
		
		private int? _AuNetCustId;
		
		private int? _AuNetCustPayId;
		
		private Guid? _SageBankGuid;
		
		private string _Ccv;
		
		private Guid? _SageCardGuid;
		
		private string _MaskedAccount;
		
		private string _MaskedCard;
		
		private string _Expires;
		
		private bool? _Testing;
		
		private string _PreferredGivingType;
		
		private string _PreferredPaymentType;
		
		private string _Routing;
		
   		
    	
		private EntityRef< Person> _Person;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnPeopleIdChanging(int value);
		partial void OnPeopleIdChanged();
		
		partial void OnAuNetCustIdChanging(int? value);
		partial void OnAuNetCustIdChanged();
		
		partial void OnAuNetCustPayIdChanging(int? value);
		partial void OnAuNetCustPayIdChanged();
		
		partial void OnSageBankGuidChanging(Guid? value);
		partial void OnSageBankGuidChanged();
		
		partial void OnCcvChanging(string value);
		partial void OnCcvChanged();
		
		partial void OnSageCardGuidChanging(Guid? value);
		partial void OnSageCardGuidChanged();
		
		partial void OnMaskedAccountChanging(string value);
		partial void OnMaskedAccountChanged();
		
		partial void OnMaskedCardChanging(string value);
		partial void OnMaskedCardChanged();
		
		partial void OnExpiresChanging(string value);
		partial void OnExpiresChanged();
		
		partial void OnTestingChanging(bool? value);
		partial void OnTestingChanged();
		
		partial void OnPreferredGivingTypeChanging(string value);
		partial void OnPreferredGivingTypeChanged();
		
		partial void OnPreferredPaymentTypeChanging(string value);
		partial void OnPreferredPaymentTypeChanged();
		
		partial void OnRoutingChanging(string value);
		partial void OnRoutingChanged();
		
    #endregion
		public PaymentInfo()
		{
			
			
			this._Person = default(EntityRef< Person>); 
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="PeopleId", UpdateCheck=UpdateCheck.Never, Storage="_PeopleId", DbType="int NOT NULL", IsPrimaryKey=true)]
		public int PeopleId
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

		
		[Column(Name="AuNetCustId", UpdateCheck=UpdateCheck.Never, Storage="_AuNetCustId", DbType="int")]
		public int? AuNetCustId
		{
			get { return this._AuNetCustId; }

			set
			{
				if (this._AuNetCustId != value)
				{
				
                    this.OnAuNetCustIdChanging(value);
					this.SendPropertyChanging();
					this._AuNetCustId = value;
					this.SendPropertyChanged("AuNetCustId");
					this.OnAuNetCustIdChanged();
				}

			}

		}

		
		[Column(Name="AuNetCustPayId", UpdateCheck=UpdateCheck.Never, Storage="_AuNetCustPayId", DbType="int")]
		public int? AuNetCustPayId
		{
			get { return this._AuNetCustPayId; }

			set
			{
				if (this._AuNetCustPayId != value)
				{
				
                    this.OnAuNetCustPayIdChanging(value);
					this.SendPropertyChanging();
					this._AuNetCustPayId = value;
					this.SendPropertyChanged("AuNetCustPayId");
					this.OnAuNetCustPayIdChanged();
				}

			}

		}

		
		[Column(Name="SageBankGuid", UpdateCheck=UpdateCheck.Never, Storage="_SageBankGuid", DbType="uniqueidentifier")]
		public Guid? SageBankGuid
		{
			get { return this._SageBankGuid; }

			set
			{
				if (this._SageBankGuid != value)
				{
				
                    this.OnSageBankGuidChanging(value);
					this.SendPropertyChanging();
					this._SageBankGuid = value;
					this.SendPropertyChanged("SageBankGuid");
					this.OnSageBankGuidChanged();
				}

			}

		}

		
		[Column(Name="ccv", UpdateCheck=UpdateCheck.Never, Storage="_Ccv", DbType="varchar(5)")]
		public string Ccv
		{
			get { return this._Ccv; }

			set
			{
				if (this._Ccv != value)
				{
				
                    this.OnCcvChanging(value);
					this.SendPropertyChanging();
					this._Ccv = value;
					this.SendPropertyChanged("Ccv");
					this.OnCcvChanged();
				}

			}

		}

		
		[Column(Name="SageCardGuid", UpdateCheck=UpdateCheck.Never, Storage="_SageCardGuid", DbType="uniqueidentifier")]
		public Guid? SageCardGuid
		{
			get { return this._SageCardGuid; }

			set
			{
				if (this._SageCardGuid != value)
				{
				
                    this.OnSageCardGuidChanging(value);
					this.SendPropertyChanging();
					this._SageCardGuid = value;
					this.SendPropertyChanged("SageCardGuid");
					this.OnSageCardGuidChanged();
				}

			}

		}

		
		[Column(Name="MaskedAccount", UpdateCheck=UpdateCheck.Never, Storage="_MaskedAccount", DbType="varchar(30)")]
		public string MaskedAccount
		{
			get { return this._MaskedAccount; }

			set
			{
				if (this._MaskedAccount != value)
				{
				
                    this.OnMaskedAccountChanging(value);
					this.SendPropertyChanging();
					this._MaskedAccount = value;
					this.SendPropertyChanged("MaskedAccount");
					this.OnMaskedAccountChanged();
				}

			}

		}

		
		[Column(Name="MaskedCard", UpdateCheck=UpdateCheck.Never, Storage="_MaskedCard", DbType="varchar(30)")]
		public string MaskedCard
		{
			get { return this._MaskedCard; }

			set
			{
				if (this._MaskedCard != value)
				{
				
                    this.OnMaskedCardChanging(value);
					this.SendPropertyChanging();
					this._MaskedCard = value;
					this.SendPropertyChanged("MaskedCard");
					this.OnMaskedCardChanged();
				}

			}

		}

		
		[Column(Name="Expires", UpdateCheck=UpdateCheck.Never, Storage="_Expires", DbType="varchar(10)")]
		public string Expires
		{
			get { return this._Expires; }

			set
			{
				if (this._Expires != value)
				{
				
                    this.OnExpiresChanging(value);
					this.SendPropertyChanging();
					this._Expires = value;
					this.SendPropertyChanged("Expires");
					this.OnExpiresChanged();
				}

			}

		}

		
		[Column(Name="testing", UpdateCheck=UpdateCheck.Never, Storage="_Testing", DbType="bit")]
		public bool? Testing
		{
			get { return this._Testing; }

			set
			{
				if (this._Testing != value)
				{
				
                    this.OnTestingChanging(value);
					this.SendPropertyChanging();
					this._Testing = value;
					this.SendPropertyChanged("Testing");
					this.OnTestingChanged();
				}

			}

		}

		
		[Column(Name="PreferredGivingType", UpdateCheck=UpdateCheck.Never, Storage="_PreferredGivingType", DbType="varchar(2)")]
		public string PreferredGivingType
		{
			get { return this._PreferredGivingType; }

			set
			{
				if (this._PreferredGivingType != value)
				{
				
                    this.OnPreferredGivingTypeChanging(value);
					this.SendPropertyChanging();
					this._PreferredGivingType = value;
					this.SendPropertyChanged("PreferredGivingType");
					this.OnPreferredGivingTypeChanged();
				}

			}

		}

		
		[Column(Name="PreferredPaymentType", UpdateCheck=UpdateCheck.Never, Storage="_PreferredPaymentType", DbType="varchar(2)")]
		public string PreferredPaymentType
		{
			get { return this._PreferredPaymentType; }

			set
			{
				if (this._PreferredPaymentType != value)
				{
				
                    this.OnPreferredPaymentTypeChanging(value);
					this.SendPropertyChanging();
					this._PreferredPaymentType = value;
					this.SendPropertyChanged("PreferredPaymentType");
					this.OnPreferredPaymentTypeChanged();
				}

			}

		}

		
		[Column(Name="Routing", UpdateCheck=UpdateCheck.Never, Storage="_Routing", DbType="varchar(10)")]
		public string Routing
		{
			get { return this._Routing; }

			set
			{
				if (this._Routing != value)
				{
				
                    this.OnRoutingChanging(value);
					this.SendPropertyChanging();
					this._Routing = value;
					this.SendPropertyChanged("Routing");
					this.OnRoutingChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="FK_PaymentInfo_People", Storage="_Person", ThisKey="PeopleId", IsForeignKey=true)]
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
						previousValue.PaymentInfos.Remove(this);
					}

					this._Person.Entity = value;
					if (value != null)
					{
						value.PaymentInfos.Add(this);
						
						this._PeopleId = value.PeopleId;
						
					}

					else
					{
						
						this._PeopleId = default(int);
						
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

