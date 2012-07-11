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
	[Table(Name="dbo.RecurringGiving")]
	public partial class RecurringGiving : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _PeopleId;
		
		private int? _AuNetCustId;
		
		private int? _AuNetCustPayId;
		
		private Guid? _SageBankGuid;
		
		private string _Ccv;
		
		private DateTime? _StartWhen;
		
		private string _Type;
		
		private Guid? _SageCardGuid;
		
		private string _MaskedAccount;
		
		private string _MaskedCard;
		
		private string _Expires;
		
		private DateTime? _NextDate;
		
		private bool? _Testing;
		
		private string _SemiEvery;
		
		private int? _Day1;
		
		private int? _Day2;
		
		private int? _EveryN;
		
		private string _Period;
		
		private DateTime? _StopWhen;
		
		private int? _StopAfter;
		
   		
    	
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
		
		partial void OnStartWhenChanging(DateTime? value);
		partial void OnStartWhenChanged();
		
		partial void OnTypeChanging(string value);
		partial void OnTypeChanged();
		
		partial void OnSageCardGuidChanging(Guid? value);
		partial void OnSageCardGuidChanged();
		
		partial void OnMaskedAccountChanging(string value);
		partial void OnMaskedAccountChanged();
		
		partial void OnMaskedCardChanging(string value);
		partial void OnMaskedCardChanged();
		
		partial void OnExpiresChanging(string value);
		partial void OnExpiresChanged();
		
		partial void OnNextDateChanging(DateTime? value);
		partial void OnNextDateChanged();
		
		partial void OnTestingChanging(bool? value);
		partial void OnTestingChanged();
		
		partial void OnSemiEveryChanging(string value);
		partial void OnSemiEveryChanged();
		
		partial void OnDay1Changing(int? value);
		partial void OnDay1Changed();
		
		partial void OnDay2Changing(int? value);
		partial void OnDay2Changed();
		
		partial void OnEveryNChanging(int? value);
		partial void OnEveryNChanged();
		
		partial void OnPeriodChanging(string value);
		partial void OnPeriodChanged();
		
		partial void OnStopWhenChanging(DateTime? value);
		partial void OnStopWhenChanged();
		
		partial void OnStopAfterChanging(int? value);
		partial void OnStopAfterChanged();
		
    #endregion
		public RecurringGiving()
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

		
		[Column(Name="StartWhen", UpdateCheck=UpdateCheck.Never, Storage="_StartWhen", DbType="datetime")]
		public DateTime? StartWhen
		{
			get { return this._StartWhen; }

			set
			{
				if (this._StartWhen != value)
				{
				
                    this.OnStartWhenChanging(value);
					this.SendPropertyChanging();
					this._StartWhen = value;
					this.SendPropertyChanged("StartWhen");
					this.OnStartWhenChanged();
				}

			}

		}

		
		[Column(Name="Type", UpdateCheck=UpdateCheck.Never, Storage="_Type", DbType="varchar(2)")]
		public string Type
		{
			get { return this._Type; }

			set
			{
				if (this._Type != value)
				{
				
                    this.OnTypeChanging(value);
					this.SendPropertyChanging();
					this._Type = value;
					this.SendPropertyChanged("Type");
					this.OnTypeChanged();
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

		
		[Column(Name="NextDate", UpdateCheck=UpdateCheck.Never, Storage="_NextDate", DbType="datetime")]
		public DateTime? NextDate
		{
			get { return this._NextDate; }

			set
			{
				if (this._NextDate != value)
				{
				
                    this.OnNextDateChanging(value);
					this.SendPropertyChanging();
					this._NextDate = value;
					this.SendPropertyChanged("NextDate");
					this.OnNextDateChanged();
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

		
		[Column(Name="SemiEvery", UpdateCheck=UpdateCheck.Never, Storage="_SemiEvery", DbType="varchar(2)")]
		public string SemiEvery
		{
			get { return this._SemiEvery; }

			set
			{
				if (this._SemiEvery != value)
				{
				
                    this.OnSemiEveryChanging(value);
					this.SendPropertyChanging();
					this._SemiEvery = value;
					this.SendPropertyChanged("SemiEvery");
					this.OnSemiEveryChanged();
				}

			}

		}

		
		[Column(Name="Day1", UpdateCheck=UpdateCheck.Never, Storage="_Day1", DbType="int")]
		public int? Day1
		{
			get { return this._Day1; }

			set
			{
				if (this._Day1 != value)
				{
				
                    this.OnDay1Changing(value);
					this.SendPropertyChanging();
					this._Day1 = value;
					this.SendPropertyChanged("Day1");
					this.OnDay1Changed();
				}

			}

		}

		
		[Column(Name="Day2", UpdateCheck=UpdateCheck.Never, Storage="_Day2", DbType="int")]
		public int? Day2
		{
			get { return this._Day2; }

			set
			{
				if (this._Day2 != value)
				{
				
                    this.OnDay2Changing(value);
					this.SendPropertyChanging();
					this._Day2 = value;
					this.SendPropertyChanged("Day2");
					this.OnDay2Changed();
				}

			}

		}

		
		[Column(Name="EveryN", UpdateCheck=UpdateCheck.Never, Storage="_EveryN", DbType="int")]
		public int? EveryN
		{
			get { return this._EveryN; }

			set
			{
				if (this._EveryN != value)
				{
				
                    this.OnEveryNChanging(value);
					this.SendPropertyChanging();
					this._EveryN = value;
					this.SendPropertyChanged("EveryN");
					this.OnEveryNChanged();
				}

			}

		}

		
		[Column(Name="Period", UpdateCheck=UpdateCheck.Never, Storage="_Period", DbType="varchar(2)")]
		public string Period
		{
			get { return this._Period; }

			set
			{
				if (this._Period != value)
				{
				
                    this.OnPeriodChanging(value);
					this.SendPropertyChanging();
					this._Period = value;
					this.SendPropertyChanged("Period");
					this.OnPeriodChanged();
				}

			}

		}

		
		[Column(Name="StopWhen", UpdateCheck=UpdateCheck.Never, Storage="_StopWhen", DbType="datetime")]
		public DateTime? StopWhen
		{
			get { return this._StopWhen; }

			set
			{
				if (this._StopWhen != value)
				{
				
                    this.OnStopWhenChanging(value);
					this.SendPropertyChanging();
					this._StopWhen = value;
					this.SendPropertyChanged("StopWhen");
					this.OnStopWhenChanged();
				}

			}

		}

		
		[Column(Name="StopAfter", UpdateCheck=UpdateCheck.Never, Storage="_StopAfter", DbType="int")]
		public int? StopAfter
		{
			get { return this._StopAfter; }

			set
			{
				if (this._StopAfter != value)
				{
				
                    this.OnStopAfterChanging(value);
					this.SendPropertyChanging();
					this._StopAfter = value;
					this.SendPropertyChanged("StopAfter");
					this.OnStopAfterChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="FK_AuthorizeNetIds_People", Storage="_Person", ThisKey="PeopleId", IsForeignKey=true)]
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
						previousValue.RecurringGivings.Remove(this);
					}

					this._Person.Entity = value;
					if (value != null)
					{
						value.RecurringGivings.Add(this);
						
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

