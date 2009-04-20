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
	[Table(Name="dbo.Contribution")]
	public partial class Contribution : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _ContributionId;
		
		private int _CreatedBy;
		
		private DateTime _CreatedDate;
		
		private int _FundId;
		
		private int _ContributionTypeId;
		
		private int? _PeopleId;
		
		private DateTime? _ContributionDate;
		
		private decimal? _ContributionAmount;
		
		private string _ContributionDesc;
		
		private int? _ContributionStatusId;
		
		private bool _PledgeFlag;
		
		private int? _ModifiedBy;
		
		private DateTime? _ModifiedDate;
		
		private DateTime? _BrokerContactDate;
		
		private string _BrokerName;
		
		private DateTime? _CheckReceivedDate;
		
		private DateTime? _DateSold;
		
		private string _Amount;
		
		private string _FundDistributionInfo;
		
		private string _ReturnedCheckNum;
		
		private string _ReturnedCheckBank;
		
		private DateTime? _ReturnedCheckDate;
		
		private DateTime? _PostingDate;
		
   		
   		private EntitySet< BundleDetail> _BundleDetails;
		
    	
		private EntityRef< ContributionFund> _ContributionFund;
		
		private EntityRef< ContributionStatus> _ContributionStatus;
		
		private EntityRef< ContributionType> _ContributionType;
		
		private EntityRef< Person> _Person;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnContributionIdChanging(int value);
		partial void OnContributionIdChanged();
		
		partial void OnCreatedByChanging(int value);
		partial void OnCreatedByChanged();
		
		partial void OnCreatedDateChanging(DateTime value);
		partial void OnCreatedDateChanged();
		
		partial void OnFundIdChanging(int value);
		partial void OnFundIdChanged();
		
		partial void OnContributionTypeIdChanging(int value);
		partial void OnContributionTypeIdChanged();
		
		partial void OnPeopleIdChanging(int? value);
		partial void OnPeopleIdChanged();
		
		partial void OnContributionDateChanging(DateTime? value);
		partial void OnContributionDateChanged();
		
		partial void OnContributionAmountChanging(decimal? value);
		partial void OnContributionAmountChanged();
		
		partial void OnContributionDescChanging(string value);
		partial void OnContributionDescChanged();
		
		partial void OnContributionStatusIdChanging(int? value);
		partial void OnContributionStatusIdChanged();
		
		partial void OnPledgeFlagChanging(bool value);
		partial void OnPledgeFlagChanged();
		
		partial void OnModifiedByChanging(int? value);
		partial void OnModifiedByChanged();
		
		partial void OnModifiedDateChanging(DateTime? value);
		partial void OnModifiedDateChanged();
		
		partial void OnBrokerContactDateChanging(DateTime? value);
		partial void OnBrokerContactDateChanged();
		
		partial void OnBrokerNameChanging(string value);
		partial void OnBrokerNameChanged();
		
		partial void OnCheckReceivedDateChanging(DateTime? value);
		partial void OnCheckReceivedDateChanged();
		
		partial void OnDateSoldChanging(DateTime? value);
		partial void OnDateSoldChanged();
		
		partial void OnAmountChanging(string value);
		partial void OnAmountChanged();
		
		partial void OnFundDistributionInfoChanging(string value);
		partial void OnFundDistributionInfoChanged();
		
		partial void OnReturnedCheckNumChanging(string value);
		partial void OnReturnedCheckNumChanged();
		
		partial void OnReturnedCheckBankChanging(string value);
		partial void OnReturnedCheckBankChanged();
		
		partial void OnReturnedCheckDateChanging(DateTime? value);
		partial void OnReturnedCheckDateChanged();
		
		partial void OnPostingDateChanging(DateTime? value);
		partial void OnPostingDateChanged();
		
    #endregion
		public Contribution()
		{
			
			this._BundleDetails = new EntitySet< BundleDetail>(new Action< BundleDetail>(this.attach_BundleDetails), new Action< BundleDetail>(this.detach_BundleDetails)); 
			
			
			this._ContributionFund = default(EntityRef< ContributionFund>); 
			
			this._ContributionStatus = default(EntityRef< ContributionStatus>); 
			
			this._ContributionType = default(EntityRef< ContributionType>); 
			
			this._Person = default(EntityRef< Person>); 
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="ContributionId", UpdateCheck=UpdateCheck.Never, Storage="_ContributionId", AutoSync=AutoSync.OnInsert, DbType="int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int ContributionId
		{
			get { return this._ContributionId; }

			set
			{
				if (this._ContributionId != value)
				{
				
                    this.OnContributionIdChanging(value);
					this.SendPropertyChanging();
					this._ContributionId = value;
					this.SendPropertyChanged("ContributionId");
					this.OnContributionIdChanged();
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

		
		[Column(Name="FundId", UpdateCheck=UpdateCheck.Never, Storage="_FundId", DbType="int NOT NULL")]
		public int FundId
		{
			get { return this._FundId; }

			set
			{
				if (this._FundId != value)
				{
				
					if (this._ContributionFund.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnFundIdChanging(value);
					this.SendPropertyChanging();
					this._FundId = value;
					this.SendPropertyChanged("FundId");
					this.OnFundIdChanged();
				}

			}

		}

		
		[Column(Name="ContributionTypeId", UpdateCheck=UpdateCheck.Never, Storage="_ContributionTypeId", DbType="int NOT NULL")]
		public int ContributionTypeId
		{
			get { return this._ContributionTypeId; }

			set
			{
				if (this._ContributionTypeId != value)
				{
				
					if (this._ContributionType.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnContributionTypeIdChanging(value);
					this.SendPropertyChanging();
					this._ContributionTypeId = value;
					this.SendPropertyChanged("ContributionTypeId");
					this.OnContributionTypeIdChanged();
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

		
		[Column(Name="ContributionDate", UpdateCheck=UpdateCheck.Never, Storage="_ContributionDate", DbType="datetime")]
		public DateTime? ContributionDate
		{
			get { return this._ContributionDate; }

			set
			{
				if (this._ContributionDate != value)
				{
				
                    this.OnContributionDateChanging(value);
					this.SendPropertyChanging();
					this._ContributionDate = value;
					this.SendPropertyChanged("ContributionDate");
					this.OnContributionDateChanged();
				}

			}

		}

		
		[Column(Name="ContributionAmount", UpdateCheck=UpdateCheck.Never, Storage="_ContributionAmount", DbType="Decimal(11,2)")]
		public decimal? ContributionAmount
		{
			get { return this._ContributionAmount; }

			set
			{
				if (this._ContributionAmount != value)
				{
				
                    this.OnContributionAmountChanging(value);
					this.SendPropertyChanging();
					this._ContributionAmount = value;
					this.SendPropertyChanged("ContributionAmount");
					this.OnContributionAmountChanged();
				}

			}

		}

		
		[Column(Name="ContributionDesc", UpdateCheck=UpdateCheck.Never, Storage="_ContributionDesc", DbType="varchar(256)")]
		public string ContributionDesc
		{
			get { return this._ContributionDesc; }

			set
			{
				if (this._ContributionDesc != value)
				{
				
                    this.OnContributionDescChanging(value);
					this.SendPropertyChanging();
					this._ContributionDesc = value;
					this.SendPropertyChanged("ContributionDesc");
					this.OnContributionDescChanged();
				}

			}

		}

		
		[Column(Name="ContributionStatusId", UpdateCheck=UpdateCheck.Never, Storage="_ContributionStatusId", DbType="int")]
		public int? ContributionStatusId
		{
			get { return this._ContributionStatusId; }

			set
			{
				if (this._ContributionStatusId != value)
				{
				
					if (this._ContributionStatus.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnContributionStatusIdChanging(value);
					this.SendPropertyChanging();
					this._ContributionStatusId = value;
					this.SendPropertyChanged("ContributionStatusId");
					this.OnContributionStatusIdChanged();
				}

			}

		}

		
		[Column(Name="PledgeFlag", UpdateCheck=UpdateCheck.Never, Storage="_PledgeFlag", DbType="bit NOT NULL")]
		public bool PledgeFlag
		{
			get { return this._PledgeFlag; }

			set
			{
				if (this._PledgeFlag != value)
				{
				
                    this.OnPledgeFlagChanging(value);
					this.SendPropertyChanging();
					this._PledgeFlag = value;
					this.SendPropertyChanged("PledgeFlag");
					this.OnPledgeFlagChanged();
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

		
		[Column(Name="BrokerContactDate", UpdateCheck=UpdateCheck.Never, Storage="_BrokerContactDate", DbType="datetime")]
		public DateTime? BrokerContactDate
		{
			get { return this._BrokerContactDate; }

			set
			{
				if (this._BrokerContactDate != value)
				{
				
                    this.OnBrokerContactDateChanging(value);
					this.SendPropertyChanging();
					this._BrokerContactDate = value;
					this.SendPropertyChanged("BrokerContactDate");
					this.OnBrokerContactDateChanged();
				}

			}

		}

		
		[Column(Name="BrokerName", UpdateCheck=UpdateCheck.Never, Storage="_BrokerName", DbType="varchar(50)")]
		public string BrokerName
		{
			get { return this._BrokerName; }

			set
			{
				if (this._BrokerName != value)
				{
				
                    this.OnBrokerNameChanging(value);
					this.SendPropertyChanging();
					this._BrokerName = value;
					this.SendPropertyChanged("BrokerName");
					this.OnBrokerNameChanged();
				}

			}

		}

		
		[Column(Name="CheckReceivedDate", UpdateCheck=UpdateCheck.Never, Storage="_CheckReceivedDate", DbType="datetime")]
		public DateTime? CheckReceivedDate
		{
			get { return this._CheckReceivedDate; }

			set
			{
				if (this._CheckReceivedDate != value)
				{
				
                    this.OnCheckReceivedDateChanging(value);
					this.SendPropertyChanging();
					this._CheckReceivedDate = value;
					this.SendPropertyChanged("CheckReceivedDate");
					this.OnCheckReceivedDateChanged();
				}

			}

		}

		
		[Column(Name="DateSold", UpdateCheck=UpdateCheck.Never, Storage="_DateSold", DbType="datetime")]
		public DateTime? DateSold
		{
			get { return this._DateSold; }

			set
			{
				if (this._DateSold != value)
				{
				
                    this.OnDateSoldChanging(value);
					this.SendPropertyChanging();
					this._DateSold = value;
					this.SendPropertyChanged("DateSold");
					this.OnDateSoldChanged();
				}

			}

		}

		
		[Column(Name="Amount", UpdateCheck=UpdateCheck.Never, Storage="_Amount", DbType="varchar(15)")]
		public string Amount
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

		
		[Column(Name="FundDistributionInfo", UpdateCheck=UpdateCheck.Never, Storage="_FundDistributionInfo", DbType="varchar(256)")]
		public string FundDistributionInfo
		{
			get { return this._FundDistributionInfo; }

			set
			{
				if (this._FundDistributionInfo != value)
				{
				
                    this.OnFundDistributionInfoChanging(value);
					this.SendPropertyChanging();
					this._FundDistributionInfo = value;
					this.SendPropertyChanged("FundDistributionInfo");
					this.OnFundDistributionInfoChanged();
				}

			}

		}

		
		[Column(Name="ReturnedCheckNum", UpdateCheck=UpdateCheck.Never, Storage="_ReturnedCheckNum", DbType="varchar(10)")]
		public string ReturnedCheckNum
		{
			get { return this._ReturnedCheckNum; }

			set
			{
				if (this._ReturnedCheckNum != value)
				{
				
                    this.OnReturnedCheckNumChanging(value);
					this.SendPropertyChanging();
					this._ReturnedCheckNum = value;
					this.SendPropertyChanged("ReturnedCheckNum");
					this.OnReturnedCheckNumChanged();
				}

			}

		}

		
		[Column(Name="ReturnedCheckBank", UpdateCheck=UpdateCheck.Never, Storage="_ReturnedCheckBank", DbType="varchar(40)")]
		public string ReturnedCheckBank
		{
			get { return this._ReturnedCheckBank; }

			set
			{
				if (this._ReturnedCheckBank != value)
				{
				
                    this.OnReturnedCheckBankChanging(value);
					this.SendPropertyChanging();
					this._ReturnedCheckBank = value;
					this.SendPropertyChanged("ReturnedCheckBank");
					this.OnReturnedCheckBankChanged();
				}

			}

		}

		
		[Column(Name="ReturnedCheckDate", UpdateCheck=UpdateCheck.Never, Storage="_ReturnedCheckDate", DbType="datetime")]
		public DateTime? ReturnedCheckDate
		{
			get { return this._ReturnedCheckDate; }

			set
			{
				if (this._ReturnedCheckDate != value)
				{
				
                    this.OnReturnedCheckDateChanging(value);
					this.SendPropertyChanging();
					this._ReturnedCheckDate = value;
					this.SendPropertyChanged("ReturnedCheckDate");
					this.OnReturnedCheckDateChanged();
				}

			}

		}

		
		[Column(Name="PostingDate", UpdateCheck=UpdateCheck.Never, Storage="_PostingDate", DbType="datetime")]
		public DateTime? PostingDate
		{
			get { return this._PostingDate; }

			set
			{
				if (this._PostingDate != value)
				{
				
                    this.OnPostingDateChanging(value);
					this.SendPropertyChanging();
					this._PostingDate = value;
					this.SendPropertyChanged("PostingDate");
					this.OnPostingDateChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
   		[Association(Name="BUNDLE_DETAIL_CONTR_FK", Storage="_BundleDetails", OtherKey="ContributionId")]
   		public EntitySet< BundleDetail> BundleDetails
   		{
   		    get { return this._BundleDetails; }

			set	{ this._BundleDetails.Assign(value); }

   		}

		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="FK_Contribution_ContributionFund", Storage="_ContributionFund", ThisKey="FundId", IsForeignKey=true)]
		public ContributionFund ContributionFund
		{
			get { return this._ContributionFund.Entity; }

			set
			{
				ContributionFund previousValue = this._ContributionFund.Entity;
				if (((previousValue != value) 
							|| (this._ContributionFund.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._ContributionFund.Entity = null;
						previousValue.Contributions.Remove(this);
					}

					this._ContributionFund.Entity = value;
					if (value != null)
					{
						value.Contributions.Add(this);
						
						this._FundId = value.FundId;
						
					}

					else
					{
						
						this._FundId = default(int);
						
					}

					this.SendPropertyChanged("ContributionFund");
				}

			}

		}

		
		[Association(Name="FK_Contribution_ContributionStatus", Storage="_ContributionStatus", ThisKey="ContributionStatusId", IsForeignKey=true)]
		public ContributionStatus ContributionStatus
		{
			get { return this._ContributionStatus.Entity; }

			set
			{
				ContributionStatus previousValue = this._ContributionStatus.Entity;
				if (((previousValue != value) 
							|| (this._ContributionStatus.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._ContributionStatus.Entity = null;
						previousValue.Contributions.Remove(this);
					}

					this._ContributionStatus.Entity = value;
					if (value != null)
					{
						value.Contributions.Add(this);
						
						this._ContributionStatusId = value.Id;
						
					}

					else
					{
						
						this._ContributionStatusId = default(int?);
						
					}

					this.SendPropertyChanged("ContributionStatus");
				}

			}

		}

		
		[Association(Name="FK_Contribution_ContributionType", Storage="_ContributionType", ThisKey="ContributionTypeId", IsForeignKey=true)]
		public ContributionType ContributionType
		{
			get { return this._ContributionType.Entity; }

			set
			{
				ContributionType previousValue = this._ContributionType.Entity;
				if (((previousValue != value) 
							|| (this._ContributionType.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._ContributionType.Entity = null;
						previousValue.Contributions.Remove(this);
					}

					this._ContributionType.Entity = value;
					if (value != null)
					{
						value.Contributions.Add(this);
						
						this._ContributionTypeId = value.Id;
						
					}

					else
					{
						
						this._ContributionTypeId = default(int);
						
					}

					this.SendPropertyChanged("ContributionType");
				}

			}

		}

		
		[Association(Name="FK_Contribution_People", Storage="_Person", ThisKey="PeopleId", IsForeignKey=true)]
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
						previousValue.Contributions.Remove(this);
					}

					this._Person.Entity = value;
					if (value != null)
					{
						value.Contributions.Add(this);
						
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

   		
		private void attach_BundleDetails(BundleDetail entity)
		{
			this.SendPropertyChanging();
			entity.Contribution = this;
		}

		private void detach_BundleDetails(BundleDetail entity)
		{
			this.SendPropertyChanging();
			entity.Contribution = null;
		}

		
	}

}

