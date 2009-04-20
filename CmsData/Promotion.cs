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
	[Table(Name="dbo.Promotions")]
	public partial class Promotion : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _PromotionId;
		
		private int _CreatedBy;
		
		private DateTime _CreatedDate;
		
		private bool _RecordStatus;
		
		private int _ChurchId;
		
		private DateTime? _PromotionDate;
		
		private DateTime? _OpenedDate;
		
		private DateTime? _CompletedDate;
		
		private string _PromotionName;
		
		private string _PromotionDescription;
		
		private int? _ModifiedBy;
		
		private DateTime? _ModifiedDate;
		
   		
   		private EntitySet< PromotionControl> _PromotionControls;
		
    	
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnPromotionIdChanging(int value);
		partial void OnPromotionIdChanged();
		
		partial void OnCreatedByChanging(int value);
		partial void OnCreatedByChanged();
		
		partial void OnCreatedDateChanging(DateTime value);
		partial void OnCreatedDateChanged();
		
		partial void OnRecordStatusChanging(bool value);
		partial void OnRecordStatusChanged();
		
		partial void OnChurchIdChanging(int value);
		partial void OnChurchIdChanged();
		
		partial void OnPromotionDateChanging(DateTime? value);
		partial void OnPromotionDateChanged();
		
		partial void OnOpenedDateChanging(DateTime? value);
		partial void OnOpenedDateChanged();
		
		partial void OnCompletedDateChanging(DateTime? value);
		partial void OnCompletedDateChanged();
		
		partial void OnPromotionNameChanging(string value);
		partial void OnPromotionNameChanged();
		
		partial void OnPromotionDescriptionChanging(string value);
		partial void OnPromotionDescriptionChanged();
		
		partial void OnModifiedByChanging(int? value);
		partial void OnModifiedByChanged();
		
		partial void OnModifiedDateChanging(DateTime? value);
		partial void OnModifiedDateChanged();
		
    #endregion
		public Promotion()
		{
			
			this._PromotionControls = new EntitySet< PromotionControl>(new Action< PromotionControl>(this.attach_PromotionControls), new Action< PromotionControl>(this.detach_PromotionControls)); 
			
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="PromotionId", UpdateCheck=UpdateCheck.Never, Storage="_PromotionId", AutoSync=AutoSync.OnInsert, DbType="int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int PromotionId
		{
			get { return this._PromotionId; }

			set
			{
				if (this._PromotionId != value)
				{
				
                    this.OnPromotionIdChanging(value);
					this.SendPropertyChanging();
					this._PromotionId = value;
					this.SendPropertyChanged("PromotionId");
					this.OnPromotionIdChanged();
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

		
		[Column(Name="PromotionDate", UpdateCheck=UpdateCheck.Never, Storage="_PromotionDate", DbType="datetime")]
		public DateTime? PromotionDate
		{
			get { return this._PromotionDate; }

			set
			{
				if (this._PromotionDate != value)
				{
				
                    this.OnPromotionDateChanging(value);
					this.SendPropertyChanging();
					this._PromotionDate = value;
					this.SendPropertyChanged("PromotionDate");
					this.OnPromotionDateChanged();
				}

			}

		}

		
		[Column(Name="OpenedDate", UpdateCheck=UpdateCheck.Never, Storage="_OpenedDate", DbType="datetime")]
		public DateTime? OpenedDate
		{
			get { return this._OpenedDate; }

			set
			{
				if (this._OpenedDate != value)
				{
				
                    this.OnOpenedDateChanging(value);
					this.SendPropertyChanging();
					this._OpenedDate = value;
					this.SendPropertyChanged("OpenedDate");
					this.OnOpenedDateChanged();
				}

			}

		}

		
		[Column(Name="CompletedDate", UpdateCheck=UpdateCheck.Never, Storage="_CompletedDate", DbType="datetime")]
		public DateTime? CompletedDate
		{
			get { return this._CompletedDate; }

			set
			{
				if (this._CompletedDate != value)
				{
				
                    this.OnCompletedDateChanging(value);
					this.SendPropertyChanging();
					this._CompletedDate = value;
					this.SendPropertyChanged("CompletedDate");
					this.OnCompletedDateChanged();
				}

			}

		}

		
		[Column(Name="PromotionName", UpdateCheck=UpdateCheck.Never, Storage="_PromotionName", DbType="varchar(40) NOT NULL")]
		public string PromotionName
		{
			get { return this._PromotionName; }

			set
			{
				if (this._PromotionName != value)
				{
				
                    this.OnPromotionNameChanging(value);
					this.SendPropertyChanging();
					this._PromotionName = value;
					this.SendPropertyChanged("PromotionName");
					this.OnPromotionNameChanged();
				}

			}

		}

		
		[Column(Name="PromotionDescription", UpdateCheck=UpdateCheck.Never, Storage="_PromotionDescription", DbType="varchar(256)")]
		public string PromotionDescription
		{
			get { return this._PromotionDescription; }

			set
			{
				if (this._PromotionDescription != value)
				{
				
                    this.OnPromotionDescriptionChanging(value);
					this.SendPropertyChanging();
					this._PromotionDescription = value;
					this.SendPropertyChanged("PromotionDescription");
					this.OnPromotionDescriptionChanged();
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
   		
   		[Association(Name="PROMOTION_CONTROL_PROMO_FK", Storage="_PromotionControls", OtherKey="PromotionId")]
   		public EntitySet< PromotionControl> PromotionControls
   		{
   		    get { return this._PromotionControls; }

			set	{ this._PromotionControls.Assign(value); }

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

   		
		private void attach_PromotionControls(PromotionControl entity)
		{
			this.SendPropertyChanging();
			entity.Promotion = this;
		}

		private void detach_PromotionControls(PromotionControl entity)
		{
			this.SendPropertyChanging();
			entity.Promotion = null;
		}

		
	}

}

