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
	[Table(Name="dbo.ContributionFund")]
	public partial class ContributionFund : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _FundId;
		
		private int _ChurchId;
		
		private int _CreatedBy;
		
		private DateTime _CreatedDate;
		
		private bool _RecordStatus;
		
		private string _FundName;
		
		private string _FundDescription;
		
		private int _FundStatusId;
		
		private int _FundTypeId;
		
		private bool _FundPledgeFlag;
		
		private decimal? _FundTarget;
		
		private DateTime _FundOpenDate;
		
		private DateTime? _FundCloseDate;
		
		private DateTime? _FundReopenDate;
		
		private DateTime? _FundDropDate;
		
		private int? _FundAccountCode;
		
		private string _FundIncomeDept;
		
		private string _FundIncomeAccount;
		
		private string _FundIncomeFund;
		
		private string _FundCashDept;
		
		private string _FundCashAccount;
		
		private string _FundCashFund;
		
		private int? _ModifiedBy;
		
		private DateTime? _ModifiedDate;
		
   		
   		private EntitySet< BundleHeader> _BundleHeaders;
		
   		private EntitySet< Contribution> _Contributions;
		
    	
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnFundIdChanging(int value);
		partial void OnFundIdChanged();
		
		partial void OnChurchIdChanging(int value);
		partial void OnChurchIdChanged();
		
		partial void OnCreatedByChanging(int value);
		partial void OnCreatedByChanged();
		
		partial void OnCreatedDateChanging(DateTime value);
		partial void OnCreatedDateChanged();
		
		partial void OnRecordStatusChanging(bool value);
		partial void OnRecordStatusChanged();
		
		partial void OnFundNameChanging(string value);
		partial void OnFundNameChanged();
		
		partial void OnFundDescriptionChanging(string value);
		partial void OnFundDescriptionChanged();
		
		partial void OnFundStatusIdChanging(int value);
		partial void OnFundStatusIdChanged();
		
		partial void OnFundTypeIdChanging(int value);
		partial void OnFundTypeIdChanged();
		
		partial void OnFundPledgeFlagChanging(bool value);
		partial void OnFundPledgeFlagChanged();
		
		partial void OnFundTargetChanging(decimal? value);
		partial void OnFundTargetChanged();
		
		partial void OnFundOpenDateChanging(DateTime value);
		partial void OnFundOpenDateChanged();
		
		partial void OnFundCloseDateChanging(DateTime? value);
		partial void OnFundCloseDateChanged();
		
		partial void OnFundReopenDateChanging(DateTime? value);
		partial void OnFundReopenDateChanged();
		
		partial void OnFundDropDateChanging(DateTime? value);
		partial void OnFundDropDateChanged();
		
		partial void OnFundAccountCodeChanging(int? value);
		partial void OnFundAccountCodeChanged();
		
		partial void OnFundIncomeDeptChanging(string value);
		partial void OnFundIncomeDeptChanged();
		
		partial void OnFundIncomeAccountChanging(string value);
		partial void OnFundIncomeAccountChanged();
		
		partial void OnFundIncomeFundChanging(string value);
		partial void OnFundIncomeFundChanged();
		
		partial void OnFundCashDeptChanging(string value);
		partial void OnFundCashDeptChanged();
		
		partial void OnFundCashAccountChanging(string value);
		partial void OnFundCashAccountChanged();
		
		partial void OnFundCashFundChanging(string value);
		partial void OnFundCashFundChanged();
		
		partial void OnModifiedByChanging(int? value);
		partial void OnModifiedByChanged();
		
		partial void OnModifiedDateChanging(DateTime? value);
		partial void OnModifiedDateChanged();
		
    #endregion
		public ContributionFund()
		{
			
			this._BundleHeaders = new EntitySet< BundleHeader>(new Action< BundleHeader>(this.attach_BundleHeaders), new Action< BundleHeader>(this.detach_BundleHeaders)); 
			
			this._Contributions = new EntitySet< Contribution>(new Action< Contribution>(this.attach_Contributions), new Action< Contribution>(this.detach_Contributions)); 
			
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="FundId", UpdateCheck=UpdateCheck.Never, Storage="_FundId", DbType="int NOT NULL", IsPrimaryKey=true)]
		public int FundId
		{
			get { return this._FundId; }

			set
			{
				if (this._FundId != value)
				{
				
                    this.OnFundIdChanging(value);
					this.SendPropertyChanging();
					this._FundId = value;
					this.SendPropertyChanged("FundId");
					this.OnFundIdChanged();
				}

			}

		}

		
		[Column(Name="ChurchId", UpdateCheck=UpdateCheck.Never, Storage="_ChurchId", DbType="int NOT NULL")]
		public int ChurchId
		{
			get { return this._ChurchId; }

			set
			{
				if (this._ChurchId != value)
				{
				
                    this.OnChurchIdChanging(value);
					this.SendPropertyChanging();
					this._ChurchId = value;
					this.SendPropertyChanged("ChurchId");
					this.OnChurchIdChanged();
				}

			}

		}

		
		[Column(Name="CreatedBy", UpdateCheck=UpdateCheck.Never, Storage="_CreatedBy", DbType="int NOT NULL")]
		public int CreatedBy
		{
			get { return this._CreatedBy; }

			set
			{
				if (this._CreatedBy != value)
				{
				
                    this.OnCreatedByChanging(value);
					this.SendPropertyChanging();
					this._CreatedBy = value;
					this.SendPropertyChanged("CreatedBy");
					this.OnCreatedByChanged();
				}

			}

		}

		
		[Column(Name="CreatedDate", UpdateCheck=UpdateCheck.Never, Storage="_CreatedDate", DbType="datetime NOT NULL")]
		public DateTime CreatedDate
		{
			get { return this._CreatedDate; }

			set
			{
				if (this._CreatedDate != value)
				{
				
                    this.OnCreatedDateChanging(value);
					this.SendPropertyChanging();
					this._CreatedDate = value;
					this.SendPropertyChanged("CreatedDate");
					this.OnCreatedDateChanged();
				}

			}

		}

		
		[Column(Name="RecordStatus", UpdateCheck=UpdateCheck.Never, Storage="_RecordStatus", DbType="bit NOT NULL")]
		public bool RecordStatus
		{
			get { return this._RecordStatus; }

			set
			{
				if (this._RecordStatus != value)
				{
				
                    this.OnRecordStatusChanging(value);
					this.SendPropertyChanging();
					this._RecordStatus = value;
					this.SendPropertyChanged("RecordStatus");
					this.OnRecordStatusChanged();
				}

			}

		}

		
		[Column(Name="FundName", UpdateCheck=UpdateCheck.Never, Storage="_FundName", DbType="varchar(40) NOT NULL")]
		public string FundName
		{
			get { return this._FundName; }

			set
			{
				if (this._FundName != value)
				{
				
                    this.OnFundNameChanging(value);
					this.SendPropertyChanging();
					this._FundName = value;
					this.SendPropertyChanged("FundName");
					this.OnFundNameChanged();
				}

			}

		}

		
		[Column(Name="FundDescription", UpdateCheck=UpdateCheck.Never, Storage="_FundDescription", DbType="varchar(256)")]
		public string FundDescription
		{
			get { return this._FundDescription; }

			set
			{
				if (this._FundDescription != value)
				{
				
                    this.OnFundDescriptionChanging(value);
					this.SendPropertyChanging();
					this._FundDescription = value;
					this.SendPropertyChanged("FundDescription");
					this.OnFundDescriptionChanged();
				}

			}

		}

		
		[Column(Name="FundStatusId", UpdateCheck=UpdateCheck.Never, Storage="_FundStatusId", DbType="int NOT NULL")]
		public int FundStatusId
		{
			get { return this._FundStatusId; }

			set
			{
				if (this._FundStatusId != value)
				{
				
                    this.OnFundStatusIdChanging(value);
					this.SendPropertyChanging();
					this._FundStatusId = value;
					this.SendPropertyChanged("FundStatusId");
					this.OnFundStatusIdChanged();
				}

			}

		}

		
		[Column(Name="FundTypeId", UpdateCheck=UpdateCheck.Never, Storage="_FundTypeId", DbType="int NOT NULL")]
		public int FundTypeId
		{
			get { return this._FundTypeId; }

			set
			{
				if (this._FundTypeId != value)
				{
				
                    this.OnFundTypeIdChanging(value);
					this.SendPropertyChanging();
					this._FundTypeId = value;
					this.SendPropertyChanged("FundTypeId");
					this.OnFundTypeIdChanged();
				}

			}

		}

		
		[Column(Name="FundPledgeFlag", UpdateCheck=UpdateCheck.Never, Storage="_FundPledgeFlag", DbType="bit NOT NULL")]
		public bool FundPledgeFlag
		{
			get { return this._FundPledgeFlag; }

			set
			{
				if (this._FundPledgeFlag != value)
				{
				
                    this.OnFundPledgeFlagChanging(value);
					this.SendPropertyChanging();
					this._FundPledgeFlag = value;
					this.SendPropertyChanged("FundPledgeFlag");
					this.OnFundPledgeFlagChanged();
				}

			}

		}

		
		[Column(Name="FundTarget", UpdateCheck=UpdateCheck.Never, Storage="_FundTarget", DbType="Decimal(11,2)")]
		public decimal? FundTarget
		{
			get { return this._FundTarget; }

			set
			{
				if (this._FundTarget != value)
				{
				
                    this.OnFundTargetChanging(value);
					this.SendPropertyChanging();
					this._FundTarget = value;
					this.SendPropertyChanged("FundTarget");
					this.OnFundTargetChanged();
				}

			}

		}

		
		[Column(Name="FundOpenDate", UpdateCheck=UpdateCheck.Never, Storage="_FundOpenDate", DbType="datetime NOT NULL")]
		public DateTime FundOpenDate
		{
			get { return this._FundOpenDate; }

			set
			{
				if (this._FundOpenDate != value)
				{
				
                    this.OnFundOpenDateChanging(value);
					this.SendPropertyChanging();
					this._FundOpenDate = value;
					this.SendPropertyChanged("FundOpenDate");
					this.OnFundOpenDateChanged();
				}

			}

		}

		
		[Column(Name="FundCloseDate", UpdateCheck=UpdateCheck.Never, Storage="_FundCloseDate", DbType="datetime")]
		public DateTime? FundCloseDate
		{
			get { return this._FundCloseDate; }

			set
			{
				if (this._FundCloseDate != value)
				{
				
                    this.OnFundCloseDateChanging(value);
					this.SendPropertyChanging();
					this._FundCloseDate = value;
					this.SendPropertyChanged("FundCloseDate");
					this.OnFundCloseDateChanged();
				}

			}

		}

		
		[Column(Name="FundReopenDate", UpdateCheck=UpdateCheck.Never, Storage="_FundReopenDate", DbType="datetime")]
		public DateTime? FundReopenDate
		{
			get { return this._FundReopenDate; }

			set
			{
				if (this._FundReopenDate != value)
				{
				
                    this.OnFundReopenDateChanging(value);
					this.SendPropertyChanging();
					this._FundReopenDate = value;
					this.SendPropertyChanged("FundReopenDate");
					this.OnFundReopenDateChanged();
				}

			}

		}

		
		[Column(Name="FundDropDate", UpdateCheck=UpdateCheck.Never, Storage="_FundDropDate", DbType="datetime")]
		public DateTime? FundDropDate
		{
			get { return this._FundDropDate; }

			set
			{
				if (this._FundDropDate != value)
				{
				
                    this.OnFundDropDateChanging(value);
					this.SendPropertyChanging();
					this._FundDropDate = value;
					this.SendPropertyChanged("FundDropDate");
					this.OnFundDropDateChanged();
				}

			}

		}

		
		[Column(Name="FundAccountCode", UpdateCheck=UpdateCheck.Never, Storage="_FundAccountCode", DbType="int")]
		public int? FundAccountCode
		{
			get { return this._FundAccountCode; }

			set
			{
				if (this._FundAccountCode != value)
				{
				
                    this.OnFundAccountCodeChanging(value);
					this.SendPropertyChanging();
					this._FundAccountCode = value;
					this.SendPropertyChanged("FundAccountCode");
					this.OnFundAccountCodeChanged();
				}

			}

		}

		
		[Column(Name="FundIncomeDept", UpdateCheck=UpdateCheck.Never, Storage="_FundIncomeDept", DbType="varchar(25) NOT NULL")]
		public string FundIncomeDept
		{
			get { return this._FundIncomeDept; }

			set
			{
				if (this._FundIncomeDept != value)
				{
				
                    this.OnFundIncomeDeptChanging(value);
					this.SendPropertyChanging();
					this._FundIncomeDept = value;
					this.SendPropertyChanged("FundIncomeDept");
					this.OnFundIncomeDeptChanged();
				}

			}

		}

		
		[Column(Name="FundIncomeAccount", UpdateCheck=UpdateCheck.Never, Storage="_FundIncomeAccount", DbType="varchar(25) NOT NULL")]
		public string FundIncomeAccount
		{
			get { return this._FundIncomeAccount; }

			set
			{
				if (this._FundIncomeAccount != value)
				{
				
                    this.OnFundIncomeAccountChanging(value);
					this.SendPropertyChanging();
					this._FundIncomeAccount = value;
					this.SendPropertyChanged("FundIncomeAccount");
					this.OnFundIncomeAccountChanged();
				}

			}

		}

		
		[Column(Name="FundIncomeFund", UpdateCheck=UpdateCheck.Never, Storage="_FundIncomeFund", DbType="varchar(25) NOT NULL")]
		public string FundIncomeFund
		{
			get { return this._FundIncomeFund; }

			set
			{
				if (this._FundIncomeFund != value)
				{
				
                    this.OnFundIncomeFundChanging(value);
					this.SendPropertyChanging();
					this._FundIncomeFund = value;
					this.SendPropertyChanged("FundIncomeFund");
					this.OnFundIncomeFundChanged();
				}

			}

		}

		
		[Column(Name="FundCashDept", UpdateCheck=UpdateCheck.Never, Storage="_FundCashDept", DbType="varchar(25) NOT NULL")]
		public string FundCashDept
		{
			get { return this._FundCashDept; }

			set
			{
				if (this._FundCashDept != value)
				{
				
                    this.OnFundCashDeptChanging(value);
					this.SendPropertyChanging();
					this._FundCashDept = value;
					this.SendPropertyChanged("FundCashDept");
					this.OnFundCashDeptChanged();
				}

			}

		}

		
		[Column(Name="FundCashAccount", UpdateCheck=UpdateCheck.Never, Storage="_FundCashAccount", DbType="varchar(25) NOT NULL")]
		public string FundCashAccount
		{
			get { return this._FundCashAccount; }

			set
			{
				if (this._FundCashAccount != value)
				{
				
                    this.OnFundCashAccountChanging(value);
					this.SendPropertyChanging();
					this._FundCashAccount = value;
					this.SendPropertyChanged("FundCashAccount");
					this.OnFundCashAccountChanged();
				}

			}

		}

		
		[Column(Name="FundCashFund", UpdateCheck=UpdateCheck.Never, Storage="_FundCashFund", DbType="varchar(25) NOT NULL")]
		public string FundCashFund
		{
			get { return this._FundCashFund; }

			set
			{
				if (this._FundCashFund != value)
				{
				
                    this.OnFundCashFundChanging(value);
					this.SendPropertyChanging();
					this._FundCashFund = value;
					this.SendPropertyChanged("FundCashFund");
					this.OnFundCashFundChanged();
				}

			}

		}

		
		[Column(Name="ModifiedBy", UpdateCheck=UpdateCheck.Never, Storage="_ModifiedBy", DbType="int")]
		public int? ModifiedBy
		{
			get { return this._ModifiedBy; }

			set
			{
				if (this._ModifiedBy != value)
				{
				
                    this.OnModifiedByChanging(value);
					this.SendPropertyChanging();
					this._ModifiedBy = value;
					this.SendPropertyChanged("ModifiedBy");
					this.OnModifiedByChanged();
				}

			}

		}

		
		[Column(Name="ModifiedDate", UpdateCheck=UpdateCheck.Never, Storage="_ModifiedDate", DbType="datetime")]
		public DateTime? ModifiedDate
		{
			get { return this._ModifiedDate; }

			set
			{
				if (this._ModifiedDate != value)
				{
				
                    this.OnModifiedDateChanging(value);
					this.SendPropertyChanging();
					this._ModifiedDate = value;
					this.SendPropertyChanged("ModifiedDate");
					this.OnModifiedDateChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
   		[Association(Name="BundleHeaders__Fund", Storage="_BundleHeaders", OtherKey="FundId")]
   		public EntitySet< BundleHeader> BundleHeaders
   		{
   		    get { return this._BundleHeaders; }

			set	{ this._BundleHeaders.Assign(value); }

   		}

		
   		[Association(Name="FK_Contribution_ContributionFund", Storage="_Contributions", OtherKey="FundId")]
   		public EntitySet< Contribution> Contributions
   		{
   		    get { return this._Contributions; }

			set	{ this._Contributions.Assign(value); }

   		}

		
	#endregion
	
	#region Foreign Keys
    	
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

   		
		private void attach_BundleHeaders(BundleHeader entity)
		{
			this.SendPropertyChanging();
			entity.Fund = this;
		}

		private void detach_BundleHeaders(BundleHeader entity)
		{
			this.SendPropertyChanging();
			entity.Fund = null;
		}

		
		private void attach_Contributions(Contribution entity)
		{
			this.SendPropertyChanging();
			entity.ContributionFund = this;
		}

		private void detach_Contributions(Contribution entity)
		{
			this.SendPropertyChanging();
			entity.ContributionFund = null;
		}

		
	}

}

